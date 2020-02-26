
namespace ScrapeWeb.Models
{
    /// <summary>
    /// It represents the configuration of a page that ScrapeWeb will analyse.
    /// </summary>
    public sealed class Target
    {
        /// <summary>
        /// Get or Set the descriptive title of the extract, used in the CSV and Email for reference.
        /// </summary>
        public string Title;
        /// <summary>
        /// Get or Set the full URL of the page to scrape.
        /// </summary>
        public string Url;
        /// <summary>
        /// Get or Set the value indicating if the <see cref="SearchingElement"/> has to be selected based on a set of <see cref="Keywords"/>.
        /// If false, the <see cref="SearchingElement"/> will be selected entirely. Default is false;
        /// </summary>
        public bool UseKeywords;
        /// <summary>
        /// Get or Set the value indicating if the page has to be retrieving using the compression/decompression for reading the content. Default is false.
        /// </summary>
        public bool UseCompression;
        /// <summary>
        /// Get or Set the value indicating if ScraperWeb has to use additional headers to made the web request. Default is false. 
        /// </summary>
        public bool UseHeaders;
        /// <summary>
        /// Get or Set the main html block that will be extracted from the target #<see cref="Url"/>.
        /// The descendant elements will be found within this  <see cref="SearchingElement"/>
        /// </summary>
        public string SearchingElement;
        /// <summary>
        /// Get or Set the collection of the single element that will be searched as part of the <see cref="SearchingElement"/>.
        /// ScrapeWeb will select the first node in the collection. A single node selection will be executed.
        /// The values are x-path query like as ".//h2[contains(@class, 'Title')]". Check https://html-agility-pack.net/ for reference. 
        /// </summary>
        public string[] SingleDescendantElementsToFind;
        /// <summary>
        /// Get or Set the collection of the elements that will be searched as part of the <see cref="SearchingElement"/>.
        /// ScrapeWeb will select all the nodes returned by the query.  
        /// The values are x-path query like as ".//h2[contains(@class, 'Title')]". Check https://html-agility-pack.net/ for reference.
        /// </summary>
        public string[] MultipleDescendantElementsToFind;
        /// <summary>
        /// Get or Set the collection of keywords that ScrapeWeb has to search as part of the <see cref="SearchingElement"/>.
        /// If
        /// </summary>
        public string[] Keywords;
        /// <summary>
        /// Get or Set the value of the Cookie header that will be submitted to the web request.
        /// </summary>
        public string Cookie;
        /// <summary>
        /// Get or Set the value of the Accpet header that will be submitted to the web request.
        /// </summary>
        public string Accept;
        /// <summary>
        /// Get or Set the value of the AcceptLanguage header that will be submitted to the web request.
        /// </summary>
        public string AcceptLanguage;
        /// <summary>
        /// Get or Set the value of the CacheControl header that will be submitted to the web request.
        /// </summary>
        public string CacheControl;
        /// <summary>
        /// Get or Set the value of the Host header that will be submitted to the web request.
        /// </summary>
        public string Host;
        /// <summary>
        /// Get or Set the value of the AcceptEncoding header that will be submitted to the web request.
        /// </summary>
        public string AcceptEncoding;
        /// <summary>
        /// Get or Set the value of the UserAgent header that will be submitted to the web request.
        /// </summary>
        public string UserAgent;   

        public Target()
        {
            this.SingleDescendantElementsToFind = new string[] { };
            this.MultipleDescendantElementsToFind = new string[] { };

            UseCompression = false;
            UseHeaders = false;
            UseKeywords = false;

        }
    }
}
