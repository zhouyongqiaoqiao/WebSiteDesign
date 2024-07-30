using AutoMapper;
using FactoryWebApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWebApiCore.Entity.EntityDto;
using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Interface;
using ProjectWebApiCore.Result;

namespace FactoryWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")] //全部按每个方法名区分
    //[Route("[controller]")]
    public class SystemController : Controller
    {
        private ISystemlogService logService;
        private IMapper _mapper;

        public SystemController(ISystemlogService systemlogService, IMapper mapper)
        {
            this.logService = systemlogService;
            this._mapper = mapper;
        }

        private IActionResult Index()
        {
            return new JsonResult(new { status = false,msg="ddd" });
            //return View();
        }

        /// <summary>
        /// 获取系统日志列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        //[Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost]
        [HttpGet("{pageIndex:int}/{pageSize:int}/{startDate:double}/{endDate:double}")]
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public ApiResult<PagingData<SystemLogDto>> GetSystemLogPage(int pageIndex,int pageSize, double? startDate, double? endDate)
        {
            try
            {
                DateTime startTime, endTime;
                if (startDate == null || endDate == null)
                {
                    DateTime now = DateTime.Now;
                    startTime = new DateTime(now.Year, now.Month, now.Day);
                    endTime = new DateTime(now.Year, now.Month, now.Day).AddDays(1);
                }
                else {
                    startTime = TimeHelper.GetRealTime4Seconds(startDate.Value);
                    endTime = TimeHelper.GetRealTime4Seconds(endDate.Value);
                }
                PagingData<SystemLog> list = this.logService.QueryPage<SystemLog>((s) => s.Date >= startTime && s.Date <= endTime, pageSize, pageIndex, (log) => log.Id, false);
                PagingData<SystemLogDto> result = this._mapper.Map<PagingData<SystemLog>, PagingData<SystemLogDto>>(list);
                return new ApiResult<PagingData<SystemLogDto>>() { Data = result };
            }
            catch (Exception ex)
            {
                return new ApiResult<PagingData<SystemLogDto>>() { Success = false, Message = ex.Message };
            }

        }
    }
}
