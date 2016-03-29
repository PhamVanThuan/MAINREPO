using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Common.Service.Interfaces
{
    public class X2ServiceResponse
    {
        private bool _IsError = false;

        public bool IsError { get { return _IsError; } }

        //private bool _hasStageTransitionKeys = false;

        //public bool HasStageTransitionKeys
        //{
        //    get
        //    {
        //        return _hasStageTransitionKeys;
        //    }
        //}

        //private List<int> _StageTransitionKeys = null;

        //public List<int> StageTransitionKeys
        //{
        //    get
        //    {
        //        return _StageTransitionKeys;
        //    }
        //}

        public string NavigateValue;

        public X2ServiceResponse(XmlDocument xd, bool IsError)
        {
            this._IsError = IsError;
            //XmlNode xn = xd.SelectSingleNode("//X2Response/StageTransitions");
            //if (null != xn)
            //{
            //    _hasStageTransitionKeys = true;
            //    XmlNodeList xnl = xn.SelectNodes("//StageTransition");
            //    _StageTransitionKeys = new List<int>();
            //    foreach (XmlNode xnStageTran in xnl)
            //    {
            //        _StageTransitionKeys.Add(Convert.ToInt32(xnStageTran.Attributes["Key"].Value));
            //    }
            //}
        }
    }

    public class X2ServiceResponseWithInstance : X2ServiceResponse
    {
        public long InstanceId { get; set; }

        public X2ServiceResponseWithInstance(XmlDocument xd, bool IsError, long instanceId):base(xd,IsError)
        {
            this.InstanceId = instanceId;
        }
    }

    public interface IX2Service
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="principal"></param>
        void LogIn(SAHLPrincipal principal);

        /// <summary>
        ///
        /// </summary>
        /// <param name="principal"></param>
        void LogOut(SAHLPrincipal principal);

        X2ServiceResponse CreateWorkFlowInstanceWithComplete(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings);

        X2ServiceResponse CreateWorkFlowInstanceWithComplete(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data);

        void CreateWorkFlowInstance(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings);

        void CreateWorkFlowInstance(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data);

        void StartActivity(SAHLPrincipal principal, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings);

        void StartActivity(SAHLPrincipal principal, Int64 InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data);

        X2ServiceResponse CompleteActivity(SAHLPrincipal principal, Dictionary<string, string> InputFields, bool IgnoreWarnings);

        X2ServiceResponse CompleteActivity(SAHLPrincipal principal, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data);
        X2ServiceResponse CreateCompleteActivity(SAHLPrincipal principal, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data);

        void CancelActivity(SAHLPrincipal principal);

        /// <summary>
        /// Method to swallow the x2 exception
        /// </summary>
        /// <param name="principal"></param>
        void TryCancelActivity(SAHLPrincipal principal);

        /// <summary>
        /// Gets information about any current X2 Activities for the principal if they are any in progress.
        /// </summary>
        /// <param name="principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <returns></returns>
        IX2Info GetX2Info(SAHLPrincipal principal);

        /// <summary>
        /// Gets all activities for a WorkFlow i.e. all context menu options (e.g. create instance)
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="WorkFlowID"></param>
        /// <returns></returns>
        IEventList<IActivity> GetWorkFlowActivitiesForPrincipal(SAHLPrincipal principal, long WorkFlowID);

        /// <summary>
        /// Gets all the activities available to a user for a particular instance - used to list the context menu items avaialble
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        IEventList<IActivity> GetUserActivitiesForInstance(SAHLPrincipal principal, long InstanceID);

        /// <summary>
        /// Gets all Forms associated with the State of an Instance
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        IEventList<IForm> GetFormsForInstance(long InstanceID);

        /// <summary>
        /// Retreives the row from the appropriate X2Data table matching the given InstanceID.
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <returns>An X2Data row as an IDictionary</returns>
        IDictionary<string, object> GetX2DataRow(long InstanceID);

        /// <summary>
        /// Retreives rows from the appropriate X2Data table matching the given field to the value.
        /// </summary>
        /// <param name="x2StorageTable"></param>
        /// <param name="x2StorageTableField"></param>
        /// <param name="keyValue"></param>
        /// <returns>A DataTable with 0 or more row(s) that satisfy the query criteria</returns>
        DataTable GetX2DataRowByFieldAndKey(string x2StorageTable, string x2StorageTableField, int keyValue);

        /// <summary>
        /// Sets a row of data to the appropriate X2Data table matching the given InstanceID.
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <param name="X2Data">An X2Data row as an IDictionary</param>
        void SetX2DataRow(long InstanceID, IDictionary<string, object> X2Data);

        /// <summary>
        /// Used by Life workflow to determine if the view is in "workflow" mode or not (which is only the case if you are on the default form)
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="ViewName"></param>
        /// <returns></returns>
        bool IsViewDefaultFormForState(SAHLPrincipal principal, string ViewName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_DT"></param>
        /// <param name="p_StatementName"></param>
        /// <param name="StateID"></param>
        /// <param name="principal"></param>
        void GetCustomInstanceDetails(DataTable p_DT, string p_StatementName, int StateID, SAHLPrincipal principal);

        /// <summary>
        ///
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        IWorkList GetWorkListItemByInstanceID(SAHLPrincipal principal, long InstanceID);

        void WorkFlowWizardNext(SAHLPrincipal principal, string CurrentViewName, ISimpleNavigator Navigator);

        void WorkFlowWizardNext(SAHLPrincipal principal, string CurrentViewName, ISimpleNavigator Navigator, Dictionary<string, string> FieldInputs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="CurrentViewName"></param>
        /// <param name="Navigator"></param>
        /// <param name="FieldInputs"></param>
        /// <param name="NavigateTo"></param>
        void WorkFlowWizardNext(SAHLPrincipal principal, string CurrentViewName, ISimpleNavigator Navigator, Dictionary<string, string> FieldInputs, string NavigateTo);

        /// <summary>
        /// Navigates to the view specified for the current State of the instance. Must be called after a CompleteActivity
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="Navigator"></param>
        void WorkflowNavigate(SAHLPrincipal principal, ISimpleNavigator Navigator);

        /// <summary>
        ///
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        string GetURLForCurrentState(long InstanceID);

        /// <summary>
        /// After a case has been reassigned ask the engine to recalc the security and peoples worklists
        /// </summary>
        /// <param name="Instance"></param>
        /// <returns></returns>
        bool RefreshWorkListAndSecurity(Int64 Instance);

        /// <summary>
        /// Tells X2 to clear a specific lookup from memory.
        /// </summary>
        /// <remarks>This does NOT usually need to be called explicitly, as it will be called automatically by the <see cref="ILookupRepository"/> when an item is cleared.</remarks>
        void ClearLookup(string lookUp);

        /// <summary>
        /// Tells X2 to clear all cached lookups from memory.
        /// </summary>
        /// <remarks>This does NOT usually need to be called explicitly, as it will be called automatically by the <see cref="ILookupRepository"/> when the cache is cleared.</remarks>
        void ClearLookups();

        /// <summary>
        /// Sends the "CLEARDSMETACACHE" command to X2
        /// </summary>
        /// <returns></returns>
        bool ClearMetaCache();

        /// <summary>
        /// Sends the "RELOADUISTATEMENT" command to X2
        /// </summary>
        /// <returns></returns>
        bool ClearUIStatements();

        /// <summary>
        /// Sends the "CLEARDSCACHE" command to X2
        /// </summary>
        /// <returns></returns>
        bool ClearDSCache();
        void ClearRuleCache();
    }
}