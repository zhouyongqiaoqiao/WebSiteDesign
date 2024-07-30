namespace ProjectWebApiCore.EntityEnum
{
    /// <summary>
    /// 在职状态
    /// </summary>
    public enum StatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [AGBasic.EnumDescription("正常")]
        Normal = 1,

        /// <summary>
        /// 停用
        /// </summary>
        [AGBasic.EnumDescription("停用")]
        Stopped = 0,

    }
}
