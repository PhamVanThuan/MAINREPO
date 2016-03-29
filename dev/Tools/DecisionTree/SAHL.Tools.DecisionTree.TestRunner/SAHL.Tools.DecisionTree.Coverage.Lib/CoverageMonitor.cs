using SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResults;
using SAHL.Tools.DecisionTree.TestRunner.Lib;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Lib
{
    public class CoverageMonitor : ICoverageMonitor
    {
        public Dictionary<string, CoverageTree> CoverageTrees { get; set; }
        private CoverageTree currentCoverageTree;

        public void BindToTreeExecutionManager(ITreeExecutionManager treeExecutionManager)
        {
            treeExecutionManager.DebugLocationChanged += TreeExecutionManager_DebugLocationChanged;
            treeExecutionManager.DecisionTreeExecutionStarted += TreeExecutionManager_DecisionTreeExecutionStarted;

            this.CoverageTrees = new Dictionary<string, CoverageTree>();
        }

        private void TreeExecutionManager_DecisionTreeExecutionStarted(object sender, DecisionTreeExecutionStartedArgs e)
        {
            if (!CoverageTrees.ContainsKey(e.TreeName))
            {
                var coverageTree = new CoverageTree(e.TreeName, e.Nodes, e.Links);
                CoverageTrees.Add(e.TreeName, coverageTree);
                currentCoverageTree = coverageTree;
            }
            else
            {
                currentCoverageTree = CoverageTrees[e.TreeName];
            }
        }

        private void TreeExecutionManager_DebugLocationChanged(object sender, DebugLocationChangedArgs e)
        {
            currentCoverageTree.UpdateCoverage(e.PreviousNodeId, e.JustExecutedNodeId, e.PreviousNodeResult.Value);
        }

        public void WriteCoverageResult(string outputPath, ICoverageResultWriter coverageWriter)
        {
            var treeResults = getTreeResults();

            var coverageSummary = new CoverageResultSummary(treeResults);

            coverageWriter.WriteCoverageReport(coverageSummary, outputPath);
        }

        private List<TreeCoverageResult> getTreeResults()
        {
            var treeResults = new List<TreeCoverageResult>();

            foreach (var tree in CoverageTrees)
            {
                var nodeResults = getNodeCoverageResults(tree.Value.Nodes.Values.ToList(), tree.Value);
                var treeResult = new TreeCoverageResult(tree.Key, nodeResults);
                treeResults.Add(treeResult);
            }
            return treeResults;
        }

        private List<NodeCoverageResult> getNodeCoverageResults(List<Node> nodes, CoverageTree tree)
        {
            var nodeResults = new List<NodeCoverageResult>();

            foreach (var node in nodes)
            {
                if (node.nodeType != "End")
                {
                    var nodeLinks = tree.Links.Where(x => x.FromNodeID == node.ID);
                    var linkResults = new List<LinkCoverageResult>();
                    foreach (var nodeLink in nodeLinks)
                    {
                        var toNode = tree.Nodes.Values.FirstOrDefault(x => x.ID == nodeLink.ToNodeID);
                        string toNodeName = String.Empty;
                        if (toNode != null) { toNodeName = toNode.Name; }
                        linkResults.Add(new LinkCoverageResult(nodeLink.LinkType, toNodeName, tree.LinkIDs.Contains(nodeLink.ID)));
                    }

                    var nodeResult = new NodeCoverageResult(node.Name, node.nodeType, linkResults);
                    nodeResults.Add(nodeResult);
                }
            }
            return nodeResults;
        }
    }
}
