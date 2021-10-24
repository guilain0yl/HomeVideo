﻿using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeVideo.Web.Controllers
{
    public static class MultipartRequestHelper
    {
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
                throw new InvalidDataException("Missing content-type boundary.");

            if (boundary.Length > lengthLimit)
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");

            return boundary;
        }

        public static bool IsMultipartContentType(string contentType)
            => !string.IsNullOrEmpty(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;

        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
            => contentDisposition != null
            && contentDisposition.DispositionType.Equals("form-data")
            && string.IsNullOrEmpty(contentDisposition.FileName.Value)
            && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
            => contentDisposition != null
            && contentDisposition.DispositionType.Equals("form-data")
            && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
            || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
    }
}
