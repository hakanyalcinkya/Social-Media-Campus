﻿using SocialMediaCampus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaCampus.HelperViewModel
{
    public class SharedViewModel
    {
        public List<SiteUsers> UserList { get; set; }
        public List<SharedModel> SharedList { get; set; }
    }
}