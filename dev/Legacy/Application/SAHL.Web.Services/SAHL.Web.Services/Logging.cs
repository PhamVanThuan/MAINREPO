using System;
using SAHL.X2.Common;
using System.Configuration;

namespace SAHL.Web.Services
{
    public class Logging
    {

       // public static void InitLogging()
       // {
       //     LogSection section = ConfigurationManager.GetSection("LogSection") as LogSection;
       //     LogSettingsClass lsl = new LogSettingsClass();
       //     lsl.AppName = string.Format("SAHL.WEB.SERVICES");
       //     if (section != null)
       //     {
       //         lsl.ConsoleLevel = Convert.ToInt32(section.Logging["Console"].level);
       //         lsl.ConsoleLevelLock = Convert.ToBoolean(section.Logging["Console"].Lock);
       //         lsl.FileLevel = Convert.ToInt32(section.Logging["File"].level);
       //         lsl.FileLevelLock = Convert.ToBoolean(section.Logging["File"].Lock);
       //         lsl.FilePath = section.Logging["File"].path;
       //     }
       //     lsl.NumDaysToStore = 14;
       //     lsl.RollOverSizeInKB = 4096;
       //     LogPlugin.SeedLogSettings(lsl);
       //}

    }
}
