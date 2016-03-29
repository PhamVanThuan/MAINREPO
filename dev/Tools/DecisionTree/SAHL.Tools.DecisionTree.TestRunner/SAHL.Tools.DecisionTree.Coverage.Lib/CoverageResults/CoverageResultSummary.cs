using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults
{
    public class CoverageResultSummary
    {
        public decimal PercentCovered { get; private set; }

        public int TotalLinks { get; private set; }

        public int CoveredLinks { get; private set; }

        public int TotalNumberOfTrees { get { return TreeResults.Count; } }

        public List<TreeCoverageResult> TreeResults { get; set; }

        public CoverageResultSummary(List<TreeCoverageResult> treeResults)
        {
            this.TreeResults = treeResults;

            this.TotalLinks = treeResults.Sum(x => x.TotalNumberOfLinks);
            this.CoveredLinks = treeResults.Sum(x => x.TotalCoveredLinks);
            if (TotalLinks > 0)
            {
                this.PercentCovered = Math.Round((decimal)CoveredLinks / (decimal)TotalLinks * 100, 2);
            }
        }
    }
}