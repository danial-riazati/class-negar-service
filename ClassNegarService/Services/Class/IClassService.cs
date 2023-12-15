﻿using System;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Models.Class;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Services
{
    public interface IClassService
    {
        public Task AddClass(AddClassModel model, int professorId);
        public Task<List<ProfessorClassesModel>> GetAllProfessorClasses(int professorId);
        public Task<List<StudentClassesModel>> GetAllStudentClasses(int studentId);

    }
}

