using System;
using System.IO;
using System.Net;
using SAHL.Tools.Reportenator.ReportServer;

namespace SAHL.Tools.Reportenator
{
    public class ReportExecutor : IDisposable
    {
        ReportingService2010 reportingService;

        public ReportExecutor(string url, string userName, string password, string domain)
        {
            this.URL = url;
            this.UserName = userName;
            this.Password = password;
            this.Domain = domain;
        }

        public ReportingService2010 ReportingService
        {
            get
            {
                if (reportingService == null)
                {
                    reportingService = new ReportingService2010() { Url = this.URL, Credentials = new NetworkCredential(this.UserName, this.Password, this.Domain) };
                }
                return reportingService;
            }
        }

        private string URL { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private string Domain { get; set; }

        public void DeployReports(string reportName, string reportFolder)
        {
            // get the report definition
            string name = Path.GetFileNameWithoutExtension(reportName);
            string fullpath = "/" + reportFolder + "/" + name;

            Byte[] definition = null;
            using (FileStream stream = File.OpenRead(reportName))
            {
                definition = new Byte[stream.Length];
                stream.Read(definition, 0, (int)stream.Length);
            }

            //deploy the report
            Warning[] warnings = new Warning[] { };
            CatalogItem item = ReportingService.CreateCatalogItem("Report", name, @"/" + reportFolder, true, definition, null, out warnings);
            DataSource[] oldDSs = ReportingService.GetItemDataSources(fullpath);
            int dsIndex = 0;
            foreach (DataSource ds in oldDSs)
            {
                DataSourceReference reference = new DataSourceReference();
                reference.Reference = "/Data Sources/" + oldDSs[dsIndex].Name;
                oldDSs[dsIndex].Item = reference;
                dsIndex++;
            }

            //set the reports data sources
            ReportingService.SetItemDataSources(fullpath, oldDSs);
        }

        public void Dispose()
        {
            ReportingService.Dispose();
        }
    }
}