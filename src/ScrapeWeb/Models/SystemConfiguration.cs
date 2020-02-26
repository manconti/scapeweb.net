using System.Collections.Generic;

namespace ScrapeWeb.Models
{
    /// <summary>
    /// It represents the overall configuration for the entire ScrapeWeb running.
    /// </summary>
    public class SystemConfiguration
    {
        /// <summary>
        /// Get or Set the value indicating if ScrapeWeb will send an email. Default value is True.
        /// </summary>
        public bool SendEmail;
        /// <summary>
        /// Get or Set the value indicating if ScrapeWeb wil generate the CSV. Default value is True.
        /// </summary>
        public bool GenerateCsv;
        /// <summary>
        /// Get or Set the value indicating if ScrapeWeb will attach the CSV to the email sent. This parameter is ignored if <see cref="SendEmail"/> is False.
        /// </summary>
        public bool SendCsv;
        /// <summary>
        /// Get or Set the value indicating which email template ScrapeWeb will use. 
        /// </summary>
        public string Template;
        /// <summary>
        /// Get or Set the <see cref="EmailConfiguration"/>.
        /// </summary>
        public EmailConfiguration EmailConfiguration;
        /// <summary>
        /// Get or Set the <see cref="EmailMessage"/>.
        /// </summary>
        public EmailMessage EmailMessage;

        public SystemConfiguration()
        {
            this.SendEmail = true;
            this.GenerateCsv = true;
        }
    }

    /// <summary>
    /// It represents the configuration for sending the email. ScrapeWeb can either use SendGrid or use a standard SMTP configuration. 
    /// </summary>
    public class EmailConfiguration
    {

        /// <summary>
        /// Get or Set the list of attachments to be added to the email.
        /// </summary>
        public List<EmailAttachment> Attachments;

        /// <summary>
        /// Get the template for the current email configuration.
        /// </summary>
        public EmailTemplate Template { get; internal set; }
        /// <summary>
        /// Get or Set the value indicating if the SendGrid API will be used. 
        /// </summary>
        public bool UseSendGrid;
        /// <summary>
        /// Get or Set the value for the SendGrid Api Key. This parameter is ignored if <see cref="UseSendGrid"/> is set to False. 
        /// </summary>
        public string ApiKey;
        /// <summary>
        /// Get or Set the login for the SMTP sending email. This parameter is ignored if <see cref="UseSendGrid"/> is set to True. 
        /// </summary>
        public string Login;
        /// <summary>
        /// Get or Set the password for the SMTP sending email. This parameter is ignored if <see cref="UseSendGrid"/> is set to True. 
        /// </summary>
        public string Password;
        /// <summary>
        /// Get or Set the SMTP server name. This parameter is ignored if <see cref="UseSendGrid"/> is set to True. 
        /// </summary>
        public string SmtpServer;

        public EmailConfiguration(EmailTemplate template)
        {
            this.Template = template;
            this.Attachments = new List<EmailAttachment>();

            if (!Template.IsValid())
            {
                throw new System.MissingFieldException(Constants.ExEmailTemplateNotValid);
            }
        }


    }
    /// <summary>
    /// It represents an attachment that will be sent within the email;
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// Get or Set the content of the attachment as a Base64 string
        /// </summary>
        public string Base64String;
        /// <summary>
        /// Get or Set the name of the attachment
        /// </summary>
        public string Name;

        public EmailAttachment(string base64string, string name)
        {
            Base64String = base64string;
            Name = name;
        }
    }

    /// <summary>
    /// It represents the configuration for the email sent by ScrapeWeb
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Get or Set the sender of the email sent. 
        /// </summary>
        public string From;
        /// <summary>
        /// Get or Set the subject of the email sent.
        /// </summary>
        public string Subject;
        /// <summary>
        /// Get or Set the list of Recipients for the email. 
        /// </summary>
        public string[] Recipients;
    }

    /// <summary>
    /// It represents the HTML segments of the email template that will be sent
    /// </summary>
    public class EmailTemplate
    {
        /// <summary>
        /// Get or Set the full body template for the email. It must contain the placeholder %ITEMTEMPLATE%
        /// </summary>
        public string BodyTemplate;
        /// <summary>
        /// Get or Set the item template for the email. It can contain the placeholders %HREF% (link to the Url), %SOURCE% (representing the Title of the site) and %ROWS% (the rows found for the current web)
        /// </summary>
        public string ItemTemplate;
        /// <summary>
        /// Get or Set the row template for the email. It must contain the placeholder %FINDING%
        /// </summary>
        public string RowTemplate;

        internal bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(BodyTemplate) || string.IsNullOrWhiteSpace(ItemTemplate) || string.IsNullOrWhiteSpace(RowTemplate))
                return false;
            else
                return true;
        }

    }

}
