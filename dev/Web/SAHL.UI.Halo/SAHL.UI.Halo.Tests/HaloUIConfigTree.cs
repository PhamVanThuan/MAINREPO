using SAHL.Core.Testing.Config.UI;
using SAHL.UI.Halo.Tests.HaloUIConfigPredicates;
using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Core.Testing.Factories;
namespace SAHL.UI.Halo.Tests
{
    public class HaloUIConfigTree
    {
        private SAHL.Core.Testing.Config.UI.HaloUIConfig haloUIConfig;
        public HaloUIConfigTree(SAHL.Core.Testing.Config.UI.HaloUIConfig haloUIConfig)
        {
            this.haloUIConfig = haloUIConfig;
        }

        public List<HaloUIConfigItem> Traverse<T>(Action<HaloUIConfigItem> currentNode)
            where T : IHaloUIConfigPredicate
        {
            var predicate = Activator.CreateInstance<T>();
            return this.Traverse(currentNode, new IHaloUIConfigPredicate[] { predicate });
        }

        public List<HaloUIConfigItem> Traverse(Action<HaloUIConfigItem> currentNode = null, IHaloUIConfigPredicate[] predicates = null)
        {
            var nodeCount = 0;
            var _visitedNodes = new List<HaloUIConfigItem>();
            foreach (var haloUIConfigItem in haloUIConfig.HaloUIConfigurations)
            {
                do
                {
                    nodeCount = _visitedNodes.Count;
                    this.Traverse(haloUIConfigItem, _visitedNodes,currentNode,predicates);
                    if (nodeCount != 0 && nodeCount == _visitedNodes.Count)
                    {
                        break;
                    }
                }
                while (true);
            }
            return _visitedNodes;
        }

        private void Traverse(HaloUIConfigItem node, List<HaloUIConfigItem> visitedNodes, Action<HaloUIConfigItem> currentNode = null, IHaloUIConfigPredicate[] predicates = null)
        {
            if (node.Id == Guid.Empty)
            {
                node.Id = Guid.NewGuid();
            }
            node.Name = node.Name.Trim();
            node.Type = node.Type.Trim();
            if (node == null)
            {
                return;
            }
            if (visitedNodes.Select(x => String.Format("{0}{1}", x.Name, x.Type)).Contains(String.Format("{0}{1}", node.Name, node.Type)))
            {
                return;
            }
            visitedNodes.Add(node);
            if (currentNode != null)
            {
                if (predicates != null)
                {
                    var satisfiesPredicate = predicates.Select(x => x.Get()).Where(x => x.Invoke(node)).Any();
                    if (satisfiesPredicate)
                    {
                        currentNode.Invoke(node);
                    }
                }
                else
                {
                    currentNode.Invoke(node);
                }
            }
            foreach (var node1 in node.RootTiles)
            {
                this.Traverse(node1, visitedNodes, currentNode, predicates);
            }
            foreach (var node1 in node.ChildTiles)
            {
                this.Traverse(node1, visitedNodes, currentNode, predicates);
            }
            foreach (var node2 in node.Actions)
            {
                this.Traverse(node2, visitedNodes, currentNode, predicates);
            }
            foreach (var node3 in node.Wizards)
            {
                this.Traverse(node3, visitedNodes, currentNode, predicates);
            }
            foreach (var node4 in node.LinkedRootTiles)
            {
                this.Traverse(node4, visitedNodes, currentNode, predicates);
            }
            return;
        }
    }
}
