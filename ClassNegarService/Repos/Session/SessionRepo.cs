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
        public Task AddProfessorAttendance(int sessionId, int professorId)
        {
            throw new NotImplementedException();
        }

        public Task AddStudentAttendance(int sessionId, int studentId)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> CreateSession(int classId)
        {
            var session = new Session { ClassId = classId, StartedAt = DateTime.Now };
            await _dbcontext.AddAsync(session);
            await _dbcontext.SaveChangesAsync();

            return session.Id;
        }

        public async Task<int?> FindSessionForNow(int classId)
        {
            var now = DateTime.Now;
            var res = (from s in _dbcontext.Sessions
                       where s.ClassId == classId && s.StartedAt < now && s.EndedAt > now
                       select s.Id).FirstOrDefault();

            if (res == 0) return null;
            return res;
        }


    }
}

