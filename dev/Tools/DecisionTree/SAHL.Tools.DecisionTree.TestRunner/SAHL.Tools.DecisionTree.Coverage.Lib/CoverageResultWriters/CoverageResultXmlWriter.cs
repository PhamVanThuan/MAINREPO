using SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults;
using System.IO;
using System.Xml.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResultWriters
{
    public class CoverageResultXmlWriter : ICoverageResultWriter
    {
        public void WriteCoverageReport(CoverageResultSummary resultSummary, string outputPath)
        {
            string fullPath = Path.GetFullPath(outputPath);

            XDocument xdoc = new XDocument();

            XElement treeResults = new XElement("TreeResults");
            foreach (var tree in resultSummary.TreeResults)
            {
                treeResults.Add(CreateTreeResultElement(tree));
            }

            XElement summary = new XElement("Summary",
                new XElement("Coverage", resultSummary.PercentCovered),
                new XElement("TotalTrees", resultSummary.TreeResults.Count),
                new XElement("TotalLinks", resultSummary.TotalLinks),
                new XElement("TotalCoveredLinks", resultSummary.CoveredLinks),
                treeResults
            );
            xdoc.Add(summary);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string outputFilename = Path.Combine(fullPath, "Summary.xml");
            xdoc.Save(outputFilename);
        }

        private XElement CreateTreeResultElement(TreeCoverageResult tree)
        {
            XElement nodeResults = new XElement("NodeResults");

            foreach (var node in tree.Nodes)
            {
                nodeResults.Add(CreateNodeResultElement(node));
            }

            XElement treeResult = new XElement("TreeResult",
                new XElement("Name", tree.TreeName),
                new XElement("TotalNodes", tree.Nodes.Count),
                new XElement("TotalLinks", tree.TotalNumberOfLinks),
                new XElement("CoveredLinks", tree.TotalCoveredLinks),
                new XElement("Coverage", tree.CoveragePercent),
                nodeResults);

            return treeResult;
        }

        private XElement CreateNodeResultElement(NodeCoverageResult node)
        {
            XElement linkResults = new XElement("LinkResults");
            foreach (var link in node.LinkCoverage)
            {
                linkResults.Add(CreateLinkResultElement(link));
            }

            XElement nodeResult = new XElement("NodeResult",
                new XElement("Name", node.NodeName),
                new XElement("Type", node.NodeType),
                new XElement("TotalLinks", node.NumberOfLinks),
                new XElement("CoveredLinks", node.CoveredLinks),
                new XElement("Coverage", node.CoveragePercent),
                linkResults);
            return nodeResult;
        }

        private XElement CreateLinkResultElement(LinkCoverageResult link)
        {
            XElement linkResult = new XElement("LinkResult",
                            new XElement("LinkType", link.LinkType),
                            new XElement("ToNodeName", link.ToNodeName),
                            new XElement("Covered", link.Covered));
            return linkResult;
        }
    }
}