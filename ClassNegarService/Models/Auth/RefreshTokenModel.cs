using System;
namespace ClassNegarService.Models.Auth
{
	public class RefreshTokenModel
	{
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

