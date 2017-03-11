using System;
using System.Linq;
using System.Text.RegularExpressions;
using Quartz.Util;

namespace BargainBot.Helper
{
    public static class UrlValidator
    {
        public static string GetAsin(string url)
        {
            const string pattern = @"(http|https)?(www.)?amazon.[a-zA-Z.]+/([\w-]+/)?(dp|gp/product|exec/obidos/asin)/(\w+/)?(\w{10})";
            var r = new Regex(pattern, RegexOptions.IgnoreCase);
            var captured = r.Match(url);

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute) && !captured.Groups[6].Value.IsNullOrWhiteSpace())
            {
                return captured.Groups[6].Value;
            }

            return "";
        }

        public static bool IsCountryFromUrlAllowed(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return false;
            }

            var urlBuilder = new UriBuilder(url);
            var host = urlBuilder.Host;

            //TODO: Support other domains
            if (host.Split('.').Last() != "com")
            {
                return false;
            }

            return true;

        }
    }
}