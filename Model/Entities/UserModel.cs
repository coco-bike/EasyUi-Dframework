using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string TelNumber { get;set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EMail { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public long Count { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadPicUrl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime BuildTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 最新登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 状态(0：删除，1：启用，2：禁用)
        /// </summary>
        public int State { get; set; }
        public virtual ICollection<RoleModel> RoleModels { get; set; }
    }
}
