using System;
namespace ClassNegarService.Models.Class
{
    public class AdminClassCalendarModel
    {
        public string DayOfWeek { get; set; }
        public List<AdminClassCalendarItemModel> Classes { get; set; }
    }

}

