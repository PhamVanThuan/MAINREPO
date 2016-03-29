using System;
using Castle.ActiveRecord;
using PostSharp.Aspects;
using SAHL.Common.Logging;
using SAHL.Common.Logging.Attributes;

namespace SAHL.Common.BusinessModel.Aspects
{
    [Serializable]
    [SAHLExceptionAspectAttribute(AttributeExclude = true)]
    [LoggerVerboseTraceAspect(AttributeExclude = true)]
    [ReadUncommittedTransactionAspect(AttributeExclude = true)]
    public class ReadUncommittedTransactionAspect : OnMethodBoundaryAspect
    {
        public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
        {
            base.CompileTimeInitialize(method, aspectInfo);
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = new TransactionScope(TransactionMode.Inherits, System.Data.IsolationLevel.ReadUncommitted, OnDispose.Commit);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            base.OnSuccess(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            TransactionScope transactionScope = args.MethodExecutionTag as TransactionScope;
            if (transactionScope != null)
            {
                transactionScope.Dispose();
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            base.OnException(args);
        }
    }
}