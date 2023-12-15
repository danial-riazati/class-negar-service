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

        public async Task AddClass(AddClassModel model, string code, string password, int professorId)
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

            _dbcontext.Add(newClass);
            await _dbcontext.SaveChangesAsync();
        }
    }
}

