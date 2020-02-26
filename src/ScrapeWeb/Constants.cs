namespace ScrapeWeb
{
    public class Constants
    {

        #region ExceptionMessage;
        public const string ExEmailTemplateNotValid = "The email template fields are not properly provided";
        #endregion

        #region Files&Folder
        public const string OutputFileName = "output.csv";
        public const string DataFolder = "Data";
        public const string DataFile = "data.json";
        public const string ConfigurationFile = "config.json";
        public const string OutputFolder = "Data/Output";
        public const string TemplateFolder = "Data/Templates";
        #endregion
        #region Email
        public const string EmailTemplate = "email.template.html";
        public const string EmailItemTemplate = "item.template.html";
        public const string EmailRowTemplate = "item.row.template.html";

        public const string EmailSubjectMarkup = "%SUBJECT%";
        public const string EmailDateMarkup = "%DATE%";
        public const string EmailDateFormatting = "dd-MM-yyyy";
        public const string EmailSourceMarkup = "%SOURCE%";
        public const string EmailTotalMarkup = "%TOTAL%";
        public const string EmailFindingMarkup = "%FINDING%";
        public const string EmailRowsMarkup = "%ROWS%";
        public const string EmailHrefMarkup = "%HREF%";
        public const string EmailItemTemplateMarkup = "%ITEMTEMPLATE%";


        #endregion
    }
}
