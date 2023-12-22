using System;
using ClassNegarService.Models.Notification;

namespace ClassNegarService.Repos.Session
{
    public interface ISessionRepo
    {
        public Task<int?> FindSessionForNow(int classId);
        public Task<int?> CreateSession(int classId);

        public Task AddProfessorAttendance(int sessionId, int professorId);
        public Task AddStudentAttendance(int sessionId, int studentId);


    }
}

