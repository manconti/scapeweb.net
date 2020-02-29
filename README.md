# scrapeweb.net
.Net Core version of a configurable and modular web scraper. Able to scrape multiple pages and having multiple output options.
Scraping web sites is something that has to be done carefully and responsibly. ScapeWeb.net cannot be considered responsible for any use that is not respecting the laws or that is generating illegal activity. 
This tool has been built as an utility to retrieve in a responsible way specific content for a number of web site and to collect them in a human readable format.

## The ScrapeWeb Library
The library is composed by 3 types of services, as well as 3 data models.
- Target: the configuration of a page that ScrapeWeb will analyse. The system will accept a collection of Targets. 
- SystemConfiguration: the overall configuration of the application: if a CSV needs to be generated, the email needs to be sent etc... 
- Outcome: it contains all the data extracted by ScrapeWeb. 

### Target
A target is composed by:
- An URL 
- A Ttle
- An HTML block that is selected as entry point. 
- A list of elements that will be searched within the main HTML block. Two types are allowed: 
- - SingleNode: Scrapeweb will take only the first node found in the selected elements. 
- - MultipleNodes: Scrapeweb will retrieve all the nodes that matches the search criteria
- A list of Keywords: Scrapeweb is capable of selecting (within the HTML block) specific keywords and highligh them 
- Some web site requires additional headers: ScrapeWeb allows to supply them specifically for each target. 

###
