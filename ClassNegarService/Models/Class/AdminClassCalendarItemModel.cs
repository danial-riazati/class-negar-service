using System;
namespace ClassNegarService.Models.Class
{
    public class AdminClassCalendarItemModel
    {
        public string Name { get; set; }
        public string ProfessorName { get; set; }
        public bool IsAttendingNow { get; set; }
        public string ClassLocation { get; set; }
        public string ClassTime { get; set; }

    }
}


