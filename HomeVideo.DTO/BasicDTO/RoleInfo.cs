using ArgCheck.Core;
using Common.Lib.DataHelper;
using Drapper.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeVideo.DTO.BasicDTO
{
    public class RoleInfo : OwnerCommonInfo
    {
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [NotNullCondition("角色名称不可以为空！")]
        public string RoleName { get; set; } = string.Empty;
    }
}
