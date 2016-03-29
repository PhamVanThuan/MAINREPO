using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Automation.Framework.DataAccess;
using Automation.Framework.Models;
using Common.Constants;
using Common.Models;

using FrameworkModels = Automation.Framework.Models;

namespace Automation.Framework
{
    public class X2Helper
    {
        /// <summary>
        /// Performs an activity against the case provided to it.
        /// </summary>
        /// <param name="keyValue">KeyValue</param>
        /// <param name="step">Current Step</param>
        /// <param name="workflow">Workflow that we are in</param>
        /// <returns></returns>
        internal WorkflowReturnData PerformActivity(int keyValue, Step step, WorkflowScript workflow)
        {
            string error = string.Empty;
            try
            {
                var activity = DataHelper.GetActivity(step.WorkflowActivity, workflow.WorkflowName);
                var workflowID = DataHelper.GetWorkflowID(workflow.WorkflowName);
                //write something to get list of instances
                Instance instance = DataHelper.GetInstanceForActivity(keyValue, workflowID, step.WorkflowActivity, workflow.PrimaryKey, workflow.DataTable, activity.ActivityType);
                //get x2data for that instance and workflow
                var x2Data = new Dictionary<string, string>();
                if (step.FieldInputList == null)
                    x2Data = DataHelper.GetX2DataTable(instance.ID, workflow.DataTable);
                //connect to the engine
                var x2Conn = new EngineConnector(workflow.WorkflowName, workflow.ProcessName);

                string sessionID = x2Conn.Login(ref error);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);
                x2Conn.SetADUser(step.Identity);
                //perform activity
                bool performActivity = x2Conn.PerformAction(sessionID, instance.ID, activity.Name, ref error, x2Data, step.IgnoreWarnings, step.Data);
                return new WorkflowReturnData() { ActivityCompleted = performActivity, InstanceID = instance.ID, Error = error };
            }
            catch (Exception e)
            {
                return new WorkflowReturnData { ActivityCompleted = false, InstanceID = -1, Error = string.IsNullOrEmpty(error) ? e.Message : error };
            }
        }

        /// <summary>
        /// Routes to the correct create workflow instance method based on the workflow that is passed to the method.
        /// </summary>
        /// <param name="workflow">Workflow where the case is to be created</param>
        /// <param name="keyValue">keyValue</param>
        /// <param name="identity">User for case create</param>
        internal WorkflowReturnData CreateWorkflowInstance(WorkflowScript workflow, int keyValue, string identity)
        {
            Dictionary<string, string> inputFields = GetInputFields(workflow, keyValue, identity);
            WorkflowReturnData returnData = null;
            //we need the inputFields dictionary
            var methodToCall = typeof(X2Helper).GetMethod(string.Format("Create{0}", workflow.WorkflowName.Replace(" ", string.Empty)));
            if (methodToCall != null)
            {
                returnData = (WorkflowReturnData)methodToCall.Invoke(this, new object[] { keyValue, inputFields, identity, workflow.ProcessName });
            }
            return returnData;
        }

        internal Dictionary<string, string> GetInputFields(WorkflowScript workflow, int keyValue, string identity)
        {
            Dictionary<string, string> inputFields = null;
            //find the method
            var dictionaryMethodToCall = typeof(X2Helper).GetMethod(string.Format("GetInputFields{0}", workflow.WorkflowName.Replace(" ", string.Empty)));
            if (dictionaryMethodToCall != null)
            {
                inputFields = (Dictionary<string, string>)dictionaryMethodToCall.Invoke(this, new object[] { keyValue, identity });
            }
            return inputFields;
        }

        /// <summary>
        /// Generic case creation that calls the x2Service.CreateWorkflowInstance
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="processName"></param>
        /// <param name="processVersion"></param>
        /// <param name="workflowName"></param>
        /// <param name="activityName"></param>
        /// <param name="inputFields"></param>
        /// <param name="ignoreWarnings"></param>
        /// <returns></returns>
        public WorkflowReturnData CreateCase(string identity, string processName, string processVersion, string workflowName, string activityName, Dictionary<string, string> inputFields,
            bool ignoreWarnings, object data = null)
        {
            string Error = string.Empty;
            try
            {
                var engineConn = new EngineConnector(workflowName, processName);
                string SessionID = engineConn.Login(ref Error);
                if (!string.IsNullOrEmpty(Error))
                    throw new Exception(Error);
                engineConn.SetADUser(identity);
                var InstanceID = engineConn.CreateCase(SessionID, workflowName, inputFields, ref Error, activityName, data);
                if (!string.IsNullOrEmpty(Error))
                    throw new Exception(Error);
                return new WorkflowReturnData { InstanceID = InstanceID, ActivityCompleted = true };
            }
            catch (Exception e)
            {
                var message = e.InnerException == null ? e.Message : e.InnerException.ToString();
                return new WorkflowReturnData { ActivityCompleted = false, InstanceID = -1, Error = string.IsNullOrEmpty(Error) ? message : Error };
            }
        }

