using System;
using ClassNegarService.Db;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;

namespace ClassNegarService.Repos
{
    public class AuthRepo : IAuthRepo
    {
        private readonly ClassNegarDbContext _dbcontext;

        public AuthRepo(ClassNegarDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<bool> CheckFingerPrintIdForAnotherUsers(string fingerprintId, int myUserId)
        {
            var user = (from u in _dbcontext.Users
                        where u.FingerPrintId == fingerprintId && u.Id != myUserId && u.LastLogin > DateTime.Now.AddHours(-1)
                        select u).FirstOrDefault();

            if (user == null) return true;

            return false;
        }

        public async Task<bool> CheckUserPassword(User user, string password)
        {
            if (user.Password == password)
                return true;

            return false;
        }

        public async Task<User?> FindUserByUserName(string username)
        {
            var user = (from u in _dbcontext.Users
                        where u.UserName == username
                        select u).FirstOrDefault();


            return user;

        }

        public async Task UpdateUser(User user)
        {
            _dbcontext.Update(user);
            await _dbcontext.SaveChangesAsync();
        }
    }
}

