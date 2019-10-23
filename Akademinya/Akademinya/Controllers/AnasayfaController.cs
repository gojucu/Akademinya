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
            return View();
        }

        //Uyelik
        [Route("UyeOl")]
        public ActionResult UyeOl()
        {
            return View();
        }
    }
}