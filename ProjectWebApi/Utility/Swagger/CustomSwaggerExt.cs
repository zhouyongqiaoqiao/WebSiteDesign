
using Microsoft.AspNetCore.Components.Web;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ProjectWebApi.Utility.Swagger
{
    /// <summary>
    /// 这个类就是扩展了Swagger的相关配置
    /// </summary>
    public static class CustomSwaggerExt
    {
        /// <summary>
        /// 配置Swagger
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="includeVersion">是否包含版本号,默认false</param>
        public static void AddSwaggerExt(this WebApplicationBuilder builder,bool includeVersion =false)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                #region 要启用swagger版本控制要在api控制器或方法上加上特性【ApiExplorerSettings(GroupName = "版本号")】
                {
                    if (includeVersion)
                    {
                        typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                        {
                            options.SwaggerDoc(version, new Microsoft.OpenApi.Models.OpenApiInfo()
                            {
                                Title = $"ZY_版本：{version} Api文档",
                                Version = version,
                                Description = $"通用版本的CoreApi版本{version}"
                            });
                        });
                    }
                    else {
                        options.SwaggerDoc("v1", new OpenApiInfo()
                        {
                            Title = $"{Assembly.GetEntryAssembly().GetName()}Api文档",
                            Version = "v1",
                            Description = $"通用版本的CoreApi版本v1"
                        });

                    }
                }
                #endregion

                #region 配置展示注释
                {
                    //xml文档位置绝对路径
                    var file = Path.Combine(AppContext.BaseDirectory, System.Reflection.Assembly.GetEntryAssembly().GetName().Name + ".xml");
                    //显示控制器层
                    options.IncludeXmlComments(file, true);
                    //对action的名称进行排序，如果有多个，就可以看到效果了
                    options.OrderActionsBy(o => o.RelativePath);
                }
                #endregion



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
        }

        /// <summary>
        /// 使用Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="includeVersion">是否包含版本号,默认false</param>
        public static void UseSwaggerExt(this WebApplication app, bool includeVersion = false)
        {
            app.UseSwagger();
            if (includeVersion)
            {
                app.UseSwaggerUI(options => {
                    foreach (string version in typeof(ApiVersions).GetEnumNames())
                    {
                        options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"版本：{version}");
                    }
                });
            }
            else {
                app.UseSwaggerUI();
            }            
        }

    }
}
