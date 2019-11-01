﻿using System;
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
            if (Session["Uye"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Anasayfa");
            }

        }
        [HttpPost]
        [Route("UyeOl")]
        [ValidateInput(false)]
        public ActionResult UyeOl(Uye uye, string Sifre)
        {
            if (Session["Uye"] == null)
            {
                try
                {
                    MembershipUser user = Membership.CreateUser(uye.KullaniciAdi, Sifre, uye.Mail);
                    uye.Id = (Guid)user.ProviderUserKey;
                    Session["Uye"] = uye;

                    db.Uye.Add(uye);
                    db.SaveChanges();

                    FormsAuthentication.RedirectFromLoginPage(uye.KullaniciAdi, true);
                    Session["Uye"] = uye;
                    Session["UyeXID"] = uye.Id;
                    return RedirectToAction("Index", "Anasayfa");
                }
                catch
                {
                    ViewBag.Hata = "Bu bilgilere ait kayıtlı kullanıcı bulunmakta tekrar deneyiniz";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Anasayfa");
            }



        }
        [Route("UyeGirisi")]
        public ActionResult UyeGirisi()
        {
            if(Response.Cookies["userGuid"]!= null){
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
        public ActionResult UyeGirisi(string kullaniciAdi, string Sifre)
        {
            try
            {


                Uye uye = db.Uye.FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == Sifre);
                    if (uye.Id > 0)
                    {
                        var uyeGuid = Genel.userguidOlustur();
                        uye.CookieGuid = uyeGuid;
                        //Response.Cookies["userid"].Value = uyeGuid.ToString();
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

            return View();
        }

        [Route("SifremiUnuttum")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SifremiUnuttum(String Mail)
        {
            var uyeMailler = db.Uye.ToList().Where(x => x.Mail.Contains(Mail)).Count();
            if (uyeMailler < 1)
            {
                ViewBag.Uyari = "Girdiğiniz mail adresine ait mail bulunamadı!";
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
                MembershipUser mu = Membership.GetUser(uye.KullaniciAdi);
                mu.ChangePassword(mu.ResetPassword(), password);

                ViewBag.Uyari = "Yeni şifreniz mail adresinize yollanmıştır.";
                Genel.MailSender(Mail, password);
                db.SaveChanges();
            }
            return View();
        }
    }
}