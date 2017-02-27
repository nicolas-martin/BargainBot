using System;
using System.Web;
using BargainBot.Helper;
using BitlyDotNET.Implementations;

namespace BargainBot.Client
{
    public class BitlyClient
    {
        private static string _account = Constants.Bitly.Account;
        private static string _apiKey = Constants.Bitly.ApiKey;
        private BitlyService _bitlyService;

        public BitlyClient()
        {
            _bitlyService = new BitlyService(_account, _apiKey);
        }

        public string ShortenAndAddRefToUrl(string url)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["tag"] = Constants.Amazon.AssociateTag;

            uriBuilder.Query = query.ToString();

            //TODO: Validate that it works 
            //https://affiliate-program.amazon.com/home/tools/linkchecker
            var endUrl = uriBuilder.ToString();

            var shortedEndUrl = "";

            if (Uri.IsWellFormedUriString(endUrl, UriKind.Absolute))
            {
                shortedEndUrl = _bitlyService.Shorten(endUrl);
            }

            return shortedEndUrl;
        }
    }
}