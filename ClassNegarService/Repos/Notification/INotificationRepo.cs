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


    }
}
