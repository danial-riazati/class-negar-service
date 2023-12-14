using System;
namespace ClassNegarService.Db
{
    public class Class
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Semester { get; set; }
        public int ProfessorId { get; set; }
        public int MaxSize { get; set; }
        public string Password { get; set; }


    }
}

