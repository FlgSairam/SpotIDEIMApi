using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;


namespace DapperAuthApi
{
  
        public class EmailSender : IEmailSender
        {
            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {

                //var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                //{
                //    Credentials = new NetworkCredential("f50f1425f1048e", "06775df6ab573b"),
                //    EnableSsl = true
                //};
                //client.Send("from@example.com", email, subject, htmlMessage);

                htmlMessage = htmlMessage.Replace("&amp;", "&");

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    Credentials = new NetworkCredential("internalsystems@fluentgrid.com", "FGIS@17"),
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