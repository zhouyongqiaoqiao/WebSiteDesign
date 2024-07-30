using ProjectWebApiCore.Entity.EntityMap;
using ProjectWebApiCore.Interface;
using SqlSugar;

namespace ProjectWebApiCore.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(ISqlSugarClient sqlSugarClient) : base(sqlSugarClient)
        {
        }

        public int GetAllUserCount()
        {
            return this.SqlClient.Queryable<User>().Where((x) => x.Status == 1).Count();
        }
    }
}
