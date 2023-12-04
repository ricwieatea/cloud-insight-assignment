using Azure.Storage.Blobs;
using RandomPayloadAssignment.Models;

namespace RandomPayloadAssignment.Blob
{
    public class BlobStorage : IBlobStorage
    {
        const string CONNECTION_STRING = "UseDevelopmentStorage=true";
        const string BLOB_CONTAINER_NAME = "cloud-insight-assignment";

        public async Task<RandomPayload> Get(string blobName)
        {
                var container = new BlobContainerClient(CONNECTION_STRING, BLOB_CONTAINER_NAME);
                BlobClient blobClient = container.GetBlobClient(blobName);

                var blobDownload = await blobClient.DownloadContentAsync();

                if (blobDownload.Value == null || blobDownload.Value.Content == null)
                    return new RandomPayload();

                var result = blobDownload.Value.Content.ToObjectFromJson<RandomPayload>();

                return result;
        }

        public async Task<string> Upload(byte[] data)
        {
            var container = new BlobContainerClient(CONNECTION_STRING, BLOB_CONTAINER_NAME);
            var fileName = $"sync-{DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss")}.json";
            BlobClient blobClient = container.GetBlobClient(fileName);

            using (var stream = new MemoryStream(data))
            {
                await blobClient.UploadAsync(stream);
            }

            return fileName;
        }
    }
}
