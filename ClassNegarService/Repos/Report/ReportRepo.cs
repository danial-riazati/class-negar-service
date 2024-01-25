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

        public async Task<ProfessorClassAnalysisResultModel> GetProfessorClassAnalysis(int classId)
        {
            var enrollmentCount = (from e in _dbcontext.Enrollments
                                   where e.ClassId == classId
                                   select e).Count();

            var sessions = (from s in _dbcontext.Sessions
                            where s.ClassId == classId
                            orderby s.StartedAt
                            select s).ToList();
            var result = new ProfessorClassAnalysisResultModel { ClassSessions = sessions.Count, ClassAttendance = new List<int>() };
            int total = 0;
            foreach (var s in sessions)
            {
                var sessionAttendance = (from a in _dbcontext.StudentAttendances
                                         where a.SessionId == s.Id
                                         select a).Count();
                var inpercent = (sessionAttendance * 100) / enrollmentCount;
                result.ClassAttendance.Add(inpercent);
                total += inpercent;

            }
            result.ClassAttendance.AddRange(Enumerable.Repeat(0, 10 - result.ClassAttendance.Count % 10));
            if (result.ClassSessions == 0)
                result.MediumOfClassAttendance = 0;
            else
                result.MediumOfClassAttendance = total / result.ClassSessions;
            return result;

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

        public async Task<List<ProfessorClassAttendanceResultModel>> GetStudentsClassAttendance(int classId)
        {
            var students = (from s in _dbcontext.Enrollments
                            join ss in _dbcontext.Users
                            on s.StudentId equals ss.Id
                            where s.ClassId == classId
                            select ss).ToList();
            var result = new List<ProfessorClassAttendanceResultModel>();
            foreach (var s in students)
            {
                result.Add(new ProfessorClassAttendanceResultModel { Attendance = await GetStudentClassAttendance(classId, s.Id), StudentName = s.FirstName + " " + s.LastName });
            }

            return result;

        }
    }
}

