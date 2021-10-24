using BasicController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.Web.Controllers
{
    public class UserController : BaseController
    {
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [HttpPost]
        public JsonResult Login(string loginName, string password)
        {
            var flag = userBLL.Login(loginName, password, out var token, out resultInfo.extMessage);

            return flag ? resultInfo.Success(data: token) : resultInfo.Fail();
        }

        public JsonResult Logout()
        {
            var flag = userBLL.LogOut(out resultInfo.extMessage, Token);

            return flag ? resultInfo.Success() : resultInfo.Fail();
        }

        public JsonResult AddUser(UserInfo info)
            => NoDataCaller(userBLL.AddInfo, info);

        public JsonResult ModifyUser(UserInfo info)
            => NoDataCaller(userBLL.UpdateInfo, info);

        public JsonResult ModifyPassword(UserInfo info)
            => NoDataCaller(userBLL.UpdatePassword, info);

        public JsonResult LockUser(UserInfo info)
            => NoDataCaller(userBLL.LockInfo, info);

        public JsonResult UnlockUser(UserInfo info)
            => NoDataCaller(userBLL.UnlockInfo, info);

        public JsonResult DeleteUser(UserInfo info)
            => NoDataCaller(userBLL.DeleteInfo, info);

        public JsonResult PageUser(PageArgs<UserInfo> info)
        {
            var data = userBLL.Page(info.PageIndex, info.PageSize, out var total, info.Data, Token);

            return data == null ? resultInfo.Fail() : resultInfo.Success(data: PageData.Create(data, total));
        }
    }
}
