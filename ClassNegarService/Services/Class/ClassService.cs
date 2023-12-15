using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Models.Class;
using ClassNegarService.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ClassNegarService.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepo _classRepo;
        private readonly IConfiguration _configuration;

        public ClassService(
            IClassRepo classRepo,
            IConfiguration configuration
            )
        {
            _classRepo = classRepo;
            _configuration = configuration;
        }

        public Task AddClass(AddClassModel model)
        {
            throw new NotImplementedException();
        }
    }
}

