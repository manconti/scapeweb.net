# scrapeweb.net
.Net Core version of a configurable and modular web scraper. Able to scrape multiple pages and having multiple output options.
Scraping web sites is something that has to be done carefully and responsibly. ScapeWeb.net cannot be considered responsible for any use that is not respecting the laws or that is generating illegal activity. 
This tool has been built as an utility to retrieve in a responsible way specific content for a number of web site and to collect them in a human readable format.

It uses:
- HtmlAgilityPack 
- CsvHelper
- NewtonSoft.Json
- SendGrid API 

## The ScrapeWeb Library
The library is composed by 3 tdata models as well as 3 types of services.

## The Data Models
- Target: the configuration of a page that ScrapeWeb will analyse. The system will accept a collection of Targets. 
- SystemConfiguration: the overall configuration of the application: if a CSV needs to be generated, the email needs to be sent etc... 
- Outcome: it contains all the data extracted by ScrapeWeb. 

### Target
A target is composed by:
- An URL 
- A Ttle
- An HTML block that is selected as entry point. 
- A list of elements that will be searched within the main HTML block. Two types are allowed: 
    - SingleNode: Scrapeweb will take only the first node found in the selected elements. 
    - MultipleNodes: Scrapeweb will retrieve all the nodes that matches the search criteria
- A list of Keywords: Scrapeweb is capable of selecting (within the HTML block) specific keywords and highligh them 
- Some web site requires additional headers: ScrapeWeb allows to supply them specifically for each target. 

### System configuration
The system Configuration represents the overall configuration for the entire application. It is not required for the scraping, but it is mandatory for sending the emails. 
A System Configuration is composed by:
- SendEmail: in the ScrapeWeb.Application it will be used for sending or not the email. 
- GenerateCsv: in the ScrapeWeb.Application it will be used for generating the CSV file. 
- EmailConfiguration: a class that allows either to use SendGrid API or to use standard SMTP sending. It containss 
- EmailMessage: It contains the main fields for the email message (Sender, Recipiens, Subject) as well a the email template (see below) and the list of attachments. 

### Outcome
Scrapeweb will generate a list of Outcome as result of the scraping activity. An Outcome is composed by:
- Title: it is the same title of the Target class.
- Url: it is the same url of the Target class. 
- KeywordsFound: return is - as part of the results - any keywords has been found. 
- HighlightedOutputElements: the collection of the elements extracted by Scrapeweb, as HTML value, where the keywords are highlighted as STRONG as well as with yellow background. This property is used by the EmailService, for easier visualisation. 
- TextOnlyOutputElements: same values of the HighlightedOutputElements, but only text. This property is used by the CsvService.

## The Services
- ScrapeService: it accept a list of Target data and it returns a list of Outcome. It will scrape the sources and return the findings.
- EmailService: it accept a SystemConfiguration (including email configuration) and a list of Outcomes. It sends an email, attaching (or not) the CSV generated
- CsvService: it accepts a list of Outcome and returns a MemoryStream of a generated CSV


# The Email Template
The EmailTemplate allows to fully customise how the email will be sent, it's HTML content and structure. The provided template in the Scrapeweb.Application is fully responsive and already ready to be used. 
The email template is composed by 3 elements:
- The Body template: the main full html body
- the Item template: it represent a Target element (= one Url)
- The Rows template: within each item, it will display all rows founds

## The Body Template
The body template, besides the standard Html structure has to contain a text placeholder called %ITEMTEMPLATE% that will be populated by the EmailService.

## The Item Template
The Item template must contain two text placeholders: 
- %HREF%, pointing to the Url of the target configuration. 
- %SOURCE%, the Title of the target configuration. 

## The Row Template
The Row template must contain a text placeholder called %FINDING% that will be populated with the findings (properly highhlighted). If no keywords are used all HTML block found will be included in the Row Template. 


