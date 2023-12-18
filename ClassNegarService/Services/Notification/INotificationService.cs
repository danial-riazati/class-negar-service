using System;
using ClassNegarService.Models.Notification;

namespace ClassNegarService.Services.Notification
{
    public interface INotificationService
    {
        public Task AddNotification(AddNotificationModel model, int professorId);
        public Task<List<string>> GetNotificationLikes(int userId, int userRole, int notificationId);
        public Task<List<string>> GetNotificationDislikes(int userId, int userRole, int notificationId);
        public Task AddLike(int userId, int userRole, int notificationId);
        public Task AddDislike(int userId, int userRole, int notificationId);
        public Task RemoveLike(int userId, int userRole, int notificationId);
        public Task RemoveDislike(int userId, int userRole, int notificationId);

    }
}

