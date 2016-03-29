using System;
using PostSharp.Aspects;
using SAHL.Common.CacheData;
using SAHL.Common.Logging;
using SAHL.Common.Logging.Attributes;
using offerRole = SAHL.Common.BusinessModel.Interfaces.Repositories.OfferRole;

namespace SAHL.Common.BusinessModel.Aspects
{
    [Serializable]
    [SAHLExceptionAspectAttribute(AttributeExclude = true)]
    [LoggerVerboseTraceAspect(AttributeExclude = true)]
    [ReadUncommittedTransactionAspect(AttributeExclude = true)]
    public class DSCacheAttribute_OfferRole : OnMethodBoundaryAspect
    {
        private offerRole.WorkflowAssignment cachedDataset;

        public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
        {
            base.CompileTimeInitialize(method, aspectInfo);
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (WorkflowSecurityRepositoryCacheHelper.Instance.IsOfferRoleCached)
            {
                args.FlowBehavior = FlowBehavior.Return;
                args.ReturnValue = cachedDataset;
            }
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            if (!WorkflowSecurityRepositoryCacheHelper.Instance.IsOfferRoleCached)
            {
                this.cachedDataset = args.ReturnValue as offerRole.WorkflowAssignment;
                WorkflowSecurityRepositoryCacheHelper.Instance.IsOfferRoleCached = true;
            }
        }
    }
}