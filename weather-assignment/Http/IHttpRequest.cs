namespace WeatherAssignment.Http;

public interface IHttpRequest
{
    Task<string> Get(string url);
}
