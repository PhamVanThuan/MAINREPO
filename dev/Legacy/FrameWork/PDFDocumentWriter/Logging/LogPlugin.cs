using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace PDFDocumentWriter.Logging
{
    #region TextLogger
    public abstract class TextLoggingBase
    {
        private int _NumDaysToStore = 14;
        private int FileSize = ((1024) * 1);
        private string _LogFolder = @".\";
        private string _LogFileName = "";
        private string _AppName = "";
        private string _Suffix = "_Log";
        protected string LogFileName
        {
            get
            {
                _LogFileName = string.Format("{0}{1}{2}.txt", _AppName, DateTime.Now.ToString("yyyy-MM-dd"), _Suffix);
                return (_LogFolder + _LogFileName);
            }
        }

        public TextLoggingBase(string AppName, string LogFolder, int RollOverSizeInKB, int NumDaysToStore)
        {
            if (!LogFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                LogFolder += Path.DirectorySeparatorChar;
            }
            _LogFolder = LogFolder;
            FileSize = ((1024) * RollOverSizeInKB);
            _AppName = AppName + "_";
            _NumDaysToStore = NumDaysToStore;
        }

        protected void WriteTextLog(string Message, params object[] Args)
        {
            string str = string.Format(Message, Args);
            WriteLogEntry(str);
        }

        protected void WriteLogEntry(string Entry)
        {
            try
            {
                CheckLogFileSize();
                using (StreamWriter sw = File.AppendText(LogFileName))
                {
                    //sw.WriteLine("----------------------------------------------------------------");
                    sw.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss"), Entry));
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }


        #region Helper Methods
        private void CreateLogFile()
        {
            TextWriter tw = File.CreateText(LogFileName);
            tw.Close();
            DoNumFileRollOvers();
        }
        private void CreateFirstBackup()
        {
            string FirstBackup = string.Format("{2}\\{0}_{1}_OLD1.txt", _AppName, DateTime.Now.ToString("yyyy-MM-dd"), Path.GetDirectoryName(LogFileName));
            File.Copy(LogFileName, FirstBackup, true);
            File.Delete(LogFileName);
            CreateLogFile();
        }

        private void DoNumFileRollOvers()
        {
            ArrayList arlFilesToNuke = new ArrayList();
            DateTime dt = DateTime.Now.AddDays((-1 * _NumDaysToStore));
            string searchpattern = string.Format("{0}*.txt", _AppName);
            string[] LogFilesToDate = Directory.GetFiles(_LogFolder, searchpattern);
            foreach (string s in LogFilesToDate)
            {
                // Get the date section out of the file name.
                string str = Path.GetFileNameWithoutExtension(s);
                str = str.Replace(_AppName, "");
                str = str.Replace(_Suffix, "");

                DateTime dtTemp = DateTime.Parse(str);
                if (dt > dtTemp)
                {
                    // We want to nuke this file
                    arlFilesToNuke.Add(s);
                }
            }
            foreach (string s in arlFilesToNuke)
            {
                File.Delete(_LogFolder + s);
            }
        }

        private void CheckLogFileSize()
        {
            // Check that a file for today exits
            if (!File.Exists(LogFileName))
            {
                CreateLogFile();
                return;
            }
            else
            {
                // We have a file for today, Check its size
                FileInfo fi = new FileInfo(LogFileName);
                if (fi.Length > (long)FileSize)
                {
                    // Backup

                    // 1: Check to see if we have a backup for today.
                    string searchpattern = string.Format("{0}_{1}_OLD*.txt", _AppName, DateTime.Now.ToString("yyyy-MM-dd"));
                    // Get the current Number of files. IE betstone_today_OLD1, betstone_today_OLD2 ... 
                    int NumBackupLogs = Directory.GetFiles(_LogFolder, searchpattern).Length;
                    if (NumBackupLogs > 0)
                    {
                        // We need to roll the backups
                        for (int i = (NumBackupLogs + 1); i > 1; i--)
                        {
                            string CurrentBackupFileName = string.Format("{3}\\{0}_{1}_OLD{2}.txt", _AppName, DateTime.Now.ToString("yyyy-MM-dd"), (i - 1), Path.GetDirectoryName(LogFileName));
                            string NewBackupFileName = string.Format("{3}\\{0}_{1}_OLD{2}.txt", _AppName, DateTime.Now.ToString("yyyy-MM-dd"), (i), Path.GetDirectoryName(LogFileName));
                            if (File.Exists(NewBackupFileName))
                            {
                                // This should never happen!
                                File.Delete(NewBackupFileName);
                            }
                            File.Copy(CurrentBackupFileName, NewBackupFileName, true);
                            File.Delete(CurrentBackupFileName);
                        }
                        // Now Write the new backup
                        CreateFirstBackup();
                        return;
                    }
                    else
                    {
                        // We need to create the first backup
                        CreateFirstBackup();
                    }
                    CreateLogFile();
                    return;
                }
                else
                {
                    // File is okay to write to
                    return;
                }
            }
        }
        #endregion

    }

    public class TextLogger : TextLoggingBase
    {
        protected EventLogEntryType _ThreshHold;
        protected bool _Locking = false;
        public TextLogger(EventLogEntryType ThreshHold, bool Locking, string AppName, string LogFolder, int RollOverSizeInKB, int NumDaysToStore)
            : base(AppName, LogFolder, RollOverSizeInKB, NumDaysToStore)
        {
            _ThreshHold = ThreshHold;
            _Locking = Locking;
        }

        public void WriteToTextLog(string Message, params object[] Args)
        {
            base.WriteTextLog(Message, Args);
        }

    }


    #endregion
    [Serializable] // This cause needed in Filters Domain(via the Game struct)
    public class LogSettingsClass
    {
        public int ConsoleLevel = 3;
        public bool ConsoleLevelLock = false;

        public int FileLevel = 3;
        public EventLogEntryType Threshhold
        {
            get
            {
                switch (FileLevel)
                {
                    case 0:
                    case 1:
                        {
                            return EventLogEntryType.Error;
                        }
                    case 2:
                        {
                            return EventLogEntryType.Warning;
                        }
                    case 3:
                        {
                            return EventLogEntryType.Information;
                        }
                    default:
                        {
                            return EventLogEntryType.Error;
                        }
                }
            }
        }
        public bool FileLevelLock = false;
        public int RollOverSizeInKB = 1000;
        public int NumDaysToStore = 14;

        public string FilePath = @"c:\";
        public string AppName = "PDF Document Writer";

        public LogSettingsClass()
        {
        }
    }

    public interface ILogCapture
    {
        void AddLogMessage(string msg);
    }

    #region LoggingBase
    public abstract class LoggingBase
    {
        protected static object syncObj = new object();
        protected ILogCapture capture = null;
        protected TextLogger tl;

        private LogSettingsClass LogSettings = null;
        public string _LogFileName = "";

        public LoggingBase(string LogFileName)
        {
            LogSettings = new LogSettingsClass();
            _LogFileName = LogFileName;
        }

        protected bool Init()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        protected void SeedLogSetting(LogSettingsClass logsettings)
        {
            LogSettings = logsettings;
            tl = new TextLogger(logsettings.Threshhold, logsettings.FileLevelLock, logsettings.AppName,
              logsettings.FilePath, logsettings.RollOverSizeInKB, logsettings.NumDaysToStore);
        }

        /// <summary>
        /// Funtion to check the log request passed in. If XMan is configured to log its logginglevel or
        /// higher then the information will be logged else not.
        /// </summary>
        /// <param name="level">The level of the message to be logged.(Based on eventlogentry)</param>
        /// <param name="LoggingLevel"></param>
        /// <param name="Locking"></param>
        /// <returns>true if this should be logged, else false.</returns>
        protected bool CheckLoggingLevel(EventLogEntryType level, int LoggingLevel, bool Locking)
        {
            int RequiredLevel = 0;

            switch (level)
            {
                case EventLogEntryType.Error:
                    {
                        RequiredLevel = 1;
                        break;
                    }
                case EventLogEntryType.Warning:
                    {
                        RequiredLevel = 2;
                        break;
                    }
                case EventLogEntryType.Information:
                    {
                        RequiredLevel = 3;
                        break;
                    }
            }

            if (!Locking)
            {
                if (LoggingLevel >= RequiredLevel)
                {
                    return (true);
                }
            }
            else
            {
                if (LoggingLevel == RequiredLevel)
                {
                    return (true);
                }
            }

            return (false);
        }

        protected void BaseConsoleLog(string Message, EventLogEntryType level)
        {
            if (CheckLoggingLevel(level, LogSettings.ConsoleLevel, LogSettings.ConsoleLevelLock))
                Console.WriteLine(Message);
        }

        protected void BaseTextLog(string Message, EventLogEntryType level)
        {
            if (CheckLoggingLevel(level, LogSettings.FileLevel, LogSettings.FileLevelLock))
            {
                tl.WriteToTextLog(Message);
            }
        }

        protected string GetCallStackInfo(StackTrace str)
        {
            StackFrame stf = str.GetFrame(0);
            if (null == stf.GetFileName())
            {
                return (string.Format("{0} ", stf.GetMethod()));
            }
            else
            {
                return (string.Format("{0}, {1}, {2} ", stf.GetFileName(), stf.GetMethod(), stf.GetFileLineNumber()));
            }
        }


        public void SetExternalLogCapture(ILogCapture cap)
        {
            this.capture = cap;
        }

        protected void DoLog(string Msg, EventLogEntryType level)
        {
            lock (syncObj)
            {
                if (null != capture)
                {
                    capture.AddLogMessage(Msg);
                }
                BaseConsoleLog(Msg, level);
                BaseTextLog(Msg, level);
            }
        }
    }

    #endregion

    public class LogPlugin : LoggingBase, IDisposable
    {
        protected bool Disposed = false;
        //protected static LogPlugin LogPluginRef = null;
        protected static Dictionary<string, LogPlugin> Loggers = new Dictionary<string, LogPlugin>();

        public static void SeedLogSettings(LogSettingsClass lsl)
        {
            SeedLogSettings(lsl, "Default");
        }
        public static void SeedLogSettings(LogSettingsClass lsl, string Name)
        {
            LogPlugin l = null;
            if (!Loggers.ContainsKey(Name))
            {
                l = LogPlugin.Instance(Name);
            }
            else
            {
                l = Loggers[Name];
            }
            l.SeedLogSetting(lsl);
        }

        #region IDisposable

        public static LogPlugin Instance(string LogSource)
        {
            LogPlugin LogPluginRef = null;
            if (Loggers.ContainsKey(LogSource))
            {
                LogPluginRef = Loggers[LogSource];
            }
            if (null == LogPluginRef)
            {
                lock (syncObj)
                {
                    if (null == LogPluginRef)
                    {
                        LogPlugin tempref = new LogPlugin();
                        if (null != tempref && tempref.Init())
                        {
                            LogPluginRef = tempref;
                            Loggers.Add(LogSource, LogPluginRef);
                        }
                    }
                }
            }
            return LogPluginRef;
        }

        public static LogPlugin Instance()
        {
            string LogSource = "Default";
            LogPlugin LogPluginRef = null;
            if (Loggers.ContainsKey(LogSource))
            {
                LogPluginRef = Loggers[LogSource];
            }
            if (null == LogPluginRef)
            {
                lock (syncObj)
                {
                    if (null == LogPluginRef)
                    {
                        LogPlugin tempref = new LogPlugin();
                        if (null != tempref && tempref.Init())
                        {
                            LogPluginRef = tempref;
                            Loggers.Add("Default", LogPluginRef);
                        }
                    }
                }
            }
            return LogPluginRef;
        }

        public static void Destroy()
        {
            //if (null != LogPluginRef)
            //{
            //    LogPluginRef.Dispose();
            //    LogPluginRef = null;
            //}
            string[] Keys = new string[Loggers.Keys.Count];
            Loggers.Keys.CopyTo(Keys, 0);
            for (int i = 0; i < Keys.Length; i++)
            {
                LogPlugin l = Loggers[Keys[i]];
                l.Dispose();
                l = null;
                Loggers.Remove(Keys[i]);
            }
            Loggers.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool Disposing)
        {
            if (!this.Disposed)
            {
                if (Disposing)
                {
                }
            }
            Disposed = true;
        }

        ~LogPlugin()
        {
            Dispose(false);
        }


        private LogPlugin()
            : base("PDFDocumentWriter")
        {
        }
        #endregion

        public static void SetCapture(ILogCapture cap)
        {
            LogPlugin.Instance().SetExternalLogCapture(cap);
        }

        public static void LogInfo(string Msg)
        {
            LogPlugin.Instance().DoLog(Msg, EventLogEntryType.Information);
        }
        public static void LogInfo(string Msg, params object[] args)
        {
            LogPlugin.Instance().DoLog(string.Format(Msg, args), EventLogEntryType.Information);
        }

        public static void LogWarning(string Msg)
        {
            LogPlugin.Instance().DoLog(Msg, EventLogEntryType.Warning);
        }

        public static void LogWarning(string Msg, params object[] args)
        {
            LogPlugin.Instance().DoLog(string.Format(Msg, args), EventLogEntryType.Warning);
        }

        public static void LogError(string Msg)
        {
            LogPlugin.Instance().DoLog(Msg, EventLogEntryType.Error);
        }

        public static void LogError(string Msg, params object[] args)
        {
            LogPlugin.Instance().DoLog(string.Format(Msg, args), EventLogEntryType.Error);
        }

        public static void LogInfoForSource(string Msg, string LogSource)
        {
            LogPlugin.Instance(LogSource).DoLog(Msg, EventLogEntryType.Information);
        }

        public static void LogWarningForSource(string Msg, string LogSource)
        {
            LogPlugin.Instance(LogSource).DoLog(Msg, EventLogEntryType.Warning);
        }

        public static void LogErrorForSource(string Msg, string LogSource)
        {
            LogPlugin.Instance(LogSource).DoLog(Msg, EventLogEntryType.Error);
        }
    }
}
