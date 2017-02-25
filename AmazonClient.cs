using System;
using NKCraddock.AmazonItemLookup.Client;

namespace BargainBot
{
    public class AmazonClient
    {
        private static readonly string AwsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        private static readonly string AwsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        private static readonly string AwsAssociateTag = Environment.GetEnvironmentVariable("AWS_ASSOCIATE_TAG");

        public double? GetPriceByAsin(string asin)
        {
            return GetClient().ItemLookupByAsin(asin).OfferPrice;
        }

        private static AwsProductApiClient GetClient()
        {

            var client = new AwsProductApiClient(new ProductApiConnectionInfo
            {
                AWSAccessKey = AwsAccessKey,
                AWSSecretKey = AwsSecretKey,
                AWSAssociateTag = AwsAssociateTag,
                //http://docs.aws.amazon.com/AWSECommerceService/latest/DG/SOAPEndpoints.html
                AWSServerUri = "webservices.amazon.ca"
            });

            return client;
        }

    }
}