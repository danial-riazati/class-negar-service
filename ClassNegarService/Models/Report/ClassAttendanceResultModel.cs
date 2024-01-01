using System;
namespace ClassNegarService.Models.Report
{
    public class ClassAttendanceResultModel
    {
        public List<DateTime> Attendance { get; set; }
        public List<DateTime> Absence { get; set; }
    }
}
