using RandomPayloadAssignment.Models;

namespace RandomPayloadAssignment.Blob;
public interface IBlobStorage
{
    public Task<RandomPayload>Get(string blobName);
    public Task<string> Upload(byte[] data);
}
