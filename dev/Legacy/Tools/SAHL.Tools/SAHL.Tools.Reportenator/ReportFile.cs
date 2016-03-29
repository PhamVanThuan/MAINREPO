namespace SAHL.Tools.Reportenator
{
    public class ReportFile
    {
        public ReportFile(string reportName, string reportServerFolder)
        {
            this.ReportName = reportName;
            this.ReportServerFolder = reportServerFolder;
        }

        public string ReportName { get; private set; }

        public string ReportServerFolder { get; private set; }

        public string Error { get; set; }
    }
}