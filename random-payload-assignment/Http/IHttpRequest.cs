namespace RandomPayloadAssignment.Http;

public interface IHttpRequest
{
    Task<string> Get(string url);
}
