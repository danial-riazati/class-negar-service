using System;
using ClassNegarService.Models.Notification;
using ClassNegarService.Models.Report;
using ClassNegarService.Models.Session;

namespace ClassNegarService.Services.Report
{
    public interface IReportService
    {
        public Task<ClassAttendanceResultModel?> GetClassAttendance(int classId, int userId, int userRole);
        public Task<List<ProfessorClassAttendanceResultModel>?> GetProfessorClassAttendance(int classId, int userId, int userRole);
        public Task<ProfessorClassAnalysisResultModel?> GetProfessorClassAnalysis(int classId, int userId, int userRole);

    }
}

