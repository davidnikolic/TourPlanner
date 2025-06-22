using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Logging
{
    internal class Log4NetWrapper : ILoggerWrapper
    {
        private readonly ILog _logger;

        private Log4NetWrapper(ILog logger)
        {
            _logger = logger;
        }

        public static ILoggerWrapper CreateLogger(Type callerType, string configFilePath)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(configFilePath));
            return new Log4NetWrapper(LogManager.GetLogger(callerType));
        }


        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message, Exception ex = null)
        {
            _logger.Error(message, ex);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }
    }
}
