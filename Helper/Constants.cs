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
            public static readonly string FlakyImageUrlPattern = "http://images.amazon.com/images/P/{0}.01._SCMZZZZZZZ_.jpg";

            public static class ItemAttribute
            {
                public static readonly string Title = "Title";
                public static readonly string Feature = "Feature";
            }
        }
    }
}