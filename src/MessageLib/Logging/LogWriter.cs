using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MessageLib.Logging
{
    /// <summary>
    /// LogWriter
    /// </summary>
    internal class LogWriter
    {
        /// <summary>
        /// Single instance
        /// </summary>
        internal static LogWriter Instance = new LogWriter();

        private BlockingCollection<string[]> collection = new BlockingCollection<string[]>();

        /// <summary>
        /// Constructor
        /// </summary>
        private LogWriter()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new LogListener());
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var log = collection.Take();
                    Trace.WriteLine(log[0], log[1]);
                }
            }, TaskCreationOptions.LongRunning);
        }

        internal void WriteLog(string log, string logLevel)
        {
            collection.Add(new string[] { log, logLevel });
        }

    }
}
