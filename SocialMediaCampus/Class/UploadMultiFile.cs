using SocialMediaCampus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaCampus.Class
{
    public class UploadMultiFile
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public int SharedModelId { get; set; }
        public virtual SharedModel SharedModels { get; set; }
    }
}