using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens; //引用的是Microsoft.IdentityModel.Tokens System.IdentityModel.Tokens
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizationServer.Utility
{
    public class CustomJWTService : ICustomJWTService
    {
        public static readonly JWTTokenOptions JWTTokenOptions = new JWTTokenOptions() { Issuer="http://127.0.0.1",SecurityKey = "我是周勇，这是我的加密Key,长度最好大于64", Audience="http://127.0.0.1"};
        public CustomJWTService(IOptionsMonitor<JWTTokenOptions> options)
        {
           // this.JWTTokenOptions = options.CurrentValue;
            ThrowIfInvalidOptions(JWTTokenOptions);
        }

        public string GetToken(CurrentUser user)
        {
            //1.定义需要使用到的Claims(有效的载荷)
            Claim[] claims = new[] {
                new Claim("name",user.Name),
                new Claim(ClaimTypes.Role,user.ID),
                new Claim("id",user.ID),
                new Claim("roldId",user.roldID.ToString()),
                new Claim("phone","13545645369"),

            };
            //2. 准备加密Key
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTTokenOptions.SecurityKey));
            //3. 选择加密方式，生成凭证 Credentials
            SigningCredentials creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                JWTTokenOptions.Issuer,
                JWTTokenOptions.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(50),
                creds
                );
            //4. 生成token
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static void ThrowIfInvalidOptions(JWTTokenOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ExpirationTime <= DateTime.Now)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(Utility.JWTTokenOptions.ExpirationTime));
            }

            //if (options.SigningCredentials == null)
            //{
            //    throw new ArgumentNullException(nameof(JWTTokenOptions.SigningCredentials));
            //}

            //if (options.JtiGenerator == null)
            //{
            //    throw new ArgumentNullException(nameof(JWTTokenOptions.JtiGenerator));
            //}
        }
    }
}
