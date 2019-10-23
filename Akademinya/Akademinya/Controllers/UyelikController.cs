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
            return View();
        }
        [HttpPost]
        [Route("UyeOl")]
        [ValidateInput(false)]
        public ActionResult UyeOl(Uye uye, string Sifre)
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

                return RedirectToAction("Index", "Anasayfa");
            }
            catch
            {
                ViewBag.Hata = "Bu bilgilere ait kayıtlı kullanıcı bulunmakta tekrar deneyiniz";
                return View();
            }


        }
        [Route("UyeGirisi")]
        public ActionResult UyeGirisi()
        {
            ViewBag.Kullanici = Session["Kullanici"];
            return View();
        }

        [HttpPost]
        [Route("UyeGirisi")]
        [ValidateInput(false)]
        public ActionResult UyeGirisi(string kullaniciAdi, string Sifre)
        {
            if (Membership.ValidateUser(kullaniciAdi, Sifre))
            {
                FormsAuthentication.RedirectFromLoginPage(kullaniciAdi, true);
                Uye uye = db.Uye.FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi);
                Session["Kullanici"] = uye;
                Session["KullaniXID"] = uye.Id;

                //string gor = uye.Id.ToString();

                return RedirectToAction("Index", "Anasayfa");
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı Adı Veya Parola Yanlış";
                return View();
            }

        }

        [Route("UyeCikis")]
        [HttpPost]
        public ActionResult UyeCikis()
        {

            Session["Kullanici"] = null;
            return RedirectToAction("GirisYap", "Uyelik");
        }
    }
}