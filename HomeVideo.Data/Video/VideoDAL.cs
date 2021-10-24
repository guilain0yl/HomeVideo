using Common.Lib.DataHelper;
using HomeVideo.DTO;
using HomeVideo.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeVideo.DAL.Video
{
    public class VideoDAL
        : SqlOperationWithLogicDeleteAdapter<VideoInfo>,
        IVideoDAL
    {
        protected override string ConnStr => ConnectionStrings.VideoConnectiong;
    }
}
