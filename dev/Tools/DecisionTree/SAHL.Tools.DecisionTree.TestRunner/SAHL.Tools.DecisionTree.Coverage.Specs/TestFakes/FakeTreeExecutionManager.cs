using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DecisionTree.Coverage.Specs.TestFakes
{
    public class FakeTreeExecutionManager : ITreeExecutionManager
    {
        private int pathID;


        public dynamic Enumerations { get;private set; }

        public ISystemMessageCollection SystemMessages { get;private set; }

        public dynamic TreeVariablesObject { get;private set; }


        public event DebugLocationChanged DebugLocationChanged;

        public event DecisionTreeExecutionEnded DecisionTreeExecutionEnded;

        public event DecisionTreeExecutionStarted DecisionTreeExecutionStarted;


        public void Process(string treeName, int treeVersion, List<ITestInput> testCaseInputs, List<ITestInput> scenarioInputs)
        {
            var tree = GetTreeByName(treeName);
            var links = tree.TreeLinks;
            OnDecisionTreeStarted(treeName, tree.TreeNodes, links);


            foreach(var linkID in tree.Paths[pathID])
            {
                var link = tree.TreeLinks.First(x => x.ID == linkID);
                var prevNode = tree.TreeNodes.First(x=>x.Key == link.FromNodeID);
                var currentNode = tree.TreeNodes.First(x => x.Key == link.ToNodeID);

                OnDebugLocationChanged(currentNode.Key, prevNode.Key, link.LinkType == "DecisionYes", true);
            }
            OnDecisionTreeEnded(treeName);
        }


        internal void SetRunPath(int pathID)
        {
            this.pathID = pathID;
        }

        public void OnDecisionTreeStarted(string treeName, Dictionary<int, Node> nodes, List<Link> links)
        {
            if (DecisionTreeExecutionStarted != null)
            {
                DecisionTreeExecutionStarted(this, new DecisionTreeExecutionStartedArgs(treeName, nodes, links));
            }
        }

        public void OnDebugLocationChanged(int? justExecuted, int? previousNode, bool? previousNodeResult, bool nodeResult)
        {
            if (DebugLocationChanged!=null)
            {
                DebugLocationChanged(this, new DebugLocationChangedArgs(justExecuted, previousNode, nodeResult, previousNodeResult));
            }
        }

        public void OnDecisionTreeEnded(string treeName)
        {
            if (DecisionTreeExecutionEnded!=null)
            {
                DecisionTreeExecutionEnded(this, new DecisionTreeExecutionEndedArgs(treeName));
            }
        }

        public void SetupTreeExecutionManager(string assemblyPath, ISystemMessageCollection systemMessages)
        {
            throw new NotImplementedException();
        }


        private ITestFakeTree GetTreeByName(string treeName)
        {
            ITestFakeTree tree = new MapleTree();
            if (treeName == "Pine")
            {
                tree = new PineTree();
            }
            else if (treeName == "Birch")
            {
                tree = new BirchTree();
            }
            return tree;
        }
    }
}