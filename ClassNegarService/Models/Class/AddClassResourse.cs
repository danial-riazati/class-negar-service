using System;
using System.ComponentModel.DataAnnotations;

namespace ClassNegarService.Models.Class
{
    public class AddClassResourse
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int ClassId { get; set; }
        [Required]
        public string Base64Data { get; set; }
    }
}


