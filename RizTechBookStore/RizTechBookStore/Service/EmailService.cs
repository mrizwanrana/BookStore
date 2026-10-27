using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using RizTechBookStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RizTechBookStore.Service
{
    public class EmailService : IEmailService
    {
        private readonly SMTPConfigModel _smtpConfigModel;
        private readonly IWebHostEnvironment _env;
        public EmailService(IOptions<SMTPConfigModel> smtpConfigModel, IWebHostEnvironment env)
        {
            _smtpConfigModel = smtpConfigModel.Value;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }
        public async Task SendTestEmail(UserEmailOptions emailOptions)
        {
            emailOptions.Subject = UpdatePlaceHolder("Hello {{UserName}}, This is test email subject from RizTech book store.", emailOptions.PlaceHolders);
            emailOptions.Body = UpdatePlaceHolder(GetEmailBody("EmailTemplate"), emailOptions.PlaceHolders);

            await SendEmail(emailOptions);
        }

        public async Task SendEmailForConfirmation(UserEmailOptions emailOptions)
        {
            emailOptions.Subject = UpdatePlaceHolder("Hello {{UserName}}, Confirm your email id.", emailOptions.PlaceHolders);
            emailOptions.Body = UpdatePlaceHolder(GetEmailBody("EmailComfirm"), emailOptions.PlaceHolders);

            await SendEmail(emailOptions);
        }

        public async Task SendEmailForgotPassword(UserEmailOptions emailOptions)
        {
            emailOptions.Subject = UpdatePlaceHolder("Hello {{UserName}}, reset you password.", emailOptions.PlaceHolders);
            emailOptions.Body = UpdatePlaceHolder(GetEmailBody("ForgotPassword"), emailOptions.PlaceHolders);

            await SendEmail(emailOptions);
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            if (userEmailOptions == null) throw new ArgumentNullException(nameof(userEmailOptions));
            if (userEmailOptions.ToEmails == null || userEmailOptions.ToEmails.Count == 0) throw new ArgumentException("No recipients specified", nameof(userEmailOptions));

            using (var mailMessage = new MailMessage()
            {
                Subject = userEmailOptions.Subject ?? string.Empty,
                Body = userEmailOptions.Body ?? string.Empty,
                From = new MailAddress(_smtpConfigModel.SenderAddress, _smtpConfigModel.SenderDisplayName),
                IsBodyHtml = _smtpConfigModel.IsBodyHTML,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            })
            {
                foreach (var toEmail in userEmailOptions.ToEmails)
                {
                    if (!string.IsNullOrWhiteSpace(toEmail))
                        mailMessage.To.Add(toEmail);
                }

                var networkCredential = new NetworkCredential(_smtpConfigModel.Username, _smtpConfigModel.Password);
                using (var smtpClient = new SmtpClient
                {
                    Host = _smtpConfigModel.Host,
                    Port = _smtpConfigModel.Port,
                    EnableSsl = _smtpConfigModel.EnableSSL,
                    UseDefaultCredentials = _smtpConfigModel.UseDefaultCredentials,
                    Credentials = networkCredential
                })
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

        private string GetEmailBody(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName)) return string.Empty;
            var fileName = string.Format("{0}.html", templateName);
            var templateFolder = Path.Combine(_env.ContentRootPath, "EmailTemplate");
            var filePath = Path.Combine(templateFolder, fileName);
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }
            return File.ReadAllText(filePath);
        }

        private string UpdatePlaceHolder(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var keyValue in keyValuePairs)
                {
                    if (text.Contains(keyValue.Key))
                    {
                        text = text.Replace(keyValue.Key, keyValue.Value);
                    }
                }
            }
            return text;
        }
    }
}
