using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherAssignment.Azure;

namespace WeatherAssignment.Controllers;

[Route("[controller]")]
public class WeatherController : Controller
{
    private readonly ICosmosDatasource Datasource;
    public WeatherController(ICosmosDatasource datasource)
    {
        Datasource = datasource;
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var response = await Datasource.GetAll();

        var lowestTemperatures = response
            .GroupBy(c => c.City)
            .Select(t => t.OrderBy(x => x.Temperature).FirstOrDefault())
            .ToList();

        var higestWindspeed = response
            .GroupBy(c => c.City)
            .Select(t => t.OrderBy(x => x.WindSpeed).FirstOrDefault())
            .ToList();

        return Ok(new
        {
            WindSpeedData = higestWindspeed,
            TemperatureData = lowestTemperatures
        });
    }
}
