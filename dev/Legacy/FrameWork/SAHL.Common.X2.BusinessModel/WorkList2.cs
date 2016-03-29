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
    public partial class WorkList : IEntityValidation, IWorkList, IDAOObject
    {
        public static IEventList<IWorkList> FindByADUserName(SAHLPrincipal principal)
        {
            if (principal == null)
                return null;

            string groups = SAHLPrincipalCache.GetPrincipalCache(principal).GetCachedRolesAsStringForQuery(true, true); //principal.GetCachedRolesAsStringForQuery(true, true);
            string HQL = String.Format("SELECT DISTINCT wl.* FROM WorkList_DAO wl WHERE wl.ADUserName IN ({0})", groups);
            SimpleQuery<WorkList_DAO> q = new SimpleQuery<WorkList_DAO>(HQL);
            //q.SetParameter("groups", groups);
            WorkList_DAO[] result = q.Execute();

            if (result == null)
                result = new WorkList_DAO[0];

            IList<WorkList_DAO> list = new List<WorkList_DAO>(result);
            return new DAOEventList<WorkList_DAO, IWorkList, WorkList>(list);
        }

        public static IEventList<IWorkList> FindByUserAndState(SAHLPrincipal principal, long StateID)
        {
            if (principal == null)
                return null;

            string groups = SAHLPrincipalCache.GetPrincipalCache(principal).GetCachedRolesAsStringForQuery(true, true); //principal.GetCachedRolesAsStringForQuery(false, false);
            string HQL = String.Format("SELECT DISTINCT wl FROM WorkList_DAO wl WHERE wl.Instance.State.ID = ? AND wl.ADUserName IN ({0})", groups);

            SimpleQuery<WorkList_DAO> q = new SimpleQuery<WorkList_DAO>(HQL, StateID);
            //q.SetParameter("groups", groups);
            WorkList_DAO[] result = q.Execute();

            if (result == null)
                result = new WorkList_DAO[0];

            IList<WorkList_DAO> list = new List<WorkList_DAO>(result);
            return new DAOEventList<WorkList_DAO, IWorkList, WorkList>(list);
        }

        public static IWorkList FindByUserAndInstanceID(SAHLPrincipal principal, long InstanceID)
        {
            if (principal == null)
                return null;

            string groups = SAHLPrincipalCache.GetPrincipalCache(principal).GetCachedRolesAsStringForQuery(false, false);//principal.GetCachedRolesAsStringForQuery(false, false);
            string HQL = String.Format("SELECT DISTINCT wl FROM WorkList_DAO wl WHERE wl.Instance.ID = ? AND wl.ADUserName IN ({0})", groups);

            SimpleQuery<WorkList_DAO> q = new SimpleQuery<WorkList_DAO>(HQL, InstanceID);
            //q.SetParameter("groups", groups);
            WorkList_DAO[] result = q.Execute();

            if (result == null || result.Length == 0)
                return null;

            return new WorkList(result[0]);
        }
    }
}