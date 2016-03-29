using Microsoft.Scripting.Hosting;
using SAHL.DecisionTree.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RubyParserService
{
    public class Node
    {
        public int id;

        public NodeType nodeType { get; protected set; }

        public string rubyCode { get; protected set; }

        public ScriptScope scope { get; set; }

        public Node(int id, NodeType nodeType, string rubyCode)
        {
            this.id = id;
            this.nodeType = nodeType;
            this.rubyCode = rubyCode;
            // scope contains the variables and message definitions
        }

        public bool Process()
        {
            bool result = false;

            var decoratedRubyCode = DecorateRubyCode(rubyCode);
            scope.Engine.Execute(decoratedRubyCode, scope);
            result = scope.Engine.Runtime.Globals.GetVariable("Variables").outputs.NodeResult;
            ////var scriptExecutionResult = scope.GetVariable("node_result");
            //try
            //{
            //    result = (bool)scriptExecutionResult;
            //}
            //catch (Exception)
            //{
            //    result = false;
            //}
            return result;
        }

        private string DecorateRubyCode(string rubyCode)
        {
            string results = "";

            Regex outputVarRegex = new Regex(@".*Variables::outputs.(?<variableName>[\w]+)[\s]+=");

            var  matches = outputVarRegex.Matches(rubyCode);

            foreach (Match m in matches)
            {
                string variableName = m.Groups["variableName"].Value;
                string input = "Variables::inputs." + variableName;
                string output = "Variables::outputs." + variableName;
                string exp = string.Format("\n begin \n   {0} = {1} \n ", input, output);
                //string exp = string.Format("\nbegin \n  if Variables::inputs.response_to? :{0}\n    {1} = {2} \n end \n ", variableName, input , output);
                string errorCatcher = string.Format("rescue \n   Messages.AddWarning('{0} is a non-defined input variables on {1} node with ID:{2}') \n end \n",variableName, nodeType, id);
                results = results + exp + errorCatcher;
            }
            
            if (!nodeType.Equals(NodeType.Decision))
            {
                results += " \n Variables::outputs.NodeResult = true";
            }
            rubyCode += results;
            return rubyCode;
        }
    }

    public enum NodeType { 
        Process,
        Decision,
        Subtree,
        Start,
        Stop
    }
}
