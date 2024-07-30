using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Entity.EntityMap
{
    public partial class WebSite
    {
        public const string TableName = "WebSite";
        public const string C_CreatedTime = nameof(CreateTime);
        public const string C_Id = nameof(Id);
        /// <summary>
        /// 主键 编号 （4位数）
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)] //, IsIdentity = true
        public string Id { get; set; }

        /// <summary>
        /// 网站名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 网站主题
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommend { get; set; } = false;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 主图片链接
        /// </summary>
        public string MainImgUrl { get; set; } = string.Empty;

        /// <summary>
        /// 下载链接
        /// </summary>
        public string DownloadLink { get; set; } = string.Empty;

        /// <summary>
        /// 排序编号   （编号越大，排名越后）
        /// </summary>
        public int SortNo { get; set; } = 10;
    }
}
