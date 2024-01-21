using System;
using ClassNegarService.Db;
using ClassNegarService.Models.Notification;

namespace ClassNegarService.Repos.Notification
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly ClassNegarDbContext _dbcontext;

        public NotificationRepo(ClassNegarDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddComment(int userId, int notificationId, string description, DateTime publishedAt)
        {
            var comment = new NotificationComment
            {
                NotificationId = notificationId,
                Description = description,
                PublishedAt = publishedAt,
                UserId = userId
            };
            await _dbcontext.AddAsync(comment);
            await _dbcontext.SaveChangesAsync();
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

        public async Task<int> CheckUserLikeStatus(int notificationId, int userId)
        {
            var isLiked = (from l in _dbcontext.NotificationLikes
                           where l.UserId == userId && l.NotificationId == notificationId && !l.IsRemoved
                           select l).FirstOrDefault();
            if (isLiked != null) return 1;
            var isDisliked = (from dl in _dbcontext.NotificationDislikes
                              where dl.UserId == userId && dl.NotificationId == notificationId && !dl.IsRemoved
                              select dl).FirstOrDefault();
            if (isDisliked != null) return -1;
            return 0;
        }

        public async Task<List<AllNotificationsResultModel>> GetAllNotifications(int classId)
        {
            var result = (from n in _dbcontext.ClassNotifications
                          join c in _dbcontext.Classes
                          on n.ClassId equals c.Id
                          select new AllNotificationsResultModel
                          {
                              ClassName = c.Name,
                              Id = n.Id,
                              Description = n.Description,
                              PublishedAt = n.PublishedAt,
                              Title = n.Title

                          }).ToList();
            return result;
        }

        public async Task<List<AllNotificationsResultModel>> GetAllProfessorNotifications(int userId)
        {
            var result = (from n in _dbcontext.ClassNotifications
                          join c in _dbcontext.Classes
                          on n.ClassId equals c.Id
                          where c.ProfessorId == userId
                          select new AllNotificationsResultModel
                          {
                              ClassName = c.Name,
                              Id = n.Id,
                              Description = n.Description,
                              PublishedAt = n.PublishedAt,
                              Title = n.Title

                          }).ToList();
            return result;
        }

        public async Task<List<AllNotificationsResultModel>> GetAllStudentNotifications(int userId)
        {
            var result = (from n in _dbcontext.ClassNotifications
                          join en in _dbcontext.Enrollments
                          on n.ClassId equals en.ClassId
                          join c in _dbcontext.Classes
                          on n.ClassId equals c.Id
                          where en.StudentId == userId
                          select new AllNotificationsResultModel
                          {
                              ClassName = c.Name,
                              Id = n.Id,
                              Description = n.Description,
                              PublishedAt = n.PublishedAt,
                              Title = n.Title

                          }).ToList();
            return result;
        }

        public async Task<List<AllNotificationsResultModel>> GetClassNotifications(int classId)
        {
            var result = (from n in _dbcontext.ClassNotifications
                          join c in _dbcontext.Classes
                          on n.ClassId equals c.Id
                          where n.ClassId == classId
                          select new AllNotificationsResultModel
                          {
                              ClassName = c.Name,
                              Id = n.Id,
                              Description = n.Description,
                              PublishedAt = n.PublishedAt,
                              Title = n.Title
                          }).ToList();

            return result;
        }

        public async Task<NotificationResultModel?> GetNotification(int notificationId)
        {
            var result = (from n in _dbcontext.ClassNotifications
                          join c in _dbcontext.Classes
                          on n.ClassId equals c.Id
                          where n.Id == notificationId
                          select new NotificationResultModel
                          {
                              ClassName = c.Name,
                              Id = n.Id,
                              Description = n.Description,
                              DislikesCount = n.DislikesCount,
                              LikesCount = n.LikesCount,
                              PublishedAt = n.PublishedAt,
                              Title = n.Title,

                          }).FirstOrDefault();
            return result;
        }

        public async Task<int> GetNotificationClassId(int notificationId)
        {
            var classId = (from n in _dbcontext.ClassNotifications
                           where n.Id == notificationId
                           select n.ClassId).FirstOrDefault();

            return classId;
        }

        public async Task<List<CommentResultModel>> GetNotificationComments(int notificationId)
        {
            var result = (from c in _dbcontext.NotificationComments
                          join u in _dbcontext.Users
                          on c.UserId equals u.Id
                          where c.NotificationId == notificationId
                          orderby c.PublishedAt
                          select new CommentResultModel
                          {
                              Id = c.Id,
                              NotificationId = notificationId,
                              Description = c.Description,
                              Name = u.FirstName + " " + u.LastName,
                              PublishedAt = c.PublishedAt,
                          }).ToList();

            return result;
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
                          where d.NotificationId == notificationId && d.UserId == userId && !d.IsRemoved
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
                          where d.NotificationId == notificationId && d.UserId == userId && !d.IsRemoved
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

