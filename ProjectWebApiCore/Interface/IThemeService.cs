using ProjectWebApiCore.Entity.EntityMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Interface
{
    public interface IThemeService :IBaseService
    {
        Task<List<Theme>> GetAllThemes();
        Task<List<Theme>> GetThemes(string searchText = null);
        bool AddTheme(Theme theme);

        bool UpdateTheme(Theme theme);

        bool DeleteTheme(string themeName);
    }
}
