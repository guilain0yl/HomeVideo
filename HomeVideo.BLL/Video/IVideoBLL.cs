using Common.Lib;
using Common.Lib.AutoFac;
using HomeVideo.DTO;
using HomeVideo.DTO.BasicDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeVideo.BLL.Video
{
    public interface IVideoBLL
        : IDependency,
        ICommonBLL<VideoInfo, UserInfo>
    {
    }
}
