using SocialMediaCampus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaCampus.Controllers
{
    public class SiteUsersController : Controller
    {
        private SocialMediaDBEntities db = new SocialMediaDBEntities();
        // GET: SiteUsers
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(string firstname, string lastname, string number, string email, string password)
        {
            MsgJsonResult result = new MsgJsonResult();
            firstname = firstname?.Trim();
            lastname = lastname?.Trim();
            number = number?.Trim();
            email = email?.Trim();
            password = password?.Trim();

            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname) || string.IsNullOrEmpty(number) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                result.HasError = true;
                result.Message = "Lütfen, boş alanları doldurunuz.";
            }
            else
            {
                NumberStudent numStudent = db.numberStudents.FirstOrDefault(x => x.Number == number);
                if (numStudent == null)
                {
                    result.HasError = true;
                    result.Message = "Bu numarada bir öğrenci kayıtlı değil.";
                }
                else
                {
                    SiteUsers user = db.Users.FirstOrDefault(x => x.Number == number || x.EMail == email);
                    if (user != null)
                    {
                        result.HasError = true;
                        result.Message = "Bu numrada oğrenci kayıtlı.";
                    }
                    else
                    {
                        RoleType rol = new RoleType();
                        user = db.Users.Add(new SiteUsers()
                        {
                            FirstName = firstname,
                            LastName = lastname,
                            Number = number,
                            EMail = email,
                            Password = password,
                            LastAcces = DateTime.Now,
                            Permisson = "Ogrenci",
                            Resimulr = "Profile.png"
                        });


                        if (db.SaveChanges() > 0)
                        {
                            result.HasError = false;
                            result.Message = "Kayıt olustu";

                            user.Password = string.Empty;
                            Session["Ogrenci"] = user;
                        }
                        else
                        {
                            result.HasError = true;
                            result.Message = "Bir Hata oluştu.";
                        }
                    }

                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Login(string login_numara, string login_password, bool login_rememberme)
        {
            MsgJsonResult result = new MsgJsonResult();
            login_numara = login_numara?.Trim();
            login_password = login_password?.Trim();

            if (string.IsNullOrEmpty(login_numara) || string.IsNullOrEmpty(login_password))
            {
                result.HasError = true;
                result.Message = "Öğrenci numarası ve şifre alanlarnı doldurunuz.";
            }
            else
            {
                SiteUsers user = db.Users.AsNoTracking().FirstOrDefault(x => x.Number == login_numara && x.Password == login_password);
                if (user != null)
                {
                    result.HasError = false;
                  //  result.url = "/Share/Paylasim";
                    user.Password = string.Empty;

                    Session["Ogrenci"] = user;
                    result.Message = "Mahmut";
                }
                else
                {
                    result.HasError = true;
                    result.Message = "Öğrenci numarası veya şifre yanlış";

                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LostPassword(string lost_email)
        {

            MsgJsonResult result = new MsgJsonResult();

            lost_email = lost_email?.Trim();

            if (string.IsNullOrEmpty(lost_email))
            {
                result.HasError = true;
                result.Message = "E-Mail address can not be empty.";
            }
            else
            {
                // TODO : KMB Modal Login - Lost Password
                SiteUsers user = db.Users.AsNoTracking().FirstOrDefault(x => x.EMail == lost_email);


                if (user != null)
                {
                    //
                    // TODO : Send password with e-mail.
                    //

                    result.HasError = false;
                    result.Message = "Password has been sent.";
                }
                else
                {
                    result.HasError = true;
                    result.Message = "E-Mail adresi kayıtlı değil.";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return View();
        }



        public ActionResult IlkOn()
        {
            return RedirectToAction("Paylasim", "Share");
        }
    }



}