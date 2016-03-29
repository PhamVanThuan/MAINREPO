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
    public partial class Instance : IEntityValidation, IInstance, IDAOObject
    {
        //public X2Data Data
        //{
        //    get
        //    {
        //        X2Data data = new X2Data();

        //        data.GenericKeyTypeKey = this.WorkFlow.GenericKeyTypeKey;

        //        string sql = "select * from [x2].[X2DATA]." + this.WorkFlow.StorageTable + " (nolock) where InstanceID = " + this.ID;

        //        DBHelper dbHelper = new DBHelper(Databases.X2);
        //        DataTable dt = new DataTable();
        //        dbHelper.Fill(dt, sql);

        //        data.GenericKey =  dt != null && dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][this.WorkFlow.StorageKey]) : -1;

        //        return data;
        //    }
        //}

        protected void OnWorkLists_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnWorkLists_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnInstanceActivitySecurities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnInstanceActivitySecurities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public static IEventList<IInstance> FindByPrincipal(SAHLPrincipal principal)
        {
            if (principal == null)
                return null;

            string groups = SAHLPrincipalCache.GetPrincipalCache(principal).GetCachedRolesAsStringForQuery(true, true); //principal.GetCachedRolesAsStringForQuery(true, true);
            string HQL = String.Format("SELECT DISTINCT wl.Instance FROM WorkList_DAO wl WHERE wl.ADUserName IN ({0})", groups);

            SimpleQuery<Instance_DAO> q = new SimpleQuery<Instance_DAO>(HQL);
            Instance_DAO[] result = q.Execute();

            if (result == null)
                result = new Instance_DAO[0];

            List<Instance_DAO> list = new List<Instance_DAO>(result);
            return new DAOEventList<Instance_DAO, IInstance, Instance>(list);
        }

        public static IEventList<IInstance> FindByWorkFlow(string WorkFlowName)
        {
            string HQL = "SELECT i FROM Instance_DAO i JOIN i.WorkFlow wf WHERE wf.Name = ?";

            SimpleQuery<Instance_DAO> q = new SimpleQuery<Instance_DAO>(HQL, WorkFlowName);
            Instance_DAO[] res = q.Execute();

            return new DAOEventList<Instance_DAO, IInstance, Instance>(res);
        }
    }
}