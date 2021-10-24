using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;



#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Util
* 项目描述 ：
* 类 名 称 ：HttpHelper
* 类 描 述 ：
* 所在的域 ：GUILAIN
* 命名空间 ：Util
* 机器名称 ：GUILAIN 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：guilain
* 创建时间 ：2019/10/24 11:41:11
* 更新时间 ：2019/10/24 11:41:11
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ guilain 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

namespace Util
{
    public class HttpHelper
    {
        public static readonly string formUrlencoded = "application/x-www-form-urlencoded; charset=utf-8;";

        public static readonly string fileData = "multipart/form-data; ";

        public static readonly string json = "application/json; charset=utf-8;";

        public static readonly string xml = "application/xml;charset=UTF-8";

        #region http get

        public static string HttpGet(string url, string Authorization = "") => HttpGet(url, string.Empty, string.Empty, Authorization, string.Empty, string.Empty);

        public static string HttpGet(string url, string userAgent, string referer = null, string Authorization = null, string certPath = null, string certPassword = null, IEnumerable<KeyValuePair<string, string>> keyValuePairs = null)
        {
            string result = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0";
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Accept, "*/*");

                if (!referer.IsNullOrEmpty())
                {
                    request.Referer = referer;
                }
                if (!userAgent.IsNullOrEmpty())
                {
                    request.UserAgent = userAgent;
                }
                if (!Authorization.IsNullOrEmpty())
                {
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);
                }
                if (!certPath.IsNullOrEmpty())
                {
                    X509Certificate2 cert = new X509Certificate2(certPath, certPassword);
                    request.ClientCertificates.Add(cert);
                }

