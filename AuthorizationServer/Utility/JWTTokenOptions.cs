namespace AuthorizationServer.Utility
{
    public class JWTTokenOptions
    {
        public string? Audience { get; set; }

        /// <summary>
        /// 加密Key （长度尽量保证在16字符以上）
        /// </summary>
        public string SecurityKey { get; set; } = "";

        public string? Issuer { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime => DateTime.Now + TimeSpan.FromSeconds(5);
    }
}
