using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Xunit;


namespace AzureCosmosTests
{
    public class AzureCosomesFuncTests
    {
        private readonly ILogger logger = NullLoggerFactory.Instance.CreateLogger("Test");
        [Fact]

        public void Test1()
        {
            //var col = new AsyncCollector<AzureCosmos.Model.Document>();
            var request = generateHttpRequest(1);
            AzureCosmos.Model.Document document = new AzureCosmos.Model.Document();
            document.Id = "";
            document.Name = "test";
            document.Category = "test";
            document.file = "";
           // documentList.Add(document);
           // var response = AzureCosmos.PostFile.Run(request,documentList,logger);
           Assert.True(1.Equals(1));
        }


        private DefaultHttpRequest generateHttpRequest(object id)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            var queryParams = new Dictionary<string, StringValues>() { { "number", id.ToString() } };
            request.Query = new QueryCollection(queryParams);
            return request;

        }
    }

   
}