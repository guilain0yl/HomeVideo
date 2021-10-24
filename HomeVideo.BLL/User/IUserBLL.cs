using Common.Lib;
using Common.Lib.AutoFac;
using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.Util;

namespace HomeVideo.BLL.User
{
    public interface IUserBLL :
        IDependency,
        ICommonWithLockBLL<UserInfo, UserInfo>
    {

        bool Login(string loginName, string password, out TokenInfo token, out string msg);

        bool LogOut(out string msg, UserInfo token);

        bool UpdatePassword(UserInfo data, out string msg, UserInfo token);
    }
}
