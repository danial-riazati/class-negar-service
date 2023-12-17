using System;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
namespace ClassNegarService.Models.Class
{
    public class AddClassTimeModel
    {
        public int DayOfWeek { get; set; }
        public string StartAt { get; set; }
        public string EndAt { get; set; }

    }
}

