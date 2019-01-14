using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage("Nata-matrosowa2012@yandex.ru", email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true; 
            var client = new SmtpClient("smtp.yandex.ru");
            client.Credentials = new NetworkCredential("Nata-matrosowa2012@yandex.ru", "");
            client.EnableSsl = true;
            return client.SendMailAsync(message); 
        }
    }
}
