using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text;
using Util;

namespace BasicController
{
    public sealed class ResultInfo
    {
        public CodeEnum Code { get; set; }

        public object Data { get; set; } = null;

        /// <summary>
        /// 优先级高于传递消息
        /// </summary>
        public string extMessage = string.Empty;

        public string Message
        {
            get => extMessage.IsNullOrEmpty() ? Code.GetEnumDesc() : extMessage;
            set
            {
                if (extMessage.IsNullOrEmpty())
                    extMessage = value;
            }
        }

        public ResultInfo Fail(string message = "")
            => FillModel(CodeEnum.Fail, message, null);

        public ResultInfo Success(string message = "", object data = null)
            => FillModel(CodeEnum.Success, message, data);

        public ResultInfo Created(string message = "", object data = null)
            => FillModel(CodeEnum.Success, message, data);

        public ResultInfo Judge(bool flag, string message = "", object data = null)
            => flag ? Success(message, data) : Fail(message);

        public ResultInfo Judge(string message = "", object data = null)
            => data == null ? Fail(message) : Success(message, data);

        internal ResultInfo Unlogin(string message = "")
            => FillModel(CodeEnum.Unlogin, message, null);

        internal ResultInfo FillModel(CodeEnum code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
            return this;
        }

        public static implicit operator JsonResult(ResultInfo data)
            => new JsonResult(data);

        public enum CodeEnum
        {
            [Description("操作成功")]
            Success = 0,
            [Description("操作失败")]
            Fail = -1,
            [Description("请重新登陆")]
            Unlogin = -2
        }
    }

    public sealed class PageData
    {
        public static PageData Create(object data, long totalCount)
            => new PageData { data = data, totalCount = totalCount };

        /// <summary>
        /// 返回前端的分页数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 总数据量
        /// </summary>
        public long totalCount { get; set; }
    }
}
