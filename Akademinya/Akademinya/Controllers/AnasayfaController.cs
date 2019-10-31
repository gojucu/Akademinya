﻿using Akademinya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akademinya.Controllers
{
    public class AnasayfaController : Controller
    {
        AkademinyaEntities db = new AkademinyaEntities();
        // GET: Anasayfa
        [Route("")]
        public ActionResult Index()
        {

            return View(db.Kurs.ToList());
        }

        [Route("MenuList")]
        public ActionResult MenuList()
        {
            ViewBag.Uye = Session["Uye"];
            return View();
        }

        [Route("IstekSepet")]
        public ActionResult IstekSepet()
        {
            var CookieGuid = Genel.guidKontrol();

            ViewBag.Uye = Session["Uye"];
            ViewBag.Sepet = db.AlisverisSepeti.ToList().Where(x =>x.Guid == CookieGuid);
            ViewBag.SepetSayi = db.AlisverisSepeti.ToList().Where(x => x.Guid == CookieGuid).Count();

            return View();
        }
        [Route("AramaMenu")]
        public ActionResult AramaMenu()
        {
            return View();
        }
        [Route("AramaMenu")]
        [HttpPost]
        public ActionResult AramaMenu(string Ad)
        {
            return RedirectToAction("Kurslar2",new { Ad });
        }

        [Route("Kurslar/KursDetay/{id}")]
        public ActionResult KursDetay(int id)
        {
            return View(db.Kurs.FirstOrDefault(x=>x.Id==id));
        }

        //kategoriye göre listeleme başlangıç
        [Route("Kurslarr/{Ad}")]
        public ActionResult Kurslar(string Ad)
        {
            return View();
        }
        
        [Route("Kurslar/{Ad}")]
        public ActionResult KategoriKurs(string Ad)
        {
            var kategori = db.Kategori.FirstOrDefault(x => x.Ad == Ad);
            var kurslar = db.Kurs.ToList().Where(x => x.Kategori == kategori);
            if (kategori.UstId == 0)
            {
                kurslar = db.Kurs.ToList().Where(x=>x.UstKategoriID==kategori.Id);
            }
            
            return View("KursListe", kurslar);
        }
        //Kategoriye göre listeleme son
        //Aramaya göre listeleme başlangıç
        [Route("Kurslarr2/{Ad}")]
        public ActionResult Kurslar2(string Ad)
        {
            return View();
        }

        [Route("Kurslar2/{Ad}")]
        public ActionResult AramaKurs(string Ad)
        {

            var kurslar = db.Kurs.ToList().Where(x => x.Ad.Contains(Ad)||x.Icerik.Contains(Ad)||x.Acikklama.Contains(Ad));


            return View("KursListe", kurslar);
        }
        //Aramaya göre listeleme son


        [Route("AlisverisSepeti")]
        public ActionResult AlisverisSepeti()
        {
            var CookieGuid = Genel.guidKontrol();

            ViewBag.Uye = Session["Uye"];
            ViewBag.UyeID = Session["UyeXID"];
            ViewBag.SepetSayi = db.AlisverisSepeti.ToList().Where(x=>x.Guid == CookieGuid).Count();

            return View(db.AlisverisSepeti.ToList().Where(x=>x.Guid==CookieGuid));
        }

        [Route("Odeme")]
        public ActionResult Odeme()
        {
            var CookieGuid = Genel.guidKontrol();

            ViewBag.Uye = Session["Uye"];
            ViewBag.UyeID = Session["UyeXID"];
            ViewBag.Sepet = db.AlisverisSepeti.ToList().Where( x=>x.Guid == CookieGuid);
            ViewBag.SepetSayi = db.AlisverisSepeti.ToList().Where(x=>x.Guid == CookieGuid).Count();
            return View();
        }
        [Route("Odeme")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Odeme(Kartlar kart, bool kaydet,int[] sepet)
        {
            var CookieGuid = Genel.guidKontrol();
            try
            {
                ViewBag.Uye = Session["Uye"];
                ViewBag.UyeID = Session["UyeXID"];
                var uyee = new Guid(Session["UyeXID"].ToString());
                //ViewBag.Sepet = db.AlisverisSepeti.ToList().Where(x => x.UyeID == ViewBag.UyeID&&x.Guid==CookieGuid);
                Uye uye = db.Uye.FirstOrDefault(x => x.Id == uyee);

                if (kaydet == true)
                {
                    kart.Silindi = false;
                    kart.UyeID = uyee;
                    db.Kartlar.Add(kart);
                    db.SaveChanges();

                }
                foreach (var item in sepet)
                {
                    AlisverisSepeti sepetEleman = db.AlisverisSepeti.FirstOrDefault(x => x.Id == item);

                    Islemler islem = new Islemler();
                    Kurs kurs = sepetEleman.Kurs;
                    islem.IslemTarihi = DateTime.Now;
                    islem.UyeID = ViewBag.UyeID;
                    islem.KursID = sepetEleman.Kurs.Id;
                    if (kaydet == true)
                    {
                        islem.KartID = kart.Id;
                    }
                    db.Islemler.Add(islem);

                    uye.Kurs1.Add(kurs);
                    db.SaveChanges();

                    db.AlisverisSepeti.Remove(sepetEleman);//silinmesi gerekiyor mu ?
                    db.SaveChanges();

                }
                return RedirectToAction("Odeme");
            }
            catch
            {
                return RedirectToAction("Odeme");
            }

        }

        [Route("UcretsizKursKayit/{Id}")]
        public ActionResult UcretsizKursKayit(int Id)
        {
            ViewBag.Uye = Session["Uye"];
            ViewBag.UyeID = Session["UyeXID"];
            var uyee = new Guid(Session["UyeXID"].ToString());
            Uye uye = db.Uye.FirstOrDefault(x => x.Id == uyee);

            Islemler islem = new Islemler();
            Kurs kurs = db.Kurs.FirstOrDefault(x => x.Id == Id);

            islem.IslemTarihi = DateTime.Now;
            islem.UyeID = ViewBag.UyeID;
            islem.KursID = kurs.Id;
            db.Islemler.Add(islem);

            uye.Kurs1.Add(kurs);
            db.SaveChanges();
            return RedirectToAction("KursDetay", new { Id });
        }
        

        [Route("SepeteEkle/{id}")]
        public JsonResult SepeteEkle(int id)
        {
            var CookieGuid =Genel.guidKontrol();

                var kurs = db.Kurs.FirstOrDefault(x => x.Id==id);
                //ViewBag.Uye = Session["Uye"];

                AlisverisSepeti sepet = new AlisverisSepeti
                {
                    KursID = id,
                    //UyeID = ViewBag.Uye.Id,
                    Guid = CookieGuid
                };
                db.AlisverisSepeti.Add(sepet);
                db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
            
        }

        [Route("SepettenCikar/{id}")]
        public JsonResult SepettenCikar(int id)
        {
            var CookieGuid = Genel.guidKontrol();
            AlisverisSepeti sepet = db.AlisverisSepeti.FirstOrDefault(x => x.Id == id);
            db.AlisverisSepeti.Remove(sepet);
            db.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }

        [Route("Kurslarim")]
        public ActionResult Kurslarim()
        {
            ViewBag.Uye = Session["Uye"];
            var uyeId = new Guid(Session["UyeXID"].ToString());

            Uye uye = db.Uye.FirstOrDefault(x => x.Id == uyeId);

            var kurslarim = uye.Kurs1;
            return View(kurslarim);
        }

        [Route("Profil")]
        public ActionResult Profil()
        {
            ViewBag.Uye = Session["Uye"];

            return View();
        }

        [Route("Profil/SatinAlmaGecmisi")]
        public ActionResult SatinAlmaGecmisi()
        {
            var UyeID = new Guid(Session["UyeXID"].ToString());
            Uye uye = db.Uye.FirstOrDefault(x => x.Id == UyeID);
            var islemler = db.Islemler.OrderByDescending(x=>x.IslemTarihi).ToList().Where(x => x.UyeID == uye.Id);
            return View(islemler);
        }
    }
}