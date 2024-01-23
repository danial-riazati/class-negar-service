using System;
using ClassNegarService.Models.Notification;
using ClassNegarService.Models.Session;

namespace ClassNegarService.Services.Session
{
    public interface ISessionService
    {
        public Task<SessionQrResultModel> GetSessionQr(int classId, int userId, int userRole, bool isLogin);
        public Task JoinSession(string qrcode);
        public Task<List<SessionClass>> GetSessionClass(int userId, int userRole);
        public Task JoinRfid(int classid, string rfid);


    }
}

