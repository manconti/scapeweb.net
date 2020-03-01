using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapeWeb.Models;

namespace ScrapeWeb.Services
{
    public sealed class ScrapeService
    {
        public List<LogItem> Logs { get; private set; }

        public delegate void Log();

        public event Log LogEvent;

        public ScrapeService()
        {
            this.Logs = new List<LogItem>();
            InternalLog(LogType.Info, "ScrapeService initiated");
        }

        public async Task<IList<Outcome>> ExecuteAsync(IList<Target> sources)
        {
            List<Outcome> results = new List<Outcome>();

            foreach (var source in sources)
            {
                InternalLog(LogType.Message, $"Starting scraping {source.Url}");
                var doc = new HtmlDocument();

                var pageSource = await DownloadPageAsync(source);
                if (!String.IsNullOrEmpty(pageSource)) //check if the source has been correctly downloaded
                {

                    doc.LoadHtml(pageSource);
                    InternalLog(LogType.Message, $"Page downloaded ({source.Title})");

                    var nodes = doc.DocumentNode.SelectNodes(source.SearchingElement);
                    if (nodes != null)
                    {
                        foreach (var node in nodes) //we now have the big block
                        {
                            bool wordsFound = false;

                            Outcome result = new Outcome()
                            {
                                Url = source.Url,
                                Title = source.Title
                            };

                            //searching the text in the big block, regardless, unless keywords are not required
                            if (!source.UseKeywords) //NOTE > double negation. Logically inverted
                            {
                                InternalLog(LogType.Info, $"No keywords to be found");
                                wordsFound = true;
                            }
                            else
                            {
                                foreach (var keyword in source.Keywords)
                                {
                                    if (node.InnerText.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        wordsFound = true;
                                        break;
                                    }
                                }
                            }


                            if (wordsFound)
                            {
                                //descendant in single node
                                foreach (var findingElement in source.SingleDescendantElementsToFind)
                                {
                                    var descendantElement = node.SelectSingleNode(findingElement);
                                    if (descendantElement != null)
                                    {
                                        var modifiedText = descendantElement.InnerText;

                                        foreach (var keyword in source.Keywords)
                                        {
                                            modifiedText = modifiedText.Replace(keyword, $"<strong style='background-color:yellow'>{keyword}</strong>", StringComparison.InvariantCultureIgnoreCase);
                                        }

                                        result.HighlightedOutputElements.Insert(0, modifiedText);
                                        result.TextOnlyOutputElements.Insert(0, RemoveLineEndings(descendantElement.InnerText));
                                    }
                                }

                                //descendant in multiple nodes
                                foreach (var findingElement in source.MultipleDescendantElementsToFind)
                                {
                                    var descendantElements = node.SelectNodes(findingElement);
                                    if (descendantElements != null)
                                    {

                                        foreach (var descendantElement in descendantElements)
                                        {
                                            var modifiedText = descendantElement.InnerText;

                                            foreach (var keyword in source.Keywords)
                                            {
                                                modifiedText = modifiedText.Replace(keyword, $"<strong style='background-color:yellow'>{keyword}</strong>", StringComparison.InvariantCultureIgnoreCase);
                                            }

                                            result.HighlightedOutputElements.Insert(0, modifiedText);
                                            result.TextOnlyOutputElements.Insert(0, RemoveLineEndings(descendantElement.InnerText));
                                        }
                                    }
                                }
                            }
                            results.Add(result);

                        }


                    }

                    else
                    {
                        InternalLog(LogType.Warning, "No elements found");
                    }
                }
            }

            InternalLog(LogType.Success, $"Scraping completed for {results.Count} elements");
            return results;
        }

        public string RemoveLineEndings(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();


            while (true)
            {
                if (value.Contains("  "))
                {
                    value = value.Replace("  ", string.Empty);
                }
                else
                {
                    break;
                }

            }

            return value.Replace("\r\n", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace(lineSeparator, string.Empty)
                        .Replace(paragraphSeparator, string.Empty)
                        .Replace("&nbsp;", String.Empty)
                        .Trim();

        }

        async Task<string> DownloadPageAsync(Target source)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(source.Url);
            req.Timeout = Constants.WebRequestMillisecondTimeout;
            req.Method = source.WebRequestMethod;
            req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None; //TODO authentication to be implemented
            InternalLog(LogType.Info, $"Downloading {source.Url}");
            if (source.UseHeaders)
            {
                InternalLog(LogType.Info, $"Using Headers for {source.Url}");
                if (!String.IsNullOrWhiteSpace(source.Cookie))
                {
                    CookieContainer cookieContainer = new CookieContainer();
                    cookieContainer.SetCookies(new Uri(source.Url), source.Cookie);
                }

                if (!string.IsNullOrWhiteSpace(source.UserAgent)) { req.Headers.Add(HttpRequestHeader.UserAgent, source.UserAgent); }
                if (!string.IsNullOrWhiteSpace(source.Accept)) { req.Headers.Add(HttpRequestHeader.Accept, source.Accept); }
                if (!string.IsNullOrWhiteSpace(source.AcceptLanguage)) { req.Headers.Add(HttpRequestHeader.AcceptLanguage, source.AcceptLanguage); }
                if (!string.IsNullOrWhiteSpace(source.CacheControl)) { req.Headers.Add(HttpRequestHeader.CacheControl, source.CacheControl); }
                if (!string.IsNullOrWhiteSpace(source.Host)) { req.Headers.Add(HttpRequestHeader.Host, source.Host); }
                if (!string.IsNullOrWhiteSpace(source.AcceptEncoding)) { req.Headers.Add(HttpRequestHeader.AcceptEncoding, source.AcceptEncoding); }
            }

            if (source.UseCompression)
            {
                InternalLog(LogType.Info, $"Using Compression for {source.Url}");
                req.Headers[HttpRequestHeader.AcceptEncoding] = Constants.WebRequestCompressionEncoding;
                req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            }
            try
            {

                using (var wresponse = await req.GetResponseAsync().ConfigureAwait(true))
                {
                    using (StreamReader reader = new StreamReader(wresponse.GetResponseStream(), Encoding.Default))
                    {
                        return reader.ReadToEnd();
                    }
                }

            }
            catch (Exception e)
            {
                InternalLog(LogType.Error, $"Error {e.Message}");
                InternalLog(LogType.Error, e.StackTrace);
                return String.Empty;
            }



        }


        private void InternalLog(LogType type, string message)
        {
            Logs.Insert(0, new LogItem()
            {
                Message = message,
                Type = type,
                DateTime = DateTime.UtcNow
            });

            if (LogEvent != null)
                LogEvent();
        }
    }
}
