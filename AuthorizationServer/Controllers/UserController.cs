using AuthorizationServer.Utility;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")] //全部按每个方法名区分
    public class UserController : ControllerBase
    {
        ICustomJWTService jWTService;
        public UserController(ICustomJWTService _jWTService)
        {
            this.jWTService = _jWTService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userID">用户账号</param>
        /// <param name="pwd">用户密码（加密后）</param>
        /// <returns></returns>
        [HttpGet("{userID}/{pwd}")]
        public IActionResult Login(string userID, string pwd )
        {
            if (userID != "admin" || pwd != "aa123")
            {
                return new JsonResult(new { status = false, token = "" });
            }
            //TODO : 从DB中查询并判断该用户是否存在，若存在则继续
            var user = new CurrentUser()
            {
                ID = userID,
                Name = userID,
                Password = pwd,
                roldID = 0
            };
            return new JsonResult(new { status = true, token = jWTService.GetToken(user) });
        }
    }
}
