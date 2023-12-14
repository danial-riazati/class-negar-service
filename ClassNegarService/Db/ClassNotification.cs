using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassNegarService.Db
{
    public class ClassNotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public int ClassId { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Description { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }

    }
}

