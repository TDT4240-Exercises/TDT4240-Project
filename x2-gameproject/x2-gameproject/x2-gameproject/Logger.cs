using System;
using System.IO;

namespace x2_gameproject
{
    /// <summary>
    /// Determines the severity and importance of each log message
    /// </summary>
    enum LogLevel
    {
        None,       //Do not log
        Error,      //Critical error (severe problems that can crash the program)
        Warning,    //Minor warnings (possible problems but do not crash anything)
        Info,       //Informative logging (Loading resources now...)
        Debug       //Verbose debug logging (ShipPosition = {12, 511})
    }

    /// <summary>
    /// An centralized utility logger class
    /// </summary>
    static class Logger
    {
        private static LogLevel logLevel = LogLevel.Info;
        private static string fileUri;

        /// <summary>
        /// Logs a new message
        /// </summary>
        /// <param name="message">The string message to log</param>
        /// <param name="level">LogLevel of this message</param>
        public static void Log(string message, LogLevel level)
        {
            if (level > logLevel) return;

            //Apply log color in console
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }

            //Create prefix
            string prefix = "(" + DateTime.Now.TimeOfDay + ") " + level.ToString().ToUpper() + ": ";

            //Console
            Console.Write(prefix);
            Console.WriteLine(message);

            //Write to file
            if (!string.IsNullOrEmpty(fileUri)) File.AppendAllText(fileUri, prefix + message + "\n");
        }

        /// <summary>
        /// Changes the current log level. Any log messages higher than the indicated log level will not be logged.
        /// </summary>
        /// <param name="level">The new LogLevel to change to</param>
        public static void SetLogLevel(LogLevel level)
        {
            logLevel = level;
        }

        /// <summary>
        /// Specifies which file the logger should stream the logging information to. Default is no file logging.
        /// </summary>
        /// <param name="setFileUri">The path and name of the file (use string.Empty for no file logging)</param>
        public static void SetFileOutput(string setFileUri)
        {
           fileUri = setFileUri;
           File.Create(setFileUri).Close();
        }

    }
}
