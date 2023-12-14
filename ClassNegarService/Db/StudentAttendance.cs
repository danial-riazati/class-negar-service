using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassNegarService.Db
{
    public class StudentAttendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public DateTime? JoinedAt { get; set; }
        public DateTime? QuitedAt { get; set; }

    }
}

