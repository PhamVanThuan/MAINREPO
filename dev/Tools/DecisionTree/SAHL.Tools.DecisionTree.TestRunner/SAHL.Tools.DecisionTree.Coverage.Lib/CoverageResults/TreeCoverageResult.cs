using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults
{
    public class TreeCoverageResult
    {
        public string TreeName { get; set; }

        public List<NodeCoverageResult> Nodes { get; set; }

        public int TotalNumberOfLinks { get; private set; }

        public int TotalCoveredLinks { get; private set; }

        public decimal CoveragePercent { get; private set; }

        public TreeCoverageResult(string treeName, List<NodeCoverageResult> nodes)
        {
            this.TreeName = treeName;
            this.Nodes = nodes;

            this.TotalNumberOfLinks = Nodes.Sum(x => x.NumberOfLinks);
            this.TotalCoveredLinks = Nodes.Sum(x => x.CoveredLinks);
            if (TotalNumberOfLinks > 0)
            {
                this.CoveragePercent = Math.Round(((decimal)TotalCoveredLinks / (decimal)TotalNumberOfLinks * 100), 2);
            }
        }
    }
}