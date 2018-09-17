using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scheme.Models;
using Scheme.Services.MailService;

namespace Scheme.Controllers
{
    public class HomeController : Controller
    {
        IEmailSender _sender;
        private CodeGenerator _generator;
        public HomeController(IEmailSender emailSender, CodeGenerator generator)
        {
            _sender = emailSender;
            _generator = generator;
        }
        //[Authorize]
        public IActionResult Index()
        {
            return Ok();
        }

        [NonAction]
        public MailMessage ConfirmaionMessage(int code, string address)
        {
            var mail = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress("samir.itstep@gmail.com", "Samir Mammadli"),
                Subject = "Get your code",
                Body = $@"Your verification code: <b>{code}</b>"
            };
            mail.To.Add(new MailAddress(address));

            return mail;
        }
    }
}