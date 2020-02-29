using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScrapeWeb.Models;

namespace ScrapeWeb.Test
{
    [TestClass]
    public class ScrapeWebConfiguration
    {   
        public ScrapeWebConfiguration()
        {


        }

        [TestMethod]
        public void GenerateSystemConfiguration()
        {
            SystemConfiguration configuration = new SystemConfiguration();

            EmailTemplate template = new EmailTemplate()
            {
                BodyTemplate = "body",
                ItemTemplate = "item",
                RowTemplate = "row"
            };

            configuration.EmailConfiguration = new EmailConfiguration(template)
            {
                ApiKey = "key",
                Login = "login",
                Password = "pwd",
                SmtpServer = "smtp",
                UseSendGrid = true
            };

            configuration.EmailMessage = new EmailMessage()
            {
                From = "from",
                Recipients = new string[] { "rec1", "rec2" },
                Subject = "subject"
            };


            var sConfiguration = Newtonsoft.Json.JsonConvert.SerializeObject(configuration);

            Assert.IsNotNull(sConfiguration);

        }
    }
}
