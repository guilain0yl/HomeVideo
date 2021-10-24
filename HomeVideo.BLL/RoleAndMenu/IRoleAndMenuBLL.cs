using System;
using System.Collections.Generic;
using System.Text;
using Common.Lib.AutoFac;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.BLL.RoleAndMenu
{
    public interface IRoleAndMenuBLL : IDependency
    {
        bool AddMenuAndRole(IEnumerable<RoleAndMenuInfo> roleAndMenuRelaInfos, out string msg, UserInfo token);

        IEnumerable<RoleAndMenuInfo> QueryMenuAndRole(int roleId);
    }
}
