using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Voxelated.Network;
using Voxelated.Network.Messages;

namespace Voxelated.Utilities {
    /// <summary>
    /// Logger utility for logging custom error or log messages to 
    /// console and file.
    /// </summary>
    public static class LoggerUtils {
        /// <summary>
        /// This is a little rigged contraption to allow for
        /// saving of the log file when the app is closed.
        /// Source: https://stackoverflow.com/questions/4364665/static-destructor
        /// </summary>
        private sealed class Destructor {
            ~Destructor() {
                if (profile.SaveToFile) {
                    NetMessageListener.OnInfoMessage -= LogInfoMessage;
                    SaveLogFile();
                }
            }
        }

        #region Constants
        /// <summary>
        /// Appended directly after the time stamp of error messages.
        /// </summary>
        private const string ErrorLineHeader = "ERROR: ";

        /// <summary>
        /// Appended directly after the time stamp on warning messages.
        /// </summary>
        private const string WarningLineHeader = "WARNING: ";

        /// <summary>
        /// The file extension to use for log files.
        /// </summary>
        private const string LogFileExtension = "txt";

        /// <summary>
        /// The folder to save logs in.
        /// </summary>
        private const string LogFileDirectory = "VoxLogs";

        /// <summary>
        /// The max number of log files allowed in the folder 
        /// at one time.
        /// </summary>
        private const int MaxLogCount = 8;
        #endregion

        #region Properties
        /// <summary>
        /// The collection of log messages to store.
        /// </summary>
        public static List<string> Messages { get; private set; }

        /// <summary>
        /// Returns a time stamp string
        /// </summary>
        private static string TimeStamp {
            get {
                return DateTime.Now.ToString("h:mm:ss tt") + ": ";
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// Controls how the logger operates.
        /// </summary>
        private static LogProfile profile;

        /// <summary>
        /// Class to call save log file when the app closes
        /// </summary>
        private static readonly Destructor destructor;
        #endregion

        #region Constructor(s)
        static LoggerUtils() {
            profile = LogProfile.Release;

            Messages = new List<string>();
            destructor = new Destructor();

            NetMessageListener.OnInfoMessage += LogInfoMessage;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Set what profile the logger should follow.
        /// </summary>
        public static void SetLogProfile(LogProfile logProfile) {
            profile = logProfile;

            //Set console color if needed
            if(logProfile.Output == LogOutput.Console) {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        /// <summary>
        /// Anytime an info message is recieved, log it.
        /// </summary>
        /// <param name="sender">The sender (always null).</param>
        /// <param name="e">The message that was recieved.</param>
        private static void LogInfoMessage(object sender, NetMessageArgs e) {
            if(e != null) {
                InfoMessage infoMsg = e.Message as InfoMessage;

                if(infoMsg != null) {
                    LoggerUtils.Log(infoMsg.Information, LogLevel.Debug);
                }
            }
        }

        /// <summary>
        /// Log an error to console and to the log file.
        /// </summary>
        public static void LogError(string message) {
            Messages.Add(TimeStamp + ErrorLineHeader + message);

            switch (profile.Output) {
                case LogOutput.Unity:
                    Debug.Log(TimeStamp + ErrorLineHeader + message);
                    break;
                case LogOutput.Console:
                    Console.Write(TimeStamp);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ErrorLineHeader + message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
        }

        /// <summary>
        /// Log a warning to the console and add to the log file.
        /// </summary>
        public static void LogWarning(string message, LogLevel level = LogLevel.Debug) {
            Messages.Add(TimeStamp + WarningLineHeader + message);

            if (level <= profile.Level && level > 0) {
                switch (profile.Output) {
                    case LogOutput.Unity:
                        Debug.LogWarning(TimeStamp + WarningLineHeader + message);
                        break;
                    case LogOutput.Console:
                        Console.Write(TimeStamp);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(WarningLineHeader + message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }
            }
        }

        /// <summary>
        /// Log a message to console and to the log file.
        /// </summary>
        public static void Log(string message, LogLevel level = LogLevel.Debug) {
            string fullLog = DateTime.Now.ToString("h:mm:ss tt") + ": " + message;
            Messages.Add(fullLog);

            if(level <= profile.Level && level > 0) {
                switch (profile.Output) {
                    case LogOutput.Unity:
                        Debug.Log(fullLog);
                        break;
                    case LogOutput.Console:
                        Console.WriteLine(fullLog);
                        break;
                }
            }
        }

        /// <summary>
        /// Log a message to console only if the condition is true.
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <param name="level">What level of debug it should log at.</param>
        /// <param name="condition">The condition to check first.</param>
        public static void ConditionalLog(string message, LogLevel level, bool condition) {
            if (condition) {
                Log(message, level);
            }
        }

        /// <summary>
        /// Log a message to console and to the log file.
        /// </summary>
        public static void Log(string message, Color16 color, LogLevel level = LogLevel.Debug) {
            string fullLog = DateTime.Now.ToString("h:mm:ss tt") + ": " + message;
            Messages.Add(fullLog);

            if (level <= profile.Level && level > 0) {
                switch (profile.Output) {
                    case LogOutput.Unity:
                        Debug.Log(color.ToHex()+"ff");
                        Debug.Log(string.Format("<color={0}ff>{1}</color>", color.ToHex(), fullLog));
                        break;
                    case LogOutput.Console:
                        ConsoleColor origColor = Console.ForegroundColor;
                        ConsoleColor newColor = GetClosestConsoleColor(color);

                        Console.ForegroundColor = newColor;
                        Console.WriteLine(fullLog);
                        Console.ForegroundColor = origColor;
                        break;
                }
            }
        }



        /// <summary>
        /// Clear the console of any pre-existing text.
        /// </summary>
        public static void Clear() {
            switch (profile.Output) {
                case LogOutput.Unity:
                    Debug.ClearDeveloperConsole();
                    break;
                case LogOutput.Console:
                    Console.Clear();
                    break;
            }
        }

        /// <summary>
        /// Store all of the log calls made to file.
        /// </summary>
        public static void SaveLogFile() {
            Log("Saving log file");

            List<byte> logBytes = new List<byte>();

            //Add each message to the byte array and add a new line after each.
            foreach(string msg in Messages) {
                logBytes.AddRange(Encoding.ASCII.GetBytes(msg));
                logBytes.AddRange(Encoding.ASCII.GetBytes(Environment.NewLine));
            }

            //Don't save more than 8 files at any time.
            while(FileUtils.GetFileCount(LogFileDirectory) >= MaxLogCount) {
                FileUtils.DeleteFileAtIndex(LogFileDirectory, 0);
            }

            //Save the file.
            string fullLogFileName = "VoxLogFile" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "." + LogFileExtension;
            FileUtils.SaveFile(LogFileDirectory, fullLogFileName, logBytes.ToArray(), false);
        }

        /// <summary>
        /// Tries to find the closest color of the console.
        /// https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor
        /// </summary>
        private static ConsoleColor GetClosestConsoleColor(Color16 color) {
            ConsoleColor ret = 0;
            double rr = color.R * 8, gg = color.G * 8, bb = color.B * 8, delta = double.MaxValue;

            foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor))) {
                var n = Enum.GetName(typeof(ConsoleColor), cc);
                var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
                if (t == 0.0)
                    return cc;
                if (t < delta) {
                    delta = t;
                    ret = cc;
                }
            }
            return ret;
        }
        #endregion
    }
}
