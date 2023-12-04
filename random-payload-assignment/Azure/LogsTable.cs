using RandomPayloadAssignment.Models;
using Microsoft.Azure.Cosmos.Table;

namespace RandomPayloadAssignment.Azure;
public class LogsTable : ILogsTable
{

    private CloudTable logsTable = null;
    public LogsTable()
    {
        var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");

        var logsTableClient = storageAccount.CreateCloudTableClient();

        logsTable = logsTableClient.GetTableReference("CloudInsightAssignment");

        logsTable.CreateIfNotExists();
    }

    public IEnumerable<LogEntity> GetLogs(DateTime? from = null, DateTime? to = null)
    {
        try
        {
            var query = new TableQuery<LogEntity>();
            if (from.HasValue || to.HasValue)
                query = AddTimestampFilter(query, from, to);

            var queryResult = logsTable.ExecuteQuery(query);

            return queryResult;
        }
        catch (Exception e)
        {

            throw;
        }
    }

    TableQuery<LogEntity> AddTimestampFilter(TableQuery<LogEntity> query, DateTime? from, DateTime? to)
    {
        try
        {
            string fromFilter = string.Empty;
            string toFilter = string.Empty;
            string filter = string.Empty;

            if (from.HasValue)
                fromFilter = TableQuery.GenerateFilterConditionForDate(
                                    nameof(LogEntity.Timestamp),
                                    QueryComparisons.GreaterThanOrEqual,
                                    new DateTimeOffset(DateTime.SpecifyKind(from.Value, DateTimeKind.Local))
                );

            if (to.HasValue)
                toFilter = TableQuery.GenerateFilterConditionForDate(
                        nameof(LogEntity.Timestamp),
                        QueryComparisons.LessThanOrEqual,
                        new DateTimeOffset(DateTime.SpecifyKind(to.Value, DateTimeKind.Local))
                );


            if (!string.IsNullOrWhiteSpace(fromFilter) && !string.IsNullOrWhiteSpace(toFilter))
                return query.Where(TableQuery.CombineFilters(fromFilter, "AND", toFilter));
            if (!string.IsNullOrWhiteSpace(fromFilter))
                return query.Where(fromFilter);
            else
                return query.Where(toFilter);
        }
        catch (Exception e)
        {

            throw;
        }

    }
    public void StoreLog(LogEntity log)
    {
        try
        {
            var operation = TableOperation.Insert(log);

            logsTable.Execute(operation);
        }
        catch (Exception e)
        {

            throw;
        }

    }
}
