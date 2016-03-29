using System;

namespace SAHL.DecisionTree.Shared.Core
{
    public class NodeExceptionEventsArgs : EventArgs
    {
        private int nodeId;
        private readonly int lineNum;
        private readonly string source;
        private readonly Exception nodeException;
        private readonly ExceptionType errorType;

        public int NodeId { set { nodeId = value; } get { return nodeId; } }
        
        public int LineNum { get { return lineNum; } }

        public string Source { get { return source; } }

        public ExceptionType ErrorType { get { return errorType; } }
        
        public Exception NodeException { get { return nodeException; } }

        public NodeExceptionEventsArgs(int nodeId, int lineNum, string source, ExceptionType errorType, Exception nodeException)
        {
            this.nodeId = nodeId;
            this.lineNum = lineNum;
            this.source = source;
            this.errorType = errorType;
            this.nodeException = nodeException;
        }

        public NodeExceptionEventsArgs(int lineNum, string source, ExceptionType errorType, Exception nodeException)
        {            
            this.lineNum = lineNum;
            this.source = source;
            this.errorType = errorType;
            this.nodeException = nodeException;
        }
    }

    public delegate void NodeExceptionEventHandler(object sender, NodeExceptionEventsArgs e);
}