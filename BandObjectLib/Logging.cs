using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;

namespace BandObjectLib
{
    public class Logging
    {
        //这在子线程运行没写完，也没办法

        public static void test()
        {
            Logging logging = Logging.Instance;
            AddDEBUG("test.log", "this test");
        }

        public enum LogLevel : uint
        {
            DEBUG = 0, INFO, NOTICE, WARNING, ERROR, CRITICAL,
        };
        public static ConcurrentDictionary<string, StreamWriter> log_file_dict = new ConcurrentDictionary<string, StreamWriter>();
        public static BlockingCollection<object[]> message_list = new BlockingCollection<object[]>();

        private static readonly Logging Singleton_instance = new Logging();
        public static Logging Instance { get { return Singleton_instance; } }

        private Logging() { LogCommon(); }

        public void LogCommon() //日志写入文件的方法在类的静态构造函数中实现，这样，在队列被调用的时候，会自动调用此方法
        {
            //开启线程池来写日志
            ThreadPool.QueueUserWorkItem(a =>
            {
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appdataQT = Path.Combine(appdata, "QTTabBar");
                if (!Directory.Exists(appdataQT))
                {
                    Directory.CreateDirectory(appdataQT);
                }
                StreamWriter logstart = new StreamWriter(Path.Combine(appdataQT, "logstart.log"));
                logstart.WriteLine(string.Format("{0} [{1}]: {2}", DateTime.Now.ToString(), "INFO", "log start"));
                logstart.Close();logstart = null;

                while (true)
                {


                    object[] msg = message_list.Take();
                    LogLevel lv = (LogLevel)msg[0];
                    string log_filename = (string)msg[1];
                    string message = (string)msg[2];
                    string daetimeNow = (string)msg[3];

                    log_filename = Path.Combine(appdataQT, log_filename);
                    StreamWriter streamWriter = null;
                    if (!log_file_dict.TryGetValue(log_filename, out streamWriter))
                    {
                        lock ("Itcast-DotNet-AspNet-Glable-LogLock-Addlogfile")
                        {
                            if (!log_file_dict.TryGetValue(log_filename, out streamWriter))
                            {
                                streamWriter = new StreamWriter(log_filename, true);
                                log_file_dict.TryAdd(log_filename, streamWriter);
                            }
                        }
                    }
                    lock (streamWriter)
                    {
                        //switch (lv)
                        //{
                        //    case LogLevel.DEBUG:
                        //        break;
                        //    case LogLevel.INFO:
                        //    case LogLevel.NOTICE:
                        //    case LogLevel.WARNING:
                        //    case LogLevel.ERROR:
                        //    case LogLevel.CRITICAL:
                        //    default:
                        //        break;
                        //}
                        //streamWriter.WriteLine(string.Format($"[{lv.ToString()}]: {message}"));
                        streamWriter.WriteLine(string.Format("{0} [{1}]: {2}", daetimeNow, lv.ToString(), message));
                        streamWriter.Flush();
                    }
                }

            });
        }

        public static void AddMessage(string logfile, string message, uint level = (uint)LogLevel.INFO)
        {
            message_list.Add(new object[] { level, logfile, message, DateTime.Now.ToString() });
        }
        public static void Add_DEBUG(string logfile, string message)
        {
            #if DEBUG
            AddMessage(logfile, message, (int)LogLevel.DEBUG);
            #endif
        }
        public static void AddDEBUG(string logfile, string message)
        {
            AddMessage(logfile, message, (int)LogLevel.DEBUG);
        }
        public static void AddINFO(string logfile, string message)
        {
            AddMessage(logfile, message, (int)LogLevel.INFO);
        }
        public static void AddNOTICE(string logfile, string message)
        {
            AddMessage(logfile, message, (int)LogLevel.NOTICE);
        }
        public static void AddWARNING(string logfile, string message)
        {
            AddMessage(logfile, message, (int)LogLevel.WARNING);
        }
        public static void AddERROR(string logfile, string message)
        {
            AddMessage(logfile, message, (int)LogLevel.ERROR);
        }
        public static void AddCRITICAL(string logfile, string message)
        {
            AddMessage(logfile, message, (int)LogLevel.CRITICAL);
        }
        //"QTTabBarException.log"
    }
}
