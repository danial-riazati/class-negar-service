using System;
using System.ComponentModel.DataAnnotations;

namespace ClassNegarService.Models.Notification
{
    public class AddCommentModel
    {
        [Required]
        public int NotificationId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}

