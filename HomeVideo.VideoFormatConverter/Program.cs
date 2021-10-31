using System;

namespace HomeVideo.VideoFormatConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            // 30分钟运行一次的格式转换
            // 3天运行一次的图片视频清理
            VideoFormatConverter.ConvertFormat("/home/1.mp4", "/home/1.avi");
        }
    }
}
