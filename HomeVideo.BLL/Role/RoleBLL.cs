using ArgCheck.Core;
using Common.Lib;
using Common.Lib.DataHelper;
using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeVideo.DAL.Role;
using HomeVideo.DAL.User;
using HomeVideo.DTO.BasicDTO;
using Util;

namespace HomeVideo.BLL.Role
{
    public class RoleBLL : IRoleBLL
    {
        public IRoleDAL roleDAL { get; set; }

        public IUserDAL userDAL { get; set; }

        bool ICommonBLL<RoleInfo, UserInfo>.AddInfo(RoleInfo data, out string msg, UserInfo token)
        {
            if (!data.CheckObject(out msg))
                return false;

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel, true);

            return roleDAL.InsertData(data) > 0;
        }

        bool ICommonBLL<RoleInfo, UserInfo>.UpdateInfo(RoleInfo data, out string msg, UserInfo token)
        {
            if (!data.CheckObject(out msg))
                return false;

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);

            var filterInfos = GetFilterInfos(data, token);

            return roleDAL.UpdateData(data, new string[] {
                nameof(RoleInfo.RoleName)
            }, filterInfos);
        }

        bool ICommonBLL<RoleInfo, UserInfo>.DeleteInfo(RoleInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);
            var filterInfos = GetFilterInfos(data, token);

            var tran = roleDAL.BeginTransaction();
            try
            {
                if (userDAL.RoleInUse(data.Id, tran))
                {
                    roleDAL.RollbackTransaction(tran);
                    msg = "角色正在使用中，无法删除！";
                    return false;
                }

                if (!roleDAL.DeleteDataLogic(data, filterInfos, tran))
                {
                    roleDAL.RollbackTransaction(tran);
                    msg = "修改角色信息失败！";
                    return false;
                }

                roleDAL.CommitTransaction(tran);
                return true;
            }
            catch
            {
                roleDAL.RollbackTransaction(tran);
                return false;
            }
        }

        IEnumerable<RoleInfo> ICommonBLL<RoleInfo, UserInfo>.Page(int pageIndex, int pageSize, out long total, RoleInfo data, UserInfo token)
        {
            FilterInfo[] filterInfos = null;
            if (data != null && !data.RoleName.IsNullOrEmpty())
            {
                filterInfos = new FilterInfo[] {
                FilterInfo.Like(nameof(RoleInfo.RoleName))};
            }

            return roleDAL.Pages(pageIndex, pageSize, out total, filterInfos, data);
        }

        IEnumerable<RoleInfo> IRoleBLL.QueryRole(UserInfo token)
        {
            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(RoleInfo.OwnerId))};

           return roleDAL.Query(new string[] { nameof(RoleInfo.Id), nameof(RoleInfo.RoleName) }, filterInfos, new RoleInfo { OwnerId = token.OwnerId });
        }



        FilterInfo[] GetFilterInfos(RoleInfo data, UserInfo token)
        {
            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(UserInfo.Id))};
            if (token.OwnerLevel != OwnerLevelEnum.Manager)
            {
                filterInfos.Append(FilterInfo.Like(nameof(RoleInfo.OwnerId)));
                data.OwnerId = token.OwnerId;
            }

            return filterInfos;
        }
    }
}
