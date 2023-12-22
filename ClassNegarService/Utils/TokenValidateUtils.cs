using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace ClassNegarService.Utils
{
    public class TokenValidateUtils
    {
        private IConfiguration _configuration;

        public TokenValidateUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            token = token.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetTokenValidationParameters();

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {

            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,

                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
            };
        }

    }
}

