using System;
using System.Collections.Generic;
using System.IO;

namespace ITMO.Scripts
{
    public class Logger
    {
        private readonly string fileName;
        private readonly List<string> list;

        public Logger(bool mouth)
        {
            list = new List<string>();
            const string filePath = ".\\logs\\";
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            if (mouth) fileName = filePath + DateTime.Now.ToString("dd_MM_yy_HH_mm_ss") + "_mouth.log";
            else fileName = filePath + DateTime.Now.ToString("dd_MM_yy_HH_mm_ss") + ".log";
        }

        public void AddInfo(string infoToLog)
        {
            var currentTime = DateTime.Now.ToString("HH:mm:ss.fff");
            list.Add("[" + currentTime + "]: " + infoToLog);
        }

        public void WriteInfo()
        {
            File.AppendAllLines(fileName, list);
            list.Clear();
        }

        public void Close() => WriteInfo();
    }
}