using System.Diagnostics;

namespace Z21lib
{
    public static class Log
    {
        public struct LogMessage
        {
            public DateTime Date { get; set; }

            public string Message { get; set; }

            public LogMessage(DateTime date, string message)
            {
                Date = date;
                Message = message;
            }

            public LogMessage(string message)
            {
                Date = DateTime.Now;
                Message = message;
            }

            public override string ToString()
            {
                return $"{Date:dd.MM.yyyy HH:mm:ss} | {Message}";
            }
        }

        static bool started = false;
        static List<LogMessage> Data = null!;
        static bool logToFile = false;
        static string fileName = null!;
        public static bool DebugLog = false;

        public static void Start()
        {
            started = true;
            Data = new List<LogMessage>();
        }

        public static void LogToFile(string filename)
        {
            logToFile = true;
            fileName = filename;
        }

        public static void Stop()
        {
            started = false;
        }

        public static List<LogMessage> Save()
        {
            return Data;
        }

        internal static void Write(string message)
        {
            if (started)
            {
                LogMessage msg = new LogMessage(message);
                Data.Add(msg);

                if (logToFile)
                {
                    try
                    {
                        using (StreamWriter sw = File.AppendText(fileName))
                        {
                            sw.WriteLine(msg);
                        }
                    }
                    catch (Exception e)
                    {
                        Data.Add(new LogMessage($"Cannot write log to file! Error: {e.Message}"));
                    }
                }
                if (DebugLog)
                {
                    Debug.Write(msg);
                }
            }
        }
    }
}
