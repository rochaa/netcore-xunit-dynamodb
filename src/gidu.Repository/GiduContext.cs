using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace gidu.Repository
{
    public class GiduContext : DynamoDBContext
    {
        public GiduContext(IAmazonDynamoDB client) : base(client) { }
    }
}