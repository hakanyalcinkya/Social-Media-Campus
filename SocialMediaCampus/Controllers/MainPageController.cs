﻿using SocialMediaCampus.HelperViewModel;
using SocialMediaCampus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaCampus.Controllers
{
    public class MainPageController : Controller
    {
        private SocialMediaDBEntities db = new SocialMediaDBEntities();
        // GET: MainPage
        public ActionResult Shares()
        {
            SharedViewModel model = new SharedViewModel();
            model.UserList = db.Users.ToList();
            model.SharedList = db.ShareModels.ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult SaveSingleFile()
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
                    var text = System.Web.HttpContext.Current.Request.Form["HelpString"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);

                    var extension = Path.GetExtension(filebase.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                    {
                        // var fileName = Path.GetFileName(filebase.FileName);
                        string path = Guid.NewGuid() + "_" + Path.GetExtension(filebase.FileName);
                        filebase.SaveAs(Server.MapPath("~/UploadFile/image/" + path));


                        SiteUsers user = Session["Ogrenci"] as SiteUsers;

                        SiteUsers user1 = db.Users.Find(user.Id);
                        SharedModel model = new SharedModel();
                        model.Text = text;
                        model.Type = "image";
                        model.Path = path;
                        model.Users = user1;
                        model.SharedDate = DateTime.Now;

                        db.ShareModels.Add(model);
                        db.SaveChanges();

                        return Json("Resim başarılı bir şekilde kaydedildi...");

                    }
                    else
                    {
                        return Json("Resim Kaydedilmedi...");
                    }


                    return Json("");

                }
                else { return Json("Resim kaydedilmedi..."); }
            }
            catch (Exception ex) { return Json("Error While Saving."); }
        }
        [HttpPost]
        public ActionResult SaveMultiFile()
        {
            List<string> fileImage = new List<string>();
            var text = System.Web.HttpContext.Current.Request.Form["HelpString"];
            if (Request.Files.Count > 0)
            {
                var pic = System.Web.HttpContext.Current.Request.Files["multiImage"];
                HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                var extension = Path.GetExtension(filebase.FileName).ToLower();
                Guid uniq = Guid.NewGuid();
                if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {
                    foreach (var file in Request.Files)
                    {
                        string path = Guid.NewGuid() + "_" + Path.GetExtension(filebase.FileName);
                        filebase.SaveAs(Server.MapPath("~/UploadFile/images/" + path));
                        SiteUsers user = Session["Ogrenci"] as SiteUsers;

                        SiteUsers user1 = db.Users.Find(user.Id);
                        SharedModel model = new SharedModel();
                        model.Text = text;
                        model.Type = "images";
                        model.Path = path;
                        model.Users = user1;
                        model.SharedDate = DateTime.Now;
                        model.Uniq = uniq;
                        db.ShareModels.Add(model);
                        db.SaveChanges();
                        //  return Json("Resimler başarılı bir şekilde kaydedildi...",JsonRequestBehavior.AllowGet);
                    }
                    
                }
                else
                {
                    return Json("Resimler Kaydedilmedi...");
                }
            }


            return View();
        }
        public ActionResult Profil()
        {
            return View();
        }
    }
}