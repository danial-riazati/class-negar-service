using System;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Models.Class;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Services
{
    public interface IClassService
    {
        public Task<AddClassResponseModel> AddClass(AddClassModel model, int professorId);
        public Task AddClassResourse(AddClassResourse model, int professorId);
        public Task<List<ProfessorClassesModel>> GetAllProfessorClasses(int professorId);
        public Task<List<StudentClassesModel>> GetAllStudentClasses(int studentId);
        public Task<ProfessorClassesModel?> GetProfessorClass(int professorId, int classId);
        public Task<StudentClassesModel?> GetStudentClass(int studentId, int classId);
        public Task<List<ClassResourseModel>> GetStudentResources(int studentId, int classId);
        public Task<List<ClassResourseModel>> GetProfessorResources(int professorId, int classId);
        public Task JoinClass(JoinClassModel model, int studentId);

    }

}

