using RandomPayloadAssignment.Azure;
using RandomPayloadAssignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace RandomPayloadAssignment.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsController : ControllerBase
{
    private readonly ILogsTable LogsTable;
    public LogsController(ILogsTable logsTable)
    {
        LogsTable = logsTable;
    }


    [HttpGet]
    public IActionResult Get(DateTime? from, DateTime? to)
    {
        var result = LogsTable.GetLogs(from, to);

        var logs = result.Select(x => new Log
        {
            Text = x.Text,
            Timestamp = x.Timestamp.DateTime.ToString("yyyy-MM-dd HH-mm-ss"),
            WasSuccessful = x.WasSuccessful
        }).ToArray();

        return Ok(logs);
    }
}
