using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using DapperAuthApi.Settings;

namespace DapperAuthApi
{

    public class EmailSender : IEmailSender
    {
        //private readonly EmailSettings _emailSettings;

        //public EmailSender(IOptions<EmailSettings> emailSettings)
        //{
        //    _emailSettings = emailSettings.Value;
        //}
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            htmlMessage = htmlMessage.Replace("&amp;", "&");

            //var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            //{
            //    Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
            //    EnableSsl = true
            //};


            var client = new SmtpClient("smtp.office365.com", 587)
            {
                Credentials = new NetworkCredential("internalsystems@fluentgrid.com", "FGL#ipower@$0$25"),
                EnableSsl = true
            };
            // client.Send("internalsystems@fluentgrid.com", email, subject, htmlMessage );

            var mailMessage = new MailMessage
            {
                From = new MailAddress("internalsystems@fluentgrid.com"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true // This enables HTML in the email body
            };
            mailMessage.To.Add(email);

            client.SendMailAsync(mailMessage);

            return Task.CompletedTask;
        }
    }
}