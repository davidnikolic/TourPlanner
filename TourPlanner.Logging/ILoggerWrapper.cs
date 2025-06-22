using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Logging
{
    public interface ILoggerWrapper
    {
        void Info(string message);
        void Error(string message, Exception ex = null);
        void Debug(string message);
    }
}
