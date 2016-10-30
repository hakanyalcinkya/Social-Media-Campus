using SocialMediaCampus.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialMediaCampus.Class
{
    [Table("User")]
    public class SiteUsers
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Ad"),StringLength(20),Required]
        public string FirstName { get; set; }
        [DisplayName("Soyad"), StringLength(20),Required]
        public string LastName { get; set; }
        [StringLength(10),Required,DisplayName("Şifre")]
        public string Number { get; set; }
        [StringLength(80),Required]
        public string EMail { get; set; }
        [StringLength(20),Required,DisplayName("Şifre")]
        public string Password { get; set; }
        [StringLength(150)]
        public string Resimulr { get; set; }
        [ScaffoldColumn(false)]
        public DateTime LastAcces { get; set; }
        [DisplayName("Rol Tipi"),StringLength(20)]
        public string Permisson { get; set; }

        public virtual List<SharedModel> SheredModels { get; set; }
        public virtual List<Comments> Comments{ get; set; }
    }
}