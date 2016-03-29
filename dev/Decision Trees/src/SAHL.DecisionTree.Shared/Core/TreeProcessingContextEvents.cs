using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Core
{
    public class ExecutionExceptionRaisedArgs : ErrorEventArgs
    {
        private readonly int nodeId;
        private readonly int lineNum;
        private readonly string source;
        private readonly ExceptionType errorType;

        public int NodeId { get { return nodeId; } }

        public int LineNumber { get { return lineNum; } }

        public string Source { get { return source; } }

        public string ErrorType { get { return StringEnum.GetStringValue(errorType); } }

        public ExecutionExceptionRaisedArgs(int nodeId,int lineNum, string source, ExceptionType errorType, Exception nodeException)
            : base(nodeException)
        {
            this.nodeId = nodeId;
            this.lineNum = lineNum;
            this.source = source;
            this.errorType = errorType;
        }        
    }

    public enum ExceptionType
    {
        [StringValue("Syntax")]
        Syntax,
        [StringValue("Runtime")]
        Runtime,
        [StringValue("General")]
        General
    }
    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs =
               fi.GetCustomAttributes(typeof(StringValue),
                                       false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }

    public delegate void ExecutionExceptionRaisedHandler(object sender, ExecutionExceptionRaisedArgs e);

    public class DebugEventsArgs : EventArgs
    {
        private readonly Guid debugSessionId;
        private readonly int? justExecutedNodeId;
        private readonly int? previousNodeId;
        private readonly bool currentNodeResult;
        private readonly bool? previousNodeResult;

        public DebugEventsArgs(Guid debugSessionId, bool currentNodeResult, bool? previousNodeResult, int? previousNodeId, int? justExecutedNodeId)
        {
            this.debugSessionId = debugSessionId;
            this.currentNodeResult = currentNodeResult;
            this.previousNodeResult = previousNodeResult;
            this.justExecutedNodeId = justExecutedNodeId;
            this.previousNodeId = previousNodeId;
        }

        public Guid DebugSessionId
        {
            get { return debugSessionId; }
        }

        public bool NodeResult
        {
            get { return currentNodeResult; }
        }

        public int? JustExecutedNodeId
        {
            get { return justExecutedNodeId; }
        }

        public int? PreviousNodeId
        {
            get { return previousNodeId; }
        }

        public bool? PreviousNodeResult
        {
            get { return previousNodeResult; }
        }
    }

    public delegate void DebugEventHandler(object sender, DebugEventsArgs e);

    public delegate void SubtreeExecutionStatusEventHandler(object sender, SubtreeExecutionStatusArgs e);
}
