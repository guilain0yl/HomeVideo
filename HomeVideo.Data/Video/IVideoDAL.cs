using Common.Lib.AutoFac;
using Common.Lib.DataHelper;
using HomeVideo.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeVideo.DAL.Video
{
    public interface IVideoDAL
        : IDependency,
        IBasicDataOperation<VideoInfo>,
        ILogicDeleteDataOperation<VideoInfo>,
        ISingleColumnsOperation<VideoInfo>
    {
    }
}
