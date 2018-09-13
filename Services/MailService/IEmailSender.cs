using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Scheme.Services.MailService
{
    public interface IEmailSender
    {
        string Host { get; set; }

        int Port { get; set; }

        string ErrorMessage { get; set; }

        string SMTP_USERNAME { get; set; }

        string SMTP_PASSWORD { get; set; }

        bool Send(MailMessage message);

        Task<bool> SendAsync(MailMessage message);
    }
}
