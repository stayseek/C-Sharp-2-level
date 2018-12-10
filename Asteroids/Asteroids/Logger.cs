using System;
using System.IO;

namespace Asteroids
{
    class Logger
    {
        public Action<string> WriteToLog;

        public string LogFileName { set; get; }

        public Logger (string logFileName)
        {
            LogFileName = logFileName;
            WriteToLog = LogToConsole;
            WriteToLog += LogToFile;
        }

        /// <summary>
        /// Запись лога в консоль.
        /// </summary>
        /// <param name="message"></param>
        private void LogToConsole(string message)
        {
            Console.WriteLine(DateTime.Now+" - "+message);
        }
        /// <summary>
        /// Запись лога в файл.
        /// </summary>
        /// <param name="message"></param>
        private void LogToFile(string message)
        {
            using (var r = new StreamWriter(LogFileName, true))
            {
                r.WriteLine(DateTime.Now + " - " + message);
            }
        }
    }
}
