using Drapper.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace Common.Lib.DataHelper
{
    public class CommonInfo : ILogicDeleteInfo
    {
        public LockStateEnum State { get; set; } = LockStateEnum.Unlocked;

        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;

        public int CreatorId { get; set; } = 0;

        public string CreatorName { get; set; } = string.Empty;

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public int ModifierId { get; set; } = 0;

        public string ModifierName { get; set; } = string.Empty;

        public DateTime ModifiyTime { get; set; } = DateTime.Now;

        public void FillInfo(int loginId, string loginName, bool IsInsert = false)
        {
            if (IsInsert)
            {
                CreatorId = loginId;
                CreatorName = loginName;
                CreateTime = DateTime.Now;
            }
            ModifierId = loginId;
            ModifierName = loginName;
            ModifiyTime = DateTime.Now;
        }
    }

    public class OwnerCommonInfo : CommonInfo
    {
        /// <summary>
        /// 归属层级
        /// </summary>
        [JsonIgnore]
        public OwnerLevelEnum OwnerLevel { get; set; } = OwnerLevelEnum.ALL;

        [JsonIgnore]
        public int OwnerId { get; set; } = 0;

        public void FillInfo(int loginId, string loginName, OwnerLevelEnum ownerLevelEnum, bool IsInsert = false)
        {
            OwnerLevel = ownerLevelEnum;
            FillInfo(loginId, loginName, IsInsert);
        }
    }
}
