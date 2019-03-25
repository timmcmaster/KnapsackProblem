using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackProblem
{
    public static class LogFile
    {
        private static string _fileName = "KnapsackLog.txt";

        public static void WriteLine(string logMessage)
        {
            using (StreamWriter sw = new StreamWriter(_fileName,true))
            {
                sw.WriteLine(logMessage);
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            using (StreamWriter sw = new StreamWriter(_fileName, true))
            {
                sw.WriteLine(format, args);
            }
        }
    }
}
