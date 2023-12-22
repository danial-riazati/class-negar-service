using System;
namespace ClassNegarService.Models.Session
{
    public class SessionDataModel
    {
        public DateTime ExpireAt { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public int UserRole { get; set; }
        public bool IsLogin { get; set; }
    }
}

