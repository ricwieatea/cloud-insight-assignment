using System.Text.Json;

namespace WeatherAssignment.Http;
public class HttpRequest : IHttpRequest
{
    static readonly HttpClient client = new HttpClient();
    public async Task<string> Get(string url)
    {
        try
        {
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return string.Empty;
        }
    }
}
