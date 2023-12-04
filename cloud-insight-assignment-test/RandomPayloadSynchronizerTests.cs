using RandomPayloadAssignment.Azure;
using RandomPayloadAssignment.Blob;
using RandomPayloadAssignment.Http;
using RandomPayloadAssignment.Models;
using RandomPayloadAssignment.Synchronizers;
using Moq;
using System.Text.Json;

namespace CloudInsightAssignmentTest;
public class RandomPayloadSynchronizerTests
{

    [Fact]
    public async void SyncWasSuccesful()
    {
        var httpRequest = new Mock<IHttpRequest>();
        var blobStorage = new Mock<IBlobStorage>();
        var logsTable = new Mock<ILogsTable>();

        var randomPayload = new RandomPayload()
        {
            Count = 1,
            Entries = new List<Entry> { new Entry {
                Api = "api",
                Auth = "auth",
                Category = "category",
                Cors = "cors",
                Description = "description",
                Https = false,
                Link = "link"
            } }.ToArray()
        };


        httpRequest.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(JsonSerializer.Serialize(randomPayload));
        blobStorage.Setup(x => x.Upload(It.IsAny<byte[]>())).ReturnsAsync("blob-name");
        logsTable.Setup(x => x.StoreLog(It.IsAny<LogEntity>())).Verifiable();

        var sync = new Synchronizer(httpRequest.Object, blobStorage.Object, logsTable.Object);

        await sync.FetchDataEveryMinute();

        httpRequest.Verify(x => x.Get(It.IsAny<string>()), Times.Once());
        blobStorage.Verify(x => x.Upload(It.IsAny<byte[]>()), Times.Once());
        logsTable.Verify(x => x.StoreLog(It.IsAny<LogEntity>()), Times.Once());
    }

    [Fact]
    public async void SyncWasUnsuccessful()
    {
        var httpRequest = new Mock<IHttpRequest>();
        var blobStorage = new Mock<IBlobStorage>();
        var logsTable = new Mock<ILogsTable>();

        httpRequest.Setup(x => x.Get(It.IsAny<string>())).ThrowsAsync(new Exception());
        blobStorage.Setup(x => x.Upload(It.IsAny<byte[]>())).Verifiable();
        logsTable.Setup(x => x.StoreLog(It.IsAny<LogEntity>())).Verifiable();

        var sync = new Synchronizer(httpRequest.Object, blobStorage.Object, logsTable.Object);

        var ex = await Assert.ThrowsAsync<Exception>(() => sync.FetchDataEveryMinute());
        
        httpRequest.Verify(x => x.Get(It.IsAny<string>()), Times.Once());
        blobStorage.Verify(x => x.Upload(It.IsAny<byte[]>()), Times.Never());
        logsTable.Verify(x => x.StoreLog(It.IsAny<LogEntity>()), Times.Once());

    }
}
