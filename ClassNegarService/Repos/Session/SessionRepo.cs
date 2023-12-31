namespace ClassNegarService.Repos.Session
{
    using ClassNegarService.Db;
    public class SessionRepo : ISessionRepo
    {
        private readonly ClassNegarDbContext _dbcontext;

        public SessionRepo(ClassNegarDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddProfessorAttendance(int sessionId, int professorId)
        {
            var attendance = new ProfessorAttendance { SessionId = sessionId, JoinedAt = DateTime.Now, UserId = professorId };
            await _dbcontext.AddAsync(attendance);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task AddProfessorExit(int sessionId, int professorId)
        {
            var attendance = (from pa in _dbcontext.ProfessorAttendances
                              where pa.SessionId == sessionId && pa.UserId == professorId
                              select pa).FirstOrDefault();

            if (attendance == null) return;
            attendance.QuitedAt = DateTime.Now;
            _dbcontext.Update(attendance);
            await _dbcontext.SaveChangesAsync();

        }
        public async Task AddStudentAttendance(int sessionId, int studentId)
        {
            var attendance = new StudentAttendance { SessionId = sessionId, JoinedAt = DateTime.Now, UserId = studentId };
            await _dbcontext.AddAsync(attendance);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddStudentExit(int sessionId, int studentId)
        {
            var attendance = (from sa in _dbcontext.StudentAttendances
                              where sa.SessionId == sessionId && sa.UserId == studentId
                              select sa).FirstOrDefault();

            if (attendance == null) return;
            attendance.QuitedAt = DateTime.Now;
            _dbcontext.Update(attendance);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<int?> CreateSession(int classId)
        {
            var session = new Session { ClassId = classId, StartedAt = DateTime.Now };
            await _dbcontext.AddAsync(session);
            await _dbcontext.SaveChangesAsync();

            return session.Id;
        }

        public async Task EndSession(int sessionId)
        {
            var session = (from s in _dbcontext.Sessions
                           where s.Id == sessionId
                           select s).FirstOrDefault();

            if (session == null) return;
            session.EndedAt = DateTime.Now;
            _dbcontext.Update(session);
            await _dbcontext.SaveChangesAsync();

        }

        public async Task<int?> FindSessionForNow(int classId)
        {
            var now = DateTime.Now;
            var res = (from s in _dbcontext.Sessions
                       where s.ClassId == classId && (s.StartedAt.DayOfYear == now.DayOfYear && s.StartedAt < now && s.EndedAt == null)
                       select s.Id).FirstOrDefault();

            if (res == 0) return null;
            return res;
        }

        public async Task<bool> IsStudentAlreadyLoggedIn(int sessionId, int userId)
        {
            var attendance = (from sa in _dbcontext.StudentAttendances
                              where sa.SessionId == sessionId && sa.UserId == userId
                              select sa).FirstOrDefault();
            if (attendance == null)
                return false;
            return true;
        }

        public async Task<bool> IsStudentAlreadyLoggedOutOrNotLoggedIn(int sessionId, int userId)
        {
            var attendance = (from sa in _dbcontext.StudentAttendances
                              where sa.SessionId == sessionId && sa.UserId == userId
                              select sa).FirstOrDefault();
            if (attendance == null || attendance.QuitedAt != null)
                return false;
            return true;
        }


        //public async Task<bool> IsProfessorAlreadyLoggedOutOrNotLoggedIn(int sessionId, int userId)
        //{
        //    var attendance = (from sa in _dbcontext.ProfessorAttendances
        //                      where sa.SessionId == sessionId && sa.UserId == userId
        //                      select sa).FirstOrDefault();
        //    if (attendance == null || attendance.QuitedAt != null)
        //        return true;
        //    return false;
        //}
    }
}

