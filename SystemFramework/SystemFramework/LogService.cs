
using System;
using System.IO;
using System.Xml;
using log4net;

namespace SystemFramework
{
    public static class LogService
    {
        private static ILog dlog, elog, flog, ilog, wlog, datalog;
        static LogService()
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "LogConfig.xml";
            if (File.Exists(configPath))
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            dlog = log4net.LogManager.GetLogger("DebugLogger");
            elog = log4net.LogManager.GetLogger("ErrorLogger");
            flog = log4net.LogManager.GetLogger("FatalLogger");
            ilog = log4net.LogManager.GetLogger("InfoLogger");
            wlog = log4net.LogManager.GetLogger("WarnLogger");
            datalog = log4net.LogManager.GetLogger("DataAccessLogger");
        }

        public static void DebugMessage(string log)
        {
            if (dlog.IsDebugEnabled)
                dlog.Debug(log);
        }

        public static void ErrorMessage(string log)
        {
            if (elog.IsErrorEnabled)
                elog.Error(log);
        }

        public static void FatalMessage(string log)
        {
            if (flog.IsFatalEnabled)
                flog.Fatal(log);
        }

        public static void InfoMessage(string log)
        {
            if (ilog.IsInfoEnabled)
                ilog.Info(log);
        }

        public static void WarnMessage(string log)
        {
            if (wlog.IsWarnEnabled)
                wlog.Warn(log);
        }

        public static void DataAccessMessage(string log)
        {
            if (datalog.IsInfoEnabled)
                datalog.Info(log);
        }

    }
}
