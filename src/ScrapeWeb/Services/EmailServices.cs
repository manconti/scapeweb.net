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
    public class EmailService
    {
        private SystemConfiguration _config;
        private string _emailTemplate;
        private string _itemTemplate;
        private string _rowTemplate;
        private string _attachmentPath;


        public EmailService(SystemConfiguration configuration)
        {
            _config = configuration;
            _emailTemplate = File.ReadAllText(Path.Combine(Constants.TemplateFolder, configuration.Template, Constants.EmailTemplate));
            _itemTemplate = File.ReadAllText(Path.Combine(Constants.TemplateFolder, configuration.Template, Constants.EmailItemTemplate));
            _rowTemplate = File.ReadAllText(Path.Combine(Constants.TemplateFolder, configuration.Template, Constants.EmailRowTemplate));
            _attachmentPath = Path.Combine(Constants.OutputFolder, DateTime.Now.ToString("yyyyMMdd"), Constants.OutputFileName);
        }

        public async Task Send(List<Outcome> results)
        {

            _emailTemplate = _emailTemplate.Replace(Constants.EmailSubjectMarkup, _config.EmailMessage.Subject)
                .Replace(Constants.EmailDateMarkup, System.DateTime.Now.ToString(Constants.EmailDateFormatting));

            var body = BuildBody(results);


            if (_config.EmailConfiguration.UseSendGrid)
            {

                var apiKey = _config.EmailConfiguration.ApiKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(_config.EmailMessage.From, _config.EmailMessage.From);
                var subject = $"{_config.EmailMessage.Subject} - {System.DateTime.Now.ToString(Constants.EmailDateFormatting)}";

                foreach (var recipient in _config.EmailMessage.Recipients)
                {
                    var to = new EmailAddress(recipient);
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, body);
                    if (_config.SendCsv)
                    {
                        msg.AddAttachment(Constants.OutputFileName, Convert.ToBase64String(File.ReadAllBytes(_attachmentPath)));
                    }
                    var response = await client.SendEmailAsync(msg);

                }
            }
            else
            {
                //TODO SMTP configuration
            }
        }


        private string BuildBody(List<Outcome> results)
        {
            var sitems = string.Empty;
            var sitem = string.Empty;

            foreach (var gr in results)
            {
                if (gr.HighlightedOutputElements.Any())
                {
                    sitem = _itemTemplate.Replace(Constants.EmailSourceMarkup, gr.Title)
                        .Replace(Constants.EmailTotalMarkup, gr.HighlightedOutputElements.Count.ToString())
                        .Replace(Constants.EmailHrefMarkup, gr.Url);
                    var rows = string.Empty;
                            
                    if (gr.HighlightedOutputElements.Any())
                    {

                        foreach (var f in gr.HighlightedOutputElements)
                        {
                            rows += _rowTemplate.Replace(Constants.EmailFindingMarkup, f);
                        }
                    }


                    sitem = sitem.Replace(Constants.EmailRowsMarkup, rows);
                    sitems += sitem;
                }


            }

            return _emailTemplate.Replace(Constants.EmailItemTemplateMarkup, sitems);
        }


    }
}
