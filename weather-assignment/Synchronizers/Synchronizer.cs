using WeatherAssignment.Http;
using System.Text.Json;
using WeatherAssignment.Models;
using WeatherAssignment.Azure;

namespace WeatherAssignment.Synchronizers;

public class Synchronizer : BackgroundService, ISynchronizer
{
    private readonly IHttpRequest HttpRequest;
    private readonly ICosmosDatasource Datasource;
    List<Location> Locations = new List<Location>();

    public Synchronizer(IHttpRequest httpRequest, ICosmosDatasource datasource)
    {
        HttpRequest = httpRequest;
        Datasource = datasource;
        PopulateCities();
    }

    public async void FetchDataEveryMinuteAsync()
    {
        while (true)
        {
            try
            {
                foreach (var location in Locations)
                {

                    var response = await HttpRequest.Get(location.ApiUrl);

                    if (string.IsNullOrWhiteSpace(response))
                    {
                        continue;
                    }

                    var result = JsonSerializer.Deserialize<WeatherResult>(response);

                    var resultToBytes = JsonSerializer.SerializeToUtf8Bytes(result);
                    AddMetadata(location, result.CurrentUnits);

                    Datasource.Store(result.CurrentUnits);
                }
            }
            catch (Exception e)
            {
            }

            await Task.Delay(TimeSpan.FromSeconds(60));
        }
    }

    void AddMetadata(Location location, Weather weather)
    {

        weather.partitionKey = "Weather";
        weather.id = Guid.NewGuid().ToString();
        weather.Country = location.Country;
        weather.City = location.City;
        weather.Timestamp = DateTime.UtcNow;
    }

    private void PopulateCities()
    {
        Locations.Add(new Location
        {
            Country = "Sweden",
            City = "Gothenburg",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=57.7072&longitude=11.9668&current=temperature_2m,cloud_cover,wind_speed_10m"
        });

        Locations.Add(new Location
        {
            Country = "Sweden",
            City = "Mölndal",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=57.6554&longitude=12.0138&current=temperature_2m,cloud_cover,wind_speed_10m"
        });

        Locations.Add(new Location
        {
            Country = "Sweden",
            City = "Stenungsund",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=58.0705&longitude=11.8181&current=temperature_2m,cloud_cover,wind_speed_10m"
        });

        Locations.Add(new Location
        {
            Country = "Sweden",
            City = "Stockholm",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=59.3294&longitude=18.0687&current=temperature_2m,cloud_cover,wind_speed_10m"
        });

        Locations.Add(new Location
        {
            Country = "Japan",
            City = "Tokyo",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=35.6895&longitude=139.6917&current=temperature_2m,cloud_cover,wind_speed_10m"
        });

        Locations.Add(new Location
        {
            Country = "United States",
            City = "New York",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=40.7143&longitude=-74.006&current=temperature_2m,cloud_cover,wind_speed_10m"
        });
        Locations.Add(new Location
        {
            Country = "Denmark",
            City = "Copenhagen",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=55.6759&longitude=12.5655&current=temperature_2m,cloud_cover,wind_speed_10m"
        });

        Locations.Add(new Location
        {
            Country = "Norway",
            City = "Oslo",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=59.9127&longitude=10.7461&current=temperature_2m,cloud_cover,wind_speed_10m"
        });

        Locations.Add(new Location
        {
            Country = "Finland",
            City = "Helsinki",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=60.1695&longitude=24.9354&current=temperature_2m,cloud_cover,wind_speed_10m"
        });

        Locations.Add(new Location
        {
            Country = "Belize",
            City = "Caye Caulker",
            ApiUrl = "https://api.open-meteo.com/v1/forecast?latitude=17.7366&longitude=-88.0276&current=temperature_2m,cloud_cover,wind_speed_10m"
        });
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        FetchDataEveryMinuteAsync();
        return Task.CompletedTask;
    }
}
