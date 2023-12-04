using Microsoft.Azure.Cosmos.Table;

namespace RandomPayloadAssignment.Models;
public class LogEntity : TableEntity
{
    public bool WasSuccessful { get; set; }
    public string Text { get; set; }
}
