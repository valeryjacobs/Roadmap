using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net.Http;

namespace SignatureService.Controllers
{
    [Route("api/[controller]")]
    public class SignatureController : Controller
    {
        [Route("getcontainersignature/{clientId}")]
        public String GetContainerSignature(int clientId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=vjdemodata;AccountKey=c0x/avvhx8/5Dw0W/K1PnqI4xaD85vxRyupDNG+sDJV5w2S0hOVjBNKFpVWCHi2swZQVqOdbIKGgt+HLo2gy+w==;EndpointSuffix=core.windows.net");

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference("sascontainer");
            container.CreateIfNotExistsAsync();

            return GetContainerSasUri(container);
        }

        [Route("getblobsignature/{clientId}/{blobId}")]
        public async Task<string> GetBlobSignature(int clientId, string blobURI)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=vjdemodata;AccountKey=c0x/avvhx8/5Dw0W/K1PnqI4xaD85vxRyupDNG+sDJV5w2S0hOVjBNKFpVWCHi2swZQVqOdbIKGgt+HLo2gy+w==;EndpointSuffix=core.windows.net");

            //Create the blob client object.
            CloudBlobClient blobClient =  storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlob blob =  (CloudBlob)blobClient.GetBlobReferenceFromServerAsync(new Uri(blobURI)).Result;

            return GetBlobSasUri(blob);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int clientId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=vjdemodata;AccountKey=c0x/avvhx8/5Dw0W/K1PnqI4xaD85vxRyupDNG+sDJV5w2S0hOVjBNKFpVWCHi2swZQVqOdbIKGgt+HLo2gy+w==;EndpointSuffix=core.windows.net");

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference("sascontainer");
            container.CreateIfNotExistsAsync();

            return GetContainerSasUri(container);
        }

        static string GetContainerSasUri(CloudBlobContainer container)
        {
            //Set the expiry time and permissions for the container.
            //In this case no start time is specified, so the shared access signature becomes valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Write;

            //Generate the shared access signature on the container, setting the constraints directly on the signature.
            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return container.Uri + sasContainerToken;
        }

        static string GetBlobSasUri(CloudBlob blob)
        {
            //Set the expiry time and permissions for the container.
            //In this case no start time is specified, so the shared access signature becomes valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Write;

            //Generate the shared access signature on the container, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }

        [HttpPost]
        [Route("someinputmethod")]
        public HttpResponseMessage SomeInputMethod([FromBody]InputBody inputBody)
        {
            var x = inputBody.headerData;
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        public class InputBody
        {
            public Dictionary<string, string> headerData { get; set; }
            public Dictionary<string, string> rowData { get; set; }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
