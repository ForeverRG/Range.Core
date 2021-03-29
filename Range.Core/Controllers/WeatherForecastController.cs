using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Range.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Range.Core.Controllers
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

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
      _logger = logger;
    }

    /// <summary>
    /// 获取天气集合
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    //[Authorize(Roles = "Admin")]  //角色授权
    //[Authorize(Policy = "AdminOrUser")]  //满足Admin或者User
    //[Authorize(Policy = "AdminAndUser")]  //同时满足Admin和User
    [Authorize(Policy = "NeedClaimRangeOrChen")] //满足name Cliam
    public IEnumerable<WeatherForecast> Get()
    {
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
