using Akademinya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

namespace Akademinya.Controllers
{
    public class AnasayfaController : Controller
    {
        AkademinyaEntities db = new AkademinyaEntities();
        // GET: Anasayfa
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Baslik = "Akademinya'ya hoşgeldiniz";
            return View(db.Kurs.ToList());
        }

        [Route("MenuList")]
        public ActionResult MenuList()
        {
            if (Request.Cookies["userGuid"]!=null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
            var userguid = Request.Cookies["userGuid"].Value;
            var uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userguid);
            ViewBag.Uye = uye;
                }
            }

            return View();
        }

        [Route("IstekSepet")]
        public ActionResult IstekSepet()
        {

                    var CookieGuid = Genel.guidKontrol();
            
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
            ViewBag.Baslik = Ad+" kategorisindeki kurslar";
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
            ViewBag.Ad = Ad;
            return View();
        }
        
        [Route("Kurslar/{Ad}")]
        public ActionResult KategoriKurs(string Ad, int? Page)
        {
            var kategori = db.Kategori.FirstOrDefault(x => x.Ad == Ad);
            var kurslar = db.Kurs.ToList().Where(x => x.Kategori == kategori);
            ViewBag.Ad = Ad;
            if (kategori.UstId == 0)
            {
                kurslar = db.Kurs.ToList().Where(x=>x.UstKategoriID==kategori.Id);
            }
            var kurslist= kurslar.ToList().ToPagedList(Page ?? 1, 2); ;
            
            return View("KursListe", kurslist);
        }
        //Kategoriye göre listeleme son
        //Aramaya göre listeleme başlangıç
        [Route("Kurslarr2/{Ad}")]
        public ActionResult Kurslar2(string Ad)
        {
            ViewBag.Baslik = Ad + " için arama sonuçları.";
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
            
            ViewBag.SepetSayi = db.AlisverisSepeti.ToList().Where(x=>x.Guid == CookieGuid).Count();

            return View(db.AlisverisSepeti.ToList().Where(x=>x.Guid==CookieGuid));
        }

        [Route("Odeme")]
        public ActionResult Odeme()
        {
            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
                    var CookieGuid = Genel.guidKontrol();
                    ViewBag.Sepet = db.AlisverisSepeti.ToList().Where(x => x.Guid == CookieGuid);
                    ViewBag.SepetSayi = db.AlisverisSepeti.ToList().Where(x => x.Guid == CookieGuid).Count();
                    return View();
                }
                return RedirectToAction("Index", "Anasayfa");
            }
            return RedirectToAction("Index", "Anasayfa");
        }
        [Route("Odeme")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Odeme(Kartlar kart, bool kaydet,int[] sepet)
        {
            var CookieGuid = Genel.guidKontrol();
            try
            {
                var userGuid = Request.Cookies["userGuid"].Value;
                Uye uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);
                
                //ViewBag.Sepet = db.AlisverisSepeti.ToList().Where(x => x.UyeID == ViewBag.UyeID&&x.Guid==CookieGuid);

                if (kaydet == true)
                {
                    kart.Silindi = false;
                    kart.UyeID = uye.Id;
                    db.Kartlar.Add(kart);
                    db.SaveChanges();

                }
                foreach (var item in sepet)
                {
                    AlisverisSepeti sepetEleman = db.AlisverisSepeti.FirstOrDefault(x => x.Id == item);
                    Islemler islem = new Islemler();
                    Kurs kurs = sepetEleman.Kurs;
                    UyeKurs uyeKurs = new UyeKurs();

                    islem.IslemTarihi = DateTime.Now;
                    islem.UyeID = uye.Id;
                    islem.KursID = sepetEleman.Kurs.Id;
                    if (kaydet == true)
                    {
                        islem.KartID = kart.Id;
                    }
                    db.Islemler.Add(islem);

                    uyeKurs.Aktif = true;
                    uyeKurs.UyeID = uye.Id;
                    uyeKurs.KursID = kurs.Id;
                    db.UyeKurs.Add(uyeKurs);
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
            //return View();
        }

        [Route("UcretsizKursKayit/{Id}")]
        public ActionResult UcretsizKursKayit(int Id)
        {
            var userGuid = Request.Cookies["userGuid"].Value;
            Uye uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);
            
            Islemler islem = new Islemler();
            Kurs kurs = db.Kurs.FirstOrDefault(x => x.Id == Id);
            UyeKurs uyeKurs = new UyeKurs();

            islem.IslemTarihi = DateTime.Now;
            islem.UyeID = uye.Id;
            islem.KursID = kurs.Id;
            db.Islemler.Add(islem);

            uyeKurs.KursID = kurs.Id;
            uyeKurs.UyeID = uye.Id;
            uyeKurs.Aktif = true;
            db.UyeKurs.Add(uyeKurs);

            db.SaveChanges();
            return RedirectToAction("KursDetay", new { Id });
            //return View();
        }
        

        [Route("SepeteEkle/{id}")]
        public ActionResult SepeteEkle(int id)
        {
            var CookieGuid =Genel.guidKontrol();

            var kurs = db.Kurs.FirstOrDefault(x => x.Id==id);
            
            AlisverisSepeti sepet = new AlisverisSepeti
            {
                KursID = id,
                Guid = CookieGuid
            };
            db.AlisverisSepeti.Add(sepet);
            db.SaveChanges();
            return RedirectToAction("KursDetay", new { id });

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
            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
                    var userGuid = Request.Cookies["userGuid"].Value;
                    Uye uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);
                    

                    return View(uye);
                }
                return RedirectToAction("Index", "Anasayfa");
            }
            return RedirectToAction("Index", "Anasayfa");
            //return View();
        }

        [Route("Profil")]//Şifre Değiştirme burada daha sonra uyelik kontroller'a taşı
        public ActionResult Profil()
        {
            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
                    var userGuid = Request.Cookies["userGuid"].Value;
                    ViewBag.Uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);
                    return View();
                }
                return RedirectToAction("Index", "Anasayfa");
            }
            return RedirectToAction("Index", "Anasayfa");
        }

        [Route("Profil")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Profil(string EskiSifre,string YeniSifre)
        {
            try
            {
                
                var userGuid = Request.Cookies["userGuid"].Value;
                Uye uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);
                if (EskiSifre == uye.Sifre)
                {
                uye.Sifre = YeniSifre;
                db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            catch{
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [Route("Profil/SatinAlmaGecmisi")]
        public ActionResult SatinAlmaGecmisi()
        {
            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
                    var userGuid = Request.Cookies["userGuid"].Value;
                    Uye uye = db.Uye.FirstOrDefault(x => x.CookieGuid==userGuid);


                    var islemler = db.Islemler.OrderByDescending(x => x.IslemTarihi).ToList().Where(x => x.UyeID == uye.Id);
                    return View(islemler);
                }
                return RedirectToAction("Index", "Anasayfa");
            }
            return RedirectToAction("Index", "Anasayfa");
            //return View();
        }

        [Route("KursIzlemeSayfasi/{Ad}/{id}")]
        public ActionResult KursIzlemeSayfasi(string Ad, int id)
        {
            return View();
        }

        [Route("dlayout")]
        public ActionResult dlayout()
        {
            return View();
        }
        [Route("dliste/{Ad}")]
        public ActionResult dliste(string Ad, int? page)
        {
            ViewBag.Ad = Ad;
            var kutu2 = db.Kurs.ToList().ToPagedList(page ?? 1, 2);
            return View("dListe2", kutu2);
        }
    }
}