using WeatherAssignment.Models;

namespace WeatherAssignment.Azure;

public interface ICosmosDatasource
{
    Task<Weather[]> GetAll();
    void Store(Weather data);
}
