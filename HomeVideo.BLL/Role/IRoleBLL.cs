using Common.Lib;
using Common.Lib.AutoFac;
using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.BLL.Role
{
    public interface IRoleBLL : IDependency,
        ICommonBLL<RoleInfo, UserInfo>
    {
        /// <summary>
        /// 查询所有的角色
        /// </summary>
        IEnumerable<RoleInfo> QueryRole(UserInfo token);
    }
}
