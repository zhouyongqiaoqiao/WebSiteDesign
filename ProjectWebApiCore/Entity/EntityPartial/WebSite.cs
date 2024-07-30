using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Entity.EntityMap
{
    public partial class WebSite : IComparable
    {
        [SugarColumn(IsIgnore = true)]
        public List<string> ThemeList
        {
            get
            {
                if (string.IsNullOrEmpty(Theme)) { return new List<string>(); }
                return Theme.Split(',').ToList();
            }
        }

        public int CompareTo(object obj)
        {
            WebSite other = obj as WebSite;
            if (this.SortNo != other.SortNo)
            {
                return this.SortNo.CompareTo(other.SortNo);
            }
            return this.Id.CompareTo(other.Id);
        }
    }
}
