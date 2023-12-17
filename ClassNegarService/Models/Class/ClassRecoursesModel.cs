using System;
namespace ClassNegarService.Models.Class
{
    public class ClassRecoursesModel
    {
        public int Id { get; set; }
        public string DownloadLink { get; set; }
        public DateTime InsertedAt { get; set; }
        public string Format { get; set; }
        public int Size { get; set; }
    }
}

