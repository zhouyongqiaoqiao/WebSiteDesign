using SqlSugar;

namespace ProjectWebApiCore.Result
{
    /// <summary>
    /// API 返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> 
    {
        /// <summary>
        /// Api执行是否成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public T? Data { get; set; }
    }
}
