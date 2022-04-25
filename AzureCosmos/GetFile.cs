using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureCosmos.Model;

namespace AzureCosmos
{
    public static class GetFile
    {
        [FunctionName("GetFile")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
            databaseName: "LippertCosmosDB",
            collectionName: "LippertCosmosDBContainer",
            ConnectionStringSetting = "CosmosConnectionString",
              Id = "{Query.id}",
              PartitionKey = "{Query.id}"
           // )]Document document,
          )] string document,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            dynamic data = JsonConvert.DeserializeObject(document);
            if (document == null)
            {
                log.LogInformation($"File not found");
            }
            else
            {
                log.LogInformation($"FileName={data["name"]}");
            }

            ResponseMessage response = new ResponseMessage();
            response.id = data["id"];
            response.message = "File has been retreived";
            response.content = data.ToString();
            string filePath = data["file"];
            string fileName = data["name"];
            log.LogInformation(filePath);
            Azure.Storage.Blobs.BlobClient blob = new Azure.Storage.Blobs.BlobClient(
                connectionString: Environment.GetEnvironmentVariable("bolbConnectionString"),
                blobContainerName: "filescontainter",
                blobName: fileName

                ); ;

            blob.DownloadContentAsync().Wait();
            return new OkObjectResult(data);
        }
    }
}