using SocialMediaCampus.Class;
using SocialMediaCampus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            model.UploadMultiList = db.UploadMultiFiles.ToList();
            model.CommentList = db.Comments.ToList();
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
                    else if (extension == ".doc" || extension == ".docx" || extension == ".pdf")
                    {
                        // var fileName = Path.GetFileName(filebase.FileName);
                        string path = Guid.NewGuid() + "_" + Path.GetExtension(filebase.FileName);
                        filebase.SaveAs(Server.MapPath("~/UploadFile/file/" + path));

                        SiteUsers user = Session["Ogrenci"] as SiteUsers;

                        SiteUsers user1 = db.Users.Find(user.Id);
                        SharedModel model = new SharedModel();
                        model.Text = text;
                        model.Type = "file";
                        model.Path = path;
                        model.Users = user1;
                        model.SharedDate = DateTime.Now;

                        db.ShareModels.Add(model);
                        db.SaveChanges();

                        return Json("Dosya başarılı bir şekilde kaydedildi...");

                    }
                    else
                    {
                        return Json("Dosya  Kaydedilmedi...");
                    }

                }
                else { return Json("Resim kaydedilmedi..."); }
            }
            catch (Exception ex) { return Json("Error While Saving."); }
        }

        [HttpPost]
        public ActionResult SaveMultiImage()
        {
            var text = System.Web.HttpContext.Current.Request.Form["HelpString"];
            MsgJsonResult result = new MsgJsonResult();
            if (Request.Files.Count > 0)
            {
                var allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
                //var allowedExtensionsfile = new[] { ".doc", ".docx", ".pdf" };
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (allowedExtensions.Contains(Path.GetExtension(file.FileName)))
                    {
                        result.HasError = true;
                    }
                    else
                    {
                        result.HasError = false;
                        result.Message = "Lütfen .jpg .jpeg .png uzantılı dosya seçiniz....";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                SiteUsers user = Session["Ogrenci"] as SiteUsers;
                UploadMultiFile upload = new UploadMultiFile();
                SiteUsers user1 = db.Users.Find(user.Id);
                SharedModel model = new SharedModel();
                model.Text = text;
                model.Type = "images";
                model.Users = user1;
                model.SharedDate = DateTime.Now;
                db.ShareModels.Add(model);
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (result.HasError == true)
                    {
                        string path = Guid.NewGuid() + "-" + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/UploadFile/images/" + path));
                        upload.SharedModelId = model.Id;
                        upload.FilePath = path;
                        db.UploadMultiFiles.Add(upload);
                        db.SaveChanges();
                    }
                }
                db.SaveChanges();
                result.HasError = true;
                result.Message = "Resimler başarılı bir şekilde kaydedildi...";

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result.HasError = false;
                result.Message = "Lütfen bir resim seçiniz...";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult TextPost(string txt)
        {
            if (txt != "")
            {
                SiteUsers user = Session["Ogrenci"] as SiteUsers;

                SiteUsers user1 = db.Users.Find(user.Id);
                SharedModel model = new SharedModel();
                model.Text = txt;
                model.Type = "text";
                model.Users = user1;
                model.SharedDate = DateTime.Now;
                db.ShareModels.Add(model);
                db.SaveChanges();

                return RedirectToAction("Shares", "MainPage");

            }
            else
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult VideoPost(string txt, string link)
        {
            MsgJsonResult result = new MsgJsonResult();
            string linkQ = link.Trim();
            string[] t = new string[2];
            string[] words = linkQ.Split('=');
            for (int i = 0; i < words.Length; i++)
            {
                t[i] = words[i];
            }
            int linkCount = t[1].Length;
            if (t[0] == "https://www.youtube.com/watch?v" && linkCount == 11)
            {


                SiteUsers user = Session["Ogrenci"] as SiteUsers;

                SiteUsers user1 = db.Users.Find(user.Id);
                SharedModel model = new SharedModel();
                model.Text = txt;
                model.Type = "video";
                model.Users = user1;
                model.Path = "https://www.youtube.com/embed/" + t[1];
                model.SharedDate = DateTime.Now;
                db.ShareModels.Add(model);
                db.SaveChanges();

                result.HasError = true;
                result.Message = "Video kaydedildi...";
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                result.HasError = false;
                result.Message = "Lütfen youtube linkini  giriniz...";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult PostComments(string txt, int id)
        {
            SiteUsers user = Session["Ogrenci"] as SiteUsers;
            SharedModel shared = db.ShareModels.Find(id);
            SiteUsers user1 = db.Users.Find(user.Id);
            Comments model = new Comments();

            model.CommDate = DateTime.Now;
            model.CommSiteUsers = user1;
            model.CommSharedModels = shared;
            model.TextComments = txt;

            db.Comments.Add(model);
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        public ActionResult GetComments(int id)
        {
            List<Comments> comm = db.Comments.Where(x => x.CommSharedModels.Id == id).OrderByDescending(x => x.CommDate).ToList();
            List<AllComments> allCom = new List<AllComments>();
            foreach (Comments item in comm)
            {
                AllComments comments = new AllComments();
                comments.Id = item.Id;
                comments.CommDate = item.CommDate.ToString();
                comments.CommImageUrl = item.CommSiteUsers.Resimulr;
                comments.CommText = item.TextComments;
                comments.CommName = item.CommSiteUsers.FirstName + " " + item.CommSiteUsers.LastName[0];
                allCom.Add(comments);
            }

            return Json(allCom, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Profil()
        {
            SiteUsers user1 = null;
            if (Session["Ogrenci"] != null)
            {
                SiteUsers user = null;
                user = Session["Ogrenci"] as SiteUsers;
                user1 = db.Users.Find(user.Id);
            }
            else
            {
                RedirectToAction("Login", "SiteUsers");
            }
            return View(user1);
        }
        [HttpPost]
        public ActionResult ProfilEdit(string txtFirstName, string txtLastName, string txtEMail, string txtNumber, string txtPassword, int id)
        {
            SiteUsers user = db.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                user.FirstName = txtFirstName;
                user.LastName = txtLastName;
                user.EMail = txtEMail;
                user.Number = txtNumber;
                user.Password = txtPassword;

                db.SaveChanges();

                if (Session["Ogrenci"] != null)
                {
                    Session["Ogrenci"] = user;
                }

            }

            return Json("Profiliniz güncellenmiştir.", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadProfilImage()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase filebase = Request.Files[0];
                    var extension = Path.GetExtension(filebase.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                    {
                        SiteUsers user = Session["Ogrenci"] as SiteUsers;
                        SiteUsers user1 = db.Users.Find(user.Id);
                        string fullPath = Request.MapPath("~/Uploadfile/profilImage/" + user1.Resimulr);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        string path = Guid.NewGuid() + "_" + Path.GetExtension(filebase.FileName);
                        filebase.SaveAs(Server.MapPath("~/UploadFile/profilImage/" + path));
                        user1.Resimulr = path;
                        db.SaveChanges();
                        Session["Ogrenci"] = db.Users.Find(user.Id);
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Dosya  Kaydedilmedi...");
                    }

                }
                else { return Json("Resim kaydedilmedi..."); }
            }
            catch (Exception ex)
            {
                return Json("Error While Saving.");
            }
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login", "SiteUsers");
        }
        [HttpPost]
        public ActionResult DeleteComment(int id)
        {
            string Id = id.ToString();
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comm = db.Comments.Find(id);
            if (comm == null)
            {
                return HttpNotFound();
            }
            db.Comments.Remove(comm);
            db.SaveChanges();
            return Json("",JsonRequestBehavior.AllowGet);
        }
    }
}