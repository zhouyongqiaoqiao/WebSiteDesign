using ProjectWebApiCore.EntityEnum;

namespace ProjectWebApiCore.Entity.EntityDto
{
    ///<summary>
    /// 用户
    ///</summary>
    public class UserDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }


        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = string.Empty;


        /// <summary>
        /// 离职时间
        /// </summary> 
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态文字描述
        /// </summary>
        public string StatusDescription
        {
            get
            {
                return AGBasic.EnumDescription.GetFieldText((StatusEnum)Status);
            }
        }
    }
}
