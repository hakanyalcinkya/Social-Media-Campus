using SocialMediaCampus.Class;
using SocialMediaCampus.HelperViewModel;
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
            if (Request.Files.Count > 0)
            {
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
                    var extension = Path.GetExtension(file.FileName);

                    if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                    {
                        string path = Guid.NewGuid() + "-" + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/UploadFile/images/" + path));
                        upload.SharedModelId = model.Id;
                        upload.FilePath = path;
                        db.UploadMultiFiles.Add(upload);
                        db.SaveChanges();
                    }
                    else
                    {
                        return Json("Lüfen bir dosya seciniz", JsonRequestBehavior.AllowGet);
                    }
                }
                db.SaveChanges();
            }
            else
            {
                return Json("Lütfen bir resim seçiniz...");
            }

            return Json("Resimler başarılı bir şekilde kaydedildi...", JsonRequestBehavior.AllowGet);
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
            if (t[0] == "https://www.youtube.com/watch?v" && linkCount==11)
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
        public ActionResult PostComments(string txt,int id)
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
            AllComments comments = new AllComments();
            foreach (Comments item in comm)
            {
                comments.CommDate = item.CommDate ;
                comments.CommImageUrl = item.CommSiteUsers.Resimulr;
                comments.CommText = item.TextComments;
                comments.CommName = item.CommSiteUsers.FirstName +" "+item.CommSiteUsers.LastName[0];
                allCom.Add(comments);
            }
           
            return Json(allCom,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Profil()
        {
            return View();
        }
     
    }
}