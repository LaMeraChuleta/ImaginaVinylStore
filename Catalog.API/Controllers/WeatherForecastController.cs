using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using SharedApp.Data;
using SharedApp.Models;

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
        private readonly IConfiguration _configuration;
        private AppDbContext _catalogDbContext { get; set; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext catalogDbContext, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _catalogDbContext = catalogDbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<bool> Get([FromForm] IFormFile file)
        {
            var strinconn = _configuration["BlobConnectionString"];
            var containerName = _configuration["BlobContainerName"];
            BlobContainerClient containerClient = new BlobContainerClient(strinconn, containerName);
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var result = containerClient.UploadBlob("test.jpg", ms);                
                return true;
            }
            return false;
        }
    }
}