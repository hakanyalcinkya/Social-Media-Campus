using SocialMediaCampus.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SocialMediaCampus.Models
{
    public class SocialMediaDBEntities : DbContext
    {
        public DbSet<SiteUsers> Users { get; set; }
        public DbSet<RoleType> Types { get; set; }
        public DbSet<SharedModel> ShareModels { get; set; }
        public DbSet<UploadMultiFile> UploadMultiFiles { get; set; }
        public DbSet<NumberStudent> numberStudents { get; set; }
        public DbSet<Comments> Comments { get; set; }
    }
}