using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private AppDbContext _catalogDbContext { get; set; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext catalogDbContext)
        {
            _logger = logger;
            _catalogDbContext = catalogDbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Models.CatalogMusic> Get()
        {
            return _catalogDbContext.catalogMusics.ToArray();
        }
    }
}