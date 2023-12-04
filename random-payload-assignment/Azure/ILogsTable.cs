using RandomPayloadAssignment.Models;

namespace RandomPayloadAssignment.Azure;
public interface ILogsTable
{
    public IEnumerable<LogEntity> GetLogs(DateTime? from, DateTime? to);
    public void StoreLog(LogEntity log);
}