using System;
using ClassNegarService.Models.Notification;

namespace ClassNegarService.Models.Class
{
    public class StudentClassesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Semester { get; set; }
        public string ProfessorName { get; set; }
        public int CurrentSize { get; set; }
        public string ClassLocation { get; set; }
        public bool IsAttendingNow { get; set; }
        public List<AddClassTimeModel> ClassTimes { get; set; }
        public List<AllNotificationsResultModel> Notifications { get; set; }
    }
}


