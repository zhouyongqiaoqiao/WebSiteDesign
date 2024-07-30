using Microsoft.IdentityModel.Tokens;
using ProjectWebApi.Utility.Swagger;
using ProjectWebApi.Utility;
using ProjectWebApiCore.Interface;
using ProjectWebApiCore.Service;
using SqlSugar;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text;
using ProjectWebApiCore.BusinessInterface;
using Microsoft.Extensions.Logging;
using log4net;
using Microsoft.AspNetCore.Http.Features;

namespace ProjectWebApi
{

    public class Program
    {
        private static readonly string SecurityKey = "我是周勇，这是我的加密Key,长度最好大于64"; //请求授权认证Key

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; //设置请求体最大值
            });
            // Add services to the container.

            builder.Services.Configure<FormOptions>(option =>
            {
                //设置最大1G, 这里的单位是byte
                option.MultipartBodyLengthLimit = 10 * 1024 * 1024;
            });

            //配置log4net
            builder.Services.AddLogging(cfg =>
            {
                cfg.AddLog4Net(new Log4NetProviderOptions()
                {
                    Log4NetConfigFileName = "log4net.config",
                    Watch = true
                });
            });

            builder.Services.AddControllers()
                .AddJsonOptions((options) => { options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All); }); //解决返回中文乱码问题
                                                                                                                                        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                                                                                                                                        //配置Swagger

            builder.AddSwaggerExt(true);

            #region 开启授权 并指定渠道和授权逻辑
            builder.Services.AddAuthorization()//开启授权
    .AddAuthentication("Bearer") //指定授权渠道 JwtBearerDefaults.AuthenticationScheme;
    .AddJwtBearer(options => //这里配置鉴权的逻辑
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false, //是否验证Issuer
            ValidIssuer = "http://1233", //发行人Issuer
            ValidateAudience = false, //是否验证Audience
            ValidAudience = "http://1233", //订阅人Audience
            ValidateIssuerSigningKey = true, //是否验证SecurityKey
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey)), //SecurityKey
            ValidateLifetime = true, //是否验证失效时间
            ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
            RequireExpirationTime = false,
        };
    });
            #endregion

            #region IOC 注册抽象与具体之间的管理  在UserController测试
            builder.Services.AddSingleton<ISqlSugarClient>(serverprivider =>
            {
                //SslMode=None;AllowPublicKeyRetrieval=True; 为了解决Mysql SSL Authentication error
                string connectionString = string.Format("server={0};Port={1};Database={2};Uid={3};Pwd={4};Charset=utf8mb4;SslMode=None;AllowPublicKeyRetrieval=True;", "47.116.53.148", 3306, "zy_website", "admin", "sa123456");
                ConnectionConfig connection = new ConnectionConfig()
                {
                    ConnectionString = connectionString,// "Data Source=47.116.53.148;Initial Catalog=zy_factory;Persist Security Info=True;User Id=admin;Password=sa123456",
                                                        //ConnectionString = builder.Configuration.GetConnectionString("Default"),
                    DbType = DbType.MySql, //指定数据库的类型
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                };
                //SqlSugarClient sqlSugarClient = new SqlSugarClient(connection); 
                return new SqlSugarClient(connection);
            });


            //AddSingleton 单例 ;  AddTransient每次都是新的
            //builder.Services.AddSingleton<ITestServiceA, TestServiceA>();
            //builder.Services.AddTransient<ITestServiceB, TestServiceB>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<ISystemlogService, SystemlogService>();
            builder.Services.AddSingleton<IWebSiteService, WebSiteService>();
            builder.Services.AddSingleton<IThemeService, ThemeService>();
            //配置Automapper的规则生效
            builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));
            #endregion

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

            builder.Services.AddMvc(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
            });

            var app = builder.Build();            

            //app.UseExceptionHandler("/error"); // 自定义错误处理路径
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    // 记录异常
                    
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
           
                }
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExt(true);
                app.UseDeveloperExceptionPage();
            }
            
            //app.UseMiddleware<CustomMiddleware>();
            //app.UseHttpsRedirection();// 开启SSL重定向
            //调用中间件：UseAuthentication（认证），必须在所有需要身份认证的中间件前调用，比如 UseAuthorization（授权）。
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseRouting();
            //跨域中间件生效
            app.UseCors("allcore");
            app.MapControllers(); //将控制器操作增加到路由 ，少了该句Controller的方法都无法访问
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                LogManager.GetLogger("Error").Error("UnhandledException", e.ExceptionObject as Exception);
            };
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                LogManager.GetLogger("Error").Error("UnobservedTaskException", e.Exception);
            };
            try
            {
                app.Run();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                Console.Read();
            }

        }
    }

}
