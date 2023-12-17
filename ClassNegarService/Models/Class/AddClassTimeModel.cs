using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
namespace ClassNegarService.Models.Class
{
    public class AddClassTimeModel
    {
        [Required]
        public int DayOfWeek { get; set; }
        [Required]
        public string StartAt { get; set; }
        [Required]
        public string EndAt { get; set; }

    }
}

