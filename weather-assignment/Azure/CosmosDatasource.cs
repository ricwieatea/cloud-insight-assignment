using Microsoft.Azure.Cosmos;
using WeatherAssignment.Models;

namespace WeatherAssignment.Azure;

public class CosmosDatasource : ICosmosDatasource
{

    const string ACCOUNT_ENDPOINT = "https://localhost:8081";
    const string CONNECTION_STRING = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    const string DATABASE_ID = "weather-assignment";
    const string CONTAINER_ID = "weather";
    public CosmosDatasource()
    {

    }

    public async Task<Weather[]> GetAll()
    {
        try
        {
            var weatherData = new List<Weather>();

            using CosmosClient client = new CosmosClient(
                  connectionString: CONNECTION_STRING!
            );

            var container = client.GetContainer(DATABASE_ID, CONTAINER_ID);
            var query = GetAllQuery();

            FeedIterator<Weather> resultSet = container.GetItemQueryIterator<Weather>
                (
                    query,
                    requestOptions: new QueryRequestOptions()
                    { PartitionKey = new PartitionKey("Weather"), MaxItemCount = -1 }
                );

            while (resultSet.HasMoreResults)
            {
                FeedResponse<Weather> response = await resultSet.ReadNextAsync();
                foreach (var result in response)
                {
                    weatherData.Add(result);
                }
            }

            return weatherData.ToArray();
        }
        catch (Exception e)
        {

            throw;
        }

    }

    private QueryDefinition GetAllQuery()
    {
        var query = @"SELECT * 
                     FROM weather w";

        return new QueryDefinition(query);
    }

    public async void Store(Weather data)
    {
        try
        {
            using CosmosClient client = new CosmosClient(
                connectionString: CONNECTION_STRING!
            );

            var container = client.GetContainer(DATABASE_ID, CONTAINER_ID);

            var foo = await container.CreateItemAsync<Weather>(data);
        }
        catch (Exception e)
        {

            throw;
        }
    }
}
