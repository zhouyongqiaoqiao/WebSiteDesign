namespace ProjectWebApiCore.Entity.EntityDto
{
    ///<summary>
    ///
    ///</summary>
    public partial class SystemLogDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime Date { get; set; }

        /// <summary>
        /// 时间的字符串类型
        /// </summary>
        public string? StringDateTime
        {

            get; set;

            //get
            //{
            //    return this.Date.ToString("yyyy年MM月dd日 HH:mm:ss");
            //}
        }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Logger { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Message { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Exception { get; set; }

    }
}
