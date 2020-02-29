using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScrapeWeb.Models;
using ScrapeWeb.Services;
using System.Linq;

namespace ScrapeWeb.Test
{
    [TestClass]
    public class ScrapeWebTest
    {
        List<Target> targetList = new List<Target>();
        List<Target> targetListNoKeywords = new List<Target>();
        List<Target> targetListWithHeaders = new List<Target>();
        ScrapeService scrapeService = new ScrapeService();

        [TestInitialize]
        public void Initialize()
        {
            targetList.Add(new Target()
            {
                Url = "https://centraledesmarches.com/marches-publics/liste-avancee?nb=50&rem_date_to_date%5Bfrom%5D=2020-01-01&sort=date_publication+desc&rem_type=avancee&departement=haute-savoie&page=1&e%5Bdept%5D=74",
                Title = "Central des Marchés",
                UseHeaders = false,
                SearchingElement = "//section[contains(@class, 'resultat')]",
                SingleDescendantElementsToFind = new string[] { ".//div[contains(@class, 'entete')]/h2", ".//div[contains(@class, 'maitreouvrage')]/div" },
                Keywords = new string[] { "autoroute", "route", "thonon", "machilly" },
                UseKeywords = true
            }) ;

            targetListNoKeywords.Add(new Target()
            {
                Url = "https://centraledesmarches.com/marches-publics/liste-avancee?nb=50&rem_date_to_date%5Bfrom%5D=2020-01-01&sort=date_publication+desc&rem_type=avancee&departement=haute-savoie&page=1&e%5Bdept%5D=74",
                Title = "Central des Marchés",
                UseHeaders = false,
                SearchingElement = "//section[contains(@class, 'resultat')]",
                SingleDescendantElementsToFind = new string[] { ".//div[contains(@class, 'entete')]/h2", ".//div[contains(@class, 'maitreouvrage')]/div" },
                UseKeywords = false
            });

            targetListWithHeaders.Add(new Target()
            {
                Url = "https://www.francemarches.com/appels-offre/haute-savoie?1=1",
                Title = "France Marchés",
                UseHeaders = true,
                SearchingElement = "//div[contains(@class, 'cadreBlanc shadow blockIsLink')]",
                SingleDescendantElementsToFind = new string[] { ".//a[contains(@class, 'link')]", ".//li[contains(@class, 'client')]", ".//li[contains(@class, 'dateParution')]/strong/time " },
                UseKeywords = false,
                Cookie = "datadome=9PvmzhFIkUd2LKWoWGTf7NnK.e1rzcR6HmZCsLbdYvVfownZPEzc-cyuqI6X90-6h~hz.P8fe2jPrPVJ8fKwK~zTIrDapCRM_F-SZO4TJS; atuserid=%7B%22name%22%3A%22atuserid%22%2C%22val%22%3A%22e74976d9-927f-4934-ac47-344ce01082e7%22%2C%22options%22%3A%7B%22end%22%3A%222021-03-01T09%3A20%3A13.512Z%22%2C%22path%22%3A%22%2F%22%7D%7D; atidvisitor=%7B%22name%22%3A%22atidvisitor%22%2C%22val%22%3A%7B%22vrn%22%3A%22-501001-%22%7D%2C%22options%22%3A%7B%22path%22%3A%22%2F%22%2C%22session%22%3A15724800%2C%22end%22%3A15724800%7D%7D; _ga=GA1.2.979024223.1580289614; didomi_token=eyJ1c2VyX2lkIjoiMTZmZjA5OWQtYmFkMy02NTZjLWJmZmItYmEyZWMxMzc3Y2EzIiwiY3JlYXRlZCI6IjIwMjAtMDEtMjlUMDk6MjA6MTMuNzQzWiIsInVwZGF0ZWQiOiIyMDIwLTAxLTI5VDA5OjIwOjE1LjcxMloiLCJ2ZW5kb3JzIjp7ImVuYWJsZWQiOlsiZ29vZ2xlIl0sImRpc2FibGVkIjpbXX0sInB1cnBvc2VzIjp7ImVuYWJsZWQiOlsiY29va2llcyIsImFkdmVydGlzaW5nX3BlcnNvbmFsaXphdGlvbiIsImNvbnRlbnRfcGVyc29uYWxpemF0aW9uIiwiYWRfZGVsaXZlcnkiLCJhbmFseXRpY3MiXSwiZGlzYWJsZWQiOltdfX0=; euconsent=BOt7Q8JOt7Q8dAHABBENCv-AAAAsuABAI4A; FRANCEMARCHES=90vk2245mbbu8s940mrtmb2jo0; _gid=GA1.2.1591240251.1580630831; _gat_UA-3779573-2=1; _gat_UA-3779573-4=1; _gat_UA-3779573-1=1",
                UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:72.0) Gecko/20100101 Firefox/72.0",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                AcceptLanguage = "en-US,en;q=0.5",
                CacheControl = "max-age=0",
                Host = "www.francemarches.com",
                AcceptEncoding  ="gzip, deflate, br",
                UseCompression = true
            });
    }

        [TestMethod]
        public void AssertScrapeResultsWithKeywords()
        {
            var outcome = scrapeService.ExecuteAsync(targetList).Result;
            Assert.IsTrue(outcome.Any());
        }


        [TestMethod]
        public void AssertScrapeResultsWithoutKeywords()
        {
            var outcome = scrapeService.ExecuteAsync(targetListNoKeywords).Result;
            Assert.IsTrue(outcome.Any());
        }

        [TestMethod]
        public void AssertScrapeResultsWithHeaders()
        {
            var outcome = scrapeService.ExecuteAsync(targetListWithHeaders).Result;
            Assert.IsTrue(outcome.Any());
        }
    }
}
