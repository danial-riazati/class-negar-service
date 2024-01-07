using System;
using ClassNegarService.Models.Notification;

namespace ClassNegarService.Repos.Notification
{
    public interface INotificationRepo
    {
        public Task AddNotification(string title, int classId, DateTime publishedAt, string description, int likesCount, int dislikesCount);
        public Task<int> GetNotificationClassId(int notificationId);
        public Task<List<string>> GetNotificationLikes(int notificationId);
        public Task<List<string>> GetNotificationDislikes(int notificationId);
        public Task AddLike(int notificationId, int userId);
        public Task AddDislike(int notificationId, int userId);
        public Task RemoveLike(int notificationId, int userId);
        public Task RemoveDislike(int notificationId, int userId);
        public Task UpdateIfHasLike(int notificationId, int userId);
        public Task UpdateIfHasDislike(int notificationId, int userId);
        public Task AddComment(int userId, int notificationId, string description, DateTime publishedAt);
        public Task<NotificationResultModel?> GetNotification(int notificationId);
        public Task<List<CommentResultModel>> GetNotificationComments(int notificationId);
        public Task<int> CheckUserLikeStatus(int notificationId, int userId);
        public Task<List<AllNotificationsResultModel>> GetAllNotifications(int classId);
        public Task<List<AllNotificationsResultModel>> GetAllStudentNotifications(int userId);
        public Task<List<AllNotificationsResultModel>> GetAllProfessorNotifications(int userId);


    }
}
