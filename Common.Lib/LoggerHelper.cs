using NLogger.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Lib
{
    public class LoggerHelper : LoggerOperation
    {
        private LoggerHelper(string loggerName, string configPath = "") : base(loggerName, configPath)
        {
        }

        public static readonly LoggerHelper GlobalLogger = new LoggerHelper("global");
    }
}
