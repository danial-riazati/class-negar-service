using System;
using ClassNegarService.Db;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Models.Class;
using ClassNegarService.Models.Report;
using ClassNegarService.Models.Session;
using ClassNegarService.Utils;

namespace ClassNegarService.Repos
{
    public class ClassRepo : IClassRepo
    {
        private readonly ClassNegarDbContext _dbcontext;

        public ClassRepo(ClassNegarDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<int?> AddClass(AddClassModel model, string code, string password, int professorId)
        {

            var newClass = new Class
            {
                Name = model.Name,
                ProfessorId = professorId,
                Semester = model.Semester,
                ClassLocation = model.ClassLocation,
                Code = code,
                Password = password,
                CurrentSize = 0,
                IsAttendingNow = false,
                MaxSize = model.MaxSize
            };

            await _dbcontext.AddAsync(newClass);
            await _dbcontext.SaveChangesAsync();
            return newClass.Id;
        }

        public async Task AddEnrollment(int studentId, int classId, DateTime joinedAt)
        {
            var enrollment = new Enrollment { ClassId = classId, JoinedAt = joinedAt, IsRemoved = false, StudentId = studentId };
            await _dbcontext.AddAsync(enrollment);
            var theClass = (from c in _dbcontext.Classes
                            where c.Id == classId
                            select c).FirstOrDefault() ?? throw new UnauthorizedAccessException();
            theClass.CurrentSize += 1;
            _dbcontext.Update(theClass);

            await _dbcontext.SaveChangesAsync();

        }

        public async Task AddTimeToClass(List<AddClassTimeModel> model, int classId)
        {
            List<ClassTime> times = new List<ClassTime>();
            model.ForEach((t) =>
            {
                var timeStart = StringUtils.ConvertTimeStrigToDateTime(t.StartAt);
                var timeEnd = StringUtils.ConvertTimeStrigToDateTime(t.EndAt);

                times.Add(new ClassTime { ClassId = classId, DayOfWeek = t.DayOfWeek, StartAt = timeStart, EndAt = timeEnd });
            });

            await _dbcontext.AddRangeAsync(times);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<ProfessorClassesModel>> GetAllProfessorClasses(int professorId)
        {
            var result = (from c in _dbcontext.Classes
                          join p in _dbcontext.Users
                          on c.ProfessorId equals p.Id
                          where c.ProfessorId == professorId
                          orderby c.Semester descending
                          select new ProfessorClassesModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              ClassLocation = c.ClassLocation,
                              Code = c.Code,
                              CurrentSize = c.CurrentSize,
                              MaxSize = c.MaxSize,
                              Passwrod = c.Password,
                              ProfessorName = p.FirstName + " " + p.LastName,
                              Semester = c.Semester,
                              IsAttendingNow = c.IsAttendingNow
                          }).ToList();

            return result;


        }

        public async Task<List<StudentClassesModel>> GetAllStudentClasses(int studentId)
        {
            var result = (from e in _dbcontext.Enrollments
                          join c in _dbcontext.Classes
                          on e.ClassId equals c.Id
                          join p in _dbcontext.Users
                          on c.ProfessorId equals p.Id
                          where e.StudentId == studentId
                          orderby c.Semester descending
                          select new StudentClassesModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              ClassLocation = c.ClassLocation,
                              CurrentSize = c.CurrentSize,
                              ProfessorName = p.FirstName + " " + p.LastName,
                              Semester = c.Semester,
                              IsAttendingNow = c.IsAttendingNow,
                          }).ToList();


            return result;
        }

        public async Task<int?> GetClassId(JoinClassModel model)
        {
            var result = (from c in _dbcontext.Classes
                          where c.Code == model.Code && c.Password == model.Password
                          select c).FirstOrDefault();
            if (result == null)
                return null;
            return result.Id;
        }

        public async Task<ProfessorClassesModel?> GetProfessorClass(int professorId, int classId)
        {

            var result = (from c in _dbcontext.Classes
                          join u in _dbcontext.Users
                          on c.ProfessorId equals u.Id
                          where c.Id == classId && c.ProfessorId == professorId
                          select new ProfessorClassesModel
                          {
                              Id = c.Id,
                              ClassLocation = c.ClassLocation,
                              CurrentSize = c.CurrentSize,
                              IsAttendingNow = c.IsAttendingNow,
                              ProfessorName = u.FirstName + " " + u.LastName,
                              Name = c.Name,
                              Semester = c.Semester,
                              Code = c.Code,
                              Passwrod = c.Password,
                              MaxSize = c.MaxSize,
                          }
                          ).FirstOrDefault();
            if (result == null) return null;



            return result;
        }
        public async Task<bool> HasEnrolled(int studentId, int classId)
        {
            var hasEnrolled = (from e in _dbcontext.Enrollments
                               where e.ClassId == classId && e.StudentId == studentId
                               select e).FirstOrDefault();

            return hasEnrolled != null ? true : false;
        }
        public async Task<bool> HasProfessorAccess(int professorId, int classId)
        {
            var hasAccess = (from c in _dbcontext.Classes
                             where c.Id == classId && c.ProfessorId == professorId
                             select c).FirstOrDefault();

            return hasAccess != null ? true : false;
        }

        public async Task<StudentClassesModel?> GetStudentClass(int classId)
        {


            var result = (from c in _dbcontext.Classes
                          join u in _dbcontext.Users
                          on c.ProfessorId equals u.Id
                          where c.Id == classId
                          select new StudentClassesModel
                          {
                              Id = c.Id,
                              ClassLocation = c.ClassLocation,
                              CurrentSize = c.CurrentSize,
                              IsAttendingNow = c.IsAttendingNow,
                              ProfessorName = u.FirstName + " " + u.LastName,
                              Name = c.Name,
                              Semester = c.Semester,
                          }
                          ).FirstOrDefault();
            if (result == null) return null;


            return result;
        }

        public async Task<List<ClassResourseModel>> GetClassResourses(int classId)
        {
            var resourses = (from r in _dbcontext.ClassResourses
                             where r.ClassId == classId
                             select new ClassResourseModel
                             {
                                 Id = r.Id,
                                 DownloadLink = r.DownloadLink,
                                 Format = r.Format,
                                 InsertedAt = r.InsertedAt,
                                 Size = r.Size,
                                 Name = r.Name
                             }).ToList();
            return resourses;

        }

        public async Task AddClassResourses(string name, int classId, string downloadLink, DateTime insertedAt, string format, int size)
        {
            var resourse = new ClassResourse
            {
                DownloadLink = downloadLink,
                Format = format,
                InsertedAt = insertedAt,
                Size = size,
                ClassId = classId,
                Name = name
            };
            await _dbcontext.AddAsync(resourse);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<bool?> IsRemovedFromClass(int studentId, int classId)
        {
            var res = (from e in _dbcontext.Enrollments
                       where e.ClassId == classId && e.StudentId == studentId
                       select e).FirstOrDefault();
            if (res == null) return null;
            return res.IsRemoved;


        }

        public async Task<List<AddClassTimeModel>> GetClassTimes(int classId)
        {
            var classTime = (from ct in _dbcontext.ClassTimes
                             where ct.ClassId == classId
                             select new AddClassTimeModel
                             {
                                 DayOfWeek = ct.DayOfWeek,
                                 StartAt = StringUtils.ConvertDateTimeToTimeStrig(ct.StartAt),
                                 EndAt = StringUtils.ConvertDateTimeToTimeStrig(ct.EndAt)
                             }).ToList();
            return classTime;
        }

        public async Task UpdateAttendingStatus(bool isAttending, int classId)
        {
            var theclass = (from c in _dbcontext.Classes
                            where c.Id == classId
                            select c).FirstOrDefault();
            if (theclass == null)
                return;

            theclass.IsAttendingNow = isAttending;
            _dbcontext.Update(theclass);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<AdminClassModel>> GetAllCurrentClasses()
        {
            var classes = (from c in _dbcontext.Classes
                           join u in _dbcontext.Users
                           on c.ProfessorId equals u.Id
                           where c.IsAttendingNow == true
                           select new AdminClassModel
                           {
                               Id = c.Id,
                               Name = c.Name,
                               ClassLocation = c.ClassLocation,
                               ProfessorName = u.FirstName + " " + u.LastName
                           }).ToList();
            var result = GetclassTimeAddedAdminClassModel(classes);

            return result;
        }

        public async Task<List<AdminClassModel>> GetAllDoneClasses()
        {
            var now = DateTime.Now;
            var classes = (from s in _dbcontext.Sessions
                           join c in _dbcontext.Classes
                           on s.ClassId equals c.Id
                           join u in _dbcontext.Users
                           on c.ProfessorId equals u.Id
                           where s.StartedAt.Year == now.Year && s.StartedAt.Month == now.Month
                           && s.StartedAt.Day == now.Day && s.EndedAt != null
                           orderby s.StartedAt descending
                           select new AdminClassModel
                           {
                               Id = c.Id,
                               Name = c.Name,
                               ClassLocation = c.ClassLocation,
                               ProfessorName = u.FirstName + " " + u.LastName
                           }).ToList();
            var result = GetclassTimeAddedAdminClassModel(classes);

            return result;
        }

        private List<AdminClassModel> GetclassTimeAddedAdminClassModel(List<AdminClassModel> classes)
        {
            var classTimes = (from c in classes
                              join ct in _dbcontext.ClassTimes
                              on c.Id equals ct.ClassId
                              group new { ct.DayOfWeek, ct.StartAt, ct.EndAt } by ct.ClassId into g
                              select new { Id = g.Key, Times = g.ToList() }
                           );

            List<SessionClass> list = new List<SessionClass>();
            Dictionary<int, string> week = new Dictionary<int, string>
            {
                { 0,"یکشنبه" },
                     { 1,"دوشنبه" },
                  { 2,"سه‌شنبه" },
                   { 3,"چهارشنبه" },
                    { 4,"پنجشنبه" },
                     { 5,"جمعه" },
                      { 6,"شنبه" },
            };

            foreach (var ct in classTimes)
            {

                var start = StringUtils.ConvertDateTimeToTimeStrig(ct.Times[0].StartAt);
                var end = StringUtils.ConvertDateTimeToTimeStrig(ct.Times[0].EndAt);
                string time = start + " - " + end;
                string days = "";
                foreach (var t in ct.Times)
                {
                    days += week[t.DayOfWeek] + "/";
                }
                days = days.Remove(days.Length - 1);
                list.Add(new SessionClass { Time = time, Day = days, Id = ct.Id });
            }
            var final = (from l in list
                         join c in classes
                         on l.Id equals c.Id
                         select new AdminClassModel
                         {
                             Id = c.Id,
                             Name = c.Name,
                             ClassLocation = c.ClassLocation,
                             ClassTime = $"({l.Time}){l.Day}",
                             ProfessorName = c.ProfessorName
                         }).ToList();
            return final;
        }

        public async Task<List<AdminClassCalendarModel>> GetAdminClassCalendar()
        {
            Dictionary<int, string> week = new Dictionary<int, string>
            {
                { 0,"یکشنبه" },
                     { 1,"دوشنبه" },
                  { 2,"سه‌شنبه" },
                   { 3,"چهارشنبه" },
                    { 4,"پنجشنبه" },
                     { 5,"جمعه" },
                      { 6,"شنبه" },
            };
            var res = new List<AdminClassCalendarModel>();
            res.Add(new AdminClassCalendarModel
            {
                DayOfWeek = "شنبه",
                Classes = GetCalendarOfDay(6)
            });
            for (int i = 0; i < 6; i++)
            {
                res.Add(new AdminClassCalendarModel
                {
                    DayOfWeek = week[i],
                    Classes = GetCalendarOfDay(i)
                });
            }
            return res;
        }
        private List<AdminClassCalendarItemModel> GetCalendarOfDay(int dayNum)
        {
            //  var today = (int)DateTime.Now.DayOfWeek;
            var res = (from ct in _dbcontext.ClassTimes
                       join c in _dbcontext.Classes
                       on ct.ClassId equals c.Id
                       join u in _dbcontext.Users
                       on c.ProfessorId equals u.Id
                       where ct.DayOfWeek == dayNum
                       orderby ct.StartAt
                       select new AdminClassCalendarItemModel
                       {
                           ClassLocation = c.ClassLocation,
                           IsAttendingNow = c.IsAttendingNow,
                           Name = c.Name,
                           ProfessorName = u.FirstName + " " + u.LastName,
                           ClassTime = StringUtils.ConvertDateTimeToTimeStrig(ct.StartAt)
                           + " - " +
                           StringUtils.ConvertDateTimeToTimeStrig(ct.EndAt)
                       }).ToList();
            return res;
        }

        public async Task<List<AdminReportClassModel>> GetAllAdminReportClasses()
        {
            var classes = (from c in _dbcontext.Classes
                           join u in _dbcontext.Users
                           on c.ProfessorId equals u.Id
                           orderby c.Semester descending
                           select new AdminReportClassModel
                           {
                               Id = c.Id,
                               Name = c.Name,
                               Semester = c.Semester,
                               ProfessorName = u.FirstName + " " + u.LastName
                           }).ToList();
            return classes;
        }

        public async Task<AdminClassDetailsModel?> GetAdminClassDetails(int classId)
        {
            var res = (from c in _dbcontext.Classes
                       join u in _dbcontext.Users
                       on c.ProfessorId equals u.Id
                       where c.Id == classId
                       select new AdminClassDetailsModel
                       {
                           Name = c.Name,
                           Semester = c.Semester,
                           ClassLocation = c.ClassLocation,
                           ProfessorName = u.FirstName + " " + u.LastName,
                           CurrentSize = c.CurrentSize,

                       }).FirstOrDefault();
            if (res == null) throw new UnauthorizedAccessException();
            var attendance = (from s in _dbcontext.Sessions
                              where s.ClassId == classId
                              select s.StartedAt).ToList();
            res.Attendance = attendance;

            var enrollmentCount = (from e in _dbcontext.Enrollments
                                   where e.ClassId == classId
                                   select e).Count();

            var sessions = (from s in _dbcontext.Sessions
                            where s.ClassId == classId
                            orderby s.StartedAt
                            select s).ToList();

            res.ClassSessions = sessions.Count;
            res.ClassAttendance = new List<int>();
            int total = 0;
            foreach (var s in sessions)
            {
                var sessionAttendance = (from a in _dbcontext.StudentAttendances
                                         where a.SessionId == s.Id
                                         select a).Count();
                var inpercent = (sessionAttendance * 100) / enrollmentCount;
                res.ClassAttendance.Add(inpercent);
                total += inpercent;

            }
            res.ClassAttendance.AddRange(Enumerable.Repeat(0, 10 - res.ClassAttendance.Count % 10));
            if (res.ClassSessions == 0)
                res.MediumOfClassAttendance = 0;
            else
                res.MediumOfClassAttendance = total / res.ClassSessions;


            return res;


        }
    }


}

