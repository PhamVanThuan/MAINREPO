using System;
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    public partial class WorkFlow : IEntityValidation, IWorkFlow, IDAOObject
    {
        public static IEventList<IWorkFlow> FindByPrincipal(SAHLPrincipal principal)
        {
            if (principal == null)
                return null;

            string groups = SAHLPrincipalCache.GetPrincipalCache(principal).GetCachedRolesAsStringForQuery(true, true); // principal.GetCachedRolesAsStringForQuery(true, true);

            string HQL = String.Format("SELECT DISTINCT wl.Instance.WorkFlow FROM WorkList wl WHERE wl.ADUserName IN ({0})", groups);

            //SimpleQuery q = new SimpleQuery(typeof(WorkFlow), query);
            //List<WorkFlow> wf = new List<WorkFlow>();
            //WorkFlow[] wfArr = (WorkFlow[])WorkFlow.ExecuteQuery(q);
            //wf.AddRange(wfArr);
            //return wf;

            SimpleQuery<WorkFlow_DAO> q = new SimpleQuery<WorkFlow_DAO>(HQL);
            //q.SetParameter("groups", groups);
            WorkFlow_DAO[] result = q.Execute();

            if (result == null)
                result = new WorkFlow_DAO[0];

            IList<WorkFlow_DAO> list = new List<WorkFlow_DAO>(result);
            return new DAOEventList<WorkFlow_DAO, IWorkFlow, WorkFlow>(list);
        }

        protected void OnActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnActivities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnForms_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnForms_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnInstances_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnInstances_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSecurityGroups_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSecurityGroups_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnStates_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnStates_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlows_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlows_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnNextWorkFlowActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnNextWorkFlowActivities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlowActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkFlowActivities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is WorkFlow)
                return _WorkFlow.ID == ((WorkFlow)obj).ID;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}