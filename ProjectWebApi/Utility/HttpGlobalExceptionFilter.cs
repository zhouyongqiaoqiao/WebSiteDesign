using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectWebApiCore.Result;

namespace ProjectWebApi.Utility
{
    /// <summary>
    /// 全局异常筛选
    /// </summary>
    public class HttpGlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            this._logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var actionName = context.HttpContext.Request.RouteValues["controller"] + "/" + context.HttpContext.Request.RouteValues["action"];
            _logger.LogError($"--------{actionName} Error Begin--------");
            _logger.LogError($"  Error Detail:" + context.Exception.Message);
            //拦截处理
            if (!context.ExceptionHandled)
            {
                //context.Result = new JsonResult(new
                //{
                //    status = false,
                //    msg = "请求处理出错，" + context.Exception.Message
                //});
                context.Result = new JsonResult(new ApiResult<string>() { Success = false, Message = "请求处理出错，" + context.Exception.Message });
                context.ExceptionHandled = true;
            }
            _logger.LogError($"--------{actionName} Error End--------");
        }

    }

}
