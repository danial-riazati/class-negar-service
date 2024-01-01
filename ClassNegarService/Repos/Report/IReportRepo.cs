
using ClassNegarService.Models.Report;

namespace ClassNegarService.Repos.Report
{
    public interface IReportRepo
    {
        public Task<ClassAttendanceResultModel> GetProfessorClassAttendance(int classId, int userId);
        public Task<ClassAttendanceResultModel> GetStudentClassAttendance(int classId, int userId);

    }
}

