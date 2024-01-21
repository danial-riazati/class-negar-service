using System;
using ClassNegarService.Db;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Models.Class;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Repos
{
    public interface IClassRepo
    {
        public Task<int?> AddClass(AddClassModel model, string code, string password, int professorId);
        public Task AddTimeToClass(List<AddClassTimeModel> model, int classId);
        public Task<List<ProfessorClassesModel>> GetAllProfessorClasses(int professorId);
        public Task<List<StudentClassesModel>> GetAllStudentClasses(int studentId);
        public Task<ProfessorClassesModel?> GetProfessorClass(int professorId, int classId);
        public Task<StudentClassesModel?> GetStudentClass(int classId);
        public Task<int?> GetClassId(JoinClassModel model);
        public Task AddEnrollment(int studentId, int classId, DateTime joinedAt);
        public Task<bool> HasEnrolled(int studentId, int classId);
        public Task<bool> HasProfessorAccess(int professorId, int classId);
        public Task<List<ClassResourseModel>> GetClassResourses(int classId);
        public Task AddClassResourses(string name, int classId, string downloadLink, DateTime insertedAt, string format, int size);
        public Task<bool?> IsRemovedFromClass(int studentId, int classId);
        public Task<List<AddClassTimeModel>> GetClassTimes(int classId);
        public Task UpdateAttendingStatus(bool isAttending, int classId);


    }
}


