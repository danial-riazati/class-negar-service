using System;
using ClassNegarService.Db;

namespace ClassNegarService.Repos.Notification
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly ClassNegarDbContext _dbcontext;

        public NotificationRepo(ClassNegarDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddDislike(int notificationId, int userId)
        {
            var exist = (from l in _dbcontext.NotificationDislikes
                         where l.NotificationId == notificationId && l.UserId == userId
                         select l).FirstOrDefault();

            if (exist != null)
            {
                if (exist.IsRemoved)
                {
                    exist.IsRemoved = false;
                    _dbcontext.Update(exist);
                    var notification = (from d in _dbcontext.ClassNotifications
                                        where d.Id == notificationId
                                        select d).FirstOrDefault();

                    if (notification == null) return;

                    notification.DislikesCount += 1;
                    _dbcontext.Update(notification);
                    await _dbcontext.SaveChangesAsync();
                    return;
                }
                else
                    throw new Exception("you are already disliked");
            }
            else
            {
                var newDislike = new NotificationDislike
                {
                    UserId = userId,
                    NotificationId = notificationId,
                    IsRemoved = false
                };
                await _dbcontext.AddAsync(newDislike);
                var notification = (from d in _dbcontext.ClassNotifications
                                    where d.Id == notificationId
                                    select d).FirstOrDefault();

                if (notification == null) return;

                notification.DislikesCount += 1;
                _dbcontext.Update(notification);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task AddLike(int notificationId, int userId)
        {
            var exist = (from l in _dbcontext.NotificationLikes
                         where l.NotificationId == notificationId && l.UserId == userId
                         select l).FirstOrDefault();

            if (exist != null)
            {
                if (exist.IsRemoved)
                {
                    exist.IsRemoved = false;
                    _dbcontext.Update(exist);
                    var notification = (from d in _dbcontext.ClassNotifications
                                        where d.Id == notificationId
                                        select d).FirstOrDefault();

                    if (notification == null) return;

                    notification.LikesCount += 1;
                    _dbcontext.Update(notification);
                    await _dbcontext.SaveChangesAsync();
                    return;
                }
                else
                    throw new Exception("you are already liked");

            }
            else
            {
                var newLike = new NotificationLike
                {
                    UserId = userId,
                    NotificationId = notificationId,
                    IsRemoved = false
                };
                await _dbcontext.AddAsync(newLike);
                var notification = (from d in _dbcontext.ClassNotifications
                                    where d.Id == notificationId
                                    select d).FirstOrDefault();

                if (notification == null) return;

                notification.LikesCount += 1;
                _dbcontext.Update(notification);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task AddNotification(string title, int classId, DateTime publishedAt, string description, int likesCount, int dislikesCount)
        {
            var notificationModel = new ClassNotification
            {
                ClassId = classId,
                Title = title,
                Description = description,
                LikesCount = likesCount,
                DislikesCount = dislikesCount,
                PublishedAt = publishedAt
            };
            await _dbcontext.AddAsync(notificationModel);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<int> GetNotificationClassId(int notificationId)
        {
            var classId = (from n in _dbcontext.ClassNotifications
                           where n.Id == notificationId
                           select n.ClassId).FirstOrDefault();

            return classId;
        }

        public async Task<List<string>> GetNotificationDislikes(int notificationId)
        {
            var result = (from d in _dbcontext.NotificationDislikes
                          join u in _dbcontext.Users
                          on d.UserId equals u.Id
                          where d.NotificationId == notificationId && !d.IsRemoved
                          select u.FirstName + " " + u.LastName).ToList();

            return result;
        }

        public async Task<List<string>> GetNotificationLikes(int notificationId)
        {
            var result = (from d in _dbcontext.NotificationLikes
                          join u in _dbcontext.Users
                          on d.UserId equals u.Id
                          where d.NotificationId == notificationId && !d.IsRemoved
                          select u.FirstName + " " + u.LastName).ToList();

            return result;
        }

        public async Task RemoveDislike(int notificationId, int userId)
        {
            var exist = (from l in _dbcontext.NotificationDislikes
                         where l.NotificationId == notificationId && l.UserId == userId
                         select l).FirstOrDefault();

            if (exist != null && !exist.IsRemoved)
            {

                exist.IsRemoved = true;
                _dbcontext.Update(exist);
                var notification = (from d in _dbcontext.ClassNotifications
                                    where d.Id == notificationId
                                    select d).FirstOrDefault();

                if (notification == null) return;

                notification.DislikesCount -= 1;
                _dbcontext.Update(notification);
                await _dbcontext.SaveChangesAsync();
                return;

            }
            throw new Exception("there is no dislike founded");

        }

        public async Task RemoveLike(int notificationId, int userId)
        {
            var exist = (from l in _dbcontext.NotificationLikes
                         where l.NotificationId == notificationId && l.UserId == userId
                         select l).FirstOrDefault();

            if (exist != null && !exist.IsRemoved)
            {

                exist.IsRemoved = true;
                _dbcontext.Update(exist);
                var notification = (from d in _dbcontext.ClassNotifications
                                    where d.Id == notificationId
                                    select d).FirstOrDefault();

                if (notification == null) return;

                notification.LikesCount -= 1;
                _dbcontext.Update(notification);
                await _dbcontext.SaveChangesAsync();
                return;

            }
            throw new Exception("there is no like founded");

        }

        public async Task UpdateIfHasDislike(int notificationId, int userId)
        {
            var result = (from d in _dbcontext.NotificationDislikes
                          where d.NotificationId == notificationId && d.UserId == userId
                          select d).FirstOrDefault();

            if (result == null) return;

            result.IsRemoved = true;

            _dbcontext.Update(result);

            var notification = (from d in _dbcontext.ClassNotifications
                                where d.Id == notificationId
                                select d).FirstOrDefault();

            if (notification == null) return;

            notification.DislikesCount -= 1;
            _dbcontext.Update(notification);

            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateIfHasLike(int notificationId, int userId)
        {
            var result = (from d in _dbcontext.NotificationLikes
                          where d.NotificationId == notificationId && d.UserId == userId
                          select d).FirstOrDefault();

            if (result == null) return;

            result.IsRemoved = true;

            _dbcontext.Update(result);

            var notification = (from d in _dbcontext.ClassNotifications
                                where d.Id == notificationId
                                select d).FirstOrDefault();

            if (notification == null) return;

            notification.LikesCount -= 1;
            _dbcontext.Update(notification);

            await _dbcontext.SaveChangesAsync();
        }
    }
}

