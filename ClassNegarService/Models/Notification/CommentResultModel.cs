using System;
namespace ClassNegarService.Models.Notification
{
    public class CommentResultModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NotificationId { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}


