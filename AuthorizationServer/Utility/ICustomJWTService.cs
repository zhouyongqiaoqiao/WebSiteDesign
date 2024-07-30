namespace AuthorizationServer.Utility
{
    public interface ICustomJWTService
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetToken(CurrentUser user);
    }

    public interface IVerifyAccount
    {
        /// <summary>
        /// 检查登录用户是否有效
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        bool CheckUser(string userID, string pwd);
    }
}
