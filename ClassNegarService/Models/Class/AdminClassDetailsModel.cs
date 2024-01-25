using System;
namespace ClassNegarService.Models.Class
{
    public class AdminClassDetailsModel
    {
        public string Name { get; set; }
        public int CurrentSize { get; set; }
        public string ProfessorName { get; set; }
        public DateTime Semester { get; set; }
        public string ClassLocation { get; set; }
        public int ClassSessions { get; set; }
        public int MediumOfClassAttendance { get; set; }
        public List<int> ClassAttendance { get; set; }
        public List<DateTime> Attendance { get; set; }
    }
}

