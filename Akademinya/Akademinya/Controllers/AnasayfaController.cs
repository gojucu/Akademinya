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

        [Route("Hakkimizda")]
        public ActionResult Hakkimizda()
        {
            return View();
        }
        [Route("Destek")]
        public ActionResult Destek()
        {
            return View();
        }
        [Route("Destek")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Destek(Destek destek)
        {
            db.Destek.Add(destek);
            db.SaveChanges();
            return View();
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

            var ratings = db.UyeKurs.Where(d => d.KursID.Equals(id)&&d.PuanVerdi==true).ToList();
            if (ratings.Count() > 0)
            {
                var ratingToplam = ratings.Sum(d => d.KursPuan.Value);
                ViewBag.RatingSum = ratingToplam;
                var ratingCount = ratings.Count();
                ViewBag.RatingCount = ratingCount;
            }
            else
            {
                ViewBag.RatingSum = 0;
                ViewBag.RatingCount = 0;
            }

            var comments = db.UyeKurs.Where(x => x.KursID == id&&x.PuanVerdi==true&&x.KursYorum!=null).ToList();
            ViewBag.Comments = comments;

            return View(db.Kurs.FirstOrDefault(x=>x.Id==id));
        }

        //kategoriye göre listeleme başlangıç
        [Route("Kurslarr/{Ad}")]
        public ActionResult Kurslar(string Ad)
        {
            ViewBag.Ad = Ad;
            ViewBag.Baslik = Ad + " kategorisindeki kurslar";
            return View();
        }
        
        [Route("Kurslar/{Ad}")]
        public ActionResult KategoriKurs(string Ad, int? Page)
        {
            Uye uye = null;
            ViewBag.Ad = Ad;

            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
                    var userGuid = Request.Cookies["userGuid"].Value;
                    uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);
                }
            }


            if (uye != null)
            {

                var uyekurs = db.UyeKurs.ToList().Where(x => x.UyeID == 0);

                if (uye != null)
                {
                    uyekurs = db.UyeKurs.ToList().Where(x => x.UyeID == uye.Id);
                }
                var kurss = db.Kurs.ToList();
                var list = new List<Kurs>();
                foreach(var item in kurss)
                {
                    if(uyekurs.FirstOrDefault(x => x.KursID == item.Id) == null)
                    {
                        list.Add(item);
                    }
                  
                }
                var kategori = db.Kategori.FirstOrDefault(x => x.Ad == Ad);
                var kurslar = list.ToList().Where(x => x.KategoriID == kategori.Id);
                if (kategori.UstId == 0)
                {
                    kurslar = list.ToList().Where(x => x.UstKategoriID == kategori.Id);
                }

                var kurslist = kurslar.ToList().ToPagedList(Page ?? 1, 2); ;
                return View("KursListe", kurslist);
            }
            else
            {
                var kategori = db.Kategori.FirstOrDefault(x => x.Ad == Ad);
            var kurslar = db.Kurs.ToList().Where(x => x.Kategori == kategori);

                if (kategori.UstId == 0)
                {
                    kurslar = db.Kurs.ToList().Where(x => x.UstKategoriID == kategori.Id);
                }

                var kurslist = kurslar.ToList().ToPagedList(Page ?? 1, 2); ;

                return View("KursListe", kurslist);
            }
        }
        //Kategoriye göre listeleme son
        //Aramaya göre listeleme başlangıç
        [Route("Kurslarr2/{Ad}")]
        public ActionResult Kurslar2(string Ad)
        {
            ViewBag.kontrol = "Arama";
            var Ad2 = Ad.Replace("_", " ");
            ViewBag.Ad = Ad;
            ViewBag.Baslik = Ad2 + " için arama sonuçları.";
            return View();
        }

        [Route("Kurslar2/{Ad}")]
        public ActionResult AramaKurs(string Ad, int? Page)
        {
            Uye uye = null;
            ViewBag.Ad = Ad;
            ViewBag.kontrol = "Arama";
            var Ad2 = Ad.Replace("_", " ");

            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
                    var userGuid = Request.Cookies["userGuid"].Value;
                    uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);
                }
            }

            if (uye != null)
            {

                var uyekurs = db.UyeKurs.ToList().Where(x => x.UyeID == 0);

                if (uye != null)
                {
                    uyekurs = db.UyeKurs.ToList().Where(x => x.UyeID == uye.Id);
                }
                var kurss = db.Kurs.ToList();
                var list = new List<Kurs>();
                foreach (var item in kurss)
                {
                    if (uyekurs.FirstOrDefault(x => x.KursID == item.Id) == null)
                    {
                        list.Add(item);
                    }

                }
                var kurslar = list.ToList().Where(x => x.Ad.Contains(Ad2) || x.Icerik.Contains(Ad2) || x.Acikklama.Contains(Ad2));


                var kurslist = kurslar.ToList().ToPagedList(Page ?? 1, 2); 
                return View("KursListe", kurslist);
            }
            else
            {
                var kurslar = db.Kurs.ToList().Where(x => x.Ad.Contains(Ad2) ||x.Icerik.Contains(Ad2) ||x.Acikklama.Contains(Ad2));
                var kurslist= kurslar.ToList().ToPagedList(Page ?? 1, 2);
                return View("KursListe", kurslist);
            }
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
                    uyeKurs.DegerlendirmeTarihi = DateTime.Now;
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
            uyeKurs.DegerlendirmeTarihi = DateTime.Now;
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

                    ViewBag.Baslik = "Kurslarım";

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

        [Route("KursIzlemeSayfasi/{id}")]
        public ActionResult KursIzlemeSayfasi( int id)
        {
            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
                    var userGuid = Request.Cookies["userGuid"].Value;
                    Uye uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);
                    if (uye.UyeKurs.FirstOrDefault(x => x.KursID == id) != null)
                    {
                        return View(db.Kurs.FirstOrDefault(x => x.Id == id));
                    }
                    return RedirectToAction("KursDetay","Anasayfa", new { id });
                }
                return RedirectToAction("KursDetay", "Anasayfa", new { id });
            }
            return RedirectToAction("KursDetay", "Anasayfa", new { id });
        }

        [Route("PuanVer/{id}")]
        public ActionResult PuanVer(int id)
        {
            Uye uye = null;

            if (Request.Cookies["userGuid"] != null)
            {
                if (Request.Cookies["userGuid"].Value != "")
                {
                    var userGuid = Request.Cookies["userGuid"].Value;
                    uye = db.Uye.FirstOrDefault(x => x.CookieGuid == userGuid);


                    ViewBag.UyeKursID = db.UyeKurs.FirstOrDefault(x => x.KursID == id && x.UyeID == uye.Id).Id;
                    return PartialView("/Views/Anasayfa/ModalPartial/PuanVer.cshtml");
                }
                return RedirectToAction("Index", "Anasayfa");
            }
            return RedirectToAction("Index", "Anasayfa");

        }

        [Route("PuanVerPost")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PuanVerPost(int uyeKursID,int puan,string yorum)
        {
            try
            {
                UyeKurs uyekurs = db.UyeKurs.FirstOrDefault(x => x.Id == uyeKursID);
                uyekurs.KursPuan = puan;
                uyekurs.KursYorum = yorum;
                uyekurs.PuanVerdi = true;
                uyekurs.DegerlendirmeTarihi = DateTime.Now;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return View();
            }
           
            
        }
        
    }
}