using System;
using NKCraddock.AmazonItemLookup.Client;

namespace BargainBot.Client
{
    public class AmazonClient
    {
        private static readonly string AwsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        private static readonly string AwsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        private static readonly string AwsAssociateTag = Environment.GetEnvironmentVariable("AWS_ASSOCIATE_TAG");
        private AwsProductApiClient _awsProductApiClient;

        public AmazonClient()
        {
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
            //TODO: Could also get the image to display in the card. Create new class to hold properties
            return _awsProductApiClient.ItemLookupByAsin(asin).OfferPrice;
        }
    }
}