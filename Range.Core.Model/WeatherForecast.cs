using System;

namespace Range.Core.Model
{
  /// <summary>
  /// 天气预报模型
  /// </summary>
  public class WeatherForecast
  {
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 摄氏度
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// F度
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// 概括
    /// </summary>
    public string Summary { get; set; }
  }
}
