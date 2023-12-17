using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassNegarService.Db
{
    public class ClassResourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClassId { get; set; }
        public string DownloadLink { get; set; }
        public DateTime InsertedAt { get; set; }
        public string Format { get; set; }
        public int Size { get; set; }

    }
}

