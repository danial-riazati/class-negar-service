using System;
using ClassNegarService.Db;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Models.Class;

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
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddTimeToClass(List<AddClassTimeModel> model, int classId)
        {
            List<ClassTime> times = new List<ClassTime>();
            model.ForEach((t) =>
            {
                times.Add(new ClassTime { ClassId = classId, DayOfWeek = t.DayOfWeek, TimeOfDay = t.TimeOfDay });
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
    }
}

