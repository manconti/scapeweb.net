namespace ScrapeWeb
{
    public class Constants
    {
        #region Web
        public const int WebRequestMillisecondTimeout = 10000;
        public const string WebRequestCompressionEncoding = "gzip, deflate";
        #endregion

        #region ExceptionMessage;
        public const string ExEmailTemplateNotValid = "The email template fields are not properly provided";
        public const string ExEmailSendingError = "Error sending the email §§";
        #endregion
        #region Email
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
