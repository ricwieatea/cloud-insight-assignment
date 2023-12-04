using RandomPayloadAssignment.Azure;
using RandomPayloadAssignment.Blob;
using RandomPayloadAssignment.Http;
using RandomPayloadAssignment.Models;
using System.Text.Json;

namespace RandomPayloadAssignment.Synchronizers;

public class Synchronizer : BackgroundService, ISynchronizer
{
    private readonly IHttpRequest HttpRequest;
    private readonly IBlobStorage BlobStorage;
    private readonly ILogsTable LogsTable;

    const string API_URL = "https://api.publicapis.org/random?auth=null";

    public Synchronizer(IHttpRequest httpRequest, IBlobStorage blobStorage, ILogsTable logsTable)
    {
        HttpRequest = httpRequest;
        BlobStorage = blobStorage;
        LogsTable = logsTable;
    }

    public async Task FetchDataEveryMinute()
    {
        try
        {
            var response = await HttpRequest.Get(API_URL);

            if (string.IsNullOrWhiteSpace(response))
            {
                return;
            }

            var result = JsonSerializer.Deserialize<RandomPayload>(response);
            if (result.Entries.Length < 1)
                return;

            var resultToBytes = JsonSerializer.SerializeToUtf8Bytes(result);

            var fileName = await BlobStorage.Upload(resultToBytes);

            CreateLog(true, $"Sucessfully stored payload '{fileName}'");
        }
        catch (Exception e)
        {
            CreateLog(false, $"Failed to store payload - Exception {e.Message}");
            throw;
        }
    }

    void CreateLog(bool wasSuccessful, string message)
    {
        LogsTable.StoreLog(new LogEntity
        {
            PartitionKey = "Log",
            RowKey = Guid.NewGuid().ToString(),
            WasSuccessful = wasSuccessful,
            Text = message,
            Timestamp = DateTimeOffset.UtcNow
        });
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            await FetchDataEveryMinute();
            await Task.Delay(TimeSpan.FromSeconds(60));
        }
    }
}
