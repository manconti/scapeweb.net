using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using ScrapeWeb.Models;
using ScrapeWeb.Services;
using System.Threading.Tasks;

namespace ScrapeWeb.App
{
    class Program
    {
        static ScrapeService scrapeWeb;

        static async Task Main(string[] args)
        {
            var data = JsonConvert.DeserializeObject<List<Target>>(File.ReadAllText(@"Data/data.json"));
            var config = JsonConvert.DeserializeObject<SystemConfiguration>(File.ReadAllText(@"Data/config.json"));

            config.EmailMessage.Template.BodyTemplate = File.ReadAllText(@"Data/Templates/Default/email.template.html");
            config.EmailMessage.Template.ItemTemplate = File.ReadAllText(@"Data/Templates/Default/item.template.html");
            config.EmailMessage.Template.RowTemplate = File.ReadAllText(@"Data/Templates/Default/item.row.template.html");


            if (!Directory.Exists(@"Data/Output"))
            {
                Directory.Exists(@"Data/Output");
            }

            if (!Directory.Exists(Path.Combine(@"Data/Output", DateTime.Now.ToString("yyyyMMdd"))))
            {
                Directory.CreateDirectory(Path.Combine(@"Data/Output", DateTime.Now.ToString("yyyyMMdd")));
            }

            await MainAsync(data, config);
            Console.WriteLine("*****FINISHED*****");
            Console.ReadLine();

        }


        static async Task MainAsync(List<Target> data, SystemConfiguration configuration)
        {
            scrapeWeb = new ScrapeService();
            scrapeWeb.LogEvent += ScrapeWeb_LogEvent;
            var outcomes = await scrapeWeb.ExecuteAsync(data);

            var filtered = outcomes.Where(x => x.KeywordsFound == true).GroupBy(y => y.Title);

            foreach (var value in filtered)
            {
                Log(ConsoleColor.DarkGreen, $"{value.Key} - {value.Count()}");
            }

            if (configuration.GenerateCsv)
            {
                //generating the CSV
                CsvService csv = new CsvService();
                using (MemoryStream ms = csv.Generate(outcomes))
                {
                    using (FileStream file = new FileStream(Path.Combine(@"Data/Output", DateTime.Now.ToString("yyyyMMdd"), "output.csv"), FileMode.Create, System.IO.FileAccess.Write))
                    {
                        ms.CopyTo(file);
                    }
                }
            }


            if (configuration.SendEmail)
            {
                //adding the CSV to the email
                if (configuration.GenerateCsv)
                {
                    configuration.EmailMessage.Attachments.Add(
                    new EmailAttachment(Convert.ToBase64String(File.ReadAllBytes(Path.Combine(@"Data/Output", DateTime.Now.ToString("yyyyMMdd"), "output.csv"))), "output.csv")
                    );
                }

                EmailService service = new EmailService(configuration);
                await service.SendAsync(outcomes);
            }

        }

        private static void ScrapeWeb_LogEvent()
        {
            var logItem = scrapeWeb.Logs[0]; //always the latest one;
            switch (logItem.Type)
            {
                case LogType.Info:
                    Log(ConsoleColor.Gray, logItem);
                    break;
                case LogType.Message:
                    Log(ConsoleColor.DarkCyan, logItem);
                    break;
                case LogType.Error:
                    Log(ConsoleColor.Red, logItem);
                    break;
                case LogType.Warning:
                    Log(ConsoleColor.Yellow, logItem);
                    break;
                case LogType.Success:
                    Log(ConsoleColor.Green, logItem);
                    break;
            }
        }


        private static void Log(ConsoleColor color, LogItem message)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"{message.DateTime} - {message.Message}");
            Console.ForegroundColor = currentColor;
        }

        private static void Log(ConsoleColor color, string message)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }

    }
}
