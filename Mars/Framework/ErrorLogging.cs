using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mars
{
    public static class ErrorLogging
    {
        static ErrorLogging()
        {
        }

        public static void Log(string description, bool isSerious, Developer developer)
        {
            StringBuilder error = new StringBuilder();
            StackTrace stackTrace = new StackTrace(true);
            StackFrame[] stackFrames = stackTrace.GetFrames();

            string time = DateTime.Now.ToString();
            string sourceFile = Path.GetFileName(stackFrames[1].GetFileName());
            string functionName = stackFrames[1].GetMethod().Name;
            string devName = developer.ToString();

            error.AppendLine("Occurred: " + time);
            error.AppendLine("File: " + sourceFile);
            error.AppendLine("Function: " + functionName);
            error.AppendLine("Developer: " + devName);
            error.AppendLine("Description: " + description);
            error.AppendLine("Stack Trace:");

            error.AppendLine("{");
            foreach (StackFrame frame in stackFrames)
            {
                string frameSource = Path.GetFileName(frame.GetFileName());

                if (string.IsNullOrEmpty(frameSource) == false)
                {
                    error.AppendLine("\t" + frame.GetMethod().Name + " - " + frameSource);
                }
            }
            error.AppendLine("}");

            if (Directory.Exists(Settings.AppDataPath + "\\Errors") == false)
            {
                Directory.CreateDirectory(Settings.AppDataPath + "\\Errors");
            }

            string fStart = "Error";

            if (isSerious)
            {
                fStart = "ERROR";
            }

            string fileDateTime = DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString();
            string file = Path.Combine(Settings.AppDataPath + "\\Errors", fStart + "_" + fileDateTime + ".txt");

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            File.WriteAllText(file, error.ToString());
        }
    }

    public enum Developer
    {
        Mitch,
        Darren,
        All
    }
}
