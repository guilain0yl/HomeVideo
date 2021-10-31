using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HomeVideo.VideoFormatConverter
{
    public static class VideoFormatConverter
    {
        public static string GetVersion()
        {
            //ffmpeg -i input.mkv -vcodec copy -acodec copy out.mp4
            var process = new Process();
            process.StartInfo.FileName = "/usr/bin/ffmpeg";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            process.StartInfo.Arguments = $"-version";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            string result = process.StandardError.ReadToEnd();
            process.Close();
            process.Dispose();
            return result;
        }


        public static void ConvertFormat(string srcPath, string dstPath)
        {
            var process = new Process();
            process.StartInfo.FileName = "/usr/bin/ffmpeg";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            process.StartInfo.Arguments = $"-i {srcPath} -vcodec copy -acodec copy {dstPath}";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            process.Close();
            process.Dispose();
        }
    }
}
