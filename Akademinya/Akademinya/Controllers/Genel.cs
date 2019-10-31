using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akademinya.Controllers
{
    public class Genel 
    {
        public static string guidOlustur()
        {
            string token = Guid.NewGuid().ToString();
            HttpContext.Current.Response.Cookies["userGuid"].Value = token;
            HttpContext.Current.Response.Cookies["userGuid"].Expires = DateTime.Now.AddDays(7);
            return token;
        }

        public static string guidKontrol()
        {
            string token = "";
            if (HttpContext.Current.Request.Cookies["userGuid"] != null)
            {
                if (HttpContext.Current.Request.Cookies["userGuid"].Value != "")
                {
                    token = HttpContext.Current.Request.Cookies["userGuid"].Value;
                }
            }
            else
            {
                token = guidOlustur();
            }
            return token;
        }
    }
}