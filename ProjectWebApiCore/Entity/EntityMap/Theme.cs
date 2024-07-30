using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Entity.EntityMap
{
    public class Theme
    {
        /// <summary>
        /// 主题编号 
        /// </summary>
        //[SugarColumn(IsPrimaryKey = true)] //, IsIdentity = true
        //public string Id { get; set; }

        /// <summary>
        /// 主题名称 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Name { get; set; }

    }
}
