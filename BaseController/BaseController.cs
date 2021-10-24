using Common.Lib;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HomeVideo.BLL.Menu;
using HomeVideo.BLL.Role;
using HomeVideo.BLL.RoleAndMenu;
using HomeVideo.BLL.User;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.Util;
using Util;
using HomeVideo.BLL.Video;

namespace BasicController
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [WebApiException]
    public class BaseController : ControllerBase
    {
        protected ResultInfo resultInfo = new ResultInfo();

        protected delegate bool CommonBllFunc<T>(T info, out string msg, UserInfo token);

        protected ResultInfo NoDataCaller<T>(CommonBllFunc<T> func, T data)
        {
            var flag = func(data, out resultInfo.extMessage, Token);
            return flag ? resultInfo.Success() : resultInfo.Fail();
        }

        protected UserInfo Token
        {
            get
            {
                var claim = HttpContext.User.Claims.Where(x => x.Type == ConstantHelper.ClaimKey).FirstOrDefault();
                if (claim == null || claim.Value.IsNullOrEmpty())
                    throw new ReloginException("用户信息已过期，请重新登陆。");

                var tmp = CacheHelper.SessionCache.GetValue<UserInfo>(claim.Value);

                if (tmp == null)
                {
                    CacheHelper.SessionCache.RemoveKey(claim.Value);
                    throw new ReloginException("用户信息已过期，请重新登陆。");
                }

                return tmp;
            }
        }

        public IUserBLL userBLL { get; set; }

        public IRoleBLL roleBLL { get; set; }

        public IMenuBLL menuBLL { get; set; }

        public IRoleAndMenuBLL roleAndMenuBLL { get; set; }

        public IVideoBLL videoBLL { get; set; }
    }
}
