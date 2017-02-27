using System;
using System.Linq;
using BargainBot.Helper;
using BargainBot.Model;
using NKCraddock.AmazonItemLookup.Client;

namespace BargainBot.Client
{
    public class AmazonClient
    {
        private static readonly string AwsAccessKey = Constants.Amazon.AccessKey;
        private static readonly string AwsSecretKey = Constants.Amazon.SecretKey;
        private static readonly string AwsAssociateTag = Constants.Amazon.AssociateTag;

        private AwsProductApiClient _awsProductApiClient;
        private readonly BitlyClient _bitlyClient;

        public AmazonClient(BitlyClient bitlyClient)
        {
            _bitlyClient = bitlyClient;
            _awsProductApiClient = new AwsProductApiClient(new ProductApiConnectionInfo
            {
                AWSAccessKey = AwsAccessKey,
                AWSSecretKey = AwsSecretKey,
                AWSAssociateTag = AwsAssociateTag,
                //http://docs.aws.amazon.com/AWSECommerceService/latest/DG/SOAPEndpoints.html
                AWSServerUri = "webservices.amazon.ca"
            });
        }

        public double? GetPriceByAsin(string asin)
        {
            return _awsProductApiClient.ItemLookupByAsin(asin).OfferPrice;
        }

        public Deal GetDeal(string asin)
        {
            var amazonItem = _awsProductApiClient.ItemLookupByAsin(asin);

            var shortenedAndTaggedUrl = _bitlyClient.ShortenAndAddTagToUrl(amazonItem.DetailPageURL);

            return new Deal
            {
                Id = Guid.NewGuid(),
                Name = "??" + amazonItem.Description,
                Code = amazonItem.ASIN,
                Price = amazonItem.OfferPrice,
                DateCreated = DateTime.UtcNow,
                Url = amazonItem.DetailPageURL,
                ShortenUrl = shortenedAndTaggedUrl,
                ImageUrl = amazonItem.PrimaryImageSet.Images.FirstOrDefault().URL
            };
        }
    }
}