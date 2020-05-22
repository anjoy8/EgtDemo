using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgtDemo.IServ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EgtDemo.Controllers
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
        private readonly IDemoServ _demoServ;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDemoServ demoServ)
        {
            _logger = logger;
            _demoServ = demoServ;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var demos = _demoServ.GetDemos();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
