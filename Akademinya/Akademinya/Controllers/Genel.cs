using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Akademinya.Controllers
{
    public class Genel 
    {
        public static string guidOlustur()
        {
            string token = Guid.NewGuid().ToString();
            HttpContext.Current.Response.Cookies["sepetGuid"].Value = token;
            HttpContext.Current.Response.Cookies["sepetGuid"].Expires = DateTime.Now.AddDays(7);
            return token;
        }

        public static string guidKontrol()
        {
            string token = "";
            if (HttpContext.Current.Request.Cookies["sepetGuid"] != null)
            {
                if (HttpContext.Current.Request.Cookies["sepetGuid"].Value != "")
                {
                    token = HttpContext.Current.Request.Cookies["sepetGuid"].Value;
                }
            }
            else
            {
                token = guidOlustur();
            }
            return token;
        }


        public static string userguidOlustur()
        {
            string token = Guid.NewGuid().ToString();
            HttpContext.Current.Response.Cookies["userGuid"].Value = token;
            HttpContext.Current.Response.Cookies["userGuid"].Expires = DateTime.Now.AddDays(7);
            return token;
        }

        public static string userguidKontrol()
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


        public static string adminguidOlustur()
        {
            string token = Guid.NewGuid().ToString();
            HttpContext.Current.Response.Cookies["adminGuid"].Value = token;
            HttpContext.Current.Response.Cookies["adminGuid"].Expires = DateTime.Now.AddDays(1);
            return token;
        }


        public static void MailSender(string body, string password)
        {
            body = "Şifre sıfırlama isteği yolladınız. Yeni şifreniz " + password + " olarak değiştirilmiştir. Yeni şifrenizle giriş yapıp şifrenizi düzenleyebilirsiniz.";
            var fromAddress = new MailAddress("gönderen@mail.com");
            var toAddress = new MailAddress("alici@mail.com");
            const string subject = "Akademinya | Şifre yenileme";
            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "Gönderen şifre")
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    smtp.Send(message);
                }
            }


        }
    }
}