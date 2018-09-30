﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VonderkWEB.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace VonderkWEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new Models.HomeViewModel().Get();
            return View(model);
        }

        public ActionResult Empresa()
        {
           

            return View();
        }

        public ActionResult Contacto()
        {
           

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contacto(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p><b>Emisor:</b> {0} ( {2} )( {1} )</p><p><b>Mensaje:</b></p><p>{4}</p><p><b>Compañia:</b></p><p>{3}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("matiasemarangone@gmail.com"));  // replace with valid value 
                message.From = new MailAddress("matiasemarangone@gmail.com");  // replace with valid value
                message.Subject = "Contacto - VK Site";
                message.Body = string.Format(body, model.FromName, model.FromPhone, model.FromEmail, model.FromCompany, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = false;
                    var credential = new NetworkCredential
                    {
                        UserName = "******",  // replace with valid value
                        Password = "******"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


    }
}