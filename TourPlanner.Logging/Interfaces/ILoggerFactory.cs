using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Logging.Interfaces
{
    public interface ILoggerFactory
    {
        ILoggerWrapper CreateLogger<T>();
    }
}