        #region cap2 x2 case create

        /// <summary>
        /// Retrieves the data required for the creation of the CAP 2 workflow case.
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetInputFieldsCAP2Offers(int capOfferKey, string identity)
        {
            FrameworkModels.Account account = DataHelper.GetAccountByCapOfferKey(capOfferKey);
            FrameworkModels.CapOffer capOffer = DataHelper.GetCapOfferDetails(capOfferKey);

            //these are the input fields we need to send to create method
            var inputFields = new Dictionary<string, string>();
            inputFields.Add("CapOfferKey", capOfferKey.ToString());
            inputFields.Add("CapBroker", identity);
            inputFields.Add("LegalEntityName", account.AccountName);
            inputFields.Add("AccountKey", account.AccountKey.ToString());
            inputFields.Add("CapExpireDate", capOffer.ExpiryDate.ToString());
            inputFields.Add("CapStatusKey", "1");

            return inputFields;
        }

        /// <summary>
        /// Creates an instance in the CAP 2 workflow
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="inputFields"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public WorkflowReturnData CreateCAP2Offers(int keyValue, Dictionary<string, string> inputFields, string identity, string process)
        {
            return CreateCase(identity, Workflows.CAP2Offers, (-1).ToString(),
                    process, "Create CAP2 lead", inputFields, false);
        }

        #endregion cap2 x2 case create

        #region debt counselling case create

        /// <summary>
        /// Retrieves the data required for the creation of the CAP 2 workflow case.
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetInputFieldsDebtCounselling(int debtCounsellingKey, string identity)
        {
            var account = DataHelper.GetAccountByDebtCounsellingKey(debtCounsellingKey);
            //these are the input fields we need to send to create method
            var inputFields = new Dictionary<string, string>();
            inputFields.Add("DebtCounsellingKey", debtCounsellingKey.ToString());
            inputFields.Add("AccountKey", account.AccountKey.ToString());
            inputFields.Add("ProductKey", account.RRR_ProductKey.ToString());
            return inputFields;
        }

        /// <summary>
        /// Creates an instance in the CAP 2 workflow
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="inputFields"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public WorkflowReturnData CreateDebtCounselling(int keyValue, Dictionary<string, string> inputFields, string identity, string process)
        {
            return CreateCase(identity, Workflows.DebtCounselling, (-1).ToString(),
                    process, "Create Debt Counselling Case", inputFields, false);
        }

        #endregion debt counselling case create

        #region personal loan create

        public Dictionary<string, string> GetInputFieldsPersonalLoans(int offerKey, string identity)
        {
            var inputFields = new Dictionary<string, string>();
            inputFields.Add("ApplicationKey", offerKey.ToString());
            return inputFields;
        }

        public WorkflowReturnData CreatePersonalLoans(int keyValue, Dictionary<string, string> inputFields, string identity, string process)
        {
            return CreateCase(identity, process, (-1).ToString(), Workflows.PersonalLoans, "Create Personal Loan Lead", inputFields, false);
        }

        #endregion personal loan create

        #region disability claim create
        public Dictionary<string, string> GetInputFieldsDisabilityClaim(int disabilityClaimKey, string identity)
        {
            var inputFields = new Dictionary<string, string>();
            inputFields.Add("DisabilityClaimKey", disabilityClaimKey.ToString());
            return inputFields;
        }

        public WorkflowReturnData CreateDisabilityClaim(int keyValue, Dictionary<string, string> inputFields, string identity, string process)
        {
            return CreateCase(identity, process, (-1).ToString(), Workflows.DisabilityClaim, "Create Disability Claim", inputFields, false);
        }

        #endregion

        #region helpdesk create
        public Dictionary<string, string> GetInputFieldsHelpDesk(int legalEntityKey, string identity)
        {
            var inputFields = new Dictionary<string, string>();
            inputFields.Add("LegalEntityKey", legalEntityKey.ToString());
            inputFields.Add("CurrentConsultant", identity);
            return inputFields;
        }

        public WorkflowReturnData CreateHelpDesk(int keyValue, Dictionary<string, string> inputFields, string identity, string process)
        {
            return CreateCase(identity, process, (-1).ToString(), Workflows.HelpDesk, "Create Request", inputFields, false);
        }

        #endregion

    }
}