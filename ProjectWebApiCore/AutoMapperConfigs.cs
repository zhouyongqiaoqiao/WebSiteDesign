using AutoMapper;
using ProjectWebApiCore.Entity.EntityDto;
using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Result;

namespace ProjectWebApiCore.BusinessInterface
{
    public class AutoMapperConfigs : Profile
    {
        public AutoMapperConfigs()
        {
            //在这里就可以配置映射关系了
            CreateMap<SystemLog, SystemLogDto>()
               .ForMember(c=>c.StringDateTime,s=>s.MapFrom(x=>x.Date.ToString("yyyy年MM月dd日 HH:mm:ss")))
                .ReverseMap();
            CreateMap<PagingData<SystemLog>, PagingData<SystemLogDto>>().ReverseMap();

             
            CreateMap<User, UserDto>() 
              .ReverseMap();
            CreateMap<PagingData<User>, PagingData<UserDto>>().ReverseMap();
            CreateMap<WebSite, WebSiteDto>().ReverseMap();
            CreateMap<PagingData<WebSite>, PagingData<WebSiteDto>>().ReverseMap();
        }
    }
}
