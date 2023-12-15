using System;
namespace ClassNegarService.Models.Class
{
    public class AddClassModel
    {
        public string Name { get; set; }
        public DateTime Semester { get; set; }
        public int MaxSize { get; set; }
        public string ClassLocation { get; set; }
        public List<AddClassTimeModel> Times { get; set; }
    }
}


