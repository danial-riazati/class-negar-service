using System;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Services
{
    public interface IAuthService
    {
        public Task<SigninResponseModel?> Signin(SigninModel model);
        public Task<RefreshTokenModel> RefreshToken(TokenModel model);
        public Task Revoke(string username);
    }
}

