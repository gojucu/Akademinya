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
            return View();
        }

        [Route("MenuList")]
        public ActionResult MenuList()
        {
            ViewBag.Uye = Session["Uye"];
            return View();
        }

        [Route("Kurslar/KursDetay")]
        public ActionResult KursDetay()
        {
            return View();
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

    }
}