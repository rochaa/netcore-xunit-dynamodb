using System;
using Microsoft.AspNetCore.Mvc;
using Amazon.ElasticBeanstalk;
using Amazon.Runtime;
using System.Threading.Tasks;
using Amazon;
using System.Linq;

namespace gidu.WebAPI.Controllers
{
    [Route("")]
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [Produces("text/html")]
        public async Task<ActionResult<string>> GetAsync()
        {
            var environment = "gidu-api";
            var server = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var textDefault = $"GIDU API's <br> by Jeff <br> 2019 <br>" +
                              $"{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";

            if (server == "Production")
            {
                var credencials = new BasicAWSCredentials(
                    Environment.GetEnvironmentVariable("AWS_AccessKey"),
                    Environment.GetEnvironmentVariable("AWS_SecretKey"));

                var elasticBeanstalkClient = new AmazonElasticBeanstalkClient(credencials, RegionEndpoint.SAEast1);
                var configs = (await elasticBeanstalkClient.DescribeEnvironmentsAsync()).
                        Environments.Where(e => e.EnvironmentName == environment).First();

                return $"{textDefault} <br>" +
                       $"{configs.HealthStatus} <br>" +
                       $"{configs.Status} <br>" +
                       $"{configs.VersionLabel} <br>" +
                       $"{configs.DateUpdated} <br>";
            }

            return textDefault;
        }
    }
}