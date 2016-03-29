using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Tools.MSBuildExtensions.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GetLatestVersion latest = new GetLatestVersion();
            latest.HgFolder = @"I:\\WORK\\HGBranch\\SAHL-Core";
            latest.NugetUrl = "http://sahldeploy/sahldevnugetgallery/dataservices/v2feed.svc/";
            latest.Project = "SAHL.Core";
            latest.Execute();
        }
    }
}
