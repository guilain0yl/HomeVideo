using ArgCheck.Core;
using Common.Lib;
using Drapper.Core.SqlStringHelper;
using HomeVideo.DAL.Video;
using HomeVideo.DTO;
using HomeVideo.DTO.BasicDTO;
using System;
using System.Collections.Generic;
using System.Text;
using Util;

namespace HomeVideo.BLL.Video
{
    public class VideoBLL
        : IVideoBLL
    {
        public IVideoDAL videoDAL { get; set; }

        bool ICommonBLL<VideoInfo, UserInfo>.AddInfo(VideoInfo data, out string msg, UserInfo token)
        {
            data.TrimStringPropAll();

            if (!data.CheckObject(out msg))
            {
                return false;
            }

            return videoDAL.InsertData(data) > 0;
        }

        bool ICommonBLL<VideoInfo, UserInfo>.UpdateInfo(VideoInfo data, out string msg, UserInfo token)
        {
            data.TrimStringPropAll();

            if (!data.CheckObject(out msg))
            {
                return false;
            }

            FilterInfo[] filterInfos = new FilterInfo[] { FilterInfo.Equal(nameof(VideoInfo.Id)) };


            return videoDAL.UpdateData(data, new string[] {
                nameof(VideoInfo.Name),
                nameof(VideoInfo.Cover),
                nameof(VideoInfo.PublishTime),
                nameof(VideoInfo.Path)
            }, filterInfos);
        }

        bool ICommonBLL<VideoInfo, UserInfo>.DeleteInfo(VideoInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;

            if (data.Password?.Trim() != "19951004")
            {
                msg = "密码错误！";
                return false;
            }

            FilterInfo[] filterInfos = new FilterInfo[] { FilterInfo.Equal(nameof(VideoInfo.Id)) };

            return videoDAL.DeleteDataLogic(data, filterInfos);
        }

        IEnumerable<VideoInfo> ICommonBLL<VideoInfo, UserInfo>.Page(int pageIndex, int pageSize, out long total, VideoInfo data, UserInfo token)
        {
            List<FilterInfo> filterInfos = new List<FilterInfo>();

            if (data != null)
            {
                data.TrimStringPropAll();

                if (!data.Name.IsNullOrEmpty())
                {
                    filterInfos.Add(FilterInfo.Like(nameof(VideoInfo.Name)));
                }
            }

            return videoDAL.Pages(pageIndex, pageSize, out total, filterInfos, data);
        }
    }
}
