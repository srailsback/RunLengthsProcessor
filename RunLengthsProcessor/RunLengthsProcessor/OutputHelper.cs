using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunLengthsProcessor
{

    public interface IOutputHelper
    {
        void Write(string msg, bool error = false);
        void WriteLog(string msg, bool error = false);
    }

    public class OutputHelper : IOutputHelper
    {
        private INLogger _logger;

        public OutputHelper(INLogger logger)
        {
            this._logger = logger;
        }

        public void Write(string msg, bool error = false)
        {
            Console.WriteLine(msg);
            if (!error)
            {
                _logger.Info(msg);
            }
            else
            {
                _logger.Error(msg);
            }
        }

        public void WriteLog(string msg, bool error = false)
        {
            if (!error)
            {
                _logger.Info(msg);
            }
            else
            {
                _logger.Error(msg);
            }
        }
    }
}
