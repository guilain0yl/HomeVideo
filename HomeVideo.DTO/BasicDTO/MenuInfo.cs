using ArgCheck.Core;
using Common.Lib.DataHelper;
using Drapper.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HomeVideo.DTO.BasicDTO
{
    public class MenuInfo : OwnerCommonInfo
    {
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [NotNullCondition("菜单名称不能为空！")]
        public string MenuName { get; set; } = string.Empty;

        /// <summary>
        /// 页面路径
        /// </summary>
        [NotNullCondition("菜单页面路径不能为空！")]
        public string PageUrl { get; set; } = string.Empty;

        /// <summary>
        /// 菜单等级 1为一级菜单
        /// </summary>
        public int Level { get; set; } = 0;

        /// <summary>
        /// 权径 |1:2|，一级菜单，菜单ID为2
        /// </summary>
        [JsonIgnore]
        public string TreePath { get; set; } = string.Empty;

        /// <summary>
        /// 父菜单ID 顶级菜单为自己
        /// </summary>
        public int ParentId { get; set; } = 0;

        /// <summary>
        /// 根级（一级）菜单 ID 顶级菜单为自己
        /// </summary>
        public int RootId { get; set; } = 0;

        /// <summary>
        /// 同级菜单排序
        /// </summary>
        public int OrderIndex { get; set; } = 0;

        [Extra]
        public new int OwnerId { get; set; }
    }
}
