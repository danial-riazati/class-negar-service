using System;
using System.ComponentModel.DataAnnotations;

namespace ClassNegarService.Models.Class
{
    public class AddClassModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Semester { get; set; }
        [Required]
        public int MaxSize { get; set; }
        [Required]
        public string ClassLocation { get; set; }
        [Required]
        public List<AddClassTimeModel> Times { get; set; }
    }
}


