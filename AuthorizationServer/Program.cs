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

    //��Ӱ�ȫ����--����֧��token��Ȩ����
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "������token,��ʽΪ Bearer xxxxxxxx��ע���м�����пո�",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //��Ӱ�ȫҪ��
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
    .AddJsonOptions((options) => { options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All); }); //�������������������
builder.Services.AddTransient<ICustomJWTService, CustomJWTService>();
builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOPtions"));
#region �����������
//�м�������������
builder.Services.AddCors(options =>
{
    // allcore: ��������
    options.AddPolicy("allcore", corsBuilder =>
    {
        corsBuilder.AllowAnyHeader()
                   .AllowAnyOrigin()
                   .AllowAnyMethod();
    });
});
#endregion
builder.Services    
    .AddAuthorization()//������Ȩ
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //ָ����Ȩ����);
    .AddJwtBearer(options => //�������ü�Ȩ���߼�
     {
         options.TokenValidationParameters = new TokenValidationParameters()
         {
             ValidateIssuer = true, //�Ƿ���֤Issuer
             ValidIssuer = CustomJWTService.JWTTokenOptions.Issuer, //������Issuer
             ValidateAudience = true, //�Ƿ���֤Audience
             ValidAudience = CustomJWTService.JWTTokenOptions.Audience, //������Audience
             ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CustomJWTService.JWTTokenOptions.SecurityKey)), //SecurityKey
             ValidateLifetime = false, //�Ƿ���֤ʧЧʱ��
             ClockSkew = TimeSpan.FromSeconds(30), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
             RequireExpirationTime = false,
         };
     });
var app = builder.Build();
//�����м����UseAuthentication����֤����������������Ҫ�����֤���м��ǰ���ã����� UseAuthorization����Ȩ����
app.UseAuthentication();
app.UseAuthorization();

//�����м����Ч
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
//    //TODO : ��DB�в�ѯ���жϸ��û��Ƿ���ڣ������������
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
app.MapControllers();  //���������������ӵ�·�� �����˸þ�Controller�ķ������޷�����
app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}