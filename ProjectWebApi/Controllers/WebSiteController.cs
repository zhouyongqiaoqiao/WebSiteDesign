using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tls.Crypto;
using ProjectWebApiCore.Entity.EntityDto;
using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Interface;
using ProjectWebApiCore.Result;

namespace ProjectWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")] //全部按每个方法名区分
    public class WebSiteController : Controller
    {
        private readonly ISystemlogService logService;
        private readonly IMapper mapper;
        private readonly IWebSiteService webSiteService;

        public WebSiteController(ISystemlogService systemlogService, IMapper mapper, IWebSiteService webSiteService)
        {
            this.logService = systemlogService;
            this.mapper = mapper;
            this.webSiteService = webSiteService;
        }


        /// <summary>
        /// 添加网站
        /// </summary>
        /// <param name="name">网站名称</param>
        /// <param name="theme">所属主题（多个主题以逗号分隔）</param>
        /// <param name="pageCount">网站页数</param>
        /// <param name="price">价格</param>
        /// <param name="description">描述</param>
        /// <param name="sortNo">排序 越小越靠前 默认10</param>
        /// <param name="mainImageUrl">主图链接</param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<ApiResult<WebSiteDto>> AddWebSite(string name, string theme, int pageCount, decimal price, string? description, int sortNo ,  string? mainImageUrl)
        {
            WebSite webSite = new WebSite()
            {
                Name = name,
                Theme = theme,
                PageCount = pageCount,
                Price = price,
                Description = description??string.Empty,
                MainImgUrl = mainImageUrl??string.Empty,
                Enabled = true,
                CreateTime = DateTime.Now,
                SortNo = sortNo,
                DownloadLink = ""
            };
            var result = await this.webSiteService.AddWebSite(webSite);
            WebSiteDto webSiteDto = this.mapper.Map<WebSiteDto>(result);
            return new ApiResult<WebSiteDto>() { Data = webSiteDto };
        }

        /// <summary>
        /// 修改网站
        /// </summary>
        /// <param name="id">网站ID</param>
        /// <param name="name">网站名称</param>
        /// <param name="theme">所属主题（多个主题以逗号分隔）</param>
        /// <param name="pageCount">网站页数</param>
        /// <param name="price">价格</param>
        /// <param name="description">描述</param>
        /// <param name="sortNo">排序 越小越靠前 默认10</param>
        /// <param name="mainImageUrl">主图链接</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ApiResult<bool> UpdateWebSite(WebSiteDto webSiteDto)
        {
            if (webSiteDto == null)
            {
                return new ApiResult<bool> { Success = false, Message = "参数不能为空" };
            }
            var webSite = this.webSiteService.GetWebSiteById(webSiteDto.Id);
            if (webSite == null)
            {
                return new ApiResult<bool> { Success = false, Message = "网站不存在" };
            }
            webSite = this.mapper.Map<WebSite>(webSiteDto);
            this.webSiteService.UpdateWebSite(webSite);
            return new ApiResult<bool> { Data = true, Message = "修改成功" };
        }

        /// <summary>
        /// 添加或修改网站图片详情   
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="webSiteImageDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResult<bool>> AddOrUpdateWebSiteImage(string siteId, WebSiteImageDetail[] webSiteImageDetails)
        {
            bool result = await this.webSiteService.AddOrUpdateWebSiteImageDetail(siteId, webSiteImageDetails);
            return new ApiResult<bool> { Data = result };
        }

        /// <summary>
        /// 获取指定WebSite图片详情
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpGet("{siteId}")]
        public ApiResult<List<WebSiteImageDetail>> GetWebSiteImageDetails(string siteId)
        {
            var result = this.webSiteService.GetWebSiteImageDetails(siteId);
            return new ApiResult<List<WebSiteImageDetail>> { Data = result };
        }

        /// <summary>
        /// 停用指定的网站
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{siteId}")]
        [Authorize(Roles = "admin")]
        public ApiResult<bool> StopUseWebSite(string siteId) 
        { 
            this.webSiteService.StopUseWebSite(siteId);
            return new ApiResult<bool> {Data = true, Message = "停用成功" };
        }

        /// <summary>
        /// 启用指定的网站
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{siteId}")]
        [Authorize(Roles = "admin")]
        public ApiResult<bool> EnableUseWebSite(string siteId)
        {
            this.webSiteService.EnableUseWebSite(siteId);
            return new ApiResult<bool> { Data = true, Message = "启用成功" };
        }

        /// <summary>
        /// 根据名称或ID模糊查询WebSite分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey">空代表查询所有</param>
        /// <returns></returns>
        [HttpGet("{pageIndex:int}/{pageSize:int}/")]
        [HttpGet("{pageIndex:int}/{pageSize:int}/{searchKey}")]
        public ApiResult<PagingData<WebSiteDto>> GetWebSitePage(int pageIndex, int pageSize, string? searchKey)
        {
            PagingData<WebSite> data = this.webSiteService.GetWebSitePage(pageIndex,pageSize,searchKey);
            var result = this.mapper.Map<PagingData<WebSiteDto>>(data);
            return new ApiResult<PagingData<WebSiteDto>> { Data = result };
        }

        /// <summary>
        /// 根据主题查询WebSite分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="themes">空代表查询所有</param>
        /// <param name="searchKey">查询关键字(空代表查询所有)</param>
        /// <returns></returns>  
        [HttpGet("{pageIndex:int}/{pageSize:int}/{themes}/{searchKey}")]
        [HttpGet("{pageIndex:int}/{pageSize:int}/{themes}")]
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public ApiResult<PagingData<WebSiteDto>> GetWebSitePageByTheme(int pageIndex, int pageSize, string[]? themes, string? searchKey)
        {
            PagingData<WebSite> data = this.webSiteService.GetWebSitePageByTheme(pageIndex, pageSize,themes, searchKey);
            var result = this.mapper.Map<PagingData<WebSiteDto>>(data);
            return new ApiResult<PagingData<WebSiteDto>> { Data = result };
        }
    }

}
