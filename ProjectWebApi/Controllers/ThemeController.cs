using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Interface;
using ProjectWebApiCore.Result;

namespace ProjectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")] //全部按每个方法名区分
    public class ThemeController : Controller
    {
        private readonly IThemeService themeService;
        private readonly ILogger<ThemeController> logger;
        private readonly IMapper mapper;


        public ThemeController(ILogger<ThemeController> logger, IMapper mapper, IThemeService themeService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.themeService = themeService;
        }

        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<Theme>>> GetAllThemes()
        {
            var result = await this.themeService.GetAllThemes();
            return new ApiResult<List<Theme>>() { Data = result };
        }

        /// <summary>
        /// 获取主题(模糊查询)
        /// </summary>
        /// <param name="searchKey">查询字符串</param>
        /// <returns></returns>
        [HttpGet ("{searchKey}")]
        public async Task<ApiResult<List<Theme>>> GetThemes(string searchKey = null) 
        {         
            var result = await this.themeService.GetAllThemes();        
            return new ApiResult<List<Theme>>() { Data = result };
        }

        /// <summary>
        /// 添加主题
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ApiResult<bool> AddTheme(Theme theme)
        {
            var res= this.themeService.AddTheme(theme);
            return new ApiResult<bool>() { Data = res };
        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ApiResult<bool> UpdateTheme(Theme theme)
        {
            var res= this.themeService.UpdateTheme(theme);
            return new ApiResult<bool>() { Data = res };
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [Authorize(Roles = "admin")]
        public Task<ApiResult<bool>> DeleteTheme(string name)
        {
            var res= this.themeService.DeleteTheme(name);
            return Task.FromResult(new ApiResult<bool>() { Data = res });
        }
    }
}
