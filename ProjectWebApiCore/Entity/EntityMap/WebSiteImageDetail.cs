using SqlSugar;

namespace ProjectWebApiCore.Entity.EntityMap
{
    public class WebSiteImageDetail
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int AutoId { get; set; }

        /// <summary>
        /// 网站ID
        /// </summary>
        public string WebSiteId { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 图片描述
        /// </summary>
        public string ImageDescription { get; set; }

        /// <summary>
        /// 排序 默认10
        /// </summary>
        public int SortNO { get; set; } = 10;
    }
}
