using System;
using System.Runtime.CompilerServices;
using NLog;

namespace NLogger.Core
{
    public class LoggerOperation
    {
        public virtual void Info(string message, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null, [CallerMemberName] string name = null) => _logger.Info(SupplementMessage(message, line, path, name));

        public virtual void Info(string message, Exception ex, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null, [CallerMemberName] string name = null) => _logger.Info(ex, SupplementMessage(message, line, path, name));

        public virtual void Warn(string message, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null, [CallerMemberName] string name = null) => _logger.Warn(SupplementMessage(message, line, path, name));

        public virtual void Warn(string message, Exception ex, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null, [CallerMemberName] string name = null) => _logger.Warn(ex, SupplementMessage(message, line, path, name));

        public virtual void Error(string message, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null, [CallerMemberName] string name = null) => _logger.Error(SupplementMessage(message, line, path, name));

        public virtual void Error(string message, Exception ex, [CallerLineNumber] int line = -1, [CallerFilePath] string path = null, [CallerMemberName] string name = null) => _logger.Error(ex, SupplementMessage(message, line, path, name));

        protected LoggerOperation(string loggerName, string configPath)
        {
            if (!string.IsNullOrEmpty(configPath))
            {
                var factory = LogManager.LoadConfiguration(configPath);
                _logger = factory.GetLogger(loggerName);
            }
            else
                _logger = LogManager.GetLogger(loggerName);
        }

        private string SupplementMessage(string message, int line, string path, string name) => $"the method \"{name}\" in \"{path}\",the line is \"{line}\",log:\n {message}";

        private readonly Logger _logger;
    }
}
