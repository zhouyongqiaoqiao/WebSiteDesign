using AGBasic.Collections;
using AGBasic.ObjectManagement.Managers;
using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Cache
{
    internal class WebSiteCache
    {     
        private IWebSiteService webSiteService;
        private IThemeService themeService;
        private List<Theme> themes;

        public WebSiteCache(IWebSiteService webSiteService,IThemeService themeService)
        {
            this.webSiteService = webSiteService;
            this.themeService = themeService;
        }

        public async void Initialize()
        {
            
            this.themes = await this.themeService.GetAllThemes();
            foreach (var theme in themes)
            {
                //this.webSiteService.GetThemeWebSitePageData(0,1000,theme.Id);

            }
        }


        #region WebSite Cache
        //string :theme   按照主题分类的网站缓存
        //SortedArray<WebSite> :按照网站的权重和发布时间排序的网站缓存
        private ObjectManager<string, SortedArray<WebSite>> webDic = new ObjectManager<string, SortedArray<WebSite>>();




        #endregion
    }
}
