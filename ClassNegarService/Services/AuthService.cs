using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ClassNegarService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly IConfiguration _configuration;

        public AuthService(
            IAuthRepo authRepo,
            IConfiguration configuration
            )
        {
            _authRepo = authRepo;
            _configuration = configuration;
        }

        public async Task<RefreshTokenModel> RefreshToken(TokenModel model)
        {
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            if (principal == null)
            {
                throw new Exception("Invalid access token or refresh token");
            }


            string username = principal.Claims.FirstOrDefault(x => x.Type == "username").Value;


            var user = await _authRepo.FindUserByUserName(username);

            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new Exception("Invalid access token or refresh token");
            }


            var authClaims = new List<Claim>
                {
                    new Claim("user_id",user.Id.ToString() as string),
                    new Claim("username", user.UserName as string),
                    new Claim("role_id",user.RoleId.ToString() as string),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };

            var newAccessToken = CreateToken(authClaims);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _authRepo.UpdateUser(user);


            return new RefreshTokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        public async Task Revoke(string username)
        {
            var user = await _authRepo.FindUserByUserName(username);
            if (user == null) throw new Exception("Invalid username");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _authRepo.UpdateUser(user);
        }



        public async Task<SigninResponseModel?> Signin(SigninModel model)
        {
            var user = await _authRepo.FindUserByUserName(model.Username);
            if (user != null && await _authRepo.CheckUserPassword(user, model.Password))
            {

                var authClaims = new List<Claim>
                {
                    new Claim("user_id",user.Id.ToString() as string),
                    new Claim("username", user.UserName as string),
                    new Claim("role_id",user.RoleId.ToString() as string),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };


                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _authRepo.UpdateUser(user);



                return new SigninResponseModel
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                };
            }
            return null;
        }



        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;


        }

        private string GenerateRefreshToken()
        {

            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}

