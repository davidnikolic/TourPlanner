using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Logging
{
    public static class LoggerFactory
    {
        public static ILoggerWrapper GetLogger<T>()
        {
            return Log4NetWrapper.CreateLogger(typeof(T), "log4net.config");
        }
    }
}