                if (keyValuePairs != null)
                {
                    foreach (var item in keyValuePairs)
                    {
                        request.Headers.Add(item.Key.Trim(), item.Value.Trim());
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    var cookie = response.Cookies;

                    // 确认html编码
                    string ct = response.ContentType;
                    int pos = ct.IndexOf(htmlEncoding);
                    string htmlEncoder = string.Empty;
                    if (pos > 0)
                    {
                        htmlEncoder = ct.Substring(pos + htmlEncoding.Length).Trim();
                    }

                    string encoding = string.IsNullOrEmpty(htmlEncoder) ? response.ContentEncoding : htmlEncoder;
                    if (string.IsNullOrEmpty(encoding))
                    {
                        // 默认编码
                        encoding = "UTF-8";
                    }

                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region http post

        public static string HttpPost(string url, string data) => HttpPost(url, data, string.Empty);

        public static string HttpPost(string url, string data, string Authorization = "") => HttpPost(url, data, string.Empty, formUrlencoded, Authorization, string.Empty, string.Empty);

        public static string HttpPost(string url, string data, string userAgent = "", string contentType = "", string Authorization = "", string certPath = "", string certPassword = "", IEnumerable<KeyValuePair<string, string>> keyValuePairs = null)
        {
            string result = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                request.Method = "POST";
                request.KeepAlive = false;
                request.Headers.Add(HttpRequestHeader.Accept, "*/*");
                request.Headers.Add(HttpRequestHeader.UserAgent, "PostmanRuntime/7.23.0");

                if (!userAgent.IsNullOrEmpty())
                {
                    request.UserAgent = userAgent;
                }

                if (!Authorization.IsNullOrEmpty())
                {
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);
                }

                if (keyValuePairs != null)
                {
                    foreach (var item in keyValuePairs)
                    {
                        request.Headers.Add(item.Key.Trim(), item.Value.Trim());
                    }
                }

                if (!certPath.IsNullOrEmpty())
                {
                    X509Certificate2 cert = new X509Certificate2(certPath, certPassword);
                    request.ClientCertificates.Add(cert);
                }

                Encoding encodingOut = Encoding.UTF8;

                if (contentType.IndexOf("GBK") > 0)
                {
                    encodingOut = Encoding.GetEncoding("GBK");
                }

                var postData = encodingOut.GetBytes(data);

                request.ContentType = contentType;
                request.ContentLength = postData.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // 确认html编码
                    string ct = response.ContentType;
                    int pos = ct.IndexOf(htmlEncoding);
                    string htmlEncoder = string.Empty;
                    if (pos > 0)
                    {
                        htmlEncoder = ct.Substring(pos + htmlEncoding.Length).Trim();
                    }

                    string encoding = string.IsNullOrEmpty(htmlEncoder) ? response.ContentEncoding : htmlEncoder;
                    if (string.IsNullOrEmpty(encoding))
                    {
                        // 默认编码
                        encoding = "UTF-8";
                    }

                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                    result = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var wenReq = (HttpWebResponse)ex.Response;
                using (StreamReader sr = new StreamReader(wenReq.GetResponseStream(), Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
                request.Abort();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        //public static string HttpPost1(string url, string data, string userAgent = "", string contentType = "", string Authorization = "", string certPath = "", string certPassword = "")
        //{
        //    HttpCilentFactory
        //}

        #endregion

        #region Post Form



        public static string PostForm(string url, IEnumerable<FormItem> formItems) => PostForm(url, formItems, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

        public static string PostForm(string url, IEnumerable<FormItem> formItems, string userAgent, string referer, string Authorization, string certPath, string certPassword)
        {
            string result = string.Empty;

            if (formItems.Count() < 1)
                return string.Empty;
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.KeepAlive = true;

                if (!referer.IsNullOrEmpty())
                {
                    request.Referer = referer;
                }
                if (!userAgent.IsNullOrEmpty())
                {
                    request.UserAgent = userAgent;
                }
                if (!Authorization.IsNullOrEmpty())
                {
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);
                }
                if (!certPath.IsNullOrEmpty())
                {
                    X509Certificate2 cert = new X509Certificate2(certPath, certPassword);
                    request.ClientCertificates.Add(cert);
                }

                string boundary = "----" + DateTime.Now.Ticks.ToString("x");
                request.ContentType = $"{fileData}boundary={boundary}";

                using (var postStream = new MemoryStream())
                {
                    string fileFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" + "\r\nContent-Type: application/octet-stream" + "\r\n\r\n";

                    string dataFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"" + "\r\n\r\n{1}";

                    foreach (var item in formItems)
                    {
                        string formData = item.IsFile ? string.Format(fileFormdataTemplate, item.Key, item.FileName) : string.Format(dataFormdataTemplate, item.Key, item.Value);

                        if (postStream.Length == 0)
                        {
                            formData = formData.Substring(2);
                        }

                        byte[] formDataBytes = Encoding.UTF8.GetBytes(formData);

                        postStream.Write(formDataBytes, 0, formDataBytes.Length);

                        if (item.IsFile)
                        {
                            postStream.Write(item.FileContent, 0, item.FileContent.Length);
                        }
                    }

                    var data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                    postStream.Write(data, 0, data.Length);

                    request.ContentLength = postStream.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        postStream.Position = 0;

                        byte[] buffer = new byte[1024 * 40];
                        int bytesRead = 0;
                        while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                        }
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // 确认html编码
                    string ct = response.ContentType;
                    int pos = ct.IndexOf(htmlEncoding);
                    string htmlEncoder = string.Empty;
                    if (pos > 0)
                    {
                        htmlEncoder = ct.Substring(pos + htmlEncoding.Length).Trim();
                    }

                    string encoding = string.IsNullOrEmpty(htmlEncoder) ? response.ContentEncoding : htmlEncoder;
                    if (string.IsNullOrEmpty(encoding))
                    {
                        // 默认编码
                        encoding = "UTF-8";
                    }

                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                    result = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var wenReq = (HttpWebResponse)ex.Response;
                using (StreamReader sr = new StreamReader(wenReq.GetResponseStream(), Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
                if (request != null)
                    request.Abort();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public static string PostFormForWechat(string url, byte[] data, string body, string Authorization)
        {
            string result = string.Empty;

            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.KeepAlive = true;

                if (!Authorization.IsNullOrEmpty())
                {
                    request.Headers.Add(HttpRequestHeader.Authorization, Authorization);
                }

                request.Headers.Add(HttpRequestHeader.Accept, "*/*");
                request.Headers.Add(HttpRequestHeader.UserAgent, "PostmanRuntime/7.23.0");

                string boundary = "----" + DateTime.Now.Ticks.ToString("x");
                request.ContentType = $"{fileData}boundary={boundary}";

                using (var postStream = new MemoryStream())
                {
                    string fileFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"file\"; filename=\"1.jpg\"" + "\r\n\r\n";

                    string dataFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"meta\"" + "\r\n\r\n{0}";

                    string dataFormData = string.Format(dataFormdataTemplate, body);
                    dataFormData = dataFormData.Substring(2);
                    byte[] dataFormDataBytes = Encoding.UTF8.GetBytes(dataFormData);
                    postStream.Write(dataFormDataBytes, 0, dataFormDataBytes.Length);

                    byte[] fileFormDataBytes = Encoding.UTF8.GetBytes(fileFormdataTemplate);
                    postStream.Write(fileFormDataBytes, 0, fileFormDataBytes.Length);
                    postStream.Write(data, 0, data.Length);

                    var end = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                    postStream.Write(end, 0, end.Length);

                    request.ContentLength = postStream.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        postStream.Position = 0;

                        byte[] buffer = new byte[1024 * 40];
                        int bytesRead = 0;
                        while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                        }
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // 确认html编码
                    string ct = response.ContentType;
                    int pos = ct.IndexOf(htmlEncoding);
                    string htmlEncoder = string.Empty;
                    if (pos > 0)
                    {
                        htmlEncoder = ct.Substring(pos + htmlEncoding.Length).Trim();
                    }

                    string encoding = string.IsNullOrEmpty(htmlEncoder) ? response.ContentEncoding : htmlEncoder;
                    if (string.IsNullOrEmpty(encoding))
                    {
                        // 默认编码
                        encoding = "UTF-8";
                    }

                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                    result = reader.ReadToEnd();
                }
            }

            catch (WebException ex)
            {
                var wenReq = (HttpWebResponse)ex?.Response;
                using (StreamReader sr = new StreamReader(wenReq.GetResponseStream(), Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
                if (request != null)
                    request.Abort();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        private static string PostFormNoException(string url, byte[] data, string body, string Authorization)
        {
            string boundary = "----------------------------213797664976246336460798";

            string body1 = boundary + "\r\nContent-Disposition: form-data; name=\"meta\";\r\n" + "Content-Type: application/json\r\n\r\n" + body + "\r\n" + boundary + "\r\nContent-Disposition: form-data; name=\"file\"; filename=\"1.jpg\";\r\nContent-Type: image/jpg\r\n\r\n";

            byte[] dataFormDataBytes = Encoding.UTF8.GetBytes(body1);

            var end = Encoding.UTF8.GetBytes("\r\n----------------------------213797664976246336460798--\r\n");

            byte[] all = new byte[dataFormDataBytes.Length + end.Length + data.Length];
            Array.Copy(dataFormDataBytes, 0, all, 0, dataFormDataBytes.Length);
            Array.Copy(data, 0, all, dataFormDataBytes.Length, data.Length);
            Array.Copy(end, 0, all, dataFormDataBytes.Length + data.Length, end.Length);

            HttpContent httpContent = new ByteArrayContent(all);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", Authorization);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.23.0");
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
            Task<string> t = response.Content.ReadAsStringAsync();
            return t.Result;
        }

        public class FormItem
        {
            public string Key { set; get; }

            public string Value { set; get; }

            public bool IsFile
            {
                get
                {
                    if (FileContent == null || FileContent.Length == 0)
                        return false;

                    if (FileContent != null && FileContent.Length > 0 && string.IsNullOrWhiteSpace(FileName))
                        throw new Exception("the file's name is null or empty.");
                    return true;
                }
            }

            public string FileName { set; get; }

            public byte[] FileContent { set; get; }
        }

        #endregion

        private const string htmlEncoding = "charset=";
    }
}
