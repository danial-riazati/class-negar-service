using System;
using ClassNegarService.Models.Notification;

namespace ClassNegarService.Repos.Session
{
    public interface ISessionRepo
    {
        public Task<int?> FindSessionForNow(int classId);
        public Task<int?> CreateSession(int classId);

        public Task AddProfessorAttendance(int sessionId, int professorId);
        public Task AddProfessorExit(int sessionId, int professorId);
        public Task AddStudentAttendance(int sessionId, int studentId);

        public Task<bool> IsStudentAlreadyLoggedIn(int sessionId, int userId);
        public Task<bool> IsStudentAlreadyLoggedOutOrNotLoggedIn(int sessionId, int userId);
        public Task EndSession(int sessionId);
        public Task AddStudentExit(int sessionId, int studentId);

    }
}

