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
using System.Net.Http.Headers;
using Azure.Storage.Blobs;

namespace AzureCosmos
{
    public static class PostFile
    {
        [FunctionName("PostFile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
             [CosmosDB(
            databaseName: "LippertCosmosDB",
            collectionName: "LippertCosmosDBContainer",
            ConnectionStringSetting = "CosmosConnectionString")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            var requestBody = await req.ReadFormAsync();
            name = name ?? requestBody["name"];
            string description = requestBody["description"];
            string category = requestBody["category"];
            string newId = System.Guid.NewGuid().ToString();
            var file = req.Form.Files["file"];  
            
            if (!string.IsNullOrEmpty(name))
            {
                // Add a JSON document to the output container.
                await documentsOut.AddAsync(new
                {
                    // create a random ID
                    id = newId,
                    category = category,
                    name = name,
                    description = description,
                    file = file

                });
            }

            //string Connection = Environment.GetEnvironmentVariable("bolbConnectionString");
            //string containerName = Environment.GetEnvironmentVariable("filescontainter");
            //Stream myBlob = new MemoryStream();
           
            //myBlob = file.OpenReadStream();
            //var blobClient = new BlobContainerClient(Connection, containerName);
            //var blob = blobClient.GetBlobClient(file.FileName);
            //await blob.UploadAsync(myBlob);
            

            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.message = $"This HTTP triggered function executed successfully. FileName: {file.FileName} - FileID: {newId}";
            responseMessage.id = newId;
            responseMessage.content = String.Empty;

            return new OkObjectResult(responseMessage);
        }
    }
}
