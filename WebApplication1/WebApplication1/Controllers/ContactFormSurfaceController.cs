﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebApplication1.ViewModels;
using Umbraco.Core.Models;

namespace WebApplication1.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: Default
        public ActionResult Index()
        {
                return PartialView("ContactForm", new ContactForm());
        }


        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }

            MailMessage message = new MailMessage();
            message.To.Add("username@eaaa.dk");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            // Parameters - name, parentId, contentTypeAlias
            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");
            
            //comment.CreateContent(model.Subject, CurrentPage.Id, "message");

            // Assign values
            comment.SetValue("messagename", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("messageContent", model.Message);

            // Save to Umbraco
            Services.ContentService.Save(comment);


            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587; smtp.Credentials = new System.Net.NetworkCredential("jegindfeldt@gmail.com", "pbzzyohoxfsxvelu");
                
                // send mail 
                smtp.Send(message);
                TempData["success"] = true;
            }

            return RedirectToCurrentUmbracoPage();


        }
    }
 
}




