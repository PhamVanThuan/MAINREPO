using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using PDFDocumentWriter.Logging;
using System.Configuration;

namespace PDFDocumentWriterWeb
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            LogSettingsClass lsl = new LogSettingsClass();

            LogPlugin.SeedLogSettings(lsl);
            LogPlugin.LogError("Logging Setup");
            LogSection section = ConfigurationManager.GetSection("LogSection") as LogSection;
            lsl.AppName = string.Format("Document Engine");
            lsl.ConsoleLevel = Convert.ToInt32(section.Logging["Console"].level);
            lsl.ConsoleLevelLock = Convert.ToBoolean(section.Logging["Console"].Lock);
            lsl.FileLevel = Convert.ToInt32(section.Logging["File"].level);
            lsl.FileLevelLock = Convert.ToBoolean(section.Logging["File"].Lock);
            lsl.FilePath = section.Logging["File"].path;
            lsl.NumDaysToStore = 14;
            lsl.RollOverSizeInKB = 4096;
            LogPlugin.SeedLogSettings(lsl);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}