using BasicController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.Web.Controllers
{
    public class MenuController : BaseController
    {
        public JsonResult AddTopMenu(MenuInfo info)
            => NoDataCaller(menuBLL.AddInfo, info);

        public JsonResult AddSubMenu(MenuInfo info)
            => NoDataCaller(menuBLL.AddSubMenuInfo, info);

        public JsonResult ModifyMenu(MenuInfo info)
            => NoDataCaller(menuBLL.UpdateInfo, info);

        public JsonResult LockMenu(MenuInfo info)
            => NoDataCaller(menuBLL.LockInfo, info);

        public JsonResult UnlockMenu(MenuInfo info)
            => NoDataCaller(menuBLL.UnlockInfo, info);

        public JsonResult DeleteMenu(MenuInfo info)
            => NoDataCaller(menuBLL.DeleteInfo, info);

        public JsonResult PageMenu(PageArgs<MenuInfo> info)
        {
            var data = menuBLL.Page(info.PageIndex, info.PageSize, out var total, info.Data, Token);

            return data == null ? resultInfo.Fail() : resultInfo.Success(data: PageData.Create(data, total));
        }

        public JsonResult QueryMenusForShow()
            => resultInfo.Judge(data: menuBLL.GetMenuForShow(Token));

        public JsonResult QueryPowerMenu()
            => resultInfo.Judge(data: menuBLL.QueryAllMenus(Token));
    }
}
