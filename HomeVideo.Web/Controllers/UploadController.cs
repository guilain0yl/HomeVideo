﻿using BasicController;
using HomeVideo.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Util;

namespace HomeVideo.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UploadController : ControllerBase
    {
        [DisableRequestSizeLimit]
        [TypeFilter(typeof(AllowAnonymousFilter))]
        [HttpPost]
        public async Task<JsonResult> UploadVideo()
        {
            try
            {
                var _targetFilePath = Path.Combine(Environment.CurrentDirectory, AppSetting.VideoPath);
                if (!Directory.Exists(_targetFilePath))
                {
                    Directory.CreateDirectory(_targetFilePath);
                }

                if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
                    return resultInfo.Fail("上传格式错误！");

                var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), _defaultFormOptions.MultipartBoundaryLengthLimit);
                var reader = new MultipartReader(boundary, HttpContext.Request.Body);
                var section = await reader.ReadNextSectionAsync();

                string filename = null;

                while (section != null)
                {
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition);

                    if (hasContentDispositionHeader)
                    {
                        if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                            return resultInfo.Fail("上传格式错误！");
                        else
                        {
                            var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);

                            byte[] filedata = null;


                            using (var memoryStream = new MemoryStream())
                            {
                                await section.Body.CopyToAsync(memoryStream);

                                if (memoryStream.Length == 0)
                                {
                                    return resultInfo.Fail("文件为空！");
                                }

                                filename = MD5Helper.MD5Hash(memoryStream) + Path.GetExtension(trustedFileNameForDisplay);
                                filedata = memoryStream.ToArray();
                            }

                            using var targetStream = System.IO.File.Create(Path.Combine(_targetFilePath, filename));
                            await targetStream.WriteAsync(filedata);
                        }
                    }

                    section = await reader.ReadNextSectionAsync();
                }

                if (filename.IsNullOrEmpty())
                {
                    return resultInfo.Fail("未找到文件！");
                }

                return resultInfo.Success("文件已创建", new { file = filename });
            }
            catch (Exception ex)
            {
                return resultInfo.Fail(ex.Message);
            }
        }

        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        ResultInfo resultInfo = new ResultInfo();
    }
}