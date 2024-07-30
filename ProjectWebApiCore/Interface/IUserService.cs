namespace ProjectWebApiCore.Interface
{
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// 获取所有在职用户的数量
        /// </summary>
        /// <returns></returns>
        int GetAllUserCount();
        //public void Add();
        //public void Update();
        //public void Query();
        //public void Delete(); 

    }
}