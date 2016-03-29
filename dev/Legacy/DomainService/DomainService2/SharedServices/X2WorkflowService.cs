using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.SharedServices
{
    public class X2WorkflowService : IX2WorkflowService
    {
        private IX2Repository X2Repository;
        private ICastleTransactionsService castleTransactions;

        public X2WorkflowService(IX2Repository X2Repository, ICastleTransactionsService castleTransactions)
        {
            this.X2Repository = X2Repository;
            this.castleTransactions = castleTransactions;
        }

        public void SetX2DataRow(long InstanceID, IDictionary<string, object> X2Data)
        {
            IInstance instance = this.X2Repository.GetInstanceByKey(InstanceID);

            string Query = "UPDATE X2.X2DATA.{0} SET ";
            string Where = " WHERE InstanceID = {1} ";

            foreach (KeyValuePair<string, object> KP in X2Data)
            {
                Query += (KP.Key + " = " + KP.Value + ", ");
            }
            Query = Query.Substring(0, Query.Length - 2);
            Query += Where;
            Query = String.Format(Query, instance.WorkFlow.StorageTable, InstanceID);

            this.castleTransactions.ExecuteNonQueryOnCastleTran(Query, SAHL.Common.Globals.Databases.X2, null);
        }

        public IDictionary<string, object> GetX2DataRow(long instanceID)
        {
            IInstance instance = this.X2Repository.GetInstanceByKey(instanceID);

            string query = string.Format("SELECT * FROM X2DATA.{0} (nolock) WHERE InstanceID = {1}", instance.WorkFlow.StorageTable, instanceID);
            DataTable DT = new DataTable();

            DataSet ds = this.castleTransactions.ExecuteQueryOnCastleTran(query, SAHL.Common.Globals.Databases.X2, null);
            if (ds.Tables.Count > 0)
            {
                DT = ds.Tables[0];
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    dict.Add(DT.Columns[i].ColumnName, DT.Rows[0].ItemArray[i]);
                }
            }

            return dict;
        }

        public void ArchiveValuationsFromSourceInstanceID(Int64 instanceID, string adUser, int applicationKey)
        {
            //// Use the InstanceID to locate cases whose Source InstanceID is InstanceID.ID
            //// Check the case is in the Valuations workflow (It should always be just just in case)
            //// Look for Kids of the case you find. (Only one possible in Valuations)
            //// DONT ARCHIVE THE CHILD at this stage (phase 1 august spet 2008) we are going to let the
            //// Valuation complete from adcheck
            //// Archive the parent.
            //// Dont archive the Valuations Hold case this will be done externally
            //Instance_DAO[] iids = Instance_DAO.FindAllByProperty("SourceInstanceID", InstanceID);
            //foreach (Instance_DAO iid in iids)
            //{
            //    if (iid.WorkFlow.Name.ToUpper() == "VALUATIONS")
            //    {
            //        Instance_DAO[] Brats = Instance_DAO.FindAllByProperty("ParentInstance.ID", iid.ID);
            //        WorkFlow_DAO w = WorkflowRepository.GetWorkFlowByName(Val, Origination);
            //        foreach (Instance_DAO Screaming in Brats)
            //        {
            //            //LogPlugin.LogInfo("Archiving Valuations Child Case:{0} Parent:{1}", Screaming.ID, iid.ID);
            //            //WorkflowRepository.Instance().CreateAndSaveActiveExternalActivity("EXTCleanupArchive", Screaming.ID, w.ID, null);
            //        }
            //        // Archive the valuations case
            //        //LogPlugin.LogInfo("Archiving Valuations Case:{0}", iid.ID);
            //        WorkflowRepository.Instance().CreateAndSaveActiveExternalActivity("EXTCleanupArchive", iid.ID, w.ID, null);
            //    }
            //    else
            //    {
            //        LogPlugin.LogFormattedError("Valuation Hold Instance:{0} is somehow the parent case for IID:{1} in WF:{2}"
            //        , InstanceID, iid.ID, iid.WorkFlow.Name);
            //    }
            //}
            //// Dont archive the Valuations Hold case this will be done externally
        }

        public void ArchiveQuickCashFromSourceInstanceID(Int64 instanceID, string adUser, int applicationKey)
        {
            //// Look at this later when we get to QC
            ////TODO: Refactor- This needs to be ripped out and potentially added to the commonhelper.
            //Instance_DAO[] iids = Instance_DAO.FindAllByProperty("SourceInstanceID", InstanceID);
            //foreach (Instance_DAO iid in iids)
            //{
            //    if (iid.WorkFlow.Name.ToUpper() == "QUICK CASH")
            //    {
            //        // archive them look at brats etc
            //        //LogPlugin.LogInfo("QC Case:{0} CLone:{1}", InstanceID, iid.ID);
            //    }
            //    else
            //    {
            //        LogPlugin.LogFormattedError("Quick Cash Hold Instance:{0} is somehow the parent case for IID:{1} in WF:{2}"
            //        , InstanceID, iid.ID, iid.WorkFlow.Name);
            //    }
            //}
        }

        public bool HasInstancePerformedActivity(long instanceID, string activityName)
        {
            return (this.X2Repository.GetWorkflowHistoryForInstanceAndActivity(instanceID, activityName).Count > 0);
        }
    }
}