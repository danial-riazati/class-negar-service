using System;
using ClassNegarService.Models.Enums;
using ClassNegarService.Models.Notification;
using ClassNegarService.Repos;
using ClassNegarService.Repos.Notification;

namespace ClassNegarService.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly IConfiguration _configuration;
        private readonly IClassRepo _classRepo;

        public NotificationService(
            INotificationRepo notificationRepo,
            IClassRepo classRepo,
            IConfiguration configuration
            )
        {
            _notificationRepo = notificationRepo;
            _configuration = configuration;
            _classRepo = classRepo;
        }

        public async Task AddComment(AddCommentModel model, int userId, int userRole)
        {
            var classId = await _notificationRepo.GetNotificationClassId(model.NotificationId);
            if (classId == 0) throw new UnauthorizedAccessException();

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }

            await _notificationRepo.AddComment(userId, model.NotificationId, model.Description, DateTime.Now);
        }

        public async Task AddDislike(int userId, int userRole, int notificationId)
        {
            var classId = await _notificationRepo.GetNotificationClassId(notificationId);
            if (classId == 0) throw new UnauthorizedAccessException();

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }

            await _notificationRepo.UpdateIfHasLike(notificationId, userId);
            await _notificationRepo.AddDislike(notificationId, userId);
        }

        public async Task AddLike(int userId, int userRole, int notificationId)
        {
            var classId = await _notificationRepo.GetNotificationClassId(notificationId);
            if (classId == 0) throw new UnauthorizedAccessException();

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            await _notificationRepo.UpdateIfHasDislike(notificationId, userId);
            await _notificationRepo.AddLike(notificationId, userId);
        }

        public async Task AddNotification(AddNotificationModel model, int professorId)
        {
            var hasAccess = await _classRepo.HasProfessorAccess(professorId, model.ClassId);
            if (hasAccess == false) throw new UnauthorizedAccessException();


            await _notificationRepo.AddNotification(model.Title, model.ClassId, DateTime.Now, model.Description, 0, 0);

            //await callFirebaseNotificationService
        }

        public async Task<List<AllNotificationsResultModel>> GetAllNotifications(int userId, int userRole, int classId)
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
            }
            var result = await _notificationRepo.GetAllNotifications(classId);
            return result;
        }

        public async Task<NotificationResultModel> GetNotification(int userId, int userRole, int notificationId)
        {
            var classId = await _notificationRepo.GetNotificationClassId(notificationId);
            if (classId == 0) throw new UnauthorizedAccessException();

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            var notification = await _notificationRepo.GetNotification(notificationId);
            if (notification == null) throw new InvalidDataException();
            notification.Comments = await _notificationRepo.GetNotificationComments(notificationId);
            notification.MyLikeStatus = await _notificationRepo.CheckUserLikeStatus(notificationId, userId);
            return notification;

        }

        public async Task<List<string>> GetNotificationDislikes(int userId, int userRole, int notificationId)
        {
            var classId = await _notificationRepo.GetNotificationClassId(notificationId);
            if (classId == 0) throw new UnauthorizedAccessException();

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();



            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            var result = await _notificationRepo.GetNotificationDislikes(notificationId);
            return result;


        }

        public async Task<List<string>> GetNotificationLikes(int userId, int userRole, int notificationId)
        {
            var classId = await _notificationRepo.GetNotificationClassId(notificationId);
            if (classId == 0) throw new UnauthorizedAccessException();

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();



            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            var result = await _notificationRepo.GetNotificationLikes(notificationId);
            return result;
        }

        public async Task RemoveDislike(int userId, int userRole, int notificationId)
        {
            var classId = await _notificationRepo.GetNotificationClassId(notificationId);
            if (classId == 0) throw new UnauthorizedAccessException();

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            await _notificationRepo.RemoveDislike(notificationId, userId);
        }

        public async Task RemoveLike(int userId, int userRole, int notificationId)
        {
            var classId = await _notificationRepo.GetNotificationClassId(notificationId);
            if (classId == 0) throw new UnauthorizedAccessException();

            if (userRole == (int)RoleEnum.professor)
            {
                var hasAccess = await _classRepo.HasProfessorAccess(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            else if (userRole == (int)RoleEnum.student)
            {
                var hasAccess = await _classRepo.HasEnrolled(userId, classId);
                if (hasAccess == false) throw new UnauthorizedAccessException();

            }
            await _notificationRepo.RemoveLike(notificationId, userId);
        }
    }
}

