namespace RandomPayloadAssignment.Synchronizers;

public interface ISynchronizer
{
    Task FetchDataEveryMinute();
}
