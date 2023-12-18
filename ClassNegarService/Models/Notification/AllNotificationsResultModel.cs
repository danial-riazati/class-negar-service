using System;
namespace ClassNegarService.Models.Notification
{
    public class AllNotificationsResultModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ClassName { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; }
    }
}

