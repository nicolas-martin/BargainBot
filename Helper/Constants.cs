using System.Configuration;

namespace BargainBot.Helper
{
    public static class Constants
    {
        public static class Bitly
        {
            public static readonly string Account = ConfigurationManager.AppSettings["BitlyAccount"];
            public static readonly string ApiKey = ConfigurationManager.AppSettings["BitlyApiKey"];
        }

        public static class Amazon
        {
            public static readonly string AccessKey = ConfigurationManager.AppSettings["AmazonAccessKey"];
            public static readonly string SecretKey = ConfigurationManager.AppSettings["AmazonSecretKey"];
            public static readonly string AssociateTag = ConfigurationManager.AppSettings["AmazonAssociateTag"];

            //TODO: Find a more reliable way to get these.
            public const string FlakyImageUrlPattern = "http://images.amazon.com/images/P/{0}.01._SCMZZZZZZZ_.jpg";

            public const string ImagePattern2 = "http://images.amazon.com/images/I/{0}";

            public static class ItemAttribute
            {
                public const string Title = "Title";
                public const string Feature = "Feature";
            }
        }
    }
}