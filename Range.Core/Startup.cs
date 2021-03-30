using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Range.Core.Policys;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace Range.Core
{
  public class Startup
  {
    public string ApiName { get; set; } = "Range.Core";

    public string Issure { get => "range.chen@quectel.com"; }
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var basePath = ApplicationEnvironment.ApplicationBasePath;

      services.AddControllers();  //添加控制器

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

        c.OperationFilter<AddResponseHeadersFilter>();
        c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        c.OperationFilter<SecurityRequirementsOperationFilter>();

        #region Token绑定到ConfigureServices
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
          Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
          Name = "Authorization",//jwt默认的参数名称
          In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
          Type = SecuritySchemeType.ApiKey
        });
        #endregion
      });

      //添加jwt认证服务
      services.AddAuthentication("Bearer").AddJwtBearer(c =>
      {
        c.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuer = true,
          ValidIssuer = Issure,

          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Issure)),

          ValidateAudience = true,
          ValidAudience = Issure,

          ValidateLifetime = true,
          RequireExpirationTime = true,
        };
      });

      //基于策略授权
      services.AddAuthorization(c =>
      {
        //c.AddPolicy("MustAdminRole", p => p.RequireRole("Admin"));
        //c.AddPolicy("AdminAndUser", p => p.RequireRole("Admin", "User"));
        //c.AddPolicy("AdminOrUser", p => p.RequireClaim(ClaimTypes.Role, "Admin", "User"));
        //c.AddPolicy("NeedClaimRangeOrChen", p => p.RequireClaim(ClaimTypes.Name,"Range","Chen"));

        //自定义授权处理器
        //MyRequirementTest requirementItem = new MyRequirementTest() { Age = 9, Name = "range", Sex = "男" };
        //c.AddPolicy("RequirementTest", p => p.Requirements.Add(requirementItem));
      });

      services.AddSingleton<IAuthorizationHandler, MyRequirementHandler>(); //注入授权处理器
      services.AddSingleton<MyRequirementTest>(); //注入授权注册类
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

      app.UseAuthentication();  //认证
      app.UseAuthorization(); //授权

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
