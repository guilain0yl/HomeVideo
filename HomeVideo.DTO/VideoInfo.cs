using ArgCheck.Core;
using Common.Lib.DataHelper;
using Drapper.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeVideo.DTO
{
    public class VideoInfo : ILogicDeleteInfo
    {
        [PrimaryKey]
        public int Id { get; set; }

        [NotNullCondition("视频名称不能为空！")]
        public string Name { get; set; }

        [NotNullCondition("封面不能为空！")]
        public string Cover { get; set; }

        [NotNullCondition("简介不能为空！")]
        public string Description { get; set; }

        public DateTime PublishTime { get; set; } = DateTime.Now;

        [NotNullCondition("视频地址不能为空！")]
        public string Path { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        [Extra]
        public string Password { get; set; }
    }
}
