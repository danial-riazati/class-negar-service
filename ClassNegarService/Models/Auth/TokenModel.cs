using System;
using System.ComponentModel.DataAnnotations;

namespace ClassNegarService.Models.Auth
{
    public class TokenModel
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }

}

