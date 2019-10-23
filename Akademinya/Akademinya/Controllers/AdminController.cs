using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akademinya.Models;

namespace Akademinya.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        AkademinyaEntities db = new AkademinyaEntities();
        [Route("")]
        public ActionResult Admin()
        {
            return View();
        }

        [Route("KategoriEkle")]
        public ActionResult KategoriEkle()
        {
            var UstKategoriler = db.Kategori.ToList().Where(x => x.UstId == 0&&x.Silindi!=true);
            ViewBag.UstKategoriler = UstKategoriler;

            return View();
        }

        [Route("KategoriEkle")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KategoriEkle(Kategori kat)
        {

            db.Kategori.Add(kat);
            db.SaveChanges();
            return RedirectToAction("KategoriEkle");
        }

        [Route("KategoriSil/{id}")]
        public ActionResult KategoriSil(int id)
        {
            Kategori kat = db.Kategori.FirstOrDefault(x => x.Id == id);
            kat.Silindi = true;
            db.SaveChanges();
            return RedirectToAction("KategoriEkle");
        }
    }
}