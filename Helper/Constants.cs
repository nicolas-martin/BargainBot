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
        }
    }
}