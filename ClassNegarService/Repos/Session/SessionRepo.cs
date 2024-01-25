namespace ClassNegarService.Repos.Session
{
    using System.Collections.Generic;
    using ClassNegarService.Db;
    using ClassNegarService.Models.Session;
    using ClassNegarService.Utils;

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

        public async Task<Session> GetLatestSessionDate(int classId)
        {
            var res = (from s in _dbcontext.Sessions
                       where s.ClassId == classId
                       orderby s.StartedAt descending
                       select s
                       ).FirstOrDefault();
            return res;
        }

        public async Task<List<SessionClass>> GetProfessorSessionClass(int professorId)
        {
            var classes = (from c in _dbcontext.Classes
                           where c.ProfessorId == professorId
                           select c
                                     );
            var classTimes = (from c in classes
                              join ct in _dbcontext.ClassTimes
                              on c.Id equals ct.ClassId
                              group new { ct.DayOfWeek, ct.StartAt, ct.EndAt } by ct.ClassId into g
                              select new { Id = g.Key, Times = g.ToList() }
                              );

            List<SessionClass> list = new List<SessionClass>();
            Dictionary<int, string> week = new Dictionary<int, string>
            {
                { 0,"یکشنبه" },
                     { 1,"دوشنبه" },
                  { 2,"سه‌شنبه" },
                   { 3,"چهارشنبه" },
                    { 4,"پنجشنبه" },
                     { 5,"جمعه" },
                      { 6,"شنبه" },
            };

            foreach (var ct in classTimes)
            {

                var start = StringUtils.ConvertDateTimeToTimeStrig(ct.Times[0].StartAt);
                var end = StringUtils.ConvertDateTimeToTimeStrig(ct.Times[0].EndAt);
                string time = start + " - " + end;
                string days = "";
                foreach (var t in ct.Times)
                {
                    days += week[t.DayOfWeek] + "-";
                }
                days = days.Remove(days.Length - 1);
                list.Add(new SessionClass { Time = time, Day = days, Id = ct.Id });
            }
            var final = (from l in list
                         join c in classes
                         on l.Id equals c.Id
                         select new SessionClass { Id = l.Id, Name = c.Name, Day = l.Day, Time = l.Time }).ToList();


            return final ?? throw new UnauthorizedAccessException();
        }

        public async Task<List<string>> GetSessionPresent(int sessionId)
        {
            var res = (from s in _dbcontext.StudentAttendances
                       join u in _dbcontext.Users
                        on s.UserId equals u.Id
                       where s.SessionId == sessionId
                       select u.FirstName + " " + u.LastName).ToList();

            return res;


        }

        public async Task<List<SessionClass>> GetStudentSessionClass(int studentId)
        {
            var classes = (from c in _dbcontext.Classes
                           join e in _dbcontext.Enrollments
                           on c.Id equals e.ClassId
                           where e.StudentId == studentId
                           select c
                           );
            var classTimes = (from c in classes
                              join ct in _dbcontext.ClassTimes
                              on c.Id equals ct.ClassId
                              group new { ct.DayOfWeek, ct.StartAt, ct.EndAt } by ct.ClassId into g
                              select new { Id = g.Key, Times = g.ToList() }
                              );

            List<SessionClass> list = new List<SessionClass>();
            Dictionary<int, string> week = new Dictionary<int, string>
            {
                { 0,"یکشنبه" },
                     { 1,"دوشنبه" },
                  { 2,"سه‌شنبه" },
                   { 3,"چهارشنبه" },
                    { 4,"پنجشنبه" },
                     { 5,"جمعه" },
                      { 6,"شنبه" },
            };

            foreach (var ct in classTimes)
            {

                var start = StringUtils.ConvertDateTimeToTimeStrig(ct.Times[0].StartAt);
                var end = StringUtils.ConvertDateTimeToTimeStrig(ct.Times[0].EndAt);
                string time = start + " - " + end;
                string days = "";
                foreach (var t in ct.Times)
                {
                    days += week[t.DayOfWeek] + "-";
                }
                days = days.Remove(days.Length - 1);
                list.Add(new SessionClass { Time = time, Day = days, Id = ct.Id });
            }
            var final = (from l in list
                         join c in classes
                         on l.Id equals c.Id
                         select new SessionClass { Id = l.Id, Name = c.Name, Day = l.Day, Time = l.Time }).ToList();


            return final ?? throw new UnauthorizedAccessException();

        }

        public async Task<User?> GetUserOfRfid(string rfid)
        {
            var user = (from u in _dbcontext.Users
                        where u.RfidTag == rfid
                        select u).FirstOrDefault();
            return user;
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
                return true;
            return false;
        }
    }
}

