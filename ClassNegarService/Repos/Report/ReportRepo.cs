namespace ClassNegarService.Repos.Report
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ClassNegarService.Db;
    using ClassNegarService.Models.Report;

    public class ReportRepo : IReportRepo
    {
        private readonly ClassNegarDbContext _dbcontext;

        public ReportRepo(ClassNegarDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<ClassAttendanceResultModel> GetProfessorClassAttendance(int classId, int userId)
        {

            var attendance = (from pa in _dbcontext.ProfessorAttendances
                              join s in _dbcontext.Sessions
                              on pa.SessionId equals s.Id
                              where s.ClassId == classId && pa.UserId == userId
                              select s.StartedAt).ToList();
            var res = new ClassAttendanceResultModel { Attendance = attendance, Absence = new List<DateTime>() };
            return res;

        }

        public async Task<ClassAttendanceResultModel> GetStudentClassAttendance(int classId, int userId)
        {
            var professorId = (from c in _dbcontext.Classes
                               where c.Id == classId
                               select c.ProfessorId).FirstOrDefault();
            if (professorId == 0)
                throw new UnauthorizedAccessException();

            var attendance = (from sa in _dbcontext.StudentAttendances
                              join s in _dbcontext.Sessions
                              on sa.SessionId equals s.Id
                              where s.ClassId == classId && sa.UserId == userId
                              select s.StartedAt).ToList();

            var allSessions = await GetProfessorClassAttendance(classId, professorId);
            var absensce = allSessions.Attendance.Except(attendance).ToList();
            var res = new ClassAttendanceResultModel { Attendance = attendance, Absence = absensce };
            return res;
        }
    }
}

