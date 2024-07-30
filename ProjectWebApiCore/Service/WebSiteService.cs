using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Interface;
using ProjectWebApiCore.Result;
using SqlSugar;
using System.Linq.Expressions;

namespace ProjectWebApiCore.Service
{
    public class WebSiteService : BaseService, IWebSiteService
    {
        private int curentId;
        private object lockObj = new object();
        public WebSiteService(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
            this.curentId = this.GetLastId();
        }

        private string GetNextId()
        {
            lock (lockObj)
            {
                curentId++;
                return curentId.ToString("D4");//D4表示四位数字
            }
        }

        #region IWebSiteService

        public Task<WebSite> AddWebSite(WebSite webSite)
        {
            webSite.Id = this.GetNextId();
            return this.InsertAsync(webSite);
        }

        //public void AddWebSite(string name, string themes, int pageCount, decimal price, string description, string mainImageUrl)
        //{
        //    WebSite webSite = new WebSite()
        //    {
        //        Id = this.GetNextId(),
        //        Name = name,
        //        Theme = themes,
        //        PageCount = pageCount,
        //        Price = price,
        //        Description = description,
        //        MainImgUrl = mainImageUrl,
        //        Enabled = true,
        //        CreateTime = DateTime.Now,
        //        SortNo = 10,
        //        DownloadLink = ""
        //    };
        //    this.Insert(webSite);
        //}

        public async void UpdateWebSite(WebSite webSite)
        {
            await this.UpdateAsync(webSite);
        }

        public int GetLastId()
        {
            WebSite? website = base.SqlClient.Queryable<WebSite>().OrderByPropertyName(WebSite.C_CreatedTime, OrderByType.Desc).First();
            if (website == null)
            {
                return 0;
            }
            else
            {
                int result = -1;
                int.TryParse(website.Id,out result);
                return result;
            }
        }

        private PagingData<WebSite> GetWebSitePageByTheme2(int pageIndex, int pageSize, params string[] themes)
        {
            Expression<Func<WebSite, bool>> funcWhere = null;
            if (themes != null && themes.Length > 0)
            {
                funcWhere = (site) => this.IsThemeValid(site.ThemeList, themes) && site.Enabled;
            }             

            return this.QueryPage<WebSite>(funcWhere, pageSize, pageIndex,  (site) => site.SortNo, isAsc: true);
        }

        public PagingData<WebSite> GetWebSitePageByTheme(int pageIndex, int pageSize, string[]? themes, string? searchKey)
        {
            if (string.IsNullOrEmpty(searchKey))
            {
                return this.GetWebSitePageByTheme2(pageIndex, pageSize, themes);
            }
            if (themes == null || themes.Length == 0)
            {
                return this.GetWebSitePage(pageIndex, pageSize, searchKey);
            }
            Expression<Func<WebSite, bool>> funcWhere = null;
            if (!string.IsNullOrEmpty(searchKey) && themes != null && themes.Length > 0)
            {
                funcWhere = (site) => this.IsThemeValid(site.ThemeList, themes) && site.Enabled && (site.Id.Contains(searchKey) || site.Name.Contains(searchKey));
            }
            return this.QueryPage<WebSite>(funcWhere, pageSize, pageIndex, (site) => site.SortNo, isAsc: true);
        }

        public bool ExistWebSite(string siteId)
        {
            return this.SqlClient.Queryable<WebSite>().Any(w => w.Id == siteId);
        }

        public WebSite GetWebSiteById(string id)
        {
            return this.Find<WebSite>(id);
        }

        public PagingData<WebSite> GetWebSitePage(int pageIndex, int pageSize, string searchKey)
        {
            Expression<Func<WebSite, bool>> funcWhere = null;
            if (!string.IsNullOrEmpty(searchKey))
            {
                funcWhere = (site) => (site.Id.Contains(searchKey) || site.Name.Contains(searchKey)) && site.Enabled;
            }
            return this.QueryPage<WebSite>(funcWhere, pageSize,  pageIndex, (site) => site.SortNo, isAsc: true);
        }

        public void StopUseWebSite(params string[] ids)
        {
            List<WebSite> webSites = this.SqlClient.Queryable<WebSite>().In(WebSite.C_Id, ids).ToList();
            this.SqlClient.Updateable(webSites).UpdateColumns(it => new { Enabled = false }).ExecuteCommand();
        }

        public void EnableUseWebSite(params string[] ids)
        {
            List<WebSite> webSites = this.SqlClient.Queryable<WebSite>().In(WebSite.C_Id, ids).ToList();
            this.SqlClient.Updateable(webSites).UpdateColumns(it => new { Enabled = true }).ExecuteCommand();
        }


        #region WebSiteImageDetail
        public Task<bool> AddOrUpdateWebSiteImageDetail(string webSiteId, params WebSiteImageDetail[] webSiteImageDetails)
        {
            if (!this.ExistWebSite(webSiteId))
            {
                return Task.FromResult(false);
            }
            //先删除详情，再添加详情
            this.Delete<WebSiteImageDetail>(w => w.WebSiteId == webSiteId);
            foreach (var item in webSiteImageDetails)
            {
                item.WebSiteId = webSiteId;
                this.Insert(item);
            }
            return Task.FromResult(true);
        }

        public List<WebSiteImageDetail> GetWebSiteImageDetails(string webSiteId)
        {
            return this.SqlClient.Queryable<WebSiteImageDetail>().Where(w => w.WebSiteId == webSiteId).ToList();
        } 
        #endregion
        #endregion


        #region MyRegion

        /// <summary>
        /// 判断主题是否包含在所求主题列表中
        /// </summary>
        /// <param name="themes"></param>
        /// <param name="targetTheme"></param>
        /// <returns></returns>
        private bool IsThemeValid(List<string> themes, string[] targetTheme)
        {
            var result = themes.Intersect(targetTheme).ToList();
            if (result.Count == 0)
            {
                return false;
            }
            return true;
        }





        #endregion
    }
}
