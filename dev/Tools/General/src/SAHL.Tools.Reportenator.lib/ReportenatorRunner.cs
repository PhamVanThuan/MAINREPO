using System;
using System.IO;
using System.Linq;

namespace SAHL.Tools.Reportenator.lib
{
    public class ReportenatorRunner
    {
        public ReportenatorRunner(string serverName, string userName, string password, string domain)
        {
            this.serverName = serverName;
            this.serverUserName = userName;
            this.serverPassword = password;
            this.serverDomain = domain;
        }

        private string serverName { get; set; }
        private string serverUserName { get; set; }
        private string serverPassword { get; set; }
        private string serverDomain { get; set; }

        public void ExecuteReportenatorFile(string directory, string file)
        {
            ReportenatorFileParser parser = new ReportenatorFileParser();

            UriBuilder builder = new UriBuilder("http://ServerName/SQLReportServer/ReportService2010.asmx");
            builder.Host = serverName;

            var reports = parser.GetReports(directory, file);
            if (reports.Count() == 0)
            {
                Console.WriteLine("Reportenator file is empty. No reports to deploy.");
                return;
            }
            using (ReportExecutor reportExcutor = new ReportExecutor(builder.ToString(), this.serverUserName, this.serverPassword, this.serverDomain))
            {
                int reportNum = 0;
                var errorReports = reports.Where(x => !string.IsNullOrWhiteSpace(x.Error));
                if (errorReports.Any())
                    Console.WriteLine("The following report files are missing:");
                foreach (var report in errorReports)
                {
                    Console.WriteLine(string.Format("{0}", report.ReportName));
                }
                if (errorReports.Any())
                    throw new Exception("Missing report files");

                foreach (var report in reports.Where(x => string.IsNullOrWhiteSpace(x.Error)))
                {
                    reportNum++;
                    try
                    {
                        Console.WriteLine(string.Format("{0} of {1}...{2}", reportNum, reports.Count, report.ReportName));

                        reportExcutor.DeployReports(Path.Combine(directory, report.ReportName), report.ReportServerFolder);
                        Console.WriteLine("...SUCCESS");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("...FAILURE: Error is: {0}", ex.Message));
                        throw;
                    }
                }
            }
        }
    }
}