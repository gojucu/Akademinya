using Akademinya.Models;
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
            ViewBag.Uye = Session["Uye"];

            ViewBag.SepetSayi = db.AlisverisSepeti.ToList().Where(x => x.UyeID == ViewBag.UyeID).Count();

            return View();
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


        [Route("AlisverisSepeti")]
        public ActionResult AlisverisSepeti()
        {
            var CookieGuid = Genel.guidKontrol();

            ViewBag.Uye = Session["Uye"];
            ViewBag.UyeID = Session["UyeXID"];
            ViewBag.SepetSayi = db.AlisverisSepeti.ToList().Where(x => x.UyeID == ViewBag.UyeID).Count();

            return View(db.AlisverisSepeti.ToList().Where(x => x.UyeID == ViewBag.UyeID&&x.Guid==CookieGuid));
        }

        [Route("Odeme")]
        public ActionResult Odeme()
        {
            var CookieGuid = Genel.guidKontrol();

            ViewBag.Uye = Session["Uye"];
            ViewBag.UyeID = Session["UyeXID"];
            ViewBag.Sepet = db.AlisverisSepeti.ToList().Where(x => x.UyeID == ViewBag.UyeID && x.Guid == CookieGuid);
            ViewBag.SepetSayi = db.AlisverisSepeti.ToList().Where(x => x.UyeID == ViewBag.UyeID).Count();
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
                    islem.KartID = kart.Id;
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
        

        [Route("SepeteEkle/{id}")]
        public ActionResult SepeteEkle(int id)
        {
            var CookieGuid =Genel.guidKontrol();
            if (Session["Uye"] != null)
            {
                var kurs = db.Kurs.FirstOrDefault(x => x.Id==id);
                ViewBag.Uye = Session["Uye"];
                
                AlisverisSepeti sepet = new AlisverisSepeti();
                sepet.KursID = id;
                sepet.UyeID = ViewBag.Uye.Id;
                sepet.Guid = CookieGuid;
                db.AlisverisSepeti.Add(sepet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
    }
}