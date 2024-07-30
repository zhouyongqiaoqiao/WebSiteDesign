using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectWebApi.Utility.Swagger;
using ProjectWebApiCore.Entity.EntityDto;
using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Interface;
using ProjectWebApiCore.Result;
using System.Linq.Expressions;

namespace FactoryWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")] //全部按每个方法名区分
    //[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V1))]
    public class UserController : Controller
    {
        private IMapper automapper;
        private IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> _logger,IServiceProvider serviceProvider, IUserService _userService, IMapper mapper)
        {
            this.logger = _logger;
            this.automapper = mapper;
            this.userService = _userService;
            //var user = userService.Find<User>(1);
            //var userDto = this.automapper.Map<User, UserDto>(user);
            //var Sa = serviceProvider.GetService(typeof(ITestServiceA));
            //var Sb = serviceProvider.GetService(typeof(ITestServiceB));
            //if (this.serviceA == Sa) {
            //    this.logger.LogDebug("ITestServiceA 获取的一样");
            //    Console.WriteLine("ITestServiceA 获取的一样");
            //}
            //this.logger.LogInformation("UserController构造函数执行完成");
            //this.logger.LogError("测试错误");
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="idNumber"></param>
        /// <param name="isMan"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<User>> CreateUser(string userID, string userName, string pwd, string phone, string email)
        {
            User user = new User() { Id = userID, UserName = userName, PassWord = pwd, Phone = phone, Email = email };
            user = await this.userService.InsertAsync<User>(user);
            return new ApiResult<User>() { Data = user };
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<User>> CreateUser2(User user)
        {
            user = await this.userService.InsertAsync<User>(user);
            return new ApiResult<User>() { Data = user };
        }

        /// <summary>
        /// 获取指定用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<User> GetUser(string userId)
        {
            User user = this.userService.Find<User>(userId);
            // var userid = this.User.Claims.First(i => i.Type == "id").Value;
            return new ApiResult<User>() { Data = user };
        }

        /// <summary>
        /// 获取用户分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="idOrName"></param>
        /// <returns></returns>
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        [HttpGet("{pageIndex:int}/{pageSize:int}/{idOrName}")]
        public ApiResult<PagingData<UserDto>> GetUserPage(int pageIndex, int pageSize, string? idOrName)
        {
            try
            {
                //User? user = this.userService.Find<User>(idOrName);
                //if (user != null)
                //{
                //    UserDto? userDto= this.automapper.Map<User, UserDto>(user);                     >
                //    return new ApiResult<PagingData<UserDto>>() { Data = userDto };
                //}

                Expression<Func<User, bool>> funcWhere = null;
                if (!string.IsNullOrEmpty(idOrName)) {
                    funcWhere = (s) => s.UserName.Contains(idOrName);
                }
                PagingData<User> list = this.userService.QueryPage<User>(funcWhere, pageSize, pageIndex, (log) => log.Id, false);
                PagingData<UserDto> result = this.automapper.Map<PagingData<User>, PagingData<UserDto>>(list);
                return new ApiResult<PagingData<UserDto>>() { Data = result };
            }
            catch (Exception ex)
            {
                return new ApiResult<PagingData<UserDto>>() { Success = false, Message = ex.Message };
            }

        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "admin")]
        public async Task<List<User>> GetAllUsers()
        {
            return await this.userService.GetAll<User>();
        }

        /// <summary>
        /// 根据指定用户编号 修改用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="userName">更新后的名称</param>
        /// <param name="idNumber">更新后的手机号</param>
        /// <returns></returns>
        [HttpPut("{userId}/{userName}/{idNumber}")]
        public async Task<ApiResult<bool>> UpdateUser(string userId, string userName, string phone)
        {
            User user = await this.userService.FindAsync<User>(userId);
            if (user == null)
            {
                return new ApiResult<bool>() { Data = false, Message = $"{userId}用户不存在" };
            }
            user.UserName = userName;
            user.Phone = phone;
            return new ApiResult<bool>() { Data = await this.userService.UpdateAsync<User>(user), Message = "更新失败" };
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="pwd">更新后的密码</param>
        /// <returns></returns>
        [HttpPut("{userId}/{pwd}")]
        public async Task<ApiResult<bool>> UpdateUserPassword(string userId, string pwd)
        {
            User user = await this.userService.FindAsync<User>(userId);
            if (user == null)
            {
                return new ApiResult<bool>() { Data = false, Message = $"{userId}用户不存在" };
            }
            user.PassWord = pwd;
            return new ApiResult<bool>() { Data = await this.userService.UpdateAsync<User>(user), Message = "更新失败" };
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public bool DeleteUser(string userId) {

             var res= this.userService.Delete<User>(userId);
            return res;
        }

        /// <summary>
        /// 获取所有用户数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int GetUserCount()
        {
            return this.userService.GetAllUserCount();
        } 
        
    }
}
