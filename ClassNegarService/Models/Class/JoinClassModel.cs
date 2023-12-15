using System;
using System.ComponentModel.DataAnnotations;

namespace ClassNegarService.Models.Class
{
    public class JoinClassModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Password { get; set; }

    }
}

