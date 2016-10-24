using SocialMediaCampus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialMediaCampus.Class
{
    public class Comments
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TextComments { get; set; }
        public DateTime CommDate { get; set; }
        public virtual SharedModel CommSharedModels { get; set; }
        public virtual SiteUsers CommSiteUsers { get; set; }
    }
}