using Common.Lib;
using Common.Lib.AutoFac;
using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.BLL.Menu
{
    public interface IMenuBLL :
        IDependency,
        ICommonWithLockBLL<MenuInfo, UserInfo>
    {
        bool AddSubMenuInfo(MenuInfo data, out string msg, UserInfo token);

        IEnumerable<MenuShowInfo> GetMenuForShow(UserInfo token);

        IEnumerable<MenuShowInfo> QueryAllMenus(UserInfo token);
    }
}
