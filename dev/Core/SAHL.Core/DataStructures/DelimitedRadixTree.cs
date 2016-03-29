using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SAHL.Core.DataStructures
{
    public class DelimitedRadixTree<TValue>
    {
        private readonly IEqualityComparer<string> keyEqualityComparer;
        public string[] KeyDelimiter { get; private set; }
        public Node<string, TValue> Root { get; set; }

        public DelimitedRadixTree(string keyDelimiter, TValue rootValue = default(TValue), IEqualityComparer<string> keyEqualityComparer = null)
            : this(keyDelimiter, new Node<string, TValue>(string.Empty, rootValue, GetKeyComparer(keyEqualityComparer)), keyEqualityComparer)
        {
        }

        public DelimitedRadixTree(string keyDelimiter, Node<string, TValue> rootNode, IEqualityComparer<string> keyEqualityComparer = null)
        {
            this.keyEqualityComparer = GetKeyComparer(keyEqualityComparer);

            KeyDelimiter = new[] { keyDelimiter };
            Root = rootNode;
        }

        private static IEqualityComparer<string> GetKeyComparer(IEqualityComparer<string> keyEqualityComparer)
        {
            return keyEqualityComparer ?? StringComparer.OrdinalIgnoreCase;
        }

        public void Add(string key, TValue value)
        {
            var nodesToSearch = key.Split(this.KeyDelimiter, StringSplitOptions.None);
            Add(nodesToSearch, value);
        }

        public void Add(string[] nodeChain, TValue value)
        {
            string currentKey = null;
            var currentNode = this.Root;
            for (var i = 0; i < nodeChain.Length; i++)
            {
                Node<string, TValue> node;
                var hasNode = currentNode.Children.TryGetValue(nodeChain[i], out node);
                if (hasNode)
                {
                    //set current node, and continue traversing
                    currentNode = node;
                    currentKey = currentNode.NodeKey;
                }
                else
                {
                    //new node, add and exit
                    AddNewNode(currentNode.Children, nodeChain, i, value);
                    return;
                }
            }

            if (this.keyEqualityComparer.Equals(currentKey, nodeChain[nodeChain.Length - 1]))
            {
                //node we wish to place a value in is the last node we found
                if (!Equals(currentNode.NodeValue, default(TValue)))
                {
                    //doesn't have a default value, which means we are attempting to overwrite and should prevent
                    throw new ArgumentException(string.Format("An item with the same key '{0}' has already been added.", currentKey));
                }
            }

            currentNode.NodeValue = value;
        }

        private void AddNewNode(IDictionary<string, Node<string, TValue>> nodeToAddInto, string[] nodesToSearch, int indexFrom, TValue value)
        {
            var currentNode = nodeToAddInto;
            Node<string, TValue> nodeToAdd = null;
            //iterate through the remaining tokens, starting from the last common one found
            for (; indexFrom < nodesToSearch.Length; indexFrom++)
            {
                nodeToAdd = new Node<string, TValue>(nodesToSearch[indexFrom], default(TValue), keyEqualityComparer); //assign a default value to non-leaf nodes
                currentNode.Add(nodesToSearch[indexFrom], nodeToAdd);

                currentNode = nodeToAdd.Children;
            }
            if (nodeToAdd != null)
            {
                //only set the value on the last node
                nodeToAdd.NodeValue = value;
            }
        }

        public string[] GetNodeNames(string key)
        {
            return key.Split(this.KeyDelimiter, StringSplitOptions.None);
        }

        public TValue GetValue(string key)
        {
            var tokens = GetNodeNames(key);
            return GetValue(tokens);
        }

        public TValue GetValue(string[] nodeChain)
        {
            if (!this.Root.Children.Any())
            {
                return Root.NodeValue;
            }

            Node<string, TValue> currentNode = this.Root; //start searching at root
            foreach (var item in nodeChain)
            {
                Node<string, TValue> value;
                if (currentNode.Children.TryGetValue(item, out value))
                {
                    //we have found this token, store
                    currentNode = value;
                }
                else
                {
                    //we didn't find the current token, return the value for the last token we found
                    return currentNode.NodeValue;
                }
            }

            //this node is an exact match, return its value
            return currentNode.NodeValue;
        }

        public class Node<TKey, TValue>
        {
            public TKey NodeKey { get; set; }
            public TValue NodeValue { get; set; }
            public IDictionary<TKey, Node<TKey, TValue>> Children { get; private set; }

            public Node(TKey key, TValue nodeValue, IEqualityComparer<TKey> childrenKeyComparer = null)
            {
                this.NodeKey = key;
                this.NodeValue = nodeValue;
                this.Children = new Dictionary<TKey, Node<TKey, TValue>>(childrenKeyComparer ?? EqualityComparer<TKey>.Default);
            }
        }
    }
}
