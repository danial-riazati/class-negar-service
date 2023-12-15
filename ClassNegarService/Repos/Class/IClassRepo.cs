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
        public Task AddClass(AddClassModel model, string code, string password, int professorId);


    }
}

