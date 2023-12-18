using System;
namespace ClassNegarService.Models.Notification
{
    public class NotificationResultModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ClassId { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public int MyLikeStatus { get; set; }
        public List<CommentResultModel> Comments { get; set; }
    }
}


