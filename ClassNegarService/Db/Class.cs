using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassNegarService.Db
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Semester { get; set; }
        public int ProfessorId { get; set; }
        public int MaxSize { get; set; }
        public string Password { get; set; }
        public int CurrentSize { get; set; }
        public string ClassLocation { get; set; }
        public bool IsAttendingNow { get; set; }

    }
}

