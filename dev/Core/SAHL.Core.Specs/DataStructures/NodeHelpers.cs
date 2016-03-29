using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.DataStructures;

namespace SAHL.Core.Specs.DataStructures
{
    internal class NodeHelpers
    {
        internal static DelimitedRadixTree<T>.Node<string, T> GetNodeByKey<T>(DelimitedRadixTree<T> tree, string key)
        {
            var currentChildren = tree.Root.Children;
            while (currentChildren.Any())
            {
                var child = currentChildren.Single();
                if (child.Key == key)
                {
                    return child.Value;
                }
                currentChildren = child.Value.Children;
            }
            throw new KeyNotFoundException(string.Format("Could not find the key '{0}' in the tree", key));
        }
    }
}
