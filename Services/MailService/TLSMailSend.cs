using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Scheme.Services.MailService
{
    public class TLSMailSend : IEmailSender
    {

        public string Host { get; set; }
        public int Port { get; set; }
        public string SMTP_USERNAME { get; set; }
        public string SMTP_PASSWORD { get; set; }
        public string ErrorMessage { get; set; }

        public TLSMailSend(string host, int port, string username, string password)
        {
            Host = host;
            Port = port;
            SMTP_USERNAME = username;
            SMTP_PASSWORD = password;
        }

        public TLSMailSend(MailServiceOptions opt)
        {
            int port = 587;
            Int32.TryParse(opt.Port, out port); 
            Host = opt.Host;
            Port = port;
            SMTP_USERNAME = opt.SMTP_USERNAME;
            SMTP_PASSWORD = opt.SMTP_PASSWORD;
        }

        public bool Send(MailMessage message)
        {
            using (var client = new SmtpClient(Host, Port))
            {
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                client.EnableSsl = true;
                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    return false;
                }
            }
        }

        public async Task<bool> SendAsync(MailMessage message)
        {
            using (var client = new SmtpClient(Host, Port))
            {
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                client.EnableSsl = true;

                try
                {
                    await client.SendMailAsync(message);
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    return false;
                }
            }
        }
    }




    //public string From { get; set; }
    //public string FromName { get; set; }
    //public string To { get; set; }
    //public string Subject { get; set; }
    //public string Body { get; set; }
}
