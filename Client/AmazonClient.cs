using System;
using System.Diagnostics;
using System.Linq;
using BargainBot.Helper;
using BargainBot.Model;
using NKCraddock.AmazonItemLookup;
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

        //Test out stuff here http://webservices.amazon.com/scratchpad/index.html
        public AmazonClient(BitlyClient bitlyClient)
        {
            _bitlyClient = bitlyClient;
            _awsProductApiClient = new AwsProductApiClient(new ProductApiConnectionInfo
            {
                AWSAccessKey = AwsAccessKey,
                AWSSecretKey = AwsSecretKey,
                AWSAssociateTag = AwsAssociateTag,
                //http://docs.aws.amazon.com/AWSECommerceService/latest/DG/SOAPEndpoints.html
                //TODO: Can only use my credentials on .com
                AWSServerUri = "webservices.amazon.com"
            });
        }

        public double GetPriceByAsin(string asin)
        {
            var itemLookupByAsin = _awsProductApiClient.ItemLookupByAsin(asin);
            var sanOfferPrice = itemLookupByAsin.OfferPrice ?? 0.00;
            double sanitzedPrice = sanOfferPrice == 0.00 ? (itemLookupByAsin.ListPrice ?? 0) : sanOfferPrice;
            return sanitzedPrice;
        }

        public Deal GetDeal(string asin)
        {
            var amazonItem = _awsProductApiClient.ItemLookupByAsin(asin);

            var shortenedAndTaggedUrl = _bitlyClient.ShortenAndAddTagToUrl(amazonItem.DetailPageURL);

            //TODO: Sometimes the offer price is null?
            var sanOfferPrice = amazonItem.OfferPrice ?? 0.00;
            double sanitzedPrice = sanOfferPrice == 0.00 ? (amazonItem.ListPrice ?? 0) : sanOfferPrice;

            var initialUrl = amazonItem.PrimaryImageSet.Images.FirstOrDefault(x => x.Type == AwsImageType.MediumImage)?.URL;
            var imageCode = initialUrl.Split('/').Last();

            return new Deal
            {
                Id = Guid.NewGuid(),
                Name = amazonItem.ItemAttributes.FirstOrDefault(x => x.Key.Equals(Constants.Amazon.ItemAttribute.Title)).Value,
                Code = amazonItem.ASIN,
                Price = sanitzedPrice,
                DateCreated = DateTime.UtcNow,
                ShortenUrl = shortenedAndTaggedUrl,
                ImageUrl = string.Format(Constants.Amazon.ImagePattern2, imageCode),
                IsActive = true
            };
        }
    }
}