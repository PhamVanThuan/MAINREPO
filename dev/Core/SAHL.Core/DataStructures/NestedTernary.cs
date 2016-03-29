using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.DataStructures
{
    public class NestedTernary
    {
        private readonly char[] moduleSplit = "::".ToCharArray();
        private readonly char[] propertySplit = ".".ToCharArray();

        private NestedTernaryNode root;

        public NestedTernaryNode Root
        {
            get { return this.root; }
        }

        public void Add(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentException("Cannot add null or empty string");
            }
            NestedTernaryNode current = root;
            string[] keywords = word.Split(propertySplit, StringSplitOptions.RemoveEmptyEntries);
            string[] modules = keywords[0].Split(moduleSplit, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < modules.Length; i++)
            {
                current = i == 0 ? Add(modules[i], 0, ref root, null) : Add(modules[i], 0, ref current.modules, null);
            }
            if (keywords.Length > 1)
            {
                Add(keywords[1], 0, ref current.properties, null);
            }
        }

        public IEnumerable<string> StartsWith(string wordPartial)
        {
            if (string.IsNullOrEmpty(wordPartial))
            {
                throw new ArgumentException();
            }

            var keywords = wordPartial.Split(propertySplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
            var modules = keywords[0].Split(moduleSplit, StringSplitOptions.RemoveEmptyEntries).ToArray();
            var contextNode = this.root;

            for (var i = 0; i < modules.Length; i++)
            {
                if (contextNode == null) { return Enumerable.Empty<string>(); }
                if (i > 0) { contextNode = contextNode.Modules; }

                contextNode = this.FindNode(modules[i], contextNode);
            }

            if (keywords.Length > 1 && contextNode != null && contextNode.Properties != null)
            {
                contextNode = this.FindNode(keywords[1], contextNode.Properties);
            }

            return CollectResult(contextNode, modules.Length,keywords.Length);
        }

        private IEnumerable<string> CollectResult(NestedTernaryNode contextNode, int moduleLength, int keywordLength)
        {
            IList<string> words = new List<string>();

            if (contextNode == null) { return words; }

            if (moduleLength > 1 && keywordLength == 1)
            {
                this.FindWords(contextNode, ref words);
            }
            else
            {
                if (contextNode.IsLast)
                {
                    words.Add(contextNode.ToString());
                }
                this.FindWords(contextNode.Center, ref words);
            }

            return words;
        }

        private NestedTernaryNode Add(string str, int position, ref NestedTernaryNode node, NestedTernaryNode parent)
        {
            if (node == null)
            {
                node = new NestedTernaryNode(str[position]) { parent = parent };
            }
            if (str[position] < node.Value)
            {
                return Add(str, position, ref node.left, node.parent);
            }
            if (str[position] > node.Value)
            {
                return Add(str, position, ref node.right, node.parent);
            }
            if (position + 1 == str.Length)
            {
                node.IsLast = true;
                return node;
            }
            return Add(str, position + 1, ref node.center, node);
        }

        

        private NestedTernaryNode FindNode(string str, NestedTernaryNode node)
        {
            NestedTernaryNode context = node ?? this.root;
            for (int i = 0; i < str.Length; i++)
            {
                if (context != null)
                {
                    if (i != 0)
                    {
                        context = context.Center;
                    }
                    context = this.NextNode(str[i], context);
                }
                else
                {
                    return null;
                }
            }
            return context;
        }

        private NestedTernaryNode NextNode(char str, NestedTernaryNode node)
        {
            if (node == null)
            {
                return null;
            }
            if (str < node.Value)
            {
                return this.NextNode(str, node.Left);
            }
            if (str > node.Value)
            {
                return this.NextNode(str, node.Right);
            }
            return node;
        }

        private void FindWords(NestedTernaryNode nodePartial, ref IList<string> array)
        {
            if (nodePartial == null)
            {
                return;
            }
            if (nodePartial.IsLast)
            {
                array.Add(nodePartial.ToString());
            }
            this.FindWords(nodePartial.Left, ref array);
            this.FindWords(nodePartial.Center, ref array);
            this.FindWords(nodePartial.Right, ref array);
        }
    }
}