using ArgCheck.Core;
using Common.Lib;
using Common.Lib.DataHelper;
using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeVideo.DAL.Menu;
using HomeVideo.DAL.RoleAndMenu;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.BLL.Menu
{
    public class MenuBLL : IMenuBLL
    {
        public IMenuDAL menuDAL { get; set; }

        public IRoleAndMenuDAL roleAndMenuDAL { get; set; }

        bool ICommonBLL<MenuInfo, UserInfo>.AddInfo(MenuInfo data, out string msg, UserInfo token)
        {
            if (!data.CheckObject(out msg))
                return false;

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel, true);
            data.Level = 1;

            // 清楚已有缓存
            MenuStringHelper.Cache.Clear();

            var tran = menuDAL.BeginTransaction();

            try
            {
                var id = menuDAL.InsertData(data, tran);

                if (id < 1)
                {
                    menuDAL.RollbackTransaction(tran);
                    msg = "添加顶级菜单失败！";
                    return false;
                }

                data.Id = id;
                data.TreePath = $"|{data.Level}:{id}|";
                data.RootId = data.ParentId = id;

                FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(MenuInfo.Id))};
                if (!menuDAL.UpdateData(data, new string[] {
                    nameof(MenuInfo.TreePath),
                    nameof(MenuInfo.ParentId),
                    nameof(MenuInfo.RootId)
                }, filterInfos, tran))
                {
                    menuDAL.RollbackTransaction(tran);
                    msg = "修改顶级菜单相关信息失败！";
                    return false;
                }

                menuDAL.CommitTransaction(tran);
                return true;
            }
            catch
            {
                menuDAL.RollbackTransaction(tran);
                return false;
            }
        }

        bool IMenuBLL.AddSubMenuInfo(MenuInfo data, out string msg, UserInfo token)
        {
            if (data.ParentId < 1)
            {
                msg = "子菜单的父菜单ID错误！";
                return false;
            }

            if (!data.CheckObject(out msg))
                return false;

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel, true);
            data.Level = 1;

            // 清楚已有缓存
            MenuStringHelper.Cache.Clear();

            var tran = menuDAL.BeginTransaction();

            try
            {
                FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(MenuInfo.Id))};

                var parentInfo = menuDAL.SingleColumns(null, filterInfos, new { Id = data.ParentId }, tran);

                if (parentInfo == null)
                {
                    menuDAL.RollbackTransaction(tran);
                    msg = "查询父级菜单信息失败！";
                    return false;
                }

                data.Level = parentInfo.Level + 1;
                data.RootId = parentInfo.RootId;

                var id = menuDAL.InsertData(data, tran);

                if (id < 1)
                {
                    menuDAL.RollbackTransaction(tran);
                    msg = "添加子菜单失败！";
                    return false;
                }

                data.TreePath = $"{parentInfo.TreePath}{data.Level}:{id}|";

                if (!menuDAL.UpdateData(data, new string[] {
                    nameof(MenuInfo.TreePath)
                }, filterInfos, tran))
                {
                    menuDAL.RollbackTransaction(tran);
                    msg = "修改父级菜单相关信息失败！";
                    return false;
                }

                menuDAL.CommitTransaction(tran);
                return true;
            }
            catch
            {
                menuDAL.RollbackTransaction(tran);
                return false;
            }
        }

        bool ICommonBLL<MenuInfo, UserInfo>.UpdateInfo(MenuInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;
            if (!data.CheckObject(out msg))
                return false;

            // 清楚已有缓存
            MenuStringHelper.Cache.Clear();

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);

            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(MenuInfo.Id))};

            return menuDAL.UpdateData(data, new string[] { nameof(MenuInfo.MenuName), nameof(MenuInfo.PageUrl), nameof(MenuInfo.OrderIndex) }, filterInfos);
        }

        bool ICommonWithLockBLL<MenuInfo, UserInfo>.LockInfo(MenuInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;
            if (token.OwnerLevel != OwnerLevelEnum.Manager)
            {
                msg = "当前账户无法进行高权限操作！";
                return false;
            }

            // 清楚已有缓存
            MenuStringHelper.Cache.Clear();

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);

            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(MenuInfo.Id))};

            return menuDAL.LockData(data, filterInfos);
        }

        bool ICommonWithLockBLL<MenuInfo, UserInfo>.UnlockInfo(MenuInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;

            if (token.OwnerLevel != OwnerLevelEnum.Manager)
            {
                msg = "当前账户无法进行高权限操作！";
                return false;
            }

            // 清楚已有缓存
            MenuStringHelper.Cache.Clear();

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);

            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(MenuInfo.Id))};

            return menuDAL.UnlockData(data, filterInfos);
        }

        bool ICommonBLL<MenuInfo, UserInfo>.DeleteInfo(MenuInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;
            if (token.OwnerLevel != OwnerLevelEnum.Manager)
            {
                msg = "当前账户无法进行高权限操作！";
                return false;
            }

            // 清楚已有缓存
            MenuStringHelper.Cache.Clear();

            data.FillInfo(token.Id, token.LoginName, token.OwnerLevel);

            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(MenuInfo.Id))};

            return menuDAL.DeleteDataLogic(data, filterInfos);
        }

        IEnumerable<MenuInfo> ICommonBLL<MenuInfo, UserInfo>.Page(int pageIndex, int pageSize, out long total, MenuInfo data, UserInfo token)
        {
            total = 0;

            if (token.OwnerLevel != OwnerLevelEnum.Manager)
            {
                return null;
            }

            return menuDAL.Pages(pageIndex, pageSize, out total, null);
        }

        #region ShowMenu

        IEnumerable<MenuShowInfo> IMenuBLL.QueryAllMenus(UserInfo token)
        {
            string cacheKey = $"POWER_{token.OwnerLevel}";

            if (!MenuStringHelper.Cache.TryGetValue(cacheKey, out var result))
            {
                FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(MenuInfo.OwnerLevel))};

                var menuInfos = menuDAL.Query(new string[] { nameof(MenuInfo.Id), nameof(MenuInfo.MenuName), nameof(MenuInfo.OrderIndex) }, filterInfos, new MenuInfo { OwnerLevel = token.OwnerLevel });

                result = MenuStringHelper.GenerateMenuSting(menuInfos);

                MenuStringHelper.Cache.TryAdd(cacheKey, result);
            }

            return result;
        }

        IEnumerable<MenuShowInfo> IMenuBLL.GetMenuForShow(UserInfo token)
        {
            // 小于1是特权 显示当前层级所有菜单
            string cacheKey = token.RoleId < 0 ? $"LEVEL_{token.OwnerLevel}" : $"ROLEID_{token.RoleId}";

            if (!MenuStringHelper.Cache.TryGetValue(cacheKey, out var result))
            {
                result = token.RoleId < 0
                    ? GetMenuForShowByLevel(token)
                    : GetMenuForShowByRoleId(token);

                MenuStringHelper.Cache.TryAdd(cacheKey, result);
            }

            return result;
        }

        private IEnumerable<MenuShowInfo> GetMenuForShowByLevel(UserInfo token)
        {
            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(MenuInfo.OwnerLevel))};
            var menuInfos = menuDAL.Query(null, filterInfos, new MenuInfo { OwnerLevel = token.OwnerLevel });

            return MenuStringHelper.GenerateMenuSting(menuInfos);
        }

        private IEnumerable<MenuShowInfo> GetMenuForShowByRoleId(UserInfo token)
        {
            IEnumerable<MenuInfo> menuInfos = null;

            FilterInfo[] filterInfos = new FilterInfo[] {
                FilterInfo.Equal(nameof(RoleAndMenuInfo.RoleId))};
            var powers = roleAndMenuDAL.Query(null, filterInfos, new RoleAndMenuInfo { RoleId = token.RoleId });
            if (powers != null)
                menuInfos = menuDAL.QueryMenusWithIds(powers.Select(x => x.MenuId));
            else
                menuInfos = null;

            return MenuStringHelper.GenerateMenuSting(menuInfos);
        }

        #endregion
    }
}
