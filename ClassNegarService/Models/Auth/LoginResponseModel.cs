using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace ClassNegarService.Models.Auth
{
    public class SigninResponseModel
    {
        
        public  string AccessToken { get; set; }
        public  string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }

    }

}