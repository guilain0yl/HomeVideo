using Drapper.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeVideo.DTO.BasicDTO
{
    public class RoleAndMenuInfo
    {
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; } = 0;

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuId { get; set; } = 0;

    }
}
