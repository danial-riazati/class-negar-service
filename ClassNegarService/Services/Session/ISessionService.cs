using System;
using ClassNegarService.Models.Notification;
using ClassNegarService.Models.Session;

namespace ClassNegarService.Services.Session
{
    public interface ISessionService
    {
        public Task<SessionQrResultModel> GetSessionQr(int classId, int userId, int userRole, bool isLogin);
        public Task JoinSession(string qrcode);


    }
}

