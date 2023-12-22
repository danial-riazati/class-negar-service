using System;
using ClassNegarService.Db;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Repos
{
    public interface IAuthRepo
    {
        public Task<User?> FindUserByUserName(string username);
        public Task<bool> CheckUserPassword(User user, string password);
        public Task UpdateUser(User user);
        public Task<bool> CheckFingerPrintIdForAnotherUsers(string fingerprintId, int userId);


    }
}

