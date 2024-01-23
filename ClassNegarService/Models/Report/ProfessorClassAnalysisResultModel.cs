using System;
namespace ClassNegarService.Models.Report
{
    public class ProfessorClassAnalysisResultModel
    {
        public int ClassSessions { get; set; }
        public int MediumOfClassAttendance { get; set; }
        public List<int> ClassAttendance { get; set; }
    }
}

