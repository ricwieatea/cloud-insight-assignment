using RandomPayloadAssignment.Blob;
using Microsoft.AspNetCore.Mvc;

namespace RandomPayloadAssignment.Controllers;

[ApiController]
[Route("[controller]")]
public class PayloadController : ControllerBase
{
    private readonly IBlobStorage BlobStorage;
    public PayloadController(IBlobStorage blobStorage)
    {
        BlobStorage = blobStorage;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string blobName)

    {
        var result = await BlobStorage.Get(blobName);
        return Ok(result);
    }
}
