using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeVideo.DAL.RoleAndMenu;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.BLL.RoleAndMenu
{
    public class RoleAndMenuBLL : IRoleAndMenuBLL
    {
        public IRoleAndMenuDAL roleAndMenuDAL { get; set; }

        bool IRoleAndMenuBLL.AddMenuAndRole(IEnumerable<RoleAndMenuInfo> roleAndMenuInfos, out string msg, UserInfo token)
        {
            msg = string.Empty;

            if (roleAndMenuInfos.Count() <= 0)
            {
                msg = "权限信息项不能为空";
                return false;
            }

            return roleAndMenuDAL.InsertRoleAndMenuInfos(roleAndMenuInfos);
        }

        IEnumerable<RoleAndMenuInfo> IRoleAndMenuBLL.QueryMenuAndRole(int roleId)
        {
            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(RoleAndMenuInfo.RoleId))};

            return roleAndMenuDAL.Query(null, filterInfos, new RoleAndMenuInfo { RoleId = roleId });
        }
    }
}
