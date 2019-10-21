using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akademinya.Models;

namespace Akademinya.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        AkademinyaEntities db = new AkademinyaEntities();
        [Route("Admin")]
        public ActionResult Admin()
        {
            return View();
        }

        [Route("KategoriEkleDuzenle")]
        public ActionResult KategoriEkleDuzenle()
        {
            var UstKategoriler = db.Kategori.ToList().Where(x => x.UstId == 0);
            ViewBag.UstKategoriler = UstKategoriler;

            return View();
        }

        [Route("KategoriEkleDuzenle")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KategoriEkleDuzenle(Kategori kat)
        {

            db.Kategori.Add(kat);
            return View();
        }
    }
}