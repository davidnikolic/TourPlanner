using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Logging.Interfaces;

namespace TourPlanner.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILoggerWrapper CreateLogger<T>()
        {
            return Log4NetWrapper.CreateLogger(typeof(T), "log4net.config");
        }
    }
}
