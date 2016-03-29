using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Tools.Reportenator
{
    public class ReportenatorFileParser
    {
        public List<ReportFile> GetReports(string directory, string file)
        {
            var reports = ParseReportenatorFile(Path.Combine(directory, file));

            return ValidateReportFiles(directory, reports);
        }

        public List<ReportFile> ValidateReportFiles(string directory, List<ReportFile> reports)
        {
            List<string> filesDontExist = new List<string>();
            foreach (var item in reports)
            {
                string fileToCheck = Path.Combine(directory, item.ReportName);
                if (!File.Exists(fileToCheck))
                {
                    item.Error = string.Format("Report does not exist {0}", fileToCheck);
                }
            }

            return reports;
        }

        public List<ReportFile> ParseReportenatorFile(string reportenatorFullPath)
        {
            var reportenatorContents = XDocument.Load(reportenatorFullPath);

            List<ReportFile> reportFiles = reportenatorContents.Descendants("BatchParameters").Descendants("File")
                        .Select(x => new ReportFile(x.Attribute("ReportName").Value, x.Attribute("ReportServerFolder").Value)).ToList<ReportFile>();

            return reportFiles;
        }
    }
}