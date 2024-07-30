using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Interface;
using SqlSugar;

namespace ProjectWebApiCore.Service
{
    public class ThemeService : BaseService, IThemeService
    {
        public ThemeService(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }

        public Task<List<Theme>> GetThemes(string searchText = null)
        {
            if (string.IsNullOrEmpty(searchText))
                return this.GetAllThemes();
            else
                return this.SqlClient.Queryable<Theme>().Where(t => t.Name.Contains(searchText)).ToListAsync();       
        }

        public bool AddTheme(Theme theme)
        {
            try
            {
                this.Insert(theme);
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }



        public bool DeleteTheme(string themeName)
        {
            ///TODO: Delete Theme 判断是否有website引用该主题
            return this.Delete<Theme>(t => t.Name == themeName);
        }

        public Task<List<Theme>> GetAllThemes()
        {
           return this.GetAll<Theme>();
        }

        public bool UpdateTheme(Theme theme)
        {
            try
            {
                this.SqlClient.Updateable(theme).UpdateColumns(it => new { Name = theme.Name }).ExecuteCommand();
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
