using Core.Entities.Externals;
using Core.Interfaces;
using Infrastructure.Properties;
using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public Task<bool> SendEmail(string email, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
                {
                    Port = _emailSettings.SmtpPort,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword),
                    EnableSsl = true,
                };

                var emailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SmtpUser, _emailSettings.SenderDisplayName),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body
                };

                emailMessage.To.Add(email);

                smtpClient.Send(emailMessage);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> SendRestorePasswordEmail(string email, string fullName, string url)
        {

            try
            {
                email = "ric.daniel.lpu12@gmail.com";
                var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
                {
                    Port = _emailSettings.SmtpPort,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword),
                    EnableSsl = true,
                };

                var emailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SmtpUser, _emailSettings.SenderDisplayName),
                    Subject = "Reestablicimiento de contraseña en Pizzeria Don Remolo",
                    IsBodyHtml = true
                };

                emailMessage.To.Add(email);
                emailMessage.AlternateViews.Add(GetResetPasswordEmailBody(fullName, url));

                smtpClient.Send(emailMessage);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        private AlternateView GetResetPasswordEmailBody(string fullName, string url)
        {
            var logoStream = new MemoryStream(Resources.logo);
            var Img = new LinkedResource(logoStream, MediaTypeNames.Image.Jpeg)
            {
                ContentId = "LogoImage"
            };

            string emailTemplate = Resources.RestorePassword.Replace("[USER]", fullName).Replace("[URL]", url);
            var alternateView = AlternateView.CreateAlternateViewFromString(emailTemplate, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(Img);
            
            return alternateView;
        }
    }
}
