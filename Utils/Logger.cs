using System;
using System.IO;

namespace VoiceChat.Utils
{
    public class Logger
    {
        private static Logger _instance;
        private static readonly object _lock = new object();

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new Logger();
                    }
                }
                return _instance;
            }
        }

        private StreamWriter logFile;

        private Logger() { }

        public void Init(string filename = "client.log")
        {
            logFile = new StreamWriter(filename, append: true);
            logFile.AutoFlush = true;
        }

        public void Log(string level, string message)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"[{time}] [{level}] {message}";

            Console.WriteLine(line);
            logFile?.WriteLine(line);
        }

        public void Close()
        {
            logFile?.Close();
        }
    }
}