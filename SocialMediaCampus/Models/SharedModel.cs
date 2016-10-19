using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialMediaCampus.Models
{
    [Table("UploadFile")]
    public class SharedModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Title")]
        public string Text { get; set; }
        [DisplayName("File Type")]
        public string Type { get; set; }
        [DisplayName("File Path")]
        public string Path { get; set; }
        [DisplayName("File Share Date")]
        public DateTime SharedDate { get; set; }
        public Guid Uniq { get; set; }
        public  int UserId { get; set; }
        public SiteUsers Users { get; set; }
    }
}