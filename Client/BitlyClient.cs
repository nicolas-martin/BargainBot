using System;
using System.Web;
using BargainBot.Helper;
using BitlyDotNET.Implementations;
using Quartz.Util;

namespace BargainBot.Client
{
    public class BitlyClient
    {
        private static string _account = Environment.GetEnvironmentVariable("BITLY_ACCOUNT");
        private static string _apiKey = Environment.GetEnvironmentVariable("BITLY_KEY");
        private BitlyService _bitlyService;

        public BitlyClient()
        {
            _bitlyService = new BitlyService(_account, _apiKey);
        }

        public string ShortenAndAddRefToUrl(string url)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            var testPath = uriBuilder.Path;
            var replaced = "";

            if (testPath.Contains("ref="))
            {
                var refEqPos = testPath.IndexOf("ref=", StringComparison.Ordinal);
                replaced = testPath.Replace(testPath.Substring(refEqPos, testPath.Length - refEqPos), $"ref={Constants.Referral}");
            }
            else
            {
                replaced = testPath + $"ref={Constants.Referral}";
            }


            uriBuilder.Query = query.ToString();
            uriBuilder.Path = replaced;
            var endUrl = uriBuilder.ToString();
            var shortedEndUrl = "";

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                shortedEndUrl = _bitlyService.Shorten(endUrl);
            }

            return shortedEndUrl;
        }
    }
}