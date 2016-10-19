using SocialMediaCampus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaCampus.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            SocialMediaDBEntities db = new SocialMediaDBEntities();

            return View(db.Users.ToList());
        }
    }
}