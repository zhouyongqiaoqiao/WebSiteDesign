namespace FactoryWebApi.Utils
{
    public class TimeHelper
    {
        private static readonly DateTime MinTime = new DateTime(1970, 1, 1,8,0,0,DateTimeKind.Utc);

        /// <summary>
        /// 根据秒数获取真实的时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime GetRealTime4Seconds(double seconds)
        {
            return MinTime.AddSeconds(seconds);
        }
    }
}
