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

            _config.EmailConfiguration.Template.BodyTemplate = _config.EmailConfiguration.Template.BodyTemplate
                .Replace(Constants.EmailSubjectMarkup, _config.EmailMessage.Subject)
                .Replace(Constants.EmailDateMarkup, System.DateTime.Now.ToString(Constants.EmailDateFormatting));


            if (_config.EmailConfiguration.UseSendGrid)
            {
                var apiKey = _config.EmailConfiguration.ApiKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(_config.EmailMessage.From, _config.EmailMessage.From);
                var subject = $"{_config.EmailMessage.Subject} - {System.DateTime.Now.ToString(Constants.EmailDateFormatting)}";

                List<SendGrid.Helpers.Mail.Attachment> attachments = new List<SendGrid.Helpers.Mail.Attachment>();
                //attachments
                if (_config.EmailConfiguration.Attachments.Any())
                {
                    foreach (var attachment in _config.EmailConfiguration.Attachments)
                    {
                        attachments.Add(new SendGrid.Helpers.Mail.Attachment() { Filename = attachment.Name, Content = attachment.Base64String });
                    }
                }

                foreach (var recipient in _config.EmailMessage.Recipients)
                {
                    var to = new EmailAddress(recipient);
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, BuildBody(results));
                    if (_config.EmailConfiguration.Attachments.Any())
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
                //TODO SMTP configuration
            }
        }


        private string BuildBody(IList<Outcome> results)
        {
            var sitems = string.Empty;
            foreach (var gr in results)
            {
                if (gr.HighlightedOutputElements.Any())
                {
                    string sitem = _config.EmailConfiguration.Template.ItemTemplate
                        .Replace(Constants.EmailSourceMarkup, gr.Title)
                        .Replace(Constants.EmailTotalMarkup, gr.HighlightedOutputElements.Count.ToString())
                        .Replace(Constants.EmailHrefMarkup, gr.Url);

                    var rows = string.Empty;

                    if (gr.HighlightedOutputElements.Any())
                    {

                        foreach (var f in gr.HighlightedOutputElements)
                        {
                            rows += _config.EmailConfiguration.Template.RowTemplate.Replace(Constants.EmailFindingMarkup, f);
                        }
                    }


                    sitem = sitem.Replace(Constants.EmailRowsMarkup, rows);
                    sitems += sitem;
                }


            }

            return _config.EmailConfiguration.Template.BodyTemplate.Replace(Constants.EmailItemTemplateMarkup, sitems);
        }


    }
}
