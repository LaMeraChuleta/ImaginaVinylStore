using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedApp.Data;
using SharedApp.Models;

namespace Client.Server.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class CatalogMusicController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        public CatalogMusicController(AppDbContext context, ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<CatalogMusic>> Get()
        {
            try
            {
                return await _context.catalogMusics.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new List<CatalogMusic>();
            }
            
        }
    }
}