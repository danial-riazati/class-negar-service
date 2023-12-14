using System.ComponentModel.DataAnnotations;

namespace ClassNegarService.Models.Auth
{
    public class SigninModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [MinLength(8,ErrorMessage ="Minimum 8 charactors in password is required")]
        public string Password { get; set; }
    }
}