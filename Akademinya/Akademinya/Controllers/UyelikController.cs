using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Akademinya.Models;
namespace Akademinya.Controllers
{
    [RoutePrefix("")]
    public class UyelikController : Controller
    {
        AkademinyaEntities db = new AkademinyaEntities();
        // GET: Uyelik

        [Route("UyeOl")]
        public ActionResult UyeOl()
        {
            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value == "")
                {
                    return View();
                }
                return RedirectToAction("Index", "Anasayfa");
            }
            else
            {
                return RedirectToAction("Index", "Anasayfa");
            }

        }
        [HttpPost]
        [Route("UyeOl")]
        [ValidateInput(false)]
        public JsonResult UyeOl(Uye uye)
        {
            try
            {
                if (db.Uye.ToList().Where(x => x.Mail.Contains(uye.Mail)).Count()==0)
                {
                        Uye uyee = new Uye();
                        db.Uye.Add(uye);
                        db.SaveChanges();

                        var uyeGuid = Genel.userguidOlustur();//üye olduktan sonra üye girişi işlemi
                        uye.CookieGuid = uyeGuid;
                        db.SaveChanges();

                    return Json(true, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                //ViewBag.Hata = "Bu bilgilere ait kayıtlı kullanıcı bulunmakta tekrar deneyiniz";
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("UyeGirisi")]
        public ActionResult UyeGirisi()
        {
            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value == "")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Anasayfa");
                }
            }
            else
            {
                return View();
                //return RedirectToAction("Index", "Anasayfa");
            }


        } //Ajax kısmını ekle

        [HttpPost]
        [Route("UyeGirisi")]
        [ValidateInput(false)]
        public JsonResult UyeGirisi(string Mail, string Sifre)
        {
            try
            {
                Uye uye = db.Uye.FirstOrDefault(x => x.Mail == Mail && x.Sifre == Sifre);
                if (uye.Id > 0)
                {
                    var uyeGuid = Genel.userguidOlustur();
                    uye.CookieGuid = uyeGuid;
                    db.SaveChanges();

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [Route("UyeCikis")]
        public ActionResult UyeCikis()
        {

            Response.Cookies["userGuid"].Value = null;
            return RedirectToAction("Index", "Anasayfa");
        }

        [Route("SifremiUnuttum")]
        public ActionResult SifremiUnuttum()
        {
            if (Request.Cookies["userGuid"] == null||Request.Cookies["userGuid"].Value == "")
            {
                    return View();
            }
            return RedirectToAction("Index", "Anasayfa");
        }

        [Route("SifremiUnuttum")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SifremiUnuttum(String Mail)
        {
            try
            {
                var uyeMailler = db.Uye.ToList().Where(x => x.Mail.Contains(Mail)).Count();
                if (uyeMailler < 1)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else if (uyeMailler == 1)
                {
                    Uye uye = db.Uye.FirstOrDefault(x => x.Mail == Mail);

                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    var stringChars = new char[6];
                    var random = new Random();
                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }


                    var password = new String(stringChars);
                    uye.Sifre = password;

                    //ViewBag.Uyari = "Yeni şifreniz mail adresinize yollanmıştır.";
                    Genel.MailSender(Mail, password);
                    db.SaveChanges();
                }
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { Result = false, hata = "hata" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
    }
}