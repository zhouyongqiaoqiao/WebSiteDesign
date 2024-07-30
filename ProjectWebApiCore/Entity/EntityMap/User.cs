using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApiCore.Entity.EntityMap
{
    ///<summary>
    ///用户
    ///</summary>
    public class User
    {
        [SugarColumn(IsPrimaryKey = true)] //, IsIdentity = true
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get ; set; }  

        /// <summary>
        /// 密码
        /// </summary>
        public string? PassWord { get; set; }
        
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } =string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 离职时间
        /// </summary>
        //[SugarColumn(IsNullable = true)]
        //public DateTime? DepartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 1;
    }
}
