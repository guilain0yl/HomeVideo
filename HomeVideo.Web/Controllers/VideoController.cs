using BasicController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.DTO;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace HomeVideo.Web.Controllers
{
    public class VideoController : BaseController
    {
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [HttpPost]
        public JsonResult AddVideo([FromForm] VideoInfo info)
        {
            var flag = videoBLL.AddInfo(info, out resultInfo.extMessage, null);
            return flag ? resultInfo.Success() : resultInfo.Fail();
        }

        [TypeFilter(typeof(AllowAnonymousFilter))]
        [HttpPost]
        public JsonResult ModifyVideo([FromForm] VideoInfo info)
        {
            var flag = videoBLL.UpdateInfo(info, out resultInfo.extMessage, null);
            return flag ? resultInfo.Success() : resultInfo.Fail();
        }

        [TypeFilter(typeof(AllowAnonymousFilter))]
        [HttpPost]
        public JsonResult DeleteVideo([FromForm] VideoInfo info)
        {
            var flag = videoBLL.DeleteInfo(info, out resultInfo.extMessage, null);
            return flag ? resultInfo.Success() : resultInfo.Fail();
        }

        [TypeFilter(typeof(AllowAnonymousFilter))]
        [HttpPost]
        public JsonResult PageVideo([FromForm] PageArgs<VideoInfo> info)
        {
            var data = videoBLL.Page(info.PageIndex, info.PageSize, out var total, info.Data, null);

            return data == null ? resultInfo.Fail() : resultInfo.Success(data: PageData.Create(data, total));
        }
    }
}
