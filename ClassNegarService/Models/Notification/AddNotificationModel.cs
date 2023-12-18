using System;
using System.ComponentModel.DataAnnotations;

namespace ClassNegarService.Models.Notification
{
    public class AddNotificationModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int ClassId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}


