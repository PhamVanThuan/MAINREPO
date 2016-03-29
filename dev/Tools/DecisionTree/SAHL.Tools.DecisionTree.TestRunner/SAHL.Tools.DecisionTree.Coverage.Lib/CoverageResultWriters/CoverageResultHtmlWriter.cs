using RazorEngine;
using SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults;
using System;
using System.Collections.Generic;
using System.IO;

namespace SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResultWriters
{
    public class CoverageResultHtmlWriter : ICoverageResultWriter
    {
        private const string summaryTemplatePath = ".\\SummaryTemplate.html";
        private const string treeTemplatePath = ".\\TreeTemplate.html";
        private const string reportCssPath = ".\\report.css";

        private string summaryTemplate = "";
        private string treeTemplate = "";

        public CoverageResultHtmlWriter()
        {
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, summaryTemplatePath)))
            {
                summaryTemplate = reader.ReadToEnd();
            }
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, treeTemplatePath)))
            {
                treeTemplate = reader.ReadToEnd();
            }
        }

        public void WriteCoverageReport(CoverageResultSummary resultSummary, string outputPath)
        {
            string fullPath = Path.GetFullPath(outputPath);
            Dictionary<string, string> reportsToPrint = new Dictionary<string, string>();

            string summaryPath = Path.Combine(fullPath, "index.html");
            string summary = Razor.Parse(summaryTemplate, resultSummary);
            reportsToPrint.Add(summaryPath, summary);

            foreach (var tree in resultSummary.TreeResults)
            {
                string treeReportPath = Path.Combine(fullPath, String.Format("{0}.html", tree.TreeName));
                string treeReport = Razor.Parse(treeTemplate, tree);
                reportsToPrint.Add(treeReportPath, treeReport);
            }

            if (!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }
            foreach (var report in reportsToPrint)
            {
                using (var streamWriter = new StreamWriter(report.Key))
                {
                    streamWriter.Write(report.Value);
                    streamWriter.Flush();
                }
            }
            string cssFileName = Path.GetFileName(reportCssPath);
            string cssDestinationFile = Path.Combine(fullPath, cssFileName);
            File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, reportCssPath), cssDestinationFile, true);
        }
    }
}