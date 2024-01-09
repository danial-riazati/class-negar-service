using System;
using ClassNegarService.Db;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Models.Class;
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
    }
}

