using ArgCheck.Core;
using Common.Lib;
using Common.Lib.DataHelper;
using Drapper.Core.SqlStringHelper;
using System.Collections.Generic;
using System.Linq;
using Util;
using HomeVideo.Util;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.DAL.User;

namespace HomeVideo.BLL.User
{
    public class UserBLL : IUserBLL
    {
        public IUserDAL userDAL { get; set; }

        bool IUserBLL.Login(string loginName, string password, out TokenInfo token, out string msg)
        {
            loginName = loginName?.Trim();
            password = password?.Trim();
            token = null;
            msg = string.Empty;

            if (loginName.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                msg = "登录名或密码为空！";
                return false;
            }

            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(UserInfo.LoginName)),
                FilterInfo.Equal(nameof(UserInfo.LoginPassword))};

            password = MD5Helper.MD5HashLower(password);
            var info = userDAL.SingleColumns(null, filterInfos,
                new UserInfo
                {
                    LoginName = loginName
                ,
                    LoginPassword = password
                });

            if (info == null)
            {
                msg = "登录名或密码错误！";
                return false;
            }

            if (info.State == LockStateEnum.Locked)
            {
                msg = "当前账户已被冻结！";
                return false;
            }

            if (!CacheHelper.SessionCache.SetValue(loginName, info))
            {
                msg = "缓存用户信息失败！";
                return false;
            }

            msg = "登录成功！";
            token = TokenInfo.GenerateToken(loginName);

            return true;
        }

        bool IUserBLL.LogOut(out string msg, UserInfo token)
        {
            bool flag = CacheHelper.SessionCache.RemoveKey(token.LoginName?.Trim());

            msg = flag ? "登出成功" : "登出失败";

            return flag;
        }

        bool ICommonBLL<UserInfo, UserInfo>.AddInfo(UserInfo data, out string msg, UserInfo token)
        {
            if (!data.CheckObject(out msg))
                return false;

            data.LoginPassword = MD5Helper.MD5HashLower(data.LoginPassword?.Trim());

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel, true);
            data.TrimStringPropAll();

            var tran = userDAL.BeginTransaction();

            try
            {
                if (userDAL.ExistLoginName(data.LoginName, tran))
                {
                    msg = "登录名已存在！";
                    userDAL.RollbackTransaction(tran);
                    return false;
                }

                if (userDAL.InsertData(data) < 1)
                {
                    msg = "添加账号信息失败！";
                    userDAL.RollbackTransaction(tran);
                    return false;
                }

                userDAL.CommitTransaction(tran);
                return true;
            }
            catch
            {
                userDAL.RollbackTransaction(tran);
                msg = "添加账号异常！";
                return false;
            }
        }

        bool ICommonBLL<UserInfo, UserInfo>.UpdateInfo(UserInfo data, out string msg, UserInfo token)
        {
            if (!data.CheckObject(out msg))
                return false;

            data.LoginPassword = MD5Helper.MD5HashLower(data.LoginPassword?.Trim());

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);
            data.TrimStringPropAll();

            var filterInfos = GetFilterInfos(data, token);

            return userDAL.UpdateData(data, new string[] {
                nameof(UserInfo.UserName),
                nameof(UserInfo.LoginPassword),
                nameof(UserInfo.RoleId)
            }, filterInfos);
        }

        bool IUserBLL.UpdatePassword(UserInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;
            data.LoginPassword = data.LoginPassword?.Trim();

            if (string.IsNullOrEmpty(data.LoginPassword))
            {
                msg = "请输入新密码！";
                return false;
            }

            data.LoginPassword = MD5Helper.MD5HashLower(data.LoginPassword?.Trim());

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);
            data.TrimStringPropAll();

            var filterInfos = GetFilterInfos(data, token);

            return userDAL.UpdateData(data, new string[] { nameof(UserInfo.LoginPassword) }, filterInfos);
        }

        bool ICommonWithLockBLL<UserInfo, UserInfo>.LockInfo(UserInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);

            var filterInfos = GetFilterInfos(data, token);

            return userDAL.LockData(data, filterInfos);
        }

        bool ICommonWithLockBLL<UserInfo, UserInfo>.UnlockInfo(UserInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);

            var filterInfos = GetFilterInfos(data, token);

            return userDAL.UnlockData(data, filterInfos);
        }

        bool ICommonBLL<UserInfo, UserInfo>.DeleteInfo(UserInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);

            FilterInfo[] filterInfos = GetFilterInfos(data, token);

            return userDAL.DeleteDataLogic(data, filterInfos);
        }

        IEnumerable<UserInfo> ICommonBLL<UserInfo, UserInfo>.Page(int pageIndex, int pageSize, out long total, UserInfo data, UserInfo token)
            => userDAL.Pages(pageIndex, pageSize, out total, data);

        FilterInfo[] GetFilterInfos(UserInfo data, UserInfo token)
        {
            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(UserInfo.Id))};
            if (token.OwnerLevel != OwnerLevelEnum.Manager)
            {
                filterInfos.Append(FilterInfo.Like(nameof(UserInfo.OwnerId)));
                data.OwnerId = token.OwnerId;
            }

            return filterInfos;
        }
    }
}
