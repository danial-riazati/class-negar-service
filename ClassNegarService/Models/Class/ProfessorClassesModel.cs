using System;
namespace ClassNegarService.Models.Class
{
    public class ProfessorClassesModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Passwrod { get; set; }
        public string Name { get; set; }
        public DateTime Semester { get; set; }
        public string ProfessorName { get; set; }
        public int CurrentSize { get; set; }
        public int MaxSize { get; set; }
        public string ClassLocation { get; set; }
        public bool IsAttendingNow { get; set; }
    }
}


