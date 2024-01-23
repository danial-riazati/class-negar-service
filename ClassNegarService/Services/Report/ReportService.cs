

using ClassNegarService.Models.Enums;
using ClassNegarService.Models.Report;
using ClassNegarService.Repos;
using ClassNegarService.Repos.Report;

namespace ClassNegarService.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IConfiguration _configuration;
        private readonly IReportRepo _reportRepo;
        private readonly IClassRepo _classRepo;

        public ReportService(
            IConfiguration configuration,
            IReportRepo reportRepo,
            IClassRepo classRepo
        )
        {
            _configuration = configuration;
            _reportRepo = reportRepo;
            _classRepo = classRepo;
        }

        public async Task<ClassAttendanceResultModel?> GetClassAttendance(int classId, int userId, int userRole)
        {
            if (userRole == (int)RoleEnum.professor)
            {
                throw new UnauthorizedAccessException();
            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

                var res = await _reportRepo.GetStudentClassAttendance(classId, userId);
                return res;
            }
            return null;
        }

        public async Task<ProfessorClassAnalysisResultModel?> GetProfessorClassAnalysis(int classId, int userId, int userRole)
        {
            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();
                var result = await _reportRepo.GetProfessorClassAnalysis(classId);
                return result;

            }
            else if (userRole == (int)RoleEnum.student)
            {
                throw new UnauthorizedAccessException();

            }
            return null;
        }

        public async Task<List<ProfessorClassAttendanceResultModel>?> GetProfessorClassAttendance(int classId, int userId, int userRole)
        {
            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();
                var students = await _reportRepo.GetStudentsClassAttendance(classId);
                return students;

            }
            else if (userRole == (int)RoleEnum.student)
            {
                throw new UnauthorizedAccessException();

            }
            return null;
        }
    }
}

