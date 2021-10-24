using BasicController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace HomeVideo.Web.Controllers
{
    public class TestController : BaseController
    {
        [TypeFilter(typeof(AllowAnonymousFilter))]
        public JsonResult T()
        {
            var t = HttpContext.User.Claims;
            var data = userBLL.Page(1, 10, out var count, null, null);
            return resultInfo.Success(data: PageData.Create(data, count));
        }
    }
}
