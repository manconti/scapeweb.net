using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using ScrapeWeb.Models;
using System.IO;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Net.Http;

namespace ScrapeWeb.Services
{
    public sealed class EmailService
    {
        private SystemConfiguration _config;

        public EmailService(SystemConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendAsync(IList<Outcome> results)
        {

            _config.EmailMessage.Template.BodyTemplate = _config.EmailMessage.Template.BodyTemplate
                .Replace(Constants.EmailSubjectMarkup, _config.EmailMessage.Subject)
                .Replace(Constants.EmailDateMarkup, System.DateTime.Now.ToString(Constants.EmailDateFormatting));


            var from = _config.EmailMessage.From;
            var subject = $"{_config.EmailMessage.Subject} - {System.DateTime.Now.ToString(Constants.EmailDateFormatting)}";
            var body = BuildBody(results);

            


            if (_config.EmailConfiguration.UseSendGrid)
            {
                var apiKey = _config.EmailConfiguration.ApiKey;
                var client = new SendGridClient(apiKey);


                List<SendGrid.Helpers.Mail.Attachment> attachments = new List<SendGrid.Helpers.Mail.Attachment>();
                //attachments
                if (_config.EmailMessage.Attachments.Any())
                {
                    foreach (var attachment in _config.EmailMessage.Attachments)
                    {
                        attachments.Add(new SendGrid.Helpers.Mail.Attachment() { Filename = attachment.Name, Content = attachment.Base64String });
                    }
                }

                foreach (var recipient in _config.EmailMessage.Recipients)
                {
                    var to = new EmailAddress(recipient);
                    var msg = MailHelper.CreateSingleEmail(new EmailAddress(from, from), to, subject, string.Empty, body);
                    if (_config.EmailMessage.Attachments.Any())
                    {
                        msg.Attachments = attachments;
                    }
                    var response = await client.SendEmailAsync(msg);
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new Exception(Constants.ExEmailSendingError.Replace("§§", response.Body.ReadAsStringAsync().Result));
                    }
                }
            }
            else
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(_config.EmailConfiguration.SmtpServer);
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_config.EmailConfiguration.Login, _config.EmailConfiguration.Password );
                SmtpServer.EnableSsl = _config.EmailConfiguration.SmtpSsl;

                foreach (var recipient in _config.EmailMessage.Recipients)
                {
                    mail.From = new MailAddress(from);
                    mail.To.Add(recipient);
                    mail.Subject = subject;
                    mail.Body = body;
                    foreach (var attachment in _config.EmailMessage.Attachments)
                    {
                        mail.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(Convert.FromBase64String(attachment.Base64String)), attachment.Name));
                    }

                    SmtpServer.Send(mail);
                }   
            }
        }


        private string BuildBody(IList<Outcome> results)
        {
            var sitems = string.Empty;
            foreach (var gr in results)
            {
                if (gr.HighlightedOutputElements.Any())
                {
                    string sitem = _config.EmailMessage.Template.ItemTemplate
                        .Replace(Constants.EmailSourceMarkup, gr.Title)
                        .Replace(Constants.EmailTotalMarkup, gr.HighlightedOutputElements.Count.ToString())
                        .Replace(Constants.EmailHrefMarkup, gr.Url);

                    var rows = string.Empty;

                    if (gr.HighlightedOutputElements.Any())
                    {

                        foreach (var f in gr.HighlightedOutputElements)
                        {
                            rows += _config.EmailMessage.Template.RowTemplate.Replace(Constants.EmailFindingMarkup, f);
                        }
                    }


                    sitem = sitem.Replace(Constants.EmailRowsMarkup, rows);
                    sitems += sitem;
                }


            }

            return _config.EmailMessage.Template.BodyTemplate.Replace(Constants.EmailItemTemplateMarkup, sitems);
        }


    }
}
