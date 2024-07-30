using AuthorizationServer.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((options) => {

    //添加安全定义--配置支持token授权机制
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "请输入token,格式为 Bearer xxxxxxxx（注意中间必须有空格）",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //添加安全要求
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {
                        new OpenApiSecurityScheme
                        {
                            Reference =new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
});
builder.Services.AddControllers()
    .AddJsonOptions((options) => { options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All); }); //解决返回中文乱码问题
builder.Services.AddTransient<ICustomJWTService, CustomJWTService>();
builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOPtions"));
#region 解决跨域问题
//中间件解决跨域问题
builder.Services.AddCors(options =>
{
    // allcore: 策略名称
    options.AddPolicy("allcore", corsBuilder =>
    {
        corsBuilder.AllowAnyHeader()
                   .AllowAnyOrigin()
                   .AllowAnyMethod();
    });
});
#endregion
builder.Services    
    .AddAuthorization()//开启授权
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //指定授权渠道);
    .AddJwtBearer(options => //这里配置鉴权的逻辑
     {
         options.TokenValidationParameters = new TokenValidationParameters()
         {
             ValidateIssuer = true, //是否验证Issuer
             ValidIssuer = CustomJWTService.JWTTokenOptions.Issuer, //发行人Issuer
             ValidateAudience = true, //是否验证Audience
             ValidAudience = CustomJWTService.JWTTokenOptions.Audience, //订阅人Audience
             ValidateIssuerSigningKey = true, //是否验证SecurityKey
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CustomJWTService.JWTTokenOptions.SecurityKey)), //SecurityKey
             ValidateLifetime = false, //是否验证失效时间
             ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
             RequireExpirationTime = false,
         };
     });
var app = builder.Build();
//调用中间件：UseAuthentication（认证），必须在所有需要身份认证的中间件前调用，比如 UseAuthorization（授权）。
app.UseAuthentication();
app.UseAuthorization();

//跨域中间件生效
app.UseCors("allcore");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


//app.MapGet("/login", (string userID, string pwd, ICustomJWTService jWTService) =>
//{

//    if (userID != "admin" || pwd != "aa123")
//    {
//        return new { status = false, token = "" };
//    }
//    //TODO : 从DB中查询并判断该用户是否存在，若存在则继续
//    var user = new CurrentUser()
//    {
//        ID = userID,
//        Name = userID,
//        Password = pwd,
//        roldID = 0
//    };
//    return new { status = true, token = jWTService.GetToken(user) };
//});

//app.MapGet("/weatherforecast", [Authorize] () =>
//{

//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateTime.Now.AddDays(index),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");
app.MapControllers();  //将控制器操作增加到路由 ，少了该句Controller的方法都无法访问
app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}