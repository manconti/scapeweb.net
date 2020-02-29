using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using ScrapeWeb.Models;

namespace ScrapeWeb.Services
{
    public sealed class CsvService: IDisposable
    {
        MemoryStream memoryStream;

        public CsvService()
        {


        }
        public MemoryStream Generate(IList<Outcome> contentList)
        {
            memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            var csv = new CsvWriter(streamWriter, CultureInfo.CurrentCulture);

            csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;

            foreach (var r in contentList)
            {

                if (r.TextOnlyOutputElements.Any())
                {
                    csv.WriteField(r.Title);
                    csv.WriteField(r.Url);


                    foreach (var element in r.TextOnlyOutputElements)
                    {

                        if (r.TextOnlyOutputElements.Any())
                        {
                            csv.WriteField(element);
                            //csv.WriteField(element.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\t\t", string.Empty).Replace("\t", string.Empty), true);
                        }
                    }
                    csv.NextRecord();
                }
            }
            memoryStream.Position = 0;
            return memoryStream;

        }

        void IDisposable.Dispose()
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }
}
