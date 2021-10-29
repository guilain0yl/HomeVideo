using ArgCheck.Core;
using Common.Lib;
using Drapper.Core.SqlStringHelper;
using HomeVideo.DAL.Video;
using HomeVideo.DTO;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.Util;
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
                nameof(VideoInfo.PublishYear),
                nameof(VideoInfo.Description)
            }, filterInfos);
        }

        bool ICommonBLL<VideoInfo, UserInfo>.DeleteInfo(VideoInfo data, out string msg, UserInfo token)
        {
            msg = string.Empty;

            if (data.Password?.Trim() != AppSetting.Password?.Trim())
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

            data ??= new VideoInfo();
            data.IsDeleted = false;
            filterInfos.Add(FilterInfo.Equal(nameof(VideoInfo.IsDeleted)));

            return videoDAL.Pages(pageIndex, pageSize, out total, filterInfos, data);
        }
    }
}
