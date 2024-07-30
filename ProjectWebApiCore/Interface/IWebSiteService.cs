using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Interface
{
    public interface IWebSiteService : IBaseService
    {
        /// <summary>
        /// 判断是否存在WebSite
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        bool ExistWebSite(string siteId);

        /// <summary>
        /// 添加WebSite
        /// </summary>
        /// <param name="webSite"></param>
        /// <returns></returns>
        Task<WebSite> AddWebSite(WebSite webSite);

        /// <summary>
        /// 更新WebSite
        /// </summary>
        /// <param name="webSite"></param>
        void UpdateWebSite(WebSite webSite);

        /// <summary>
        /// 添加或修改WebSite图片详情        
        /// <param name="webSiteId"></param>
        /// <param name="webSiteImageDetails"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateWebSiteImageDetail(string webSiteId, params WebSiteImageDetail[] webSiteImageDetails);

        /// <summary>
        /// 获取指定WebSite图片详情
        /// </summary>
        /// <param name="webSiteId"></param>
        /// <returns></returns>
        List<WebSiteImageDetail> GetWebSiteImageDetails(string webSiteId);
        //void UpdateWebSite(string id , string name, string theme, int pageCount, decimal price, string description, string mainImageUrl);

        /// <summary>
        /// 根据名称或ID模糊查询WebSite分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey">空代表查询所有</param>
        /// <returns></returns>
        PagingData<WebSite> GetWebSitePage(int pageIndex, int pageSize, string? searchKey);

        /// <summary>
        /// 根据主题查询WebSite分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="theme">空代表查询所有</param>
        /// <param name="searchKey">查询关键字(空代表查询所有)</param>
        /// <returns></returns>  
        PagingData<WebSite> GetWebSitePageByTheme(int pageIndex, int pageSize, string[]? themes, string? searchKey);

        /// <summary>
        /// 根据ID查询WebSite
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WebSite GetWebSiteById(string id);

        /// <summary>
        /// 停用WebSite
        /// </summary>
        /// <param name="ids"></param>
        void StopUseWebSite(params string[] ids);

        /// <summary>
        /// 启用WebSite
        /// </summary>
        /// <param name="ids"></param>
        void EnableUseWebSite(params string[] ids);

        /// <summary>
        /// 获取最后一条记录的ID
        /// </summary>
        /// <returns></returns>
        int GetLastId();
    }
}
