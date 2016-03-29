using IronRuby;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using SAHL.DecisionTree.Shared.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace SAHL.DecisionTree.Shared.Core
{
    public class Node
    {
        public bool ExecutionResultedInError { get; protected set; }

        public bool ExecutionResult { get; set; }

        public int id;

        public string Name { get;set; }

        public NodeType nodeType { get; protected set; }

        public string rubyCode { get; protected set; }

        public ScriptScope scope { get; set; }

        public IProcessingEngine engine { get; set; }

        public Node(int id, string name, NodeType nodeType, string rubyCode)
        {
            this.id = id;
            this.Name = name;
            this.nodeType = nodeType;
            this.rubyCode = rubyCode.Replace("_sgl_quote_", "'").Replace("_quote_", "\"").Replace("_newline_", "\n").Replace("_carriage_", "\r").Replace("_tab_", "\t");
        }

        public event EventHandler<NodeExceptionEventsArgs> NodeExceptionRaised;

        protected virtual void OnNodeExceptionRaised(NodeExceptionEventsArgs e)
        {
            dynamic messages = engine.GetVariable("Messages");
            messages.AddError(e.NodeException.Message);
            EventHandler<NodeExceptionEventsArgs> handler = NodeExceptionRaised;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ExecutionExceptionRaised(object sender, NodeExceptionEventsArgs e)
        {
            e.NodeId = this.id;
            OnNodeExceptionRaised(e);
        }

        public bool Process()
        {
            bool result = false;

            var decoratedRubyCode = DecorateRubyCode(rubyCode);

            engine.CodeExecutionExceptionRaised += ExecutionExceptionRaised;
            bool executionResult = engine.Process(decoratedRubyCode);
            result = engine.GetVariable("Variables").outputs.NodeResult;
            ExecutionResult = result;

            return result;
        }

        private string DecorateRubyCode(string rubyCode)
        {
            rubyCode = EvaluationTransformForInterpolatedMessages(rubyCode);
            if (!nodeType.Equals(NodeType.Decision))
            {
                rubyCode += " \n Variables::outputs.NodeResult = true";
            }
            return rubyCode;
        }

        private string EvaluationTransformForInterpolatedMessages(string rubyCode)
        {
            var interpolatedMessagePattern = @"(?<csharpProperty>Messages::[a-z]+.*\.[A-Z]\w+)";
            var evaluatedMessageReplacement = @"eval(""#{${csharpProperty}}"")";
            string result = Regex.Replace(rubyCode, interpolatedMessagePattern, evaluatedMessageReplacement);
            return result;
        }
    }

    public enum NodeType
    {
        Process,
        Decision,
        SubTree,
        ClearMessages,
        Start,
        End,
    }
}