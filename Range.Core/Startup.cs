using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Range.Core
{
  public class Startup
  {
    public string ApiName { get; set; } = "Range.Core";
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var basePath = ApplicationEnvironment.ApplicationBasePath;

      services.AddControllers();  //��ӿ���������
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = ApiName,
          Version = "v1",
          Description = $"{ApiName} HTTP API V1",
          Contact = new OpenApiContact() { Email = "Range@126.com", Name = ApiName, Url = new Uri("https://www.baidu.com") },
          License = new OpenApiLicense() { Url = new Uri("https://www.baidu.com"), Name = ApiName }
        });
        c.OrderActionsBy(o => o.RelativePath);

        //添加控制器注释
        var xmlPath = Path.Combine(basePath, $"{ApiName}.xml");
        c.IncludeXmlComments(xmlPath, true);

        //添加实体模型注释
        var xmlModelPath = Path.Combine(basePath, $"{ApiName}.Model.xml");
        c.IncludeXmlComments(xmlModelPath, true);
      });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ApiName} v1");
          c.RoutePrefix = string.Empty;
        });
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
