using System;
using PostSharp.Aspects;
using SAHL.Common.CacheData;
using SAHL.Common.Logging;
using SAHL.Common.Logging.Attributes;
using workflowRole = SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole;

namespace SAHL.Common.BusinessModel.Aspects
{
    [Serializable]
    [SAHLExceptionAspectAttribute(AttributeExclude = true)]
    [LoggerVerboseTraceAspect(AttributeExclude = true)]
    [ReadUncommittedTransactionAspect(AttributeExclude = true)]
    public class DSCacheAttribute_WorkflowRole : OnMethodBoundaryAspect
    {
        private workflowRole.WorkflowAssignment cachedDataset;

        public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
        {
            base.CompileTimeInitialize(method, aspectInfo);
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (WorkflowSecurityRepositoryCacheHelper.Instance.IsWorkflowRoleCached)
            {
                args.FlowBehavior = FlowBehavior.Return;
                args.ReturnValue = cachedDataset;
            }
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            if (!WorkflowSecurityRepositoryCacheHelper.Instance.IsWorkflowRoleCached)
            {
                this.cachedDataset = args.ReturnValue as workflowRole.WorkflowAssignment;
                WorkflowSecurityRepositoryCacheHelper.Instance.IsWorkflowRoleCached = true;
            }
        }
    }
}