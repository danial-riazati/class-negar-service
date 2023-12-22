using System;
using System.Net.NetworkInformation;
using System.Text;
using ClassNegarService.Db;
using ClassNegarService.Models.Enums;
using ClassNegarService.Models.Session;
using ClassNegarService.Repos;
using ClassNegarService.Repos.Notification;
using ClassNegarService.Repos.Session;
using ClassNegarService.Services.WebSocket;
using ClassNegarService.Utils;
using Newtonsoft.Json;

namespace ClassNegarService.Services.Session
{
    public class SessionService : ISessionService
    {
        private readonly IConfiguration _configuration;
        private readonly ISessionRepo _sessionRepo;
        private readonly IClassRepo _classRepo;
        private readonly WebSocketService _webSocketService;

        public SessionService(
            IConfiguration configuration,
            ISessionRepo sessionRepo,
            IClassRepo classRepo,
            WebSocketService webSocketService
            )
        {
            _configuration = configuration;
            _sessionRepo = sessionRepo;
            _classRepo = classRepo;
            _webSocketService = webSocketService;
        }

        public async Task<SessionQrResultModel> GetSessionQr(int classId, int userId, int userRole, bool isLogin)
        {

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();
                var isRemoved = await _classRepo.IsRemovedFromClass(userId, classId) ?? throw new UnauthorizedAccessException();
                if (isRemoved == true) throw new UnauthorizedAccessException();
            }
            //if (isLogin && await _sessionRepo.IsAlreadyLoggedIn(classId, userId))
            //{
            //    throw new Exception("you are");
            //}
            //else
            //{
            //    await _sessionRepo.IsAlreadyLoggedOut(classId, userId);
            //}

            var classTimes = await _classRepo.GetClassTimes(classId);
            bool isValidTime = false;
            var now = TimeOnly.FromDateTime(DateTime.Now);
            classTimes.ForEach(c =>
            {
                if (c.DayOfWeek == (int)DateTime.Now.DayOfWeek)
                {
                    var startAt = TimeOnly.FromDateTime(StringUtils.ConvertTimeStrigToDateTime(c.StartAt));
                    var endAt = TimeOnly.FromDateTime(StringUtils.ConvertTimeStrigToDateTime(c.EndAt));
                    if (now > startAt && now < endAt)
                        isValidTime = true;
                }

            });
            if (!isValidTime) throw new UnauthorizedAccessException();
            var expDate = DateTime.Now.AddSeconds(20);
            var model = new SessionDataModel
            {
                ClassId = classId,
                UserId = userId,
                UserRole = userRole,
                ExpireAt = expDate,
                IsLogin = isLogin,
            };
            var qrcode = JsonConvert.SerializeObject(model);
            var encrypted = EncryptionUtils.EncryptString(qrcode, _configuration["AESKEY"]);
            if (string.IsNullOrEmpty(encrypted)) throw new InvalidDataException();
            var res = new SessionQrResultModel { QrData = encrypted };

            return res;
        }

        public async Task JoinSession(string qrcode)
        {
            var decrypted = "";
            SessionDataModel model;
            try
            {
                decrypted = EncryptionUtils.DecryptString(qrcode, _configuration["AESKEY"]);
                model = JsonConvert.DeserializeObject<SessionDataModel>(decrypted) ?? throw new UnauthorizedAccessException();
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException();
            }
            if (model.UserRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(model.UserId, model.ClassId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            else if (model.UserRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(model.UserId, model.ClassId);
                if (hasAccess == false) throw new UnauthorizedAccessException();
                var isRemoved = await _classRepo.IsRemovedFromClass(model.UserId, model.ClassId) ?? throw new UnauthorizedAccessException();
                if (isRemoved == true) throw new UnauthorizedAccessException();
            }
            var currentSessionId = await _sessionRepo.FindSessionForNow(model.ClassId)
                                ?? await _sessionRepo.CreateSession(model.ClassId)
                                ?? throw new InvalidDataException();

            if (model.UserRole == (int)RoleEnum.professor)
            {
                await _sessionRepo.AddProfessorAttendance(currentSessionId, model.UserId);


            }
            else if (model.UserRole == (int)RoleEnum.student)
            {
                await _sessionRepo.AddStudentAttendance(currentSessionId, model.UserId);

            }

            await _webSocketService.SendToSocketAsync(" ", model.UserId);

        }
    }
}

