﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassNegarService.Db
{
    public class NotificationDislike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public bool IsRemoved { get; set; }

    }
}

