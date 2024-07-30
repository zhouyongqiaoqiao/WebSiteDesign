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
        private static readonly string SecurityKey = "�������£������ҵļ���Key,������ô���64"; //������Ȩ��֤Key

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; //�������������ֵ
            });
            // Add services to the container.

            builder.Services.Configure<FormOptions>(option =>
            {
                //�������1G, ����ĵ�λ��byte
                option.MultipartBodyLengthLimit = 10 * 1024 * 1024;
            });

            //����log4net
            builder.Services.AddLogging(cfg =>
            {
                cfg.AddLog4Net(new Log4NetProviderOptions()
                {
                    Log4NetConfigFileName = "log4net.config",
                    Watch = true
                });
            });

            builder.Services.AddControllers()
                .AddJsonOptions((options) => { options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All); }); //�������������������
                                                                                                                                        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                                                                                                                                        //����Swagger

            builder.AddSwaggerExt(true);

            #region ������Ȩ ��ָ����������Ȩ�߼�
            builder.Services.AddAuthorization()//������Ȩ
    .AddAuthentication("Bearer") //ָ����Ȩ���� JwtBearerDefaults.AuthenticationScheme;
    .AddJwtBearer(options => //�������ü�Ȩ���߼�
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false, //�Ƿ���֤Issuer
            ValidIssuer = "http://1233", //������Issuer
            ValidateAudience = false, //�Ƿ���֤Audience
            ValidAudience = "http://1233", //������Audience
            ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey)), //SecurityKey
            ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
            ClockSkew = TimeSpan.FromSeconds(30), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
            RequireExpirationTime = false,
        };
    });
            #endregion

            #region IOC ע����������֮��Ĺ���  ��UserController����
            builder.Services.AddSingleton<ISqlSugarClient>(serverprivider =>
            {
                //SslMode=None;AllowPublicKeyRetrieval=True; Ϊ�˽��Mysql SSL Authentication error
                string connectionString = string.Format("server={0};Port={1};Database={2};Uid={3};Pwd={4};Charset=utf8mb4;SslMode=None;AllowPublicKeyRetrieval=True;", "47.116.53.148", 3306, "zy_website", "admin", "sa123456");
                ConnectionConfig connection = new ConnectionConfig()
                {
                    ConnectionString = connectionString,// "Data Source=47.116.53.148;Initial Catalog=zy_factory;Persist Security Info=True;User Id=admin;Password=sa123456",
                                                        //ConnectionString = builder.Configuration.GetConnectionString("Default"),
                    DbType = DbType.MySql, //ָ�����ݿ������
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                };
                //SqlSugarClient sqlSugarClient = new SqlSugarClient(connection); 
                return new SqlSugarClient(connection);
            });


            //AddSingleton ���� ;  AddTransientÿ�ζ����µ�
            //builder.Services.AddSingleton<ITestServiceA, TestServiceA>();
            //builder.Services.AddTransient<ITestServiceB, TestServiceB>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<ISystemlogService, SystemlogService>();
            builder.Services.AddSingleton<IWebSiteService, WebSiteService>();
            builder.Services.AddSingleton<IThemeService, ThemeService>();
            //����Automapper�Ĺ�����Ч
            builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));
            #endregion

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

            builder.Services.AddMvc(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
            });

            var app = builder.Build();            

            //app.UseExceptionHandler("/error"); // �Զ��������·��
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    // ��¼�쳣
                    
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
            //app.UseHttpsRedirection();// ����SSL�ض���
            //�����м����UseAuthentication����֤����������������Ҫ�����֤���м��ǰ���ã����� UseAuthorization����Ȩ����
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseRouting();
            //�����м����Ч
            app.UseCors("allcore");
            app.MapControllers(); //���������������ӵ�·�� �����˸þ�Controller�ķ������޷�����
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
