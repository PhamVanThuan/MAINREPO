using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults
{
    public class NodeCoverageResult
    {
        public string NodeName { get; private set; }

        public string NodeType { get; private set; }

        public int NumberOfLinks { get; private set; }

        public int CoveredLinks { get; private set; }

        public decimal CoveragePercent { get; private set; }

        public List<LinkCoverageResult> LinkCoverage { get; private set; }

        public NodeCoverageResult(string nodeName, string nodeType, List<LinkCoverageResult> links)
        {
            this.NodeName = nodeName;
            this.NodeType = nodeType;
            this.LinkCoverage = links;

            this.NumberOfLinks = this.LinkCoverage.Count;
            this.CoveredLinks = this.LinkCoverage.Count(x => x.Covered);
            if (NumberOfLinks > 0)
            {
                this.CoveragePercent = Math.Round(((decimal)CoveredLinks / (decimal)NumberOfLinks * 100),2);
            }
        }
    }
}