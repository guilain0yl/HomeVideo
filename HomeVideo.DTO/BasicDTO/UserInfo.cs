using ArgCheck.Core;
using Common.Lib.DataHelper;
using Drapper.Core;
using System.Text.Json.Serialization;

namespace HomeVideo.DTO.BasicDTO
{
    public class UserInfo : OwnerCommonInfo
    {
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [NotNullCondition("用户名称不能为空！")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 登录名
        /// </summary>
        [NotNullCondition("登录名不能为空！")]
        public string LoginName { get; set; } = string.Empty;

        /// <summary>
        /// 登录密码 (MD5hash)
        /// </summary>
        [JsonIgnore]
        [NotNullCondition("登录密码不能为空！")]
        public string LoginPassword { get; set; } = string.Empty;

        /// <summary>
        /// 角色ID -1为特权用户
        /// </summary>
        [JsonIgnore]
        [BiggerCondition(0, "角色不能为空！")]
        public int RoleId { get; set; } = 0;

        [JsonIgnore]
        [Extra]
        public string Role { get; set; }

        [Extra]
        public string RoleName => RoleId == -1 ? "超级管理员" : Role;
    }
}
