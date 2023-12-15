using System;
using ClassNegarService.Db;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;

namespace ClassNegarService.Repos
{
    public class ClassRepo : IClassRepo
    {
        private readonly ClassNegarDbContext _dbcontext;

        public ClassRepo(ClassNegarDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
    }
}

