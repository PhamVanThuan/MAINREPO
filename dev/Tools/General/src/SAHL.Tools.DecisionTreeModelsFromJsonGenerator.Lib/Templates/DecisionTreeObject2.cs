using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib.Templates
{
    public partial class DecisionTreeObject : GeneratorObject
    {
        public string Namespace { get; protected set; }

        public string JsonText { get; private set; }

        public JArray VariablesJson { get; private set; }

        public string DecisionTreeName { get; private set; }

        public string DecisionTreeVersion { get; private set; }

        public string DecisionTreeClassName
        {
            get 
            { 
                return string.Format("{0}_{1}", Utilities.StripBadChars(DecisionTreeName), DecisionTreeVersion.ToString()); 
            }
        }

        public string NodeLinksInitializer { get; private set; }

        public string TreeNodesInitializer { get; private set; }

        public DecisionTreeObject(string rawJson, string defaultNameSpace, string treeVersion)
        {
            this.Namespace = defaultNameSpace;
            this.DecisionTreeVersion = treeVersion;
            this.JsonText = rawJson;
            this.ParseJson();
        }

        private void ParseJson()
        {
            var decisiontree = JsonConvert.DeserializeObject<dynamic>(this.JsonText);
            this.DecisionTreeName = Utilities.StripSpacesAndUpperCaseFirst(Convert.ToString(decisiontree.name));
            this.TreeNodesInitializer = BuildTreeNodesInitializer(decisiontree.tree.nodes);
            this.NodeLinksInitializer = BuildNodeLinksInitializer(decisiontree.tree.links);
            this.VariablesJson = decisiontree.tree.variables;
        }

        private string BuildNodeLinksInitializer(dynamic linksFromJson)
        {
            var nodeLinksInitializerBuilder = new StringBuilder();
            foreach (var link in linksFromJson)
            {
                var linkId = Convert.ToInt32(link.id);
                var fromNodeId = Convert.ToInt32(link.fromNodeId);
                var toNodeId = Convert.ToInt32(link.toNodeId);
                string linkType = Utilities.ToPascalCase(Convert.ToString(link.type));
                if(linkType.Equals("link", StringComparison.InvariantCultureIgnoreCase))
                {
                    nodeLinksInitializerBuilder.AppendFormat(" new Link({0},{1},{2},{3}),", linkId, fromNodeId, toNodeId, "LinkType.Standard");
                }
                else
                {
                    nodeLinksInitializerBuilder.AppendFormat(" new Link({0},{1},{2},{3}),", linkId, fromNodeId, toNodeId, string.Format("LinkType.{0}", linkType));
                }
            }
            var result = nodeLinksInitializerBuilder.ToString().TrimEnd(new char[] { ',' });
            return result;
        }

        private string BuildTreeNodesInitializer(dynamic nodesFromJson)
        {
            var nodesInitializerBuilder = new StringBuilder();
            foreach (var node in nodesFromJson)
            {
                nodesInitializerBuilder.Append("{");
                string nodeType = Convert.ToString(node.type);
                int nodeId = Convert.ToInt32(node.id);
                string scriptCode = Convert.ToString(node.code);
                string nodeName = Convert.ToString(node.name);
                
                if (nodeType.Equals("subtree", StringComparison.InvariantCultureIgnoreCase))
                {
                    string subtreeName = Convert.ToString(node.subtreeName);
                    string subtreeVariableMap = node.subtreeVariables.ToString().Replace("\n", " ").Replace("\r", " ").Replace("\"", "\\\"");
                    var subtreeVersion = Convert.ToString(node.subtreeVersion);
                    if (string.IsNullOrEmpty(subtreeVersion))
                    {
                        subtreeVersion = "1";
                    }
                    nodesInitializerBuilder.AppendFormat("{3}, new SubTree(\"{0}\",\"{1}\",\"{2}\",{3})", subtreeName, subtreeVersion, subtreeVariableMap, nodeId);
                }
                else if (nodeType.Equals("clearmessages", StringComparison.InvariantCultureIgnoreCase))
                {
                    string subtreeName = Convert.ToString(node.subtreeName);
                    var subtreeVersion = Convert.ToString(node.subtreeVersion);
                    if (string.IsNullOrEmpty(subtreeVersion))
                    {
                        subtreeVersion = "1";
                    }
                    nodesInitializerBuilder.AppendFormat("{2}, new ClearMessages(\"{0}\",\"{1}\",{2},\"{3}\")", subtreeName, subtreeVersion, nodeId, nodeName);
                }
                else
                {
                    nodesInitializerBuilder.AppendFormat("{0}, new Node({0},\"{1}\",{2},@\"{3}\")", nodeId, nodeName, string.Format("NodeType.{0}", Convert.ToString(node.type)), EscapeScriptCode(scriptCode));
                }
                nodesInitializerBuilder.Append("},");
            }
            var result = nodesInitializerBuilder.ToString().TrimEnd(new char[] { ',' });
            return result;
        }

        private string EscapeScriptCode(string scriptCode)
        {
            if (!string.IsNullOrEmpty(scriptCode))
            {
                return scriptCode.Replace("\"", "\"\"");
            }
            return "";
        }
    }
}