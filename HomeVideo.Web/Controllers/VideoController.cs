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
        public JsonResult AddVideo(VideoInfo info)
            => NoDataCaller(videoBLL.AddInfo, info);

        [TypeFilter(typeof(AllowAnonymousFilter))]
        public JsonResult ModifyVideo(VideoInfo info)
            => NoDataCaller(videoBLL.UpdateInfo, info);

        [TypeFilter(typeof(AllowAnonymousFilter))]
        public JsonResult DeleteVideo(VideoInfo info)
            => NoDataCaller(videoBLL.DeleteInfo, info);

        [TypeFilter(typeof(AllowAnonymousFilter))]
        public JsonResult PageVideo(PageArgs<VideoInfo> info)
        {
            var data = videoBLL.Page(info.PageIndex, info.PageSize, out var total, info.Data, Token);

            return data == null ? resultInfo.Fail() : resultInfo.Success(data: PageData.Create(data, total));
        }
    }
}
