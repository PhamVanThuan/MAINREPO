using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using ApplicationCapture;

//using ApplicationCapture;
using DomainService2.IOC;
using SAHL.Common;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Logging;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework;
using SAHL.X2.Framework.Common;
using SAHL.X2.Framework.DataAccess;
using SAHL.X2.Framework.DataSets;
using SAHL.X2.Framework.Interfaces;
using SAHL.X2.Framework.Logging;
using SAHL.X2.Framework.ServiceFacade;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;
using SAHL.Common.Globals;

#region WorkFlowData Application Management

public interface IX2Application_Management_Data : IX2WorkFlowDataProvider
{
    Int32 ApplicationKey { get; set; }

    String PreviousState { get; set; }

    Int32 GenericKey { get; set; }

    String CaseOwnerName { get; set; }

    Boolean IsFL { get; set; }

    String EWorkFolderID { get; set; }

    Boolean IsResub { get; set; }

    Int32 OfferTypeKey { get; set; }

    Int64 AppCapIID { get; set; }

    Boolean RequireValuation { get; set; }
}

public class X2Application_Management_Data : IX2Application_Management_Data
{
    private bool m_HasChanges = false;

    private Int32 m_ApplicationKey;

    public Int32 ApplicationKey
    {
        get
        {
            return m_ApplicationKey;
        }
        set
        {
            m_HasChanges = true;
            m_ApplicationKey = value;
        }
    }

    private String m_PreviousState;

    public String PreviousState
    {
        get
        {
            return m_PreviousState;
        }
        set
        {
            m_HasChanges = true;
            m_PreviousState = value;
        }
    }

    private Int32 m_GenericKey;

    public Int32 GenericKey
    {
        get
        {
            return m_GenericKey;
        }
        set
        {
            m_HasChanges = true;
            m_GenericKey = value;
        }
    }

    private String m_CaseOwnerName;

    public String CaseOwnerName
    {
        get
        {
            return m_CaseOwnerName;
        }
        set
        {
            m_HasChanges = true;
            m_CaseOwnerName = value;
        }
    }

    private Boolean m_IsFL;

    public Boolean IsFL
    {
        get
        {
            return m_IsFL;
        }
        set
        {
            m_HasChanges = true;
            m_IsFL = value;
        }
    }

    private String m_EWorkFolderID;

    public String EWorkFolderID
    {
        get
        {
            return m_EWorkFolderID;
        }
        set
        {
            m_HasChanges = true;
            m_EWorkFolderID = value;
        }
    }

    private Boolean m_IsResub;

    public Boolean IsResub
    {
        get
        {
            return m_IsResub;
        }
        set
        {
            m_HasChanges = true;
            m_IsResub = value;
        }
    }

    private Int32 m_OfferTypeKey;

    public Int32 OfferTypeKey
    {
        get
        {
            return m_OfferTypeKey;
        }
        set
        {
            m_HasChanges = true;
            m_OfferTypeKey = value;
        }
    }

    private Int64 m_AppCapIID;

    public Int64 AppCapIID
    {
        get
        {
            return m_AppCapIID;
        }
        set
        {
            m_HasChanges = true;
            m_AppCapIID = value;
        }
    }

    private Boolean m_RequireValuation;

    public Boolean RequireValuation
    {
        get
        {
            return m_RequireValuation;
        }
        set
        {
            m_HasChanges = true;
            m_RequireValuation = value;
        }
    }

    #region IX2WorkFlowDataProvider Members

    public void LoadData(IActiveDataTransaction Tran, Int64 InstanceID)
    {
        SqlDataAdapter SDA = null;
        DataTable WorkFlowData = new DataTable();
        try
        {
            WorkerHelper.FillFromQuery(WorkFlowData, "select * from [X2DATA].Application_Management (nolock) where InstanceID = " + InstanceID, Tran.Context, null);
            if (WorkFlowData.Rows.Count > 0)
            {
                if (WorkFlowData.Rows[0]["ApplicationKey"] != DBNull.Value)
                    m_ApplicationKey = Convert.ToInt32(WorkFlowData.Rows[0]["ApplicationKey"]);
                if (WorkFlowData.Rows[0]["PreviousState"] != DBNull.Value)
                    m_PreviousState = Convert.ToString(WorkFlowData.Rows[0]["PreviousState"]);
                if (WorkFlowData.Rows[0]["GenericKey"] != DBNull.Value)
                    m_GenericKey = Convert.ToInt32(WorkFlowData.Rows[0]["GenericKey"]);
                if (WorkFlowData.Rows[0]["CaseOwnerName"] != DBNull.Value)
                    m_CaseOwnerName = Convert.ToString(WorkFlowData.Rows[0]["CaseOwnerName"]);
                if (WorkFlowData.Rows[0]["IsFL"] != DBNull.Value)
                    m_IsFL = Convert.ToBoolean(WorkFlowData.Rows[0]["IsFL"]);
                if (WorkFlowData.Rows[0]["EWorkFolderID"] != DBNull.Value)
                    m_EWorkFolderID = Convert.ToString(WorkFlowData.Rows[0]["EWorkFolderID"]);
                if (WorkFlowData.Rows[0]["IsResub"] != DBNull.Value)
                    m_IsResub = Convert.ToBoolean(WorkFlowData.Rows[0]["IsResub"]);
                if (WorkFlowData.Rows[0]["OfferTypeKey"] != DBNull.Value)
                    m_OfferTypeKey = Convert.ToInt32(WorkFlowData.Rows[0]["OfferTypeKey"]);
                if (WorkFlowData.Rows[0]["AppCapIID"] != DBNull.Value)
                    m_AppCapIID = Convert.ToInt64(WorkFlowData.Rows[0]["AppCapIID"]);
                if (WorkFlowData.Rows[0]["RequireValuation"] != DBNull.Value)
                    m_RequireValuation = Convert.ToBoolean(WorkFlowData.Rows[0]["RequireValuation"]);
            }
        }
        catch
        {
        }
        finally
        {
            if (SDA != null)
                SDA.Dispose();
        }
    }

    public void SetDataFields(System.Collections.Generic.Dictionary<string, string> Fields)
    {
        if (Fields != null)
        {
            string[] Keys = new string[Fields.Count];
            Fields.Keys.CopyTo(Keys, 0);
            for (int i = 0; i < Fields.Count; i++)
            {
                switch (Keys[i].ToLower())
                {
                    case "applicationkey":
                        ApplicationKey = Convert.ToInt32(Fields[Keys[i]]);
                        break;
                    case "previousstate":
                        PreviousState = Convert.ToString(Fields[Keys[i]]);
                        break;
                    case "generickey":
                        GenericKey = Convert.ToInt32(Fields[Keys[i]]);
                        break;
                    case "caseownername":
                        CaseOwnerName = Convert.ToString(Fields[Keys[i]]);
                        break;
                    case "isfl":
                        IsFL = Convert.ToBoolean(Fields[Keys[i]]);
                        break;
                    case "eworkfolderid":
                        EWorkFolderID = Convert.ToString(Fields[Keys[i]]);
                        break;
                    case "isresub":
                        IsResub = Convert.ToBoolean(Fields[Keys[i]]);
                        break;
                    case "offertypekey":
                        OfferTypeKey = Convert.ToInt32(Fields[Keys[i]]);
                        break;
                    case "appcapiid":
                        AppCapIID = Convert.ToInt64(Fields[Keys[i]]);
                        break;
                    case "requirevaluation":
                        RequireValuation = Convert.ToBoolean(Fields[Keys[i]]);
                        break;
                }
            }
        }
    }

    public void SaveData(IActiveDataTransaction Tran, Int64 InstanceID)
    {
        if (m_HasChanges == true)
        {
            // Create a collection
            ParameterCollection Parameters = new ParameterCollection();
            // Add the required parameters
            WorkerHelper.AddParameter(Parameters, "@P0", SqlDbType.Int, ParameterDirection.Input, m_ApplicationKey);
            if (m_PreviousState != null)
                WorkerHelper.AddParameter(Parameters, "@P1", SqlDbType.VarChar, ParameterDirection.Input, m_PreviousState);
            else
                WorkerHelper.AddParameter(Parameters, "@P1", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value);
            WorkerHelper.AddParameter(Parameters, "@P2", SqlDbType.Int, ParameterDirection.Input, m_GenericKey);
            if (m_CaseOwnerName != null)
                WorkerHelper.AddParameter(Parameters, "@P3", SqlDbType.VarChar, ParameterDirection.Input, m_CaseOwnerName);
            else
                WorkerHelper.AddParameter(Parameters, "@P3", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value);
            WorkerHelper.AddParameter(Parameters, "@P4", SqlDbType.Bit, ParameterDirection.Input, m_IsFL);
            if (m_EWorkFolderID != null)
                WorkerHelper.AddParameter(Parameters, "@P5", SqlDbType.VarChar, ParameterDirection.Input, m_EWorkFolderID);
            else
                WorkerHelper.AddParameter(Parameters, "@P5", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value);
            WorkerHelper.AddParameter(Parameters, "@P6", SqlDbType.Bit, ParameterDirection.Input, m_IsResub);
            WorkerHelper.AddParameter(Parameters, "@P7", SqlDbType.Int, ParameterDirection.Input, m_OfferTypeKey);
            WorkerHelper.AddParameter(Parameters, "@P8", SqlDbType.BigInt, ParameterDirection.Input, m_AppCapIID);
            WorkerHelper.AddParameter(Parameters, "@P9", SqlDbType.Bit, ParameterDirection.Input, m_RequireValuation);
            WorkerHelper.ExecuteNonQuery(Tran.Context, "update [X2DATA].[Application_Management] with (rowlock) set [ApplicationKey] = @P0, [PreviousState] = @P1, [GenericKey] = @P2, [CaseOwnerName] = @P3, [IsFL] = @P4, [EWorkFolderID] = @P5, [IsResub] = @P6, [OfferTypeKey] = @P7, [AppCapIID] = @P8, [RequireValuation] = @P9 where InstanceID = '" + InstanceID + "'", Parameters);
        }
    }

    public void InsertData(IActiveDataTransaction Tran, Int64 InstanceID, Dictionary<string, string> Fields)
    {
        //// Set Data Fields
        SetDataFields(Fields);
        // Create a collection
        ParameterCollection Parameters = new ParameterCollection();
        // Add the required parameters
        WorkerHelper.AddParameter(Parameters, "@P0", SqlDbType.Int, ParameterDirection.Input, m_ApplicationKey);
        if (m_PreviousState != null)
            WorkerHelper.AddParameter(Parameters, "@P1", SqlDbType.VarChar, ParameterDirection.Input, m_PreviousState);
        else
            WorkerHelper.AddParameter(Parameters, "@P1", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value);
        WorkerHelper.AddParameter(Parameters, "@P2", SqlDbType.Int, ParameterDirection.Input, m_GenericKey);
        if (m_CaseOwnerName != null)
            WorkerHelper.AddParameter(Parameters, "@P3", SqlDbType.VarChar, ParameterDirection.Input, m_CaseOwnerName);
        else
            WorkerHelper.AddParameter(Parameters, "@P3", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value);
        WorkerHelper.AddParameter(Parameters, "@P4", SqlDbType.Bit, ParameterDirection.Input, m_IsFL);
        if (m_EWorkFolderID != null)
            WorkerHelper.AddParameter(Parameters, "@P5", SqlDbType.VarChar, ParameterDirection.Input, m_EWorkFolderID);
        else
            WorkerHelper.AddParameter(Parameters, "@P5", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value);
        WorkerHelper.AddParameter(Parameters, "@P6", SqlDbType.Bit, ParameterDirection.Input, m_IsResub);
        WorkerHelper.AddParameter(Parameters, "@P7", SqlDbType.Int, ParameterDirection.Input, m_OfferTypeKey);
        WorkerHelper.AddParameter(Parameters, "@P8", SqlDbType.BigInt, ParameterDirection.Input, m_AppCapIID);
        WorkerHelper.AddParameter(Parameters, "@P9", SqlDbType.Bit, ParameterDirection.Input, m_RequireValuation);
        WorkerHelper.ExecuteNonQuery(Tran.Context, "insert into [X2DATA].[Application_Management] values( " + InstanceID + ", @P0, @P1, @P2, @P3, @P4, @P5, @P6, @P7, @P8, @P9)", Parameters);
    }

    public bool Contains(string FieldName)
    {
        switch (FieldName.ToLower())
        {
            case "applicationkey":
                return true;
            case "previousstate":
                return true;
            case "generickey":
                return true;
            case "caseownername":
                return true;
            case "isfl":
                return true;
            case "eworkfolderid":
                return true;
            case "isresub":
                return true;
            case "offertypekey":
                return true;
            case "appcapiid":
                return true;
            case "requirevaluation":
                return true;
            default:
                return false;
        }
    }

    public string GetField(string FieldName)
    {
        switch (FieldName.ToLower())
        {
            case "applicationkey":
                return m_ApplicationKey.ToString();
            case "previousstate":
                return m_PreviousState.ToString();
            case "generickey":
                return m_GenericKey.ToString();
            case "caseownername":
                return m_CaseOwnerName.ToString();
            case "isfl":
                return m_IsFL.ToString();
            case "eworkfolderid":
                return m_EWorkFolderID.ToString();
            case "isresub":
                return m_IsResub.ToString();
            case "offertypekey":
                return m_OfferTypeKey.ToString();
            case "appcapiid":
                return m_AppCapIID.ToString();
            case "requirevaluation":
                return m_RequireValuation.ToString();
            default:
                return "";
        }
    }

    public Dictionary<string, string> GetData()
    {
        Dictionary<string, string> Data = new Dictionary<string, string>();
        Data.Add("applicationkey", m_ApplicationKey.ToString());
        if (m_PreviousState != null)
            Data.Add("previousstate", m_PreviousState.ToString());
        Data.Add("generickey", m_GenericKey.ToString());
        if (m_CaseOwnerName != null)
            Data.Add("caseownername", m_CaseOwnerName.ToString());
        Data.Add("isfl", m_IsFL.ToString());
        if (m_EWorkFolderID != null)
            Data.Add("eworkfolderid", m_EWorkFolderID.ToString());
        Data.Add("isresub", m_IsResub.ToString());
        Data.Add("offertypekey", m_OfferTypeKey.ToString());
        Data.Add("appcapiid", m_AppCapIID.ToString());
        Data.Add("requirevaluation", m_RequireValuation.ToString());
        return Data;
    }

    #endregion IX2WorkFlowDataProvider Members


    public Dictionary<string, string> DataFields
    {
        get { throw new NotImplementedException(); }
    }

    public string DataProviderName
    {
        get { throw new NotImplementedException(); }
    }

    public object GetDataField(string name)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetDataFields()
    {
        throw new NotImplementedException();
    }

    public bool HasChanges
    {
        get { throw new NotImplementedException(); }
    }

    public void SetDataField(string name, object value)
    {
        throw new NotImplementedException();
    }
}

#endregion WorkFlowData Application Management

#region WorkFlow Application Management

public class X2Application_Management : IX2WorkFlow
{
    #region States

    /// <summary>
    /// Called when the Manage Application state is entered.
    /// </summary>
    public bool OnEnter_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Manage Application state is exited.
    /// </summary>
    public bool OnExit_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Manage Application";
        return true;
    }

    /// <summary>
    /// Called when the Credit state is entered.
    /// </summary>
    public bool OnEnter_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Credit state is exited.
    /// </summary>
    public bool OnExit_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Credit";
        return true;
    }

    /// <summary>
    /// Called when the LOA state is entered.
    /// </summary>
    public bool OnEnter_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        if (!Application_Management_Data.IsFL)
        {
            List<string> dys = new List<string>();
            dys.Add("Branch Consultant D");
            dys.Add("Branch Admin D");
            dys.Add("Branch Manager D");
            dys.Add("New Business Processor D");
            dys.Add("New Business Manager D");
            WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);

            // Assign the Branch folks back in
            // Assign the Branch folks back in
            WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "LOA", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        }
        return true;
    }

    /// <summary>
    /// Called when the LOA state is exited.
    /// </summary>
    public bool OnExit_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "LOA";
        return true;
    }

    /// <summary>
    /// Called when the Common Resubmission state is entered.
    /// </summary>
    public bool OnEnter_Common_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Resubmission state is exited.
    /// </summary>
    public bool OnExit_Common_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Resubmission state is entered.
    /// </summary>
    public bool OnEnter_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Resubmission state is exited.
    /// </summary>
    public bool OnExit_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Resubmission";
        return true;
    }

    /// <summary>
    /// Called when the System Assign Processor state is entered.
    /// </summary>
    public bool OnEnter_System_Assign_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the System Assign Processor state is exited.
    /// </summary>
    public bool OnExit_System_Assign_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "System Assign Processor";
        return true;
    }

    /// <summary>
    /// Called when the QA state is entered.
    /// </summary>
    public bool OnEnter_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();  
        if (Application_Management_Data.OfferTypeKey == 0)
        {
            int tok = common.GetOfferType(messages,Application_Management_Data.ApplicationKey);
            Application_Management_Data.OfferTypeKey = tok;
            if (Application_Management_Data.OfferTypeKey == 0)
            {
                SAHL.Common.Logging.LogPlugin.LogFormattedError("OfferTypeKey is 0? WTF AID:{0} IID:{1}", Application_Management_Data.ApplicationKey, InstanceData.InstanceID);
            }
        }

        //set require valuation for all cases
        //set to false for Switch and Refinance, so it can get to credit before we pay for a valuation
        if (Application_Management_Data.OfferTypeKey == 6 || Application_Management_Data.OfferTypeKey == 8)
            Application_Management_Data.RequireValuation = false;
        else
            Application_Management_Data.RequireValuation = true;

        return true;
    }

    /// <summary>
    /// Called when the QA state is exited.
    /// </summary>
    public bool OnExit_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "QA";
        return true;
    }

    /// <summary>
    /// Called when the Request at QA state is entered.
    /// </summary>
    public bool OnEnter_Request_at_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Request at QA state is exited.
    /// </summary>
    public bool OnExit_Request_at_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Request at QA";
        return true;
    }

    /// <summary>
    /// Called when the Issue AIP state is entered.
    /// </summary>
    public bool OnEnter_Issue_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Issue AIP state is exited.
    /// </summary>
    public bool OnExit_Issue_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Issue AIP";
        return true;
    }

    /// <summary>
    /// Called when the Next Step state is entered.
    /// </summary>
    public bool OnEnter_Next_Step(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (!Application_Management_Data.IsFL)
        {
            IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
            List<string> dys = new List<string>();
            dys.Add("Branch Consultant D");
            dys.Add("Branch Admin D");
            dys.Add("Branch Manager D");
            dys.Add("QA Administrator D");
            WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        }

        return true;
    }

    /// <summary>
    /// Called when the Next Step state is exited.
    /// </summary>
    public bool OnExit_Next_Step(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive AIP state is entered.
    /// </summary>
    public bool OnEnter_Archive_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (Application_Management_Data.IsFL)
        {
            IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
            furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
            if (messages.HasErrorMessages)
                return false;
        }

        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1);
        return true;
    }

    /// <summary>
    /// Called when the Archive AIP state is exited.
    /// </summary>
    public bool OnExit_Archive_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive AIP state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Application Query state is entered.
    /// </summary>
    public bool OnEnter_Application_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        return appMan.SendEmailToConsultantForQuery(messages, Application_Management_Data.ApplicationKey, InstanceData.InstanceID, 8);
    }

    /// <summary>
    /// Called when the Application Query state is exited.
    /// </summary>
    public bool OnExit_Application_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Application Query";
        return true;
    }

    /// <summary>
    /// Called when the Signed LOA Review state is entered.
    /// </summary>
    public bool OnEnter_Signed_LOA_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (!Application_Management_Data.IsFL)
        {
            IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
            List<string> dys = new List<string>();
            dys.Add("Branch Consultant D");
            dys.Add("Branch Admin D");
            dys.Add("Branch Manager D");
            dys.Add("New Business Processor D");
            dys.Add("New Business Manager D");
            WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        }
        return true;
    }

    /// <summary>
    /// Called when the Signed LOA Review state is exited.
    /// </summary>
    public bool OnExit_Signed_LOA_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Signed LOA Review";
        return true;
    }

    /// <summary>
    /// Called when the Application Check state is entered.
    /// </summary>
    public bool OnEnter_Application_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Application Check state is exited.
    /// </summary>
    public bool OnExit_Application_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Application Check";
        return true;
    }

    /// <summary>
    /// Called when the Disbursement state is entered.
    /// </summary>
    public bool OnEnter_Disbursement(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Disbursement state is exited.
    /// </summary>
    public bool OnExit_Disbursement(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Disbursement";
        return true;
    }

    /// <summary>
    /// Called when the Disbursed state is entered.
    /// </summary>
    public bool OnEnter_Disbursed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Disbursed state is exited.
    /// </summary>
    public bool OnExit_Disbursed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Disbursed";
        return true;
    }

    /// <summary>
    /// Called when the Attorney Check state is entered.
    /// </summary>
    public bool OnEnter_Attorney_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Attorney Check state is exited.
    /// </summary>
    public bool OnExit_Attorney_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Awaiting Application state is entered.
    /// </summary>
    public bool OnEnter_Awaiting_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.IsFL = true;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        InstanceData.Subject = common.GetCaseName(messages, Application_Management_Data.ApplicationKey); ;
        InstanceData.Name = Application_Management_Data.ApplicationKey.ToString();
        Application_Management_Data.IsResub = false;

        return true;
    }

    /// <summary>
    /// Called when the Awaiting Application state is exited.
    /// </summary>
    public bool OnExit_Awaiting_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Awaiting Application";
        return true;
    }

    /// <summary>
    /// Called when the Arrears state is entered.
    /// </summary>
    public bool OnEnter_Arrears(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Arrears state is exited.
    /// </summary>
    public bool OnExit_Arrears(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Arrears";
        return true;
    }

    /// <summary>
    /// Called when the Archive QA Query state is entered.
    /// </summary>
    public bool OnEnter_Archive_QA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1);
        if (Application_Management_Data.IsFL)
        {
            furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
        }

        return true;
    }

    /// <summary>
    /// Called when the Archive QA Query state is exited.
    /// </summary>
    public bool OnExit_Archive_QA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive QA Query state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_QA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Assign at QA state is entered.
    /// </summary>
    public bool OnEnter_Assign_at_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Assign at QA state is exited.
    /// </summary>
    public bool OnExit_Assign_at_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        // Switch Loans never goto QA so dont RR.
        if (Application_Management_Data.OfferTypeKey == 6)
            return true;
        List<int> OSKeys = new List<int>();
        OSKeys.Add(1007);
        OSKeys.Add(1008);
        string User = string.Empty;
        WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeysByProcess(messages, "QA Administrator D", Application_Management_Data.ApplicationKey, OSKeys, InstanceData.InstanceID, "Assign at QA", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.QAAdministrator);
        Application_Management_Data.PreviousState = "Assign at QA";
        return true;
    }

    /// <summary>
    /// Called when the Archive No App Form state is entered.
    /// </summary>
    public bool OnEnter_Archive_No_App_Form(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();

        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1);
        if (messages.HasErrorMessages)
            return false;

        furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
        if (messages.HasErrorMessages)
            return false;

        return true;
    }

    /// <summary>
    /// Called when the Archive No App Form state is exited.
    /// </summary>
    public bool OnExit_Archive_No_App_Form(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive No App Form state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_No_App_Form(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the LOA Query state is entered.
    /// </summary>
    public bool OnEnter_LOA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the LOA Query state is exited.
    /// </summary>
    public bool OnExit_LOA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Exp LOA state is entered.
    /// </summary>
    public bool OnEnter_Archive_Exp_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();

        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 4, -1);

        if (Application_Management_Data.IsFL)
        {
            furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
        }
        return true;
    }

    /// <summary>
    /// Called when the Archive Exp LOA state is exited.
    /// </summary>
    public bool OnExit_Archive_Exp_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Exp LOA state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Exp_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Disbursement Review state is entered.
    /// </summary>
    public bool OnEnter_Disbursement_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Disbursement Review state is exited.
    /// </summary>
    public bool OnExit_Disbursement_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Disbursement Review";
        return true;
    }

    /// <summary>
    /// Called when the Common Translate  state is entered.
    /// </summary>
    public bool OnEnter_Common_Translate_(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Translate  state is exited.
    /// </summary>
    public bool OnExit_Common_Translate_(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Translate Conditions state is entered.
    /// </summary>
    public bool OnEnter_Translate_Conditions(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string AssignedUser = string.Empty;
        WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "Translate Conditions D", Application_Management_Data.ApplicationKey, 1539, InstanceData.InstanceID, "Translate Conditions", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.TranslateConditions);
        //IX2ReturnData ret = BaseHelper.RoundRobinAssignForGivenOrgStructure(messages, out AssignedUser, "Translate Conditions D",Application_Management_Data.ApplicationKey,  1539);
        return true;
    }

    /// <summary>
    /// Called when the Translate Conditions state is exited.
    /// </summary>
    public bool OnExit_Translate_Conditions(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Translate state is entered.
    /// </summary>
    public bool OnEnter_Archive_Translate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Translate state is exited.
    /// </summary>
    public bool OnExit_Archive_Translate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Translate state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Translate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the App Check state is entered.
    /// </summary>
    public bool OnEnter_App_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the App Check state is exited.
    /// </summary>
    public bool OnExit_App_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Resend Instruction state is entered.
    /// </summary>
    public bool OnEnter_Common_Resend_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Resend Instruction state is exited.
    /// </summary>
    public bool OnExit_Common_Resend_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Reassign state is entered.
    /// </summary>
    public bool OnEnter_Common_Reassign(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Reassign state is exited.
    /// </summary>
    public bool OnExit_Common_Reassign(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Common_2nd_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common 2nd Valuation state is exited.
    /// </summary>
    public bool OnExit_Common_2nd_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Valuation Review state is entered.
    /// </summary>
    public bool OnEnter_Common_Valuation_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Valuation Review state is exited.
    /// </summary>
    public bool OnExit_Common_Valuation_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Rework Application state is entered.
    /// </summary>
    public bool OnEnter_Common_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Rework Application state is exited.
    /// </summary>
    public bool OnExit_Common_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Declined by Credit state is entered.
    /// </summary>
    public bool OnEnter_Declined_by_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 5, -1);
        if (!Application_Management_Data.IsFL)
        {
            // Assign the Branch folks back in
            WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "Declined by Credit", SAHL.Common.Constants.WorkFlowProcessName.Origination);

            // Create a Manager Role, after the user assignment
            int ManagerOSKey = WorkflowAssignment.GetBranchManagerOrgStructureKey(messages, InstanceData.InstanceID);
            WorkflowAssignment.AssignBranchManagerForOrgStrucKey(messages, InstanceData.InstanceID, "Branch Manager D", ManagerOSKey, Application_Management_Data.ApplicationKey, "Declined By Credit", SAHL.Common.Globals.Process.Origination);
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "Declined By Credit", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }

        common.CreateNewRevision(messages, Application_Management_Data.ApplicationKey);

        return true;
    }

    /// <summary>
    /// Called when the Declined by Credit state is exited.
    /// </summary>
    public bool OnExit_Declined_by_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Declined by Credit";
        return true;
    }

    /// <summary>
    /// Called when the Further Info Request state is entered.
    /// </summary>
    public bool OnEnter_Further_Info_Request(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        if (!Application_Management_Data.IsFL)
        {
            // reactiveate the NBP users.
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "Further Info Request", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor);
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "Further Info Request", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }
        return true;
    }

    /// <summary>
    /// Called when the Further Info Request state is exited.
    /// </summary>
    public bool OnExit_Further_Info_Request(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Further Info Request";
        return true;
    }

    /// <summary>
    /// Called when the Back to Credit state is entered.
    /// </summary>
    public bool OnEnter_Back_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Back to Credit state is exited.
    /// </summary>
    public bool OnExit_Back_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Disputes state is entered.
    /// </summary>
    public bool OnEnter_Disputes(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        if (!Application_Management_Data.IsFL)
        {
            List<string> dys = new List<string>();
            dys.Add("Branch Consultant D");
            dys.Add("Branch Admin D");
            dys.Add("Branch Manager D");
            dys.Add("New Business Processor D");
            dys.Add("New Business Manager D");
            WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
            // Assign the Branch folks back in
            WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "Disputes", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "Disputes", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }
        return true;
    }

    /// <summary>
    /// Called when the Disputes state is exited.
    /// </summary>
    public bool OnExit_Disputes(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Disputes";
        return true;
    }

    /// <summary>
    /// Called when the Decline Bin state is entered.
    /// </summary>
    public bool OnEnter_Decline_Bin(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();          
        IX2ReturnData ret = null;
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        bool b = false;
        if (Application_Management_Data.IsFL)
        {
            furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
            furtherLending.RemoveDetailTypes(messages, Application_Management_Data.ApplicationKey);
        }

        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        if (!appMan.ArchiveApplicationManagementChildren(messages, InstanceData.InstanceID, Params.ADUserName))
        {
            return false;
        }

        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 5, -1);
        

        if (Application_Management_Data.IsResub)
        {
            if (!string.IsNullOrEmpty(Application_Management_Data.EWorkFolderID))
            {
                string EFID = Application_Management_Data.EWorkFolderID;
                b =common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2DECLINEFINAL, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName); 
                appMan.SendEmailToConsultantForNTUFinalResub(messages, Application_Management_Data.ApplicationKey);               
            }
        }
        return b;
    }

    /// <summary>
    /// Called when the Decline Bin state is exited.
    /// </summary>
    public bool OnExit_Decline_Bin(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Decline Bin state is about to be archived.
    /// </summary>
    public bool OnReturn_Decline_Bin(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common NTU state is entered.
    /// </summary>
    public bool OnEnter_Common_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common NTU state is exited.
    /// </summary>
    public bool OnExit_Common_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Decline state is entered.
    /// </summary>
    public bool OnEnter_Common_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Decline state is exited.
    /// </summary>
    public bool OnExit_Common_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the NTU state is entered.
    /// </summary>
    public bool OnEnter_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        if (!Application_Management_Data.IsFL)
        {
            // Assign the Branch folks back in
            // Assign the Branch folks back in
            WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "NTU", SAHL.Common.Constants.WorkFlowProcessName.Origination);
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "NTU", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor);
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "NTU", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }
        return true;
    }
     
    /// <summary>
    /// Called when the NTU state is exited.
    /// </summary>
    public bool OnExit_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //Application_Management_Data.PreviousState = "NTU";
        return true;
    }

    /// <summary>
    /// Called when the Decline state is entered.
    /// </summary>
    public bool OnEnter_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        bool b = false;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 5, -1);        
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        b = appMan.SendEmailToConsultantForQuery(messages, Application_Management_Data.ApplicationKey, InstanceData.InstanceID, 2);
        return b;
    }

    /// <summary>
    /// Called when the Decline state is exited.
    /// </summary>
    public bool OnExit_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //Application_Management_Data.PreviousState = "Decline";
        return true;
    }

    /// <summary>
    /// Called when the Return to sender state is entered.
    /// </summary>
    public bool OnEnter_Return_to_sender(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 1, -1);
        return true;
    }

    /// <summary>
    /// Called when the Return to sender state is exited.
    /// </summary>
    public bool OnExit_Return_to_sender(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Return to sender state is autoforwarded.
    /// </summary>
    public IX2ReturnData OnForwardState_Return_to_sender(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return Application_Management_Data.PreviousState;
    }

    /// <summary>
    /// Called when the Archive Decline state is entered.
    /// </summary>
    public bool OnEnter_Archive_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IX2ReturnData ret = null;
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        if (Application_Management_Data.IsFL)
        {
            IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
            furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
            furtherLending.RemoveDetailTypes(messages, Application_Management_Data.ApplicationKey);
            
        }

        IApplicationManagement appManagementService = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        if (!appManagementService.ArchiveApplicationManagementChildren(messages, InstanceData.InstanceID, Params.ADUserName))
        {
            return false;
        }
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 5, -1);
        List<string> dys = new List<string>();
        dys.Add("Branch Consultant D");
        dys.Add("Branch Admin D");
        dys.Add("Branch Manager D");
        dys.Add("New Business Processor D");
        dys.Add("New Business Manager D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");

        return WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
    }

    /// <summary>
    /// Called when the Archive Decline state is exited.
    /// </summary>
    public bool OnExit_Archive_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Decline state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive NTU state is entered.
    /// </summary>
    public bool OnEnter_Archive_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IX2ReturnData ret = null;
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        if (Application_Management_Data.IsFL)
        {
            furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
            furtherLending.RemoveDetailTypes(messages, Application_Management_Data.ApplicationKey);
        }
        else
        {
            furtherLending.RemoveDetailTypes(messages, Application_Management_Data.ApplicationKey);
            
        }

        if (!appMan.ArchiveApplicationManagementChildren(messages, InstanceData.InstanceID, Params.ADUserName))
        {
            return false;
        }

        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 4, -1);
       

        if (!Application_Management_Data.IsFL)
        {
            common.UpdateAccountStatus(messages, Application_Management_Data.ApplicationKey, 6);
        }

        List<string> dys = new List<string>();
        dys.Add("Branch Consultant D");
        dys.Add("Branch Admin D");
        dys.Add("Branch Manager D");
        dys.Add("New Business Processor D");
        dys.Add("New Business Manager D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        return true;
    }

    /// <summary>
    /// Called when the Archive NTU state is exited.
    /// </summary>
    public bool OnExit_Archive_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive NTU state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Multiple Applications state is entered.
    /// </summary>
    public bool OnEnter_Multiple_Applications(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Multiple Applications state is exited.
    /// </summary>
    public bool OnExit_Multiple_Applications(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Hold Application state is entered.
    /// </summary>
    public bool OnEnter_Common_Hold_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Hold Application state is exited.
    /// </summary>
    public bool OnExit_Common_Hold_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Application Hold state is entered.
    /// </summary>
    public bool OnEnter_Application_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Application Hold state is exited.
    /// </summary>
    public bool OnExit_Application_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the ContinueLoan state is entered.
    /// </summary>
    public bool OnEnter_ContinueLoan(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the ContinueLoan state is exited.
    /// </summary>
    public bool OnExit_ContinueLoan(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the ContinueLoan state is autoforwarded.
    /// </summary>
    public IX2ReturnData OnForwardState_ContinueLoan(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return Application_Management_Data.PreviousState;
    }

    /// <summary>
    /// Called when the Common Followup state is entered.
    /// </summary>
    public bool OnEnter_Common_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Followup state is exited.
    /// </summary>
    public bool OnExit_Common_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Followup Hold state is entered.
    /// </summary>
    public bool OnEnter_Followup_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Followup Hold state is exited.
    /// </summary>
    public bool OnExit_Followup_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Followup Complete state is entered.
    /// </summary>
    public bool OnEnter_Followup_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        List<string> dys = new List<string>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        return true;
    }

    /// <summary>
    /// Called when the Followup Complete state is exited.
    /// </summary>
    public bool OnExit_Followup_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Followup Complete state is about to be archived.
    /// </summary>
    public bool OnReturn_Followup_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Valuation Complete state is entered.
    /// </summary>
    public bool OnEnter_Archive_Valuation_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Valuation Complete state is exited.
    /// </summary>
    public bool OnExit_Archive_Valuation_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Valuation Complete state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Valuation_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Registration Pipeline state is entered.
    /// </summary>
    public bool OnEnter_Registration_Pipeline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Registration Pipeline state is exited.
    /// </summary>
    public bool OnExit_Registration_Pipeline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Application_Management_Data.PreviousState = "Registration Pipeline";
        return true;
    }

    /// <summary>
    /// Called when the Archive FL state is entered.
    /// </summary>
    public bool OnEnter_Archive_FL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (Application_Management_Data.IsFL)
        {
            IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
            furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
        }
        return true;
    }

    /// <summary>
    /// Called when the Archive FL state is exited.
    /// </summary>
    public bool OnExit_Archive_FL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive FL state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_FL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Origination state is entered.
    /// </summary>
    public bool OnEnter_Archive_Origination(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IX2ReturnData ret = null;
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();

        if (Application_Management_Data.IsFL)
        {
            furtherLending.FLCompleteUnholdNextApplicationWhereApplicable(messages, Application_Management_Data.ApplicationKey);
            furtherLending.SuretySignedConfirmed(messages, Application_Management_Data.ApplicationKey);
        }

        common.SetOfferEndDate(messages, Application_Management_Data.ApplicationKey);
        
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        if (!appMan.ArchiveApplicationManagementChildren(messages, InstanceData.InstanceID, Params.ADUserName))
        {
            return false;
        }

        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Accepted, -1);
        
        string EFID = Application_Management_Data.EWorkFolderID;
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Accepted, (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer);
        furtherLending.RemoveDetailTypes(messages, Application_Management_Data.ApplicationKey);

        List<string> dys = new List<string>();
        dys.Add("Branch Consultant D");
        dys.Add("Branch Admin D");
        dys.Add("Branch Manager D");
        dys.Add("New Business Processor D");
        dys.Add("New Business Manager D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        if (!string.IsNullOrEmpty(EFID))
        {
            string AssignedTo = string.Empty;
            AssignedTo = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
            //BaseHelper.ResolveDynamicRoleToUserName(messages, out AssignedTo, Application_Management_Data.ApplicationKey, "Branch Consultant D", InstanceData.InstanceID);
            return common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2DISBURSEMENTTIMER, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName);
            
        }
        return true;
    }

    /// <summary>
    /// Called when the Archive Origination state is exited.
    /// </summary>
    public bool OnExit_Archive_Origination(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Origination state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Origination(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_CheckQCAndSendToPipeline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the CheckQCAndSendToPipeline state is exited.
    /// </summary>
    public bool OnExit_CheckQCAndSendToPipeline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Ready To Followup state is entered.
    /// </summary>
    public bool OnEnter_Ready_To_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Ready To Followup state is exited.
    /// </summary>
    public bool OnExit_Ready_To_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common FL Rework Application state is entered.
    /// </summary>
    public bool OnEnter_Common_FL_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common FL Rework Application state is exited.
    /// </summary>
    public bool OnExit_Common_FL_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Instruct Failed state is entered.
    /// </summary>
    public bool OnEnter_Instruct_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Instruct Failed state is exited.
    /// </summary>
    public bool OnExit_Instruct_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Assign Admin state is entered.
    /// </summary>
    public bool OnEnter_Common_Assign_Admin(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Assign Admin state is exited.
    /// </summary>
    public bool OnExit_Common_Assign_Admin(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the System Val Complete state is entered.
    /// </summary>
    public bool OnEnter_System_Val_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the System Val Complete state is exited.
    /// </summary>
    public bool OnExit_System_Val_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Return Processor state is entered.
    /// </summary>
    public bool OnEnter_Common_Return_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Return Processor state is exited.
    /// </summary>
    public bool OnExit_Common_Return_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Relodge state is entered.
    /// </summary>
    public bool OnEnter_Common_Relodge(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Relodge state is exited.
    /// </summary>
    public bool OnExit_Common_Relodge(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Archive Followup state is entered.
    /// </summary>
    public bool OnEnter_Common_Archive_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Archive Followup state is exited.
    /// </summary>
    public bool OnExit_Common_Archive_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Manual_Archive(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Manual Archive state is exited.
    /// </summary>
    public bool OnExit_Manual_Archive(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Manual Archive state is about to be archived.
    /// </summary>
    public bool OnReturn_Manual_Archive(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the CommonArchiveMain state is entered.
    /// </summary>
    public bool OnEnter_CommonArchiveMain(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the CommonArchiveMain state is exited.
    /// </summary>
    public bool OnExit_CommonArchiveMain(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Valuation Hold state is entered.
    /// </summary>
    public bool OnEnter_Valuation_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Valuation Hold state is exited.
    /// </summary>
    public bool OnExit_Valuation_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Send to FL Hold state is entered.
    /// </summary>
    public bool OnEnter_Send_to_FL_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Send to FL Hold state is exited.
    /// </summary>
    public bool OnExit_Send_to_FL_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Rapid Hold state is entered.
    /// </summary>
    public bool OnEnter_Rapid_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Rapid Hold state is exited.
    /// </summary>
    public bool OnExit_Rapid_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Credit Hold state is entered.
    /// </summary>
    public bool OnEnter_Credit_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Credit Hold state is exited.
    /// </summary>
    public bool OnExit_Credit_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Back To Credit Hold state is entered.
    /// </summary>
    public bool OnEnter_Back_To_Credit_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Back To Credit Hold state is exited.
    /// </summary>
    public bool OnExit_Back_To_Credit_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the QuickCash Hold state is entered.
    /// </summary>
    public bool OnEnter_QuickCash_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the QuickCash Hold state is exited.
    /// </summary>
    public bool OnExit_QuickCash_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Further Valuation Required state is entered.
    /// </summary>
    public bool OnEnter_Further_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Further Valuation Required state is exited.
    /// </summary>
    public bool OnExit_Further_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Valuation Review Required state is entered.
    /// </summary>
    public bool OnEnter_Valuation_Review_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Valuation Review Required state is exited.
    /// </summary>
    public bool OnExit_Valuation_Review_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Rapid Post Credit Hold state is entered.
    /// </summary>
    public bool OnEnter_Rapid_Post_Credit_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Rapid Post Credit Hold state is exited.
    /// </summary>
    public bool OnExit_Rapid_Post_Credit_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the PipeLine NTU state is entered.
    /// </summary>
    public bool OnEnter_PipeLine_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the PipeLine NTU state is exited.
    /// </summary>
    public bool OnExit_PipeLine_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Resub Application Check state is entered.
    /// </summary>
    public bool OnEnter_Resub_Application_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Resub Application Check state is exited.
    /// </summary>
    public bool OnExit_Resub_Application_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Account_Create_Fail_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Account_Create_Fail_Hold(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Common_Reload_Case_Name(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Common_Reload_Case_Name(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_RetManageApp(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_RetManageApp(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public IX2ReturnData OnForwardState_RetManageApp(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Manage Application");
    }

    public bool OnEnter_Check_Val(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Check_Val(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_SysFurtherVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_SysFurtherVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_ReturnToSender(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_ReturnToSender(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public IX2ReturnData OnForwardState_ReturnToSender(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return Application_Management_Data.PreviousState;
    }

    public bool OnEnter_SysReviewVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_SysReviewVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Common_OnStuck(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Common_OnStuck(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Common_Branch_Rework_(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Rework Application state is exited.
    /// </summary>
    public bool OnExit_Common_Branch_Rework_(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_State99(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_State99(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_FollowupReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_FollowupReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public IX2ReturnData OnForwardState_FollowupReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return Application_Management_Data.PreviousState;
    }

    public bool OnEnter_Common_Reassign_Commission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Common_Reassign_Commission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Processor state is entered.
    /// </summary>
    public bool OnEnter_Archive_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, DYs);

        // get current user assigned to this case, get source id, assign user to source id in WA as this case is heading back to AppMan
        //WorkflowAssignment.ReassignParentMapToCurrentUser(messages, InstanceData.InstanceID, (Int64)InstanceData.SourceInstanceID, Application_Management_Data.ApplicationKey, "Archive Processor", SAHL.Common.Constants.WorkFlowProcessName.Origination);

        string ADUserName = string.Empty;
        WorkflowAssignment.GetLatestUserAcrossInstances(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, 157, "FL Processor D", "Archive Processor", SAHL.Common.Globals.Process.Origination);
        if (ADUserName == null || ADUserName == string.Empty || ADUserName.Length == 0)
        {
            //WorkflowAssignment.X2RoundRobinForGivenOSKeys(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, (Int64)InstanceData.SourceInstanceID, "Archive Processor", SAHL.Common.Constants.WorkFlowProcessName.Origination);
            WorkflowAssignment.X2RoundRobinForPointerDescription(messages, (Int64)InstanceData.SourceInstanceID, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor, Application_Management_Data.ApplicationKey, "FL Processor D", "Archive Processor", SAHL.Common.Globals.Process.Origination);
        }
        else
        {
            WorkflowAssignment.ReassignCaseToUser(messages, (Int64)InstanceData.SourceInstanceID, Application_Management_Data.ApplicationKey, ADUserName, 157, 857, "Archive Processor");
        }

        return true;
    }

    /// <summary>
    /// Called when the Archive Processor state is exited.
    /// </summary>
    public bool OnExit_Archive_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Processor state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Common_Lightstone_AVM(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Common_Lightstone_AVM(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_View_Credit_Score(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_View_Credit_Score(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Common_Credit_Score_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Common_Credit_Score_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Review_Credit_Score(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Review_Credit_Score(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Credit_Score_Checks(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Credit_Score_Checks(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    #endregion States

    #region Activities

    public bool OnStartActivity_Proceed_with_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Proceed_with_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();  
        InstanceData.Subject = common.GetCaseName(messages, Application_Management_Data.ApplicationKey);
        InstanceData.Name = Application_Management_Data.ApplicationKey.ToString();
        Application_Management_Data.IsResub = false;
        int tok = common.GetOfferType(messages,Application_Management_Data.ApplicationKey);
        Application_Management_Data.OfferTypeKey = tok;
        return true;
    }

    /// <summary>
    ///  Proceed with Application.
    /// </summary>
    public string GetStageTransition_Proceed_with_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Proceed_with_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Application_in_Order(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Application_in_Order(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        bool b = false;

        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();

        if (!appMan.ValidateApplication(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (!appMan.RefreshApplicationDocumentCheckList(messages, Application_Management_Data.ApplicationKey))
        {
            return false;
        }

        if (!appMan.ValidateApplicationApplicants(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (!appMan.CheckApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (Application_Management_Data.RequireValuation && !appMan.CheckValuationRequiredRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (!Application_Management_Data.IsFL && !Application_Management_Data.IsResub && !appMan.CheckFLCaseWithResubApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (!Application_Management_Data.IsFL && Application_Management_Data.IsResub && !appMan.CheckFLCaseApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        ICommon commonService = DomainServiceLoader.Instance.Get<ICommon>();

        commonService.PricingForRisk(messages, Application_Management_Data.ApplicationKey);

        if (!Application_Management_Data.IsFL)
        {
            return commonService.DoCreditScore(messages, Application_Management_Data.ApplicationKey, 4, Params.ADUserName, Params.IgnoreWarning);
        }

        return true;
    }

    /// <summary>
    ///  Application in Order.
    /// </summary>
    public string GetStageTransition_Application_in_Order(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Application in Order");
    }

    public string GetActivityMessage_Application_in_Order(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Send_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Send_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Send LOA.
    /// </summary>
    public string GetStageTransition_Send_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Send LOA");
    }

    public string GetActivityMessage_Send_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_QA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_QA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        bool b = false;
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        b = appMan.SendEmailToConsultantForQuery(messages, Application_Management_Data.ApplicationKey, InstanceData.InstanceID, 8);

        if (!b)
        {
            return false;
        }

        if (!Application_Management_Data.IsFL)
        {
            List<string> dys = new List<string>();
            dys.Add("QA Administrator D");
            WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);

            // Assign the Branch folks back in
            WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "Request at QA", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        }
        return false;
    }

    /// <summary>
    ///  QA Query.
    /// </summary>
    public string GetStageTransition_QA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("QA Query");
    }

    public string GetActivityMessage_QA_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Request_Resolved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Request_Resolved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        var applicationCapture = DomainServiceLoader.Instance.Get<IApplicationCapture>();
        var common = DomainServiceLoader.Instance.Get<ICommon>();

        bool hasPassedRules = applicationCapture.CheckBranchSubmitApplicationRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
        if (!Application_Management_Data.IsFL)
        {
            var ret = common.DoCreditScore(messages, Application_Management_Data.ApplicationKey, 3, Params.ADUserName, Params.IgnoreWarning);
            return hasPassedRules;
        }
        return true;
    }

    /// <summary>
    ///  Request Resolved.
    /// </summary>
    public string GetStageTransition_Request_Resolved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Request Resolved");
    }

    public string GetActivityMessage_Request_Resolved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_QA_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_QA_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        bool b = false;

        b = appMan.CheckQACompleteRules(messages,Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
        if (!b)
        {
            return false;
        }

        if (Application_Management_Data.IsFL)
        {
            b = appMan.SaveApplicationForValidation(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
            return b;
        }

        if (b)
        {
            common.PricingForRisk(messages, Application_Management_Data.ApplicationKey);
        }

        return b;
    }

    /// <summary>
    ///  QA Complete.
    /// </summary>
    public string GetStageTransition_QA_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("QA Complete");
    }

    public string GetActivityMessage_QA_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_New_Purchase(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        bool b = false;
        if (Application_Management_Data.OfferTypeKey == 7)
            b = true;
        return b;
    }

    public bool OnCompleteActivity_New_Purchase(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 1, 3);
        common.CreateNewRevision(messages, Application_Management_Data.ApplicationKey);
        List<string> dys = new List<string>();
        dys.Add("QA Administrator D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        // Assign the Branch folks back in
        // Assign the Branch folks back in
        WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "Issue AIP", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        return true;
    }

    /// <summary>
    ///  New Purchase.
    /// </summary>
    public string GetStageTransition_New_Purchase(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_New_Purchase(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Client_Accepts(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Client_Accepts(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.PricingForRisk(messages, Application_Management_Data.ApplicationKey);

        List<string> dys = new List<string>();
        dys.Add("Branch Consultant D");
        dys.Add("Branch Admin D");
        dys.Add("Branch Manager D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        return true;
    }

    /// <summary>
    ///  Client Accepts.
    /// </summary>
    public string GetStageTransition_Client_Accepts(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Client Accepts");
    }

    public string GetActivityMessage_Client_Accepts(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Client_Refuse(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        appMan.CheckADUserInSameBranchRules(messages,Application_Management_Data.ApplicationKey, Params.IgnoreWarning, Params.ADUserName);
        
        return true;
    }

    public bool OnCompleteActivity_Client_Refuse(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Client Refuse.
    /// </summary>
    public string GetStageTransition_Client_Refuse(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Client Refuse");
    }

    public string GetActivityMessage_Client_Refuse(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_1_month_timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_1_month_timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  1 month timer.
    /// </summary>
    public string GetStageTransition_1_month_timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_1_month_timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for 1 month timer.
    /// </summary>
    public DateTime GetActivityTime_1_month_timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //return (DateTime.Now.AddMinutes(15));
        return (DateTime.Now.AddMonths(1));
    }

    public bool OnStartActivity_Other_Types(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Other_Types(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Other Types.
    /// </summary>
    public string GetStageTransition_Other_Types(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Other_Types(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Query_on_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Query_on_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        if (!Application_Management_Data.IsFL)
        {
            IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
            // Deactivate the NBP users -
            List<string> dys = new List<string>();
            dys.Add("New Business Processor D");
            dys.Add("New Business Manager D");
            WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
            // Assign the Branch folks back in
            // Assign the Branch folks back in
            WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "Arrears", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        }

        return true;
    }

    public string GetStageTransition_Query_on_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Query on Application");
    }

    public string GetActivityMessage_Query_on_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Feedback_on_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Feedback_on_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        var applicationCapture = DomainServiceLoader.Instance.Get<IApplicationCapture>();
        var workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        var common = DomainServiceLoader.Instance.Get<ICommon>();

        bool hasPassedRules = applicationCapture.CheckBranchSubmitApplicationRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);

        common.PricingForRisk(messages, Application_Management_Data.ApplicationKey);

        if (!Application_Management_Data.IsFL)
        {
            List<string> dys = new List<string>();
            dys.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD);
            dys.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD);
            dys.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchManagerD);

            workflowAssignment.DeActiveUsersForInstance(messages,InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
            // Assign the NPB
            workflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages,"New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "Manage Application", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor);
        }
        return hasPassedRules;
    }

    /// <summary>
    ///  Feedback on Query.
    /// </summary>
    public string GetStageTransition_Feedback_on_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Feedback on Query received from Branch");
    }

    public string GetActivityMessage_Feedback_on_Query(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Resubmit_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Resubmit_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {

        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        bool b = common.HasInstancePerformedActivity(messages, InstanceData.InstanceID, "Override Check");
        
        if (b)
        {
            if (!Application_Management_Data.IsFL && !appMan.CheckOverRideCheckRules(messages, Application_Management_Data.ApplicationKey, true))
            {
                return false;
            }

            if (!appMan.CheckResubOverRideCheckRules(messages, Application_Management_Data.ApplicationKey, true))
            {
                return false;
            }

            // Ensuring PricingForRisk is run before returning
            common.PricingForRisk(messages, Application_Management_Data.ApplicationKey);
            if (!Application_Management_Data.IsFL)
            {
                return common.DoCreditScore(messages, Application_Management_Data.ApplicationKey, 4, Params.ADUserName, Params.IgnoreWarning);
            }
            return true;
        }
        else
        {
            if (!appMan.ValidateApplication(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
            {
                return false;
            }

            if (!appMan.RefreshApplicationDocumentCheckList(messages, Application_Management_Data.ApplicationKey))
            {
                return false;
            }

            if (!appMan.ValidateApplicationApplicants(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
            {
                return false;
            }

            if (!appMan.CheckApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
            {
                return false;
            }

            if (Application_Management_Data.RequireValuation && !appMan.CheckValuationRequiredRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
            {
                return false;
            }

            if (!Application_Management_Data.IsFL && !Application_Management_Data.IsResub && !appMan.CheckFLCaseWithResubApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
            {
                return false;
            }

            if (!Application_Management_Data.IsFL && Application_Management_Data.IsResub && !appMan.CheckFLCaseApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
            {
                return false;
            }

            // Ensuring PricingForRisk is run before returning
            common.PricingForRisk(messages, Application_Management_Data.ApplicationKey);

            if (!Application_Management_Data.IsFL)
            {
                return common.DoCreditScore(messages, Application_Management_Data.ApplicationKey, 4, Params.ADUserName, Params.IgnoreWarning);
            }

            return true;
        }
    }

    /// <summary>
    ///  Resubmit to Credit.
    /// </summary>
    public string GetStageTransition_Resubmit_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Resubmit to Credit");
    }

    public string GetActivityMessage_Resubmit_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_LOA_Received(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_LOA_Received(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  LOA Received.
    /// </summary>
    public string GetStageTransition_LOA_Received(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("LOA Received");
    }

    public string GetActivityMessage_LOA_Received(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_LOA_Accepted(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IApplicationManagement appManagementService = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        return appManagementService.CheckLOAAcceptedRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
    }

    public bool OnCompleteActivity_LOA_Accepted(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  LOA Accepted.
    /// </summary>
    public string GetStageTransition_LOA_Accepted(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("LOA Accepted");
    }

    public string GetActivityMessage_LOA_Accepted(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Instruct_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Instruct_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        bool b = false;        
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        appMan.CheckInstructAttorneyRules(messages,Application_Management_Data.ApplicationKey, Params.IgnoreWarning);        
        return true;
    }

    /// <summary>
    ///  Instruct Attorney.
    /// </summary>
    public string GetStageTransition_Instruct_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Instruct Attorney");
    }

    public string GetActivityMessage_Instruct_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Review_Disbursement_Setup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Review_Disbursement_Setup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Review Disbursement Setup.
    /// </summary>
    public string GetStageTransition_Review_Disbursement_Setup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Review Disbursement Setup");
    }

    public string GetActivityMessage_Review_Disbursement_Setup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Rollback_Disbursement(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Rollback_Disbursement(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        string EFID = Application_Management_Data.EWorkFolderID;
        if (EFID != "")
        {
            IX2ReturnData ret = null;
            if (!Application_Management_Data.IsFL)
            {
                appMan.RollbackDisbursment(messages, Application_Management_Data.ApplicationKey);                
            }

            return common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2ROLLBACKDISBURSEMENT, Application_Management_Data.ApplicationKey, Params.ADUserName, InstanceData.StateName);
        }
        return true;
    }

    /// <summary>
    ///  Rollback Disbursement.
    /// </summary>
    public string GetStageTransition_Rollback_Disbursement(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Rollback Disbursement");
    }

    public string GetActivityMessage_Rollback_Disbursement(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Resubmit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Resubmit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.ArchiveV3ITCForApplication(messages,Application_Management_Data.ApplicationKey);
        
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();  

        Application_Management_Data.IsResub = true;
        appMan.ReturnNonDisbursedLoanToProspect(messages, Application_Management_Data.ApplicationKey);
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 1, -1);
        common.CreateNewRevision(messages, Application_Management_Data.ApplicationKey);
        

        string EFID = Application_Management_Data.EWorkFolderID;
        if (EFID != "")
        {
            appMan.CheckEWorkAtCorrectState(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
            return common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2RESUB, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName);
        }
        return true;
    }

    /// <summary>
    ///  Resubmit.
    /// </summary>
    public string GetStageTransition_Resubmit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Resubmit");
    }

    public string GetActivityMessage_Resubmit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_AttAssigned(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.CreateAccountForApplication(messages, Application_Management_Data.ApplicationKey, Params.ADUserName);
        return true;
    }

    public bool OnCompleteActivity_AttAssigned(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  AttAssigned.
    /// </summary>
    public string GetStageTransition_AttAssigned(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_AttAssigned(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Application_Received(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Application_Received(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        furtherLending.AddDetailTypes(messages, Application_Management_Data.ApplicationKey, Params.ADUserName);
        return true;
    }

    /// <summary>
    ///  Application Received.
    /// </summary>
    public string GetStageTransition_Application_Received(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Application Received");
    }

    public string GetActivityMessage_Application_Received(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Note_Comment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Note_Comment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        if (Application_Management_Data.IsFL)
        {
            // Deactivate the FLCollections Users
            //WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey);
            // Assign the NPB
            //WorkflowAssignment.ReactiveUserOrRoundRobin(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID);
        }
        return true;
    }

    /// <summary>
    ///  Note Comment.
    /// </summary>
    public string GetStageTransition_Note_Comment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Note Comment");
    }

    public string GetActivityMessage_Note_Comment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Rapid_Readvance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        bool b = false;
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        if (Application_Management_Data.OfferTypeKey == (int)SAHL.Common.Globals.OfferTypes.ReAdvance)
        {
            b = furtherLending.CheckRapidShouldGotoCreditRules(messages, Application_Management_Data.ApplicationKey, false);
        }

        List<string> dys = new List<string>();
        dys.Add("New Business Processor D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);

        return b;
    }

    public bool OnCompleteActivity_Rapid_Readvance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Rapid Readvance.
    /// </summary>
    public string GetStageTransition_Rapid_Readvance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Rapid_Readvance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_2_Months(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_2_Months(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  2 Months.
    /// </summary>
    public string GetStageTransition_2_Months(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_2_Months(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for 2 Months.
    /// </summary>
    public DateTime GetActivityTime_2_Months(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //return (DateTime.Now.AddMinutes(15));
        return (DateTime.Now.AddMonths(2));
    }

    public bool OnStartActivity_Assigned_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Assigned_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Assigned QA.
    /// </summary>
    public string GetStageTransition_Assigned_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Assigned_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_45_days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_45_days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 4, -1);
        return true;
    }

    /// <summary>
    ///  45 days.
    /// </summary>
    public string GetStageTransition_45_days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_45_days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for 45 days.
    /// </summary>
    public DateTime GetActivityTime_45_days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //return (DateTime.Now.AddMinutes(15));
        return (DateTime.Now.AddDays(45));
    }

    public bool OnStartActivity_Send_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Send_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Send AIP.
    /// </summary>
    public string GetStageTransition_Send_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Send AIP");
    }

    public string GetActivityMessage_Send_AIP(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Query_on_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Query_on_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        if (!Application_Management_Data.IsFL)
        {
            IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
            List<string> dys = new List<string>();
            dys.Add("Branch Consultant D");
            dys.Add("Branch Admin D");
            dys.Add("Branch Manager D");
            dys.Add("New Business Processor D");
            dys.Add("New Business Manager D");
            WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);

            // Assign the Branch folks back in
            // Assign the Branch folks back in
            WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "LOA Query", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        }
        return true;
    }

    /// <summary>
    ///  Query on LOA.
    /// </summary>
    public string GetStageTransition_Query_on_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Query on LOA");
    }

    public string GetActivityMessage_Query_on_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_30_Days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_30_Days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  30 Days.
    /// </summary>
    public string GetStageTransition_30_Days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_30_Days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for 30 Days.
    /// </summary>
    public DateTime GetActivityTime_30_Days(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //return (DateTime.Now.AddMinutes(15));
        return (DateTime.Now.AddDays(30));
    }

    public bool OnStartActivity_Resend_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Resend_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Resend LOA.
    /// </summary>
    public string GetStageTransition_Resend_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Resend LOA");
    }

    public string GetActivityMessage_Resend_LOA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Require_Arrear_Comment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Require_Arrear_Comment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        // WorkflowAssignment.ReactiveUserOrRoundRobin(messages, "FL Collections Admin D", Application_Management_Data.ApplicationKey, 134, InstanceData.InstanceID);
        // if (Application_Management_Data.IsFL)
        // {
        //   Deactivate
        //  WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey);
        // }
        return true;
    }

    /// <summary>
    ///  Require Arrear Comment.
    /// </summary>
    public string GetStageTransition_Require_Arrear_Comment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Require Arrear Comment");
    }

    public string GetActivityMessage_Require_Arrear_Comment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Override_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Override_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IApplicationManagement appManService = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        if (!Application_Management_Data.IsFL && !appManService.CheckOverRideCheckRules(messages, Application_Management_Data.ApplicationKey, true))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    ///  Override Check.
    /// </summary>
    public string GetStageTransition_Override_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Override Check");
    }

    public string GetActivityMessage_Override_Check(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Valuation_in_Order(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Valuation_in_Order(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Valuation_in_Order(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Valuation_Resubmission_(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Valuation_Resubmission_(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Valuation_Resubmission_(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Return_Processor_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Return_Processor_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        List<string> dys = new List<string>();
        dys.Add("Branch Consultant D");
        dys.Add("Branch Admin D");
        dys.Add("Branch Manager D");
        dys.Add("New Business Processor D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        if (!Application_Management_Data.IsFL)
        {
            // reactiveate the NBP users.
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "Manage Application", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor);
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "Manage Application", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Return_Processor_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Return_with_Approve_or_Offer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Return_with_Approve_or_Offer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Return_with_Approve_or_Offer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Credit_Return_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Credit_Return_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Credit_Return_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Credit_Return_Resub_Approve(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Credit_Return_Resub_Approve(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Credit_Return_Resub_Approve(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Translation_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Translation_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Translation Complete.
    /// </summary>
    public string GetStageTransition_Translation_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Translation Complete");
    }

    public string GetActivityMessage_Translation_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Misc_Condition(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return false;
        //IX2ReturnData ret = AppMan.TranslateCondition(Application_Management_Data.ApplicationKey, InstanceData.InstanceID, Application_Management_Data.IsFL, out b);
        //return b;
    }

    public bool OnCompleteActivity_Misc_Condition(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Misc Condition.
    /// </summary>
    public string GetStageTransition_Misc_Condition(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Misc_Condition(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Select_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Select_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Select Attorney.
    /// </summary>
    public string GetStageTransition_Select_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Select Attorney");
    }

    public string GetActivityMessage_Select_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Att_not_assigned(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        return appMan.HasApplicationRole(messages, Application_Management_Data.ApplicationKey, (int)OfferRoleTypes.ConveyanceAttorney );
      
    }

    public bool OnCompleteActivity_Att_not_assigned(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Att not assigned.
    /// </summary>
    public string GetStageTransition_Att_not_assigned(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Att_not_assigned(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Valuation_Workflow(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Valuation_Workflow(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Valuation_Workflow(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Return_ReAdv_Payment_FL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Return_ReAdv_Payment_FL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Return_ReAdv_Payment_FL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_New_Business(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        // removed ticket #12833
        // string userName = string.Empty;
        // BaseHelper.HasCaseBeenAssignedToThisDynamicRoleBefore("Registrations LOA Admin D", Application_Management_Data.ApplicationKey, out userName);
        //
        // IX2ReturnData ret = null;
        // string AssignedUser = string.Empty;
        // if (string.IsNullOrEmpty(userName))
        // {
        //  ret = BaseHelper.RoundRobinAssignForGivenOrgStructure(messages, out AssignedUser, "Registrations LOA Admin D", Application_Management_Data.ApplicationKey, 579);
        // }
        // else
        // {
        //  ret = BaseHelper.ReassignOrEscalateCaseToUser(messages, Application_Management_Data.ApplicationKey, "Registrations LOA Admin D", userName, true);
        // }
        //
        // return true;
        return true;
    }

    public bool OnCompleteActivity_New_Business(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  New Business.
    /// </summary>
    public string GetStageTransition_New_Business(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_New_Business(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Further_Lending(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (Application_Management_Data.IsFL)
        {
            bool b = false;
            if (Application_Management_Data.OfferTypeKey == 2 || Application_Management_Data.OfferTypeKey == 3 || Application_Management_Data.OfferTypeKey == 4)
                b = true;
            return b;
        }
        return false;
    }

    public bool OnCompleteActivity_Further_Lending(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Further Lending.
    /// </summary>
    public string GetStageTransition_Further_Lending(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Further_Lending(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Readvance_Payments(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Readvance_Payments(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Readvance_Payments(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Readvance_Payment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Readvance_Payment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Readvance_Payment(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Assign_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //bool requireValuation;
        //AppMan.CheckIfApplicationRequiresValuation(Application_Management_Data.ApplicationKey, out requireValuation);
        //if(requireValuation)
        //{
        // Application_Management_Data.RequireValuation = requireValuation;
        //}

        // The logic is a bit messy but works as it is used in another place.
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        IValuations val =  DomainServiceLoader.Instance.Get<IValuations>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();

        bool hasValuation = val.CheckValuationExistsRecentRules(messages,Application_Management_Data.ApplicationKey, false);
        bool requireValuationApproval = !hasValuation;

        if (requireValuationApproval)
        {
            Application_Management_Data.RequireValuation = requireValuationApproval;
        }
        if (Application_Management_Data.IsFL) return true;
        string AssignedUser = string.Empty;
        
        WorkflowAssignment.GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(messages, out AssignedUser, 694, Application_Management_Data.ApplicationKey, InstanceData.InstanceID);
        if (string.IsNullOrEmpty(AssignedUser))
        {
            // no NPB user who has worked on a case for these Main Applicants before so RR
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "Manage Application", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor);
        }
        else
        {
            // Assign the case to a specific person.
            WorkflowAssignment.ReassignCaseToUser(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, AssignedUser, 106, 694, "Manage Application");
        }
        common.UpdateAssignedUserInIDM(messages, Application_Management_Data.ApplicationKey, Application_Management_Data.IsFL, InstanceData.InstanceID, "Application Management");

        return true;
    }

    public bool OnCompleteActivity_Assign_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        // if (Application_Management_Data.RequireValuation)
        // {
        //  Message = "Returned from Credit – Valuation Approved";
        // }
        return true;
    }

    /// <summary>
    ///  Assign Processor.
    /// </summary>
    public string GetStageTransition_Assign_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Assign_Processor(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_AutoValuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();          
        IFL FL = DomainServiceLoader.Instance.Get<IFL>();
        IValuations val = DomainServiceLoader.Instance.Get<IValuations>();
        val.CheckIfCanPerformFurtherValuation(messages, InstanceData.InstanceID);
        //if no valuation clone is required yet, continue the process to save the financial cost
        if (!Application_Management_Data.RequireValuation)
            return false;

        bool b = false;
        common.IsValuationInProgress(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey);
        if (b)
            return false;
        if (Application_Management_Data.IsFL)
        {
            b = FL.ValuationRequired(messages,Application_Management_Data.ApplicationKey);
        }
        else
        {
            b = val.CheckValuationExistsRecentRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
        }
        return b;
    }

    public bool OnCompleteActivity_AutoValuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  AutoValuation.
    /// </summary>
    public string GetStageTransition_AutoValuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_AutoValuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_GotoCredit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_GotoCredit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        List<string> dys = new List<string>();
        dys.Add("New Business Processor D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        return true;
    }

    /// <summary>
    ///  GotoCredit.
    /// </summary>
    public string GetStageTransition_GotoCredit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_GotoCredit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_AutoGotoQC(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        // look at the old check for QC method here if we ever even get to using QC
        return false;
    }

    public bool OnCompleteActivity_AutoGotoQC(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  AutoGotoQC.
    /// </summary>
    public string GetStageTransition_AutoGotoQC(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_AutoGotoQC(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Resend_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Resend_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        if (Convert.ToBoolean(Params.Data))
        {
            IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
            ICommon common = DomainServiceLoader.Instance.Get<ICommon>();

            string EFID = Application_Management_Data.EWorkFolderID;
            // archive ework case and resend instruction
            if (!string.IsNullOrEmpty(EFID))
            {
                bool b = false;
                IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
                string AssignedTo = string.Empty;
                AssignedTo = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
                b = common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2ARCHIVE, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName);
                string EFolderID = "";
                
                b = appMan.CreateEWorkPipelineCase(messages, Application_Management_Data.ApplicationKey, out EFolderID);
                Application_Management_Data.EWorkFolderID = EFolderID;
                return b;
            }
        }
        else
        {
            // Do nothing just resend the instruction.
        }
        return true;
    }

    public string GetStageTransition_Resend_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Resend Instruction");
    }

    public string GetActivityMessage_Resend_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Reassign_User(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Reassign_User(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateAssignedUserInIDM(messages, Application_Management_Data.ApplicationKey, Application_Management_Data.IsFL, InstanceData.InstanceID, "Application Management");
        return true;
    }

    /// <summary>
    ///  Reassign User.
    /// </summary>
    public string GetStageTransition_Reassign_User(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        Console.WriteLine("*** {0} ***", Params.Data);
        if (null != Params.Data)
            return Params.Data.ToString();
        return string.Empty;
    }

    public string GetActivityMessage_Reassign_User(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Perform_Further_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (!Application_Management_Data.RequireValuation)
        {
            BaseHelper.ShowMessage("Valuation is not required", X2MessageType.Warning); return false;
        }
                
        IValuations val = DomainServiceLoader.Instance.Get<IValuations>();
        val.CheckIfCanPerformFurtherValuation(messages,InstanceData.InstanceID);
        return true;
    }

    public bool OnCompleteActivity_Perform_Further_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {       
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        return common.IsValuationInProgress(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey); ;
    }

    /// <summary>
    ///  Perform Further Valuation.
    /// </summary>
    public string GetStageTransition_Perform_Further_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Perform Further Valuation");
    }

    public string GetActivityMessage_Perform_Further_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Review_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (!Application_Management_Data.RequireValuation)
        {
            BaseHelper.ShowMessage("Valuation is not required", X2MessageType.Warning); return false;
        }
        return true;
    }

    public bool OnCompleteActivity_Review_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        return common.IsValuationInProgress(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey);        
    }

    /// <summary>
    ///  Review Valuation Required.
    /// </summary>
    public string GetStageTransition_Review_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Review Valuation Required");
    }

    public string GetActivityMessage_Review_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Rework Application.
    /// </summary>
    public string GetStageTransition_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Rework Application");
    }

    public string GetActivityMessage_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Info_Request_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Info_Request_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Info Request Complete.
    /// </summary>
    public string GetStageTransition_Info_Request_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Info Request Complete");
    }

    public string GetActivityMessage_Info_Request_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Decline_Final(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        return appMan.CheckADUserInSameBranchRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning, Params.ADUserName); ;
    }

    public bool OnCompleteActivity_Decline_Final(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Decline_Final(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Decline Final");
    }

    public string GetActivityMessage_Decline_Final(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Motivate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Motivate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();

        if (!appMan.ValidateApplication(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (!appMan.RefreshApplicationDocumentCheckList(messages, Application_Management_Data.ApplicationKey))
        {
            return false;
        }

        if (!appMan.ValidateApplicationApplicants(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (!appMan.CheckApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (Application_Management_Data.RequireValuation && !appMan.CheckValuationRequiredRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (!Application_Management_Data.IsFL && !Application_Management_Data.IsResub && !appMan.CheckFLCaseWithResubApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        if (!Application_Management_Data.IsFL && Application_Management_Data.IsResub && !appMan.CheckFLCaseApplicationInOrderRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        ICommon commonService = DomainServiceLoader.Instance.Get<ICommon>();
        commonService.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 1, -1);
        return true;
    }

    /// <summary>
    ///  Motivate.
    /// </summary>
    public string GetStageTransition_Motivate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Motivate");
    }

    public string GetActivityMessage_Motivate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Dispute_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Dispute_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Dispute Finalised.
    /// </summary>
    public string GetStageTransition_Dispute_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Dispute Finalised");
    }

    public string GetActivityMessage_Dispute_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Credit_Decline_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Credit_Decline_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Credit_Decline_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Credit_Further_Info(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Credit_Further_Info(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Credit_Further_Info(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Credit_Dispute(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Credit_Dispute(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Credit_Dispute(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_NTU_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_NTU_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        string EFID = Application_Management_Data.EWorkFolderID;

        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();

        //182 being the Reason Description Key (Expiry due to excessive time lapse) and 11 the Reason Type Key (Application NTU)
        common.AddReasons(messages,Application_Management_Data.ApplicationKey, 182, 11);

        if (!string.IsNullOrEmpty(EFID))
        {
            string AssignedTo = string.Empty;
            IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
            IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
            AssignedTo = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
            return common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2ClientRefused, Application_Management_Data.ApplicationKey, Params.ADUserName, InstanceData.StateName);
        }

        return true;
    }

    /// <summary>
    ///  NTU Timeout.
    /// </summary>
    public string GetStageTransition_NTU_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_NTU_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for NTU Timeout.
    /// </summary>
    public DateTime GetActivityTime_NTU_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //return (DateTime.Now.AddMinutes(15));
        DateTime dt = DateTime.Now;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        return common.GetnWorkingDaysFromToday(messages, 11);
    }

    public bool OnStartActivity_Decline_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Decline_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Decline Timeout.
    /// </summary>
    public string GetStageTransition_Decline_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Decline_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for Decline Timeout.
    /// </summary>
    public DateTime GetActivityTime_Decline_Timeout(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //return (DateTime.Now.AddMinutes(15));
        return (DateTime.Now.AddDays(30));
    }

    public bool OnStartActivity_Reinstate_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        bool b = false;
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        IX2ReturnData ret = appMan.CheckIfReinstateAllowedByUser(messages, Application_Management_Data.ApplicationKey, Application_Management_Data.PreviousState, Params.IgnoreWarning, Params.ADUserName, out b);
        if (b == false)
            BaseHelper.ShowMessage(String.Format("You cannot Reinstate this NTU (Previous State: {0}) - please refer to your Manager.", Application_Management_Data.PreviousState), X2MessageType.Warning); return false;

        return true;
    }

    public bool OnCompleteActivity_Reinstate_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        string AssignedTo = string.Empty;
        bool b = false;
        AssignedTo = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
        string EFolderID = Application_Management_Data.EWorkFolderID;
        if (!string.IsNullOrEmpty(EFolderID))
        {
            b = common.PerformEWorkAction(messages, EFolderID, SAHL.Common.Constants.EworkActionNames.X2ClientWonOver, Application_Management_Data.ApplicationKey, AssignedTo, InstanceData.StateName);
            
        }
        Application_Management_Data.EWorkFolderID = EFolderID;
        WorkflowAssignment.HandleAppManRolesOnReturnFromNTUTOPrevState(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, Application_Management_Data.PreviousState, Application_Management_Data.IsFL, Application_Management_Data.AppCapIID, SAHL.Common.Constants.WorkFlowProcessName.Origination);
        return true;
    }

    /// <summary>
    ///  Reinstate NTU.
    /// </summary>
    public string GetStageTransition_Reinstate_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Reinstate NTU");
    }

    public string GetActivityMessage_Reinstate_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Reinstate_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        InstanceData.Name = Application_Management_Data.ApplicationKey.ToString();
        return true;
    }

    public bool OnCompleteActivity_Reinstate_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        List<string> dys = new List<string>();
        dys.Add("Branch Consultant D");
        dys.Add("Branch Admin D");
        dys.Add("Branch Manager D");
        dys.Add("New Business Processor D");
        dys.Add("New Business Supervisor D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        if (!Application_Management_Data.IsFL)
        {
            switch (Application_Management_Data.PreviousState.ToUpper())
            {
                case "QA":
                    {
                        List<int> OSKeys = new List<int>();
                        OSKeys.Add(1007);
                        OSKeys.Add(1008);
                        WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeysByProcess(messages, "QA Administrator D", Application_Management_Data.ApplicationKey, OSKeys, InstanceData.InstanceID, "Return To Sender From Decline", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.QAAdministrator);
                        break;
                    }
                case "MANAGE APPLICATION":
                    {
                        WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "Return To Sender From Decline", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor);
                        break;
                    }
            }
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "Return To Sender From Decline", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }
        return true;
    }

    /// <summary>
    ///  Reinstate Decline.
    /// </summary>
    public string GetStageTransition_Reinstate_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Reinstate Decline");
    }

    public string GetActivityMessage_Reinstate_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        if (!Application_Management_Data.IsFL)
        {
            // Assign the Branch folks back in
            IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
            WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "Decline", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        }
        return true;
    }

    /// <summary>
    ///  Decline.
    /// </summary>
    public string GetStageTransition_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Decline");
    }

    public string GetActivityMessage_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        string EFID = Application_Management_Data.EWorkFolderID;
        IFL FL = DomainServiceLoader.Instance.Get<IFL>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();  
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        if (Application_Management_Data.IsFL)
        {
            FL.InitialFLNTU(messages, Params.ADUserName, Application_Management_Data.ApplicationKey, InstanceData.InstanceID);
        }
        // tell the user. if it doesnt work then dont perform ntu in ework and roll the case back

        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 4, -1);
        bool b = true;

        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        b = appMan.SendEmailToConsultantForQuery(messages, Application_Management_Data.ApplicationKey, InstanceData.InstanceID, 4);

        if (!b)
        {
            return b;
        }

        if (!string.IsNullOrEmpty(EFID))
        {
            string AssignedTo = string.Empty;
            AssignedTo = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
            b = common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2NTUAdvise, Application_Management_Data.ApplicationKey, Params.ADUserName, InstanceData.StateName);
        }

        appMan.NTUCase(messages, Application_Management_Data.ApplicationKey);
        return b;
    }

    /// <summary>
    ///  NTU.
    /// </summary>
    public string GetStageTransition_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("NTU");
    }

    public string GetActivityMessage_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Disbursement_Incorrect(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Disbursement_Incorrect(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Disbursement Incorrect.
    /// </summary>
    public string GetStageTransition_Disbursement_Incorrect(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Disbursement Incorrect");
    }

    public string GetActivityMessage_Disbursement_Incorrect(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Further_Lending_Calc(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Further_Lending_Calc(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Further Lending Calc.
    /// </summary>
    public string GetStageTransition_Further_Lending_Calc(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Further Lending Calc");
    }

    public string GetActivityMessage_Further_Lending_Calc(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Apps_in_prog_of_higher_pri(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {        
        IFL FL = DomainServiceLoader.Instance.Get<IFL>();
        return FL.AppsInProgOfHigherPri(messages,Application_Management_Data.ApplicationKey);        
    }

    public bool OnCompleteActivity_Apps_in_prog_of_higher_pri(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        Application_Management_Data.PreviousState = "QA";
        return true;
    }

    /// <summary>
    ///  Apps in prog of higher pri.
    /// </summary>
    public string GetStageTransition_Apps_in_prog_of_higher_pri(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Apps_in_prog_of_higher_pri(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_Hold_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_Hold_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        SAHL.Common.Logging.LogPlugin.LogFormattedError("EXT Hold Application:{0}", Application_Management_Data.ApplicationKey);
        return true;
    }

    /// <summary>
    ///  EXT Hold Application.
    /// </summary>
    public string GetStageTransition_EXT_Hold_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_EXT_Hold_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_Reactivate_App(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_Reactivate_App(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        System.Threading.Thread.Sleep(1000);
        SAHL.Common.Logging.LogPlugin.LogFormattedError("EXT Reactivate App:{0}", Application_Management_Data.ApplicationKey);
        return true;
    }

    /// <summary>
    ///  EXT Reactivate App.
    /// </summary>
    public string GetStageTransition_EXT_Reactivate_App(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_EXT_Reactivate_App(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Highest_Priority(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        bool b = false;
        IFL FL = DomainServiceLoader.Instance.Get<IFL>();
        FL.HighestPriority(messages,Application_Management_Data.ApplicationKey);
        return b;
    }

    public bool OnCompleteActivity_Highest_Priority(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        if (Application_Management_Data.IsFL)
        {
            IFL FL = DomainServiceLoader.Instance.Get<IFL>();
            IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
            ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
            List<string> dys = new List<string>();
            dys.Add("FL Processor D");
            dys.Add("FL Supervisor D");
            dys.Add("FL Manager D");
            WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
            // do a round robin and assign other FL cases for this account to same biatch
            string AssignedTo = string.Empty;
            //IX2ReturnData ret = WorkflowAssignment.RoundRobinAndAssignOtherFLCases(messages, Application_Management_Data.ApplicationKey, "FL Processor D", 157, out AssignedTo, InstanceData.InstanceID, "QA", (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
            WorkflowAssignment.X2RoundRobinForGivenOSKey(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "QA");
            FL.AddAccountMemoMessageOnReceiptOfApplication(messages,Application_Management_Data.ApplicationKey, Params.ADUserName, AssignedTo);
            common.UpdateAssignedUserInIDM(messages, Application_Management_Data.ApplicationKey, Application_Management_Data.IsFL, InstanceData.InstanceID, "Application Management");
            return true;
        }
        return true;
    }

    /// <summary>
    ///  Highest Priority.
    /// </summary>
    public string GetStageTransition_Highest_Priority(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Highest_Priority(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Complete_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Complete_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Complete Followup.
    /// </summary>
    public string GetStageTransition_Complete_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Complete Followup");
    }

    public string GetActivityMessage_Complete_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Update_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Update_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Update Followup.
    /// </summary>
    public string GetStageTransition_Update_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Update Followup");
    }

    public string GetActivityMessage_Update_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Create_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Create_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        InstanceData.Subject = common.GetCaseName(messages, Application_Management_Data.ApplicationKey); ;
        InstanceData.Name = Application_Management_Data.ApplicationKey.ToString();
        return true;
    }

    /// <summary>
    ///  Create Followup.
    /// </summary>
    public string GetStageTransition_Create_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Create Followup");
    }

    public string GetActivityMessage_Create_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_OnFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  OnFollowup.
    /// </summary>
    public string GetStageTransition_OnFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_OnFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for OnFollowup.
    /// </summary>
    public DateTime GetActivityTime_OnFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        return common.GetFollowupTime(messages,Application_Management_Data.GenericKey);
    }

    public bool OnStartActivity_NTU_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        bool b = false;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();  
        IApplicationManagement applicationManagement = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        applicationManagement.CheckADUserInSameBranchRules(messages,Application_Management_Data.ApplicationKey, Params.IgnoreWarning, Params.ADUserName);
        b = true;

        return b;
    }

    public bool OnCompleteActivity_NTU_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        bool b = false;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        IFL FL = DomainServiceLoader.Instance.Get<IFL>();
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        b = FL.CheckNTUDeclineFinalRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
       
        if (Application_Management_Data.IsFL)
        {
            appMan.RemoveDetailFromApplicationAfterNTUFinalised(messages, Application_Management_Data.ApplicationKey);
        }

        string EFID = Application_Management_Data.EWorkFolderID;
        if (!string.IsNullOrEmpty(EFID))
        {
            string AssignedTo = string.Empty;
            AssignedTo = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
            //BaseHelper.ResolveDynamicRoleToUserName(messages, out AssignedTo,  Application_Management_Data.ApplicationKey, "Branch Consultant D",InstanceData.InstanceID);
            return common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2ClientRefused, Application_Management_Data.ApplicationKey, Params.ADUserName, InstanceData.StateName); 
        }

        return true;
    }

    /// <summary>
    ///  NTU Finalised.
    /// </summary>
    public string GetStageTransition_NTU_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("NTU Finalised");
    }

    public string GetActivityMessage_NTU_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Decline_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {       
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        return appMan.CheckADUserInSameBranchRules(messages,Application_Management_Data.ApplicationKey, Params.IgnoreWarning, Params.ADUserName) ;
    }

    public bool OnCompleteActivity_Decline_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {        
        IFL FL = DomainServiceLoader.Instance.Get<IFL>();     
        return FL.CheckNTUDeclineFinalRules(messages,Application_Management_Data.ApplicationKey, Params.IgnoreWarning);        
    }

    /// <summary>
    ///  Decline Finalised.
    /// </summary>
    public string GetStageTransition_Decline_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Decline Finalised");
    }

    public string GetActivityMessage_Decline_Finalised(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Pipeline_UpForFees(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Pipeline_UpForFees(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Pipeline_UpForFees.
    /// </summary>
    public string GetStageTransition_Pipeline_UpForFees(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Pipeline_UpForFees(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Back_To_Credit_Goto(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Back_To_Credit_Goto(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Back_To_Credit_Goto(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_SystemBackToCredit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        List<string> dys = new List<string>();
        dys.Add("Branch Consultant D");
        dys.Add("Branch Admin D");
        dys.Add("Branch Manager D");
        dys.Add("New Business Processor D");
        dys.Add("New Business Manager D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        return true;
    }

    public bool OnCompleteActivity_SystemBackToCredit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_SystemBackToCredit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Perform_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Perform_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();  
        common.IsValuationInProgress(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey);
        return true;
    }

    /// <summary>
    ///  Perform Valuation.
    /// </summary>
    public string GetStageTransition_Perform_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Perform Valuation");
    }

    public string GetActivityMessage_Perform_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_FL_Return_Common(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_FL_Return_Common(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_FL_Return_Common(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Disbursement Timer.
    /// </summary>
    public string GetStageTransition_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for Disbursement Timer.
    /// </summary>
    public DateTime GetActivityTime_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return (DateTime.Now.AddHours(6));
    }

    public bool OnStartActivity_Valuations_Request(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Valuations_Request(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Valuations_Request(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Valuations_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Valuations_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Valuations_Review(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Create_EWork_PipelineCase(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        appMan.AddDetailTypeInstructionSent(messages, Application_Management_Data.ApplicationKey);
        string EFID = Application_Management_Data.EWorkFolderID;
        IX2ReturnData ret = null;
        bool b = false;
        if (string.IsNullOrEmpty(EFID))
        {
            string EFolderID = "";
            b = appMan.CreateEWorkPipelineCase(messages, Application_Management_Data.ApplicationKey, out EFolderID);
            Application_Management_Data.EWorkFolderID = EFolderID;
            return b;
        }
        else
        {
            return common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2REINSTRUCTED, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName);
           
        }
        return true;
    }

    public bool OnCompleteActivity_Create_EWork_PipelineCase(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Create EWork PipelineCase.
    /// </summary>
    public string GetStageTransition_Create_EWork_PipelineCase(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Create_EWork_PipelineCase(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 4, -1);
        bool b = false;

        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        b = appMan.SendEmailToConsultantForQuery(messages, Application_Management_Data.ApplicationKey, InstanceData.InstanceID, 8);
         
        // EWOrk
        appMan.NTUCase(messages, Application_Management_Data.ApplicationKey);
        return b;
    }

    public string GetStageTransition_EXT_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_EXT_NTU(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Continue_with_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Continue_with_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Continue_with_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Reinstate_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Reinstate_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Reinstate_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Archive_Completed_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Archive_Completed_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Archive_Completed_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for Archive Completed Followup.
    /// </summary>
    public DateTime GetActivityTime_Archive_Completed_Followup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //return (DateTime.Now.AddMinutes(15));
        return (DateTime.Now.AddDays(10));
    }

    public bool OnStartActivity_FL_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_FL_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_FL_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("FL Rework Application");
    }

    public string GetActivityMessage_FL_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_DataStore_FL_Doc_Recieved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_DataStore_FL_Doc_Recieved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  EXT DataStore FL Doc Recieved.
    /// </summary>
    public string GetStageTransition_EXT_DataStore_FL_Doc_Recieved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_EXT_DataStore_FL_Doc_Recieved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Rapid_Return(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (Application_Management_Data.IsFL)
        {
            bool b = false;
            if (Application_Management_Data.OfferTypeKey == 2)
                b = true;
            return b;
        }
        return false;
    }

    public bool OnCompleteActivity_Rapid_Return(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Rapid_Return(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Retry_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Retry_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {

        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        appMan.AddDetailTypeInstructionSent(messages,Application_Management_Data.ApplicationKey);
        string EFID = Application_Management_Data.EWorkFolderID;
        IX2ReturnData ret = null;
        bool b = false;
        if (string.IsNullOrEmpty(EFID))
        {
            string EFolderID = "";
            b = appMan.CreateEWorkPipelineCase(messages,Application_Management_Data.ApplicationKey, out EFolderID);
            Application_Management_Data.EWorkFolderID = EFolderID;
            return b;
        }
        else
        {
            return common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2REINSTRUCTED, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName);
        }
        return true;
    }

    /// <summary>
    ///  Retry Instruction.
    /// </summary>
    public string GetStageTransition_Retry_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Retry Instruction");
    }

    public string GetActivityMessage_Retry_Instruction(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Instruction_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Instruction_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Instruction_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Pipeline_Case_Create_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Pipeline_Case_Create_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Pipeline_Case_Create_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_Reinstate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_Reinstate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.HandleAppManRolesOnReturnFromNTUTOPrevState(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, Application_Management_Data.PreviousState, Application_Management_Data.IsFL, Application_Management_Data.AppCapIID, SAHL.Common.Constants.WorkFlowProcessName.Origination);
        return true;
    }

    public string GetStageTransition_EXT_Reinstate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_EXT_Reinstate(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_NTU_Final(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_NTU_Final(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_EXT_NTU_Final(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_EXT_NTU_Final(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Assign_Admin(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Assign_Admin(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Assign_Admin(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_ValuationComplete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_ValuationComplete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IValuations val = DomainServiceLoader.Instance.Get<IValuations>();
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        try
        {            
            appMan.SendEmailToConsultantForValuationDone(messages,Application_Management_Data.ApplicationKey);
        }
        catch (Exception)
        {
        }
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_ValuationComplete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_RaiseOnComplete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_RaiseOnComplete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_RaiseOnComplete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.ArchiveV3ITCForApplication(messages,Application_Management_Data.ApplicationKey);
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 1, -1);
        common.CreateNewRevision(messages, Application_Management_Data.ApplicationKey);
        List<string> dys = new List<string>();
        dys.Add("Branch Consultant D");
        dys.Add("Branch Admin D");
        dys.Add("Branch Manager D");
        dys.Add("New Business Processor D");
        dys.Add("New Business Supervisor D");
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        if (!Application_Management_Data.IsFL)
        {
            // reactiveate the NBP users.
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "Manage Application", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.NewBusinessProcessor);
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "Manage Application", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }
        return true;
    }

    public string GetStageTransition_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Return to Manage Application");
    }

    public string GetActivityMessage_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_ArchiveFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_ArchiveFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_EXT_ArchiveFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_ArchiveValuationClone(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_ArchiveValuationClone(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_EXT_ArchiveValuationClone(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_CommonArchiveMain(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_CommonArchiveMain(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_EXT_CommonArchiveMain(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Rapid_Submit_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Rapid_Submit_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Rapid_Submit_to_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Readvance_Post_Credit_Appro(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Readvance_Post_Credit_Appro(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Readvance_Post_Credit_Appro(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_NTU_PipeLine(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_NTU_PipeLine(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        bool b;
        string EFID = Application_Management_Data.EWorkFolderID;
        IFL FL = DomainServiceLoader.Instance.Get<IFL>();
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();  
   
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();  
        if (Application_Management_Data.IsFL)
        {
            FL.InitialFLNTU(messages, Params.ADUserName, Application_Management_Data.ApplicationKey, InstanceData.InstanceID);
        }
        // tell the user. if it doesnt work then dont perform ntu in ework and roll the case back

        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 4, -1);
                       
        b = appMan.SendEmailToConsultantForQuery(messages, Application_Management_Data.ApplicationKey, InstanceData.InstanceID, 4);

        
        if (!string.IsNullOrEmpty(EFID))
        {
            string AssignedTo = string.Empty;
            AssignedTo = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
            b = common.PerformEWorkAction(messages,EFID, SAHL.Common.Constants.EworkActionNames.X2NTUAdvise, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName);
            
        }

        appMan.NTUCase(messages, Application_Management_Data.ApplicationKey);
        return b;
    }

    /// <summary>
    ///  NTU PipeLine.
    /// </summary>
    public string GetStageTransition_NTU_PipeLine(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("NTU PipeLine");
    }

    public string GetActivityMessage_NTU_PipeLine(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Held_Over(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Held_Over(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        string EFID = Application_Management_Data.EWorkFolderID;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        return common.PerformEWorkAction(messages,EFID, SAHL.Common.Constants.EworkActionNames.X2HOLDOVER, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName); 
    }

    /// <summary>
    ///  Held Over.
    /// </summary>
    public string GetStageTransition_Held_Over(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Held Over");
    }

    public string GetActivityMessage_Held_Over(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_MoveSwitch(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        bool b = false;
        if (Application_Management_Data.OfferTypeKey == 6 || Application_Management_Data.OfferTypeKey == 8)
            b = true;
        return b;
    }

    public bool OnCompleteActivity_MoveSwitch(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.PricingForRisk(messages, Application_Management_Data.ApplicationKey);

        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_MoveSwitch(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Return_Processor_Readvance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        // List<string> dys = new List<string>();
        //  dys.Add("Branch Consultant D");
        //  dys.Add("Branch Admin D");
        //  dys.Add("Branch Manager D");
        //  dys.Add("New Business Processor D");
        //  dys.Add("FL Processor D");
        //  dys.Add("FL Supervisor D");
        //  dys.Add("FL Manager D");
        //  WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
        // if (!Application_Management_Data.IsFL)
        // {
        //  // reactiveate the NBP users.
        //  WorkflowAssignment.ReactiveUserOrRoundRobin(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "Manage Application", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        // }
        // else
        // {
        //  WorkflowAssignment.ReactiveUserOrRoundRobin(messages, "FL Processor D", Application_Management_Data.ApplicationKey, 157, InstanceData.InstanceID, "Manage Application", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        // }
        return true;
    }

    public bool OnCompleteActivity_Return_Processor_Readvance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        // if (!Application_Management_Data.IsFL)
        // {
        //  // reactiveate the NBP users.
        //  WorkflowAssignment.ReactiveUserOrRoundRobin(messages, "New Business Processor D", Application_Management_Data.ApplicationKey, 106, InstanceData.InstanceID, "Manange Application", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        // }
        // else
        // {
        //  Console.WriteLine("Further Info Request: - do the FL roles");
        // }
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Return_Processor_Readvance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Pipeline_Relodge(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Pipeline_Relodge(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Pipeline_Relodge(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Ext_Held_Over(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Ext_Held_Over(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Ext_Held_Over(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Ext_Held_Over(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_Disburse(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_Disburse(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  EXT Disburse.
    /// </summary>
    public string GetStageTransition_EXT_Disburse(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_EXT_Disburse(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Ext_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Ext_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Ext_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Ext_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Reinstruct_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Reinstruct_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {       
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        return appMan.CheckInstructAttorneyRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
    }

    public string GetStageTransition_Reinstruct_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Reinstruct Attorney");
    }

    public string GetActivityMessage_Reinstruct_Attorney(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Force_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Force_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Force Disbursement Timer.
    /// </summary>
    public string GetStageTransition_Force_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Force Disbursement Timer");
    }

    public string GetActivityMessage_Force_Disbursement_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Account_Create_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Account_Create_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Account_Create_Failed(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Create_Account_For_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Create_Account_For_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.CreateAccountForApplication(messages, Application_Management_Data.ApplicationKey, Params.ADUserName);
        return false;
    }

    public string GetStageTransition_Create_Account_For_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Create Account For Application");
    }

    public string GetActivityMessage_Create_Account_For_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Reload_CaseName(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Reload_CaseName(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        string Name = string.Empty;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        InstanceData.Subject = common.GetCaseName(messages, Application_Management_Data.ApplicationKey); ;
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Reload_CaseName(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_SplitVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_SplitVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_SplitVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnSplitReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_OnSplitReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnSplitReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Further_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Further_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Further_Valuation_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnReturnFurtherVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_OnReturnFurtherVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnReturnFurtherVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnReturnReviewVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_OnReturnReviewVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnReturnReviewVal(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Valuation_Review_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Valuation_Review_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Valuation_Review_Required(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnStuck(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_OnStuck(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnStuck(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Query_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Query_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Query_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return ("Query Complete");
    }

    public string GetActivityMessage_Query_Complete(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Decline_By_Credit_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Decline_By_Credit_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Decline_By_Credit_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Decline_By_Credit_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public DateTime GetActivityTime_Decline_By_Credit_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return (DateTime.Now.AddDays(30));
    }

    public bool OnStartActivity_Branch_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Branch_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Branch_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Branch_Rework_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnCreateFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        string s = string.Empty;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.GetCaseName(messages, Application_Management_Data.ApplicationKey);
        InstanceData.Subject = s;
        InstanceData.Name = Application_Management_Data.ApplicationKey.ToString();
        return true;
    }

    public bool OnCompleteActivity_OnCreateFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.CloneActiveSecurityFromInstanceForInstance(messages, InstanceData.ParentInstanceID, InstanceData.InstanceID);
        WorkflowAssignment.ReActivateBranchUsersForOrigination(messages, InstanceData.InstanceID, Application_Management_Data.AppCapIID, Application_Management_Data.ApplicationKey, "OnCreateFollowup", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnCreateFollowup(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnFollowupReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_OnFollowupReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnFollowupReturn(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_ReAssign_Commission_Consultant(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_ReAssign_Commission_Consultant(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_ReAssign_Commission_Consultant(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (null != Params.Data)
            return Params.Data.ToString();
        return string.Empty;
    }

    public string GetActivityMessage_ReAssign_Commission_Consultant(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_90_Day_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_90_Day_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        appMan.ActivateNTUFromWatchdogTime(messages, InstanceData.InstanceID);
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_90_Day_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public DateTime GetActivityTime_90_Day_Timer(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //return (DateTime.Now.AddMinutes(9));
        return (DateTime.Now.AddDays(90));
    }

    public bool OnStartActivity_Stale_App(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Stale_App(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        string EFID = Application_Management_Data.EWorkFolderID;
        IFL FL = DomainServiceLoader.Instance.Get<IFL>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();  
        if (Application_Management_Data.IsFL)
        {
            FL.InitialFLNTU(messages, Params.ADUserName, Application_Management_Data.ApplicationKey, InstanceData.InstanceID);
        }
        // tell the user. if it doesnt work then dont perform ntu in ework and roll the case back

        common.UpdateOfferStatus(messages, Application_Management_Data.ApplicationKey, 4, -1);
        
        bool b = false;

        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        b = appMan.SendEmailToConsultantForQuery(messages, Application_Management_Data.ApplicationKey, InstanceData.InstanceID, 4);

        if (!b)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(EFID))
        {
            IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
            string AssignedTo = string.Empty;
            AssignedTo = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
            b = common.PerformEWorkAction(messages, EFID, SAHL.Common.Constants.EworkActionNames.X2NTUAdvise, Application_Management_Data.ApplicationKey, Params.ADUserName,InstanceData.StateName);
            
        }

        appMan.NTUCase(messages, Application_Management_Data.ApplicationKey);
        return b;
    }

    public string GetStageTransition_Stale_App(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Stale_App(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Create_Instance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Create_Instance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        IApplicationManagement appMan = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();  
        string Name = string.Empty;
        common.GetCaseName(messages, Application_Management_Data.ApplicationKey);
        InstanceData.Subject = Name;
        InstanceData.Name = Application_Management_Data.ApplicationKey.ToString();
        Application_Management_Data.IsResub = false;
        int tok = common.GetOfferType(messages, Application_Management_Data.ApplicationKey);
        Application_Management_Data.OfferTypeKey = tok;
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Create_Instance(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Request_Lightstone_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Request_Lightstone_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.DoLightStoneValuationForWorkFlow(messages,Application_Management_Data.ApplicationKey, Params.ADUserName);
        return true;
    }

    public string GetStageTransition_Request_Lightstone_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Request_Lightstone_Valuation(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Submit_To_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (Application_Management_Data.IsFL)
            return false;
        return true;
    }

    public bool OnCompleteActivity_Submit_To_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Submit_To_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Submit_To_Credit(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Credit_Score_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return false;
    }

    public bool OnCompleteActivity_Credit_Score_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Credit_Score_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Credit_Score_Decline(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Continue_Request_Resolved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Continue_Request_Resolved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        var applicationCapture = DomainServiceLoader.Instance.Get<IApplicationCapture>();
        var workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        bool hasPassedRules = applicationCapture.CheckBranchSubmitApplicationRules(messages, Application_Management_Data.ApplicationKey, Params.IgnoreWarning);
        if (!Application_Management_Data.IsFL)
        {
            List<string> dys = new List<string>();
            dys.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD);
            dys.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD);
            dys.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchManagerD);
            workflowAssignment.DeActiveUsersForInstance(messages,InstanceData.InstanceID, Application_Management_Data.ApplicationKey, dys);
            // Assign the QA types bak
            List<int> OSKeys = new List<int>();
            OSKeys.Add(1007);
            OSKeys.Add(1008);
            string user = string.Empty;
            workflowAssignment.ReactiveUserOrRoundRobinForOSKeysByProcess(messages,"QA Administrator D", Application_Management_Data.ApplicationKey, OSKeys, InstanceData.InstanceID, "Request Resolved", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.QAAdministrator);
        }
        return hasPassedRules;
    }

    public string GetStageTransition_Continue_Request_Resolved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Continue_Request_Resolved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_QAIsFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (Application_Management_Data.IsFL)
            return true;
        return false;
    }

    public bool OnCompleteActivity_QAIsFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_QAIsFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_QAIsFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_CredIsFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (Application_Management_Data.IsFL)
            return true;
        return false;
    }

    public bool OnCompleteActivity_CredIsFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_CredIsFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_CredIsFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Revert_to_Request_at_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Revert_to_Request_at_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Revert_to_Request_at_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Revert_to_Request_at_QA(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Revert_to_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Revert_to_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Revert_to_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Revert_to_Resubmission(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Revert_to_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Revert_to_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Revert_to_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Revert_to_Manage_Application(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Return_from_Opt_Out_Super_Lo(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Return_from_Opt_Out_Super_Lo(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Return_from_Opt_Out_Super_Lo(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_CredNotFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_CredNotFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_CredNotFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Return_Credit_Pre_Approved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        //ensure that valuations are required before submitting back to credit
        Application_Management_Data.RequireValuation = true;

        return true;
    }

    public bool OnCompleteActivity_Return_Credit_Pre_Approved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Return_Credit_Pre_Approved(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_isFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        if (Application_Management_Data.IsFL)
            return true;
        return false;
    }

    public bool OnCompleteActivity_isFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_isFL(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        return string.Empty;
    }

    #endregion Activities

    #region Roles

    public string OnGetRole_Application_Management_Branch_Consultant_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Consultant D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_New_Business_Processor_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "New Business Processor D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Registrations_LOA_Admin_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Registrations LOA Admin D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Registrations_Administrator_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Registrations Administrator D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Credit_Underwriter_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Credit Underwriter D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_FL_Processor_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "FL Processor D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_FL_Collections_Admin_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "FL Collections D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Resubmission_Admin_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Resubmission Admin D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Registrations_Supervisor_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Registrations Supervisor D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Registrations_Manager_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Registrations Manager D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Branch_Admin_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Admin D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Translate_Conditions_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Translate Conditions D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_New_Business_Supervisor_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "New Business Supervisor D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_FL_Manager_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "FL Manager D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Credit_Manager_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Credit Manager D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Credit_Supervisor_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Credit Supervisor D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_FL_Supervisor_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "FL Supervisor D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_QA_Administrator_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "QA Administrator D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Application_Management_Branch_Manager_D(IDomainMessageCollection messages, IX2Application_Management_Data Application_Management_Data, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = WorkflowAssignment.ResolveDynamicRoleToUserName(messages, "Branch Manager D", InstanceData.InstanceID);
        return s;
    }

    #endregion Roles

    #region IX2WorkFlow Members

    public bool OnEnterState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnEnterState", WorkFlowData, InstanceData, Params, null);
        try
        {
            bool returnData = this.OnEnterStateInternal(messages, WorkFlowData, InstanceData, Params);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnEnterState", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnEnterState", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnEnterState", InstanceData.InstanceID);
        }
    }

    public bool OnEnterStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        switch (Params.StateName)
        {
            case "Manage Application":
                return OnEnter_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit":
                return OnEnter_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA":
                return OnEnter_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Resubmission":
                return OnEnter_Common_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resubmission":
                return OnEnter_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "System Assign Processor":
                return OnEnter_System_Assign_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QA":
                return OnEnter_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Request at QA":
                return OnEnter_Request_at_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Issue AIP":
                return OnEnter_Issue_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Next Step":
                return OnEnter_Next_Step(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive AIP":
                return OnEnter_Archive_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Query":
                return OnEnter_Application_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Signed LOA Review":
                return OnEnter_Signed_LOA_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Check":
                return OnEnter_Application_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement":
                return OnEnter_Disbursement(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursed":
                return OnEnter_Disbursed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Attorney Check":
                return OnEnter_Attorney_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Awaiting Application":
                return OnEnter_Awaiting_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Arrears":
                return OnEnter_Arrears(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive QA Query":
                return OnEnter_Archive_QA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assign at QA":
                return OnEnter_Assign_at_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive No App Form":
                return OnEnter_Archive_No_App_Form(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA Query":
                return OnEnter_LOA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Exp LOA":
                return OnEnter_Archive_Exp_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Review":
                return OnEnter_Disbursement_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Translate ":
                return OnEnter_Common_Translate_(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Translate Conditions":
                return OnEnter_Translate_Conditions(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Translate":
                return OnEnter_Archive_Translate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "App Check":
                return OnEnter_App_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Resend Instruction":
                return OnEnter_Common_Resend_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Reassign":
                return OnEnter_Common_Reassign(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common 2nd Valuation":
                return OnEnter_Common_2nd_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Valuation Review":
                return OnEnter_Common_Valuation_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Rework Application":
                return OnEnter_Common_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Declined by Credit":
                return OnEnter_Declined_by_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Info Request":
                return OnEnter_Further_Info_Request(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Back to Credit":
                return OnEnter_Back_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disputes":
                return OnEnter_Disputes(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Bin":
                return OnEnter_Decline_Bin(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common NTU":
                return OnEnter_Common_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Decline":
                return OnEnter_Common_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return OnEnter_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return OnEnter_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return to sender":
                return OnEnter_Return_to_sender(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Decline":
                return OnEnter_Archive_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive NTU":
                return OnEnter_Archive_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Multiple Applications":
                return OnEnter_Multiple_Applications(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Hold Application":
                return OnEnter_Common_Hold_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Hold":
                return OnEnter_Application_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ContinueLoan":
                return OnEnter_ContinueLoan(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Followup":
                return OnEnter_Common_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Followup Hold":
                return OnEnter_Followup_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Followup Complete":
                return OnEnter_Followup_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Valuation Complete":
                return OnEnter_Archive_Valuation_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Registration Pipeline":
                return OnEnter_Registration_Pipeline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive FL":
                return OnEnter_Archive_FL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Origination":
                return OnEnter_Archive_Origination(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CheckQCAndSendToPipeline":
                return OnEnter_CheckQCAndSendToPipeline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Ready To Followup":
                return OnEnter_Ready_To_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common FL Rework Application":
                return OnEnter_Common_FL_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Instruct Failed":
                return OnEnter_Instruct_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Assign Admin":
                return OnEnter_Common_Assign_Admin(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "System Val Complete":
                return OnEnter_System_Val_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Return Processor":
                return OnEnter_Common_Return_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Relodge":
                return OnEnter_Common_Relodge(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Archive Followup":
                return OnEnter_Common_Archive_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Manual Archive":
                return OnEnter_Manual_Archive(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CommonArchiveMain":
                return OnEnter_CommonArchiveMain(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Hold":
                return OnEnter_Valuation_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Send to FL Hold":
                return OnEnter_Send_to_FL_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Hold":
                return OnEnter_Rapid_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Hold":
                return OnEnter_Credit_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Back To Credit Hold":
                return OnEnter_Back_To_Credit_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QuickCash Hold":
                return OnEnter_QuickCash_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Valuation Required":
                return OnEnter_Further_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Review Required":
                return OnEnter_Valuation_Review_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Post Credit Hold":
                return OnEnter_Rapid_Post_Credit_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "PipeLine NTU":
                return OnEnter_PipeLine_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resub Application Check":
                return OnEnter_Resub_Application_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Account Create Fail Hold":
                return OnEnter_Account_Create_Fail_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Reload Case Name":
                return OnEnter_Common_Reload_Case_Name(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "RetManageApp":
                return OnEnter_RetManageApp(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Check Val":
                return OnEnter_Check_Val(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "SysFurtherVal":
                return OnEnter_SysFurtherVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ReturnToSender":
                return OnEnter_ReturnToSender(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "SysReviewVal":
                return OnEnter_SysReviewVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common OnStuck":
                return OnEnter_Common_OnStuck(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Branch Rework ":
                return OnEnter_Common_Branch_Rework_(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "State99":
                return OnEnter_State99(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "FollowupReturn":
                return OnEnter_FollowupReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Reassign Commission":
                return OnEnter_Common_Reassign_Commission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Processor":
                return OnEnter_Archive_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Lightstone AVM":
                return OnEnter_Common_Lightstone_AVM(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "View Credit Score":
                return OnEnter_View_Credit_Score(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Credit Score Decline":
                return OnEnter_Common_Credit_Score_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Review Credit Score":
                return OnEnter_Review_Credit_Score(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Score Checks":
                return OnEnter_Credit_Score_Checks(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            default:
                return false;
        }
    }

    public bool OnExitState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnExitState", WorkFlowData, InstanceData, Params, null);
        try
        {
            bool returnData = this.OnExitStateInternal(messages, WorkFlowData, InstanceData, Params);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnExitState", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnExitState", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnExitState", InstanceData.InstanceID);
        }
    }

    public bool OnExitStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        switch (Params.StateName)
        {
            case "Manage Application":
                return OnExit_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit":
                return OnExit_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA":
                return OnExit_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Resubmission":
                return OnExit_Common_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resubmission":
                return OnExit_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "System Assign Processor":
                return OnExit_System_Assign_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QA":
                return OnExit_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Request at QA":
                return OnExit_Request_at_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Issue AIP":
                return OnExit_Issue_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Next Step":
                return OnExit_Next_Step(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive AIP":
                return OnExit_Archive_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Query":
                return OnExit_Application_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Signed LOA Review":
                return OnExit_Signed_LOA_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Check":
                return OnExit_Application_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement":
                return OnExit_Disbursement(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursed":
                return OnExit_Disbursed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Attorney Check":
                return OnExit_Attorney_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Awaiting Application":
                return OnExit_Awaiting_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Arrears":
                return OnExit_Arrears(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive QA Query":
                return OnExit_Archive_QA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assign at QA":
                return OnExit_Assign_at_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive No App Form":
                return OnExit_Archive_No_App_Form(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA Query":
                return OnExit_LOA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Exp LOA":
                return OnExit_Archive_Exp_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Review":
                return OnExit_Disbursement_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Translate ":
                return OnExit_Common_Translate_(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Translate Conditions":
                return OnExit_Translate_Conditions(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Translate":
                return OnExit_Archive_Translate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "App Check":
                return OnExit_App_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Resend Instruction":
                return OnExit_Common_Resend_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Reassign":
                return OnExit_Common_Reassign(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common 2nd Valuation":
                return OnExit_Common_2nd_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Valuation Review":
                return OnExit_Common_Valuation_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Rework Application":
                return OnExit_Common_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Declined by Credit":
                return OnExit_Declined_by_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Info Request":
                return OnExit_Further_Info_Request(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Back to Credit":
                return OnExit_Back_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disputes":
                return OnExit_Disputes(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Bin":
                return OnExit_Decline_Bin(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common NTU":
                return OnExit_Common_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Decline":
                return OnExit_Common_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return OnExit_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return OnExit_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return to sender":
                return OnExit_Return_to_sender(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Decline":
                return OnExit_Archive_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive NTU":
                return OnExit_Archive_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Multiple Applications":
                return OnExit_Multiple_Applications(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Hold Application":
                return OnExit_Common_Hold_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Hold":
                return OnExit_Application_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ContinueLoan":
                return OnExit_ContinueLoan(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Followup":
                return OnExit_Common_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Followup Hold":
                return OnExit_Followup_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Followup Complete":
                return OnExit_Followup_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Valuation Complete":
                return OnExit_Archive_Valuation_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Registration Pipeline":
                return OnExit_Registration_Pipeline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive FL":
                return OnExit_Archive_FL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Origination":
                return OnExit_Archive_Origination(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CheckQCAndSendToPipeline":
                return OnExit_CheckQCAndSendToPipeline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Ready To Followup":
                return OnExit_Ready_To_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common FL Rework Application":
                return OnExit_Common_FL_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Instruct Failed":
                return OnExit_Instruct_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Assign Admin":
                return OnExit_Common_Assign_Admin(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "System Val Complete":
                return OnExit_System_Val_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Return Processor":
                return OnExit_Common_Return_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Relodge":
                return OnExit_Common_Relodge(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Archive Followup":
                return OnExit_Common_Archive_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Manual Archive":
                return OnExit_Manual_Archive(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CommonArchiveMain":
                return OnExit_CommonArchiveMain(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Hold":
                return OnExit_Valuation_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Send to FL Hold":
                return OnExit_Send_to_FL_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Hold":
                return OnExit_Rapid_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Hold":
                return OnExit_Credit_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Back To Credit Hold":
                return OnExit_Back_To_Credit_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QuickCash Hold":
                return OnExit_QuickCash_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Valuation Required":
                return OnExit_Further_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Review Required":
                return OnExit_Valuation_Review_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Post Credit Hold":
                return OnExit_Rapid_Post_Credit_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "PipeLine NTU":
                return OnExit_PipeLine_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resub Application Check":
                return OnExit_Resub_Application_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Account Create Fail Hold":
                return OnExit_Account_Create_Fail_Hold(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Reload Case Name":
                return OnExit_Common_Reload_Case_Name(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "RetManageApp":
                return OnExit_RetManageApp(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Check Val":
                return OnExit_Check_Val(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "SysFurtherVal":
                return OnExit_SysFurtherVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ReturnToSender":
                return OnExit_ReturnToSender(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "SysReviewVal":
                return OnExit_SysReviewVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common OnStuck":
                return OnExit_Common_OnStuck(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Branch Rework ":
                return OnExit_Common_Branch_Rework_(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "State99":
                return OnExit_State99(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "FollowupReturn":
                return OnExit_FollowupReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Reassign Commission":
                return OnExit_Common_Reassign_Commission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Processor":
                return OnExit_Archive_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Lightstone AVM":
                return OnExit_Common_Lightstone_AVM(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "View Credit Score":
                return OnExit_View_Credit_Score(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Common Credit Score Decline":
                return OnExit_Common_Credit_Score_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Review Credit Score":
                return OnExit_Review_Credit_Score(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Score Checks":
                return OnExit_Credit_Score_Checks(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            default:
                return false;
        }
    }

    public bool OnReturnState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnReturnState", WorkFlowData, InstanceData, Params, null);
        try
        {
            bool returnData = this.OnReturnStateInternal(messages, WorkFlowData, InstanceData, Params);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnReturnState", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnReturnState", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnReturnState", InstanceData.InstanceID);
        }
    }

    public bool OnReturnStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        switch (Params.StateName)
        {
            case "Archive AIP":
                return OnReturn_Archive_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive QA Query":
                return OnReturn_Archive_QA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive No App Form":
                return OnReturn_Archive_No_App_Form(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Exp LOA":
                return OnReturn_Archive_Exp_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Translate":
                return OnReturn_Archive_Translate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Bin":
                return OnReturn_Decline_Bin(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Decline":
                return OnReturn_Archive_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive NTU":
                return OnReturn_Archive_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Followup Complete":
                return OnReturn_Followup_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Valuation Complete":
                return OnReturn_Archive_Valuation_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive FL":
                return OnReturn_Archive_FL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Origination":
                return OnReturn_Archive_Origination(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Manual Archive":
                return OnReturn_Manual_Archive(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Processor":
                return OnReturn_Archive_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            default: return false;
        }
    }

    public string GetForwardStateName(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetForwardStateName", WorkFlowData, InstanceData, Params, null);
        try
        {
            string returnData = this.GetForwardStateNameInternal(messages, WorkFlowData, InstanceData, Params);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetForwardStateName", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetForwardStateName", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetForwardStateName", InstanceData.InstanceID);
        }
    }

    public string GetForwardStateNameInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        switch (Params.StateName)
        {
            case "Return to sender":
                return GetForwardStateName_Return_to_sender(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ContinueLoan":
                return GetForwardStateName_ContinueLoan(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "RetManageApp":
                return GetForwardStateName_RetManageApp(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ReturnToSender":
                return GetForwardStateName_ReturnToSender(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "FollowupReturn":
                return GetForwardStateName_FollowupReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            default:
                return string.Empty;
        }
    }

    public bool OnStartActivity(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnStartActivity", WorkFlowData, InstanceData, Params, null);
        try
        {
            bool returnData = this.OnStartActivityInternal(messages, WorkFlowData, InstanceData, Params);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnStartActivity", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnStartActivity", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnStartActivity", InstanceData.InstanceID);
        }
    }

    public bool OnStartActivityInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        switch (Params.ActivityName)
        {
            case "Proceed with Application":
                return OnStartActivity_Proceed_with_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application in Order":
                return OnStartActivity_Application_in_Order(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Send LOA":
                return OnStartActivity_Send_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QA Query":
                return OnStartActivity_QA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Request Resolved":
                return OnStartActivity_Request_Resolved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QA Complete":
                return OnStartActivity_QA_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "New Purchase":
                return OnStartActivity_New_Purchase(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Client Accepts":
                return OnStartActivity_Client_Accepts(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Client Refuse":
                return OnStartActivity_Client_Refuse(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "1 month timer":
                return OnStartActivity_1_month_timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Other Types":
                return OnStartActivity_Other_Types(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query on Application":
                return OnStartActivity_Query_on_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Feedback on Query":
                return OnStartActivity_Feedback_on_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resubmit to Credit":
                return OnStartActivity_Resubmit_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA Received":
                return OnStartActivity_LOA_Received(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA Accepted":
                return OnStartActivity_LOA_Accepted(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Instruct Attorney":
                return OnStartActivity_Instruct_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Review Disbursement Setup":
                return OnStartActivity_Review_Disbursement_Setup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rollback Disbursement":
                return OnStartActivity_Rollback_Disbursement(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resubmit":
                return OnStartActivity_Resubmit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AttAssigned":
                return OnStartActivity_AttAssigned(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Received":
                return OnStartActivity_Application_Received(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Note Comment":
                return OnStartActivity_Note_Comment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Readvance":
                return OnStartActivity_Rapid_Readvance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "2 Months":
                return OnStartActivity_2_Months(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assigned QA":
                return OnStartActivity_Assigned_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "45 days":
                return OnStartActivity_45_days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Send AIP":
                return OnStartActivity_Send_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query on LOA":
                return OnStartActivity_Query_on_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "30 Days":
                return OnStartActivity_30_Days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resend LOA":
                return OnStartActivity_Resend_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Require Arrear Comment":
                return OnStartActivity_Require_Arrear_Comment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Override Check":
                return OnStartActivity_Override_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation in Order":
                return OnStartActivity_Valuation_in_Order(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Resubmission?":
                return OnStartActivity_Valuation_Resubmission_(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return Processor Credit":
                return OnStartActivity_Return_Processor_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return with Approve or Offer":
                return OnStartActivity_Return_with_Approve_or_Offer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Return Resubmission":
                return OnStartActivity_Credit_Return_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Return Resub Approve":
                return OnStartActivity_Credit_Return_Resub_Approve(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Translation Complete":
                return OnStartActivity_Translation_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Misc Condition":
                return OnStartActivity_Misc_Condition(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Select Attorney":
                return OnStartActivity_Select_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Att not assigned":
                return OnStartActivity_Att_not_assigned(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Workflow":
                return OnStartActivity_Valuation_Workflow(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit":
                return OnStartActivity_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return ReAdv Payment FL":
                return OnStartActivity_Return_ReAdv_Payment_FL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "New Business":
                return OnStartActivity_New_Business(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Lending":
                return OnStartActivity_Further_Lending(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Payments":
                return OnStartActivity_Readvance_Payments(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Payment":
                return OnStartActivity_Readvance_Payment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assign Processor":
                return OnStartActivity_Assign_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AutoValuation":
                return OnStartActivity_AutoValuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "GotoCredit":
                return OnStartActivity_GotoCredit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AutoGotoQC":
                return OnStartActivity_AutoGotoQC(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resend Instruction":
                return OnStartActivity_Resend_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reassign User":
                return OnStartActivity_Reassign_User(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Perform Further Valuation":
                return OnStartActivity_Perform_Further_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Review Valuation Required":
                return OnStartActivity_Review_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rework Application":
                return OnStartActivity_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Info Request Complete":
                return OnStartActivity_Info_Request_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Final":
                return OnStartActivity_Decline_Final(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Motivate":
                return OnStartActivity_Motivate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Dispute Finalised":
                return OnStartActivity_Dispute_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Decline Application":
                return OnStartActivity_Credit_Decline_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Further Info":
                return OnStartActivity_Credit_Further_Info(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Dispute":
                return OnStartActivity_Credit_Dispute(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU Timeout":
                return OnStartActivity_NTU_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Timeout":
                return OnStartActivity_Decline_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate NTU":
                return OnStartActivity_Reinstate_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Decline":
                return OnStartActivity_Reinstate_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return OnStartActivity_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return OnStartActivity_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Incorrect":
                return OnStartActivity_Disbursement_Incorrect(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Lending Calc":
                return OnStartActivity_Further_Lending_Calc(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Apps in prog of higher pri":
                return OnStartActivity_Apps_in_prog_of_higher_pri(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Hold Application":
                return OnStartActivity_EXT_Hold_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Reactivate App":
                return OnStartActivity_EXT_Reactivate_App(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Highest Priority":
                return OnStartActivity_Highest_Priority(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Complete Followup":
                return OnStartActivity_Complete_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Update Followup":
                return OnStartActivity_Update_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create Followup":
                return OnStartActivity_Create_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowup":
                return OnStartActivity_OnFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU Finalised":
                return OnStartActivity_NTU_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Finalised":
                return OnStartActivity_Decline_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Pipeline_UpForFees":
                return OnStartActivity_Pipeline_UpForFees(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Back To Credit Goto":
                return OnStartActivity_Back_To_Credit_Goto(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "SystemBackToCredit":
                return OnStartActivity_SystemBackToCredit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Perform Valuation":
                return OnStartActivity_Perform_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "FL Return Common":
                return OnStartActivity_FL_Return_Common(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Timer":
                return OnStartActivity_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuations Request":
                return OnStartActivity_Valuations_Request(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuations Review":
                return OnStartActivity_Valuations_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create EWork PipelineCase":
                return OnStartActivity_Create_EWork_PipelineCase(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT NTU":
                return OnStartActivity_EXT_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Continue with Followup":
                return OnStartActivity_Continue_with_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Followup":
                return OnStartActivity_Reinstate_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Completed Followup":
                return OnStartActivity_Archive_Completed_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "FL Rework Application":
                return OnStartActivity_FL_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT DataStore FL Doc Recieved":
                return OnStartActivity_EXT_DataStore_FL_Doc_Recieved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Return":
                return OnStartActivity_Rapid_Return(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Retry Instruction":
                return OnStartActivity_Retry_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Instruction Failed":
                return OnStartActivity_Instruction_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Pipeline Case Create Failed":
                return OnStartActivity_Pipeline_Case_Create_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Reinstate":
                return OnStartActivity_EXT_Reinstate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT NTU Final":
                return OnStartActivity_EXT_NTU_Final(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assign Admin":
                return OnStartActivity_Assign_Admin(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ValuationComplete":
                return OnStartActivity_ValuationComplete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "RaiseOnComplete":
                return OnStartActivity_RaiseOnComplete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return to Manage Application":
                return OnStartActivity_Return_to_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT_ArchiveFollowup":
                return OnStartActivity_EXT_ArchiveFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT_ArchiveValuationClone":
                return OnStartActivity_EXT_ArchiveValuationClone(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT_CommonArchiveMain":
                return OnStartActivity_EXT_CommonArchiveMain(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Submit to Credit":
                return OnStartActivity_Rapid_Submit_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Post Credit Appro":
                return OnStartActivity_Readvance_Post_Credit_Appro(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU PipeLine":
                return OnStartActivity_NTU_PipeLine(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Held Over":
                return OnStartActivity_Held_Over(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "MoveSwitch":
                return OnStartActivity_MoveSwitch(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return Processor Readvance":
                return OnStartActivity_Return_Processor_Readvance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Pipeline Relodge":
                return OnStartActivity_Pipeline_Relodge(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Ext Held Over":
                return OnStartActivity_Ext_Held_Over(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Disburse":
                return OnStartActivity_EXT_Disburse(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Ext Complete":
                return OnStartActivity_Ext_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstruct Attorney":
                return OnStartActivity_Reinstruct_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Force Disbursement Timer":
                return OnStartActivity_Force_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Account Create Failed":
                return OnStartActivity_Account_Create_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create Account For Application":
                return OnStartActivity_Create_Account_For_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reload CaseName":
                return OnStartActivity_Reload_CaseName(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "SplitVal":
                return OnStartActivity_SplitVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnSplitReturn":
                return OnStartActivity_OnSplitReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Valuation Required":
                return OnStartActivity_Further_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnReturnFurtherVal":
                return OnStartActivity_OnReturnFurtherVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnReturnReviewVal":
                return OnStartActivity_OnReturnReviewVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Review Required":
                return OnStartActivity_Valuation_Review_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnStuck":
                return OnStartActivity_OnStuck(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query Complete":
                return OnStartActivity_Query_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline By Credit Timer":
                return OnStartActivity_Decline_By_Credit_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Branch Rework Application":
                return OnStartActivity_Branch_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnCreateFollowup":
                return OnStartActivity_OnCreateFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowupReturn":
                return OnStartActivity_OnFollowupReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ReAssign Commission Consultant":
                return OnStartActivity_ReAssign_Commission_Consultant(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "90 Day Timer":
                return OnStartActivity_90_Day_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Stale App":
                return OnStartActivity_Stale_App(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create Instance":
                return OnStartActivity_Create_Instance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Request Lightstone Valuation":
                return OnStartActivity_Request_Lightstone_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Submit To Credit":
                return OnStartActivity_Submit_To_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Score Decline":
                return OnStartActivity_Credit_Score_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Continue Request Resolved":
                return OnStartActivity_Continue_Request_Resolved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QAIsFL":
                return OnStartActivity_QAIsFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CredIsFL":
                return OnStartActivity_CredIsFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Request at QA":
                return OnStartActivity_Revert_to_Request_at_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Resubmission":
                return OnStartActivity_Revert_to_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Manage Application":
                return OnStartActivity_Revert_to_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return from Opt-Out Super Lo":
                return OnStartActivity_Return_from_Opt_Out_Super_Lo(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CredNotFL":
                return OnStartActivity_CredNotFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return Credit Pre-Approved":
                return OnStartActivity_Return_Credit_Pre_Approved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "isFL":
                return OnStartActivity_isFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            default:
                return false;
        }
    }

    public bool OnCompleteActivity(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string AlertMessage)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnCompleteActivity", WorkFlowData, InstanceData, Params, new object[] { AlertMessage });
        try
        {
            bool returnData = this.OnCompleteActivityInternal(messages, WorkFlowData, InstanceData, Params, ref AlertMessage);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnCompleteActivity", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnCompleteActivity", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnCompleteActivity", InstanceData.InstanceID);
        }
    }

    public bool OnCompleteActivityInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, ref string AlertMessage)
    {
        switch (Params.ActivityName)
        {
            case "Proceed with Application":
                return OnCompleteActivity_Proceed_with_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Application in Order":
                return OnCompleteActivity_Application_in_Order(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Send LOA":
                return OnCompleteActivity_Send_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "QA Query":
                return OnCompleteActivity_QA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Request Resolved":
                return OnCompleteActivity_Request_Resolved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "QA Complete":
                return OnCompleteActivity_QA_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "New Purchase":
                return OnCompleteActivity_New_Purchase(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Client Accepts":
                return OnCompleteActivity_Client_Accepts(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Client Refuse":
                return OnCompleteActivity_Client_Refuse(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "1 month timer":
                return OnCompleteActivity_1_month_timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Other Types":
                return OnCompleteActivity_Other_Types(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Query on Application":
                return OnCompleteActivity_Query_on_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Feedback on Query":
                return OnCompleteActivity_Feedback_on_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Resubmit to Credit":
                return OnCompleteActivity_Resubmit_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "LOA Received":
                return OnCompleteActivity_LOA_Received(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "LOA Accepted":
                return OnCompleteActivity_LOA_Accepted(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Instruct Attorney":
                return OnCompleteActivity_Instruct_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Review Disbursement Setup":
                return OnCompleteActivity_Review_Disbursement_Setup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Rollback Disbursement":
                return OnCompleteActivity_Rollback_Disbursement(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Resubmit":
                return OnCompleteActivity_Resubmit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "AttAssigned":
                return OnCompleteActivity_AttAssigned(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Application Received":
                return OnCompleteActivity_Application_Received(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Note Comment":
                return OnCompleteActivity_Note_Comment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Rapid Readvance":
                return OnCompleteActivity_Rapid_Readvance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "2 Months":
                return OnCompleteActivity_2_Months(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Assigned QA":
                return OnCompleteActivity_Assigned_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "45 days":
                return OnCompleteActivity_45_days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Send AIP":
                return OnCompleteActivity_Send_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Query on LOA":
                return OnCompleteActivity_Query_on_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "30 Days":
                return OnCompleteActivity_30_Days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Resend LOA":
                return OnCompleteActivity_Resend_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Require Arrear Comment":
                return OnCompleteActivity_Require_Arrear_Comment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Override Check":
                return OnCompleteActivity_Override_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Valuation in Order":
                return OnCompleteActivity_Valuation_in_Order(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Valuation Resubmission?":
                return OnCompleteActivity_Valuation_Resubmission_(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Return Processor Credit":
                return OnCompleteActivity_Return_Processor_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Return with Approve or Offer":
                return OnCompleteActivity_Return_with_Approve_or_Offer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Credit Return Resubmission":
                return OnCompleteActivity_Credit_Return_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Credit Return Resub Approve":
                return OnCompleteActivity_Credit_Return_Resub_Approve(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Translation Complete":
                return OnCompleteActivity_Translation_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Misc Condition":
                return OnCompleteActivity_Misc_Condition(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Select Attorney":
                return OnCompleteActivity_Select_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Att not assigned":
                return OnCompleteActivity_Att_not_assigned(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Valuation Workflow":
                return OnCompleteActivity_Valuation_Workflow(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Credit":
                return OnCompleteActivity_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Return ReAdv Payment FL":
                return OnCompleteActivity_Return_ReAdv_Payment_FL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "New Business":
                return OnCompleteActivity_New_Business(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Further Lending":
                return OnCompleteActivity_Further_Lending(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Readvance Payments":
                return OnCompleteActivity_Readvance_Payments(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Readvance Payment":
                return OnCompleteActivity_Readvance_Payment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Assign Processor":
                return OnCompleteActivity_Assign_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "AutoValuation":
                return OnCompleteActivity_AutoValuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "GotoCredit":
                return OnCompleteActivity_GotoCredit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "AutoGotoQC":
                return OnCompleteActivity_AutoGotoQC(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Resend Instruction":
                return OnCompleteActivity_Resend_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reassign User":
                return OnCompleteActivity_Reassign_User(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Perform Further Valuation":
                return OnCompleteActivity_Perform_Further_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Review Valuation Required":
                return OnCompleteActivity_Review_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Rework Application":
                return OnCompleteActivity_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Info Request Complete":
                return OnCompleteActivity_Info_Request_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline Final":
                return OnCompleteActivity_Decline_Final(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Motivate":
                return OnCompleteActivity_Motivate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Dispute Finalised":
                return OnCompleteActivity_Dispute_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Credit Decline Application":
                return OnCompleteActivity_Credit_Decline_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Credit Further Info":
                return OnCompleteActivity_Credit_Further_Info(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Credit Dispute":
                return OnCompleteActivity_Credit_Dispute(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "NTU Timeout":
                return OnCompleteActivity_NTU_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline Timeout":
                return OnCompleteActivity_Decline_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reinstate NTU":
                return OnCompleteActivity_Reinstate_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reinstate Decline":
                return OnCompleteActivity_Reinstate_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline":
                return OnCompleteActivity_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "NTU":
                return OnCompleteActivity_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Disbursement Incorrect":
                return OnCompleteActivity_Disbursement_Incorrect(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Further Lending Calc":
                return OnCompleteActivity_Further_Lending_Calc(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Apps in prog of higher pri":
                return OnCompleteActivity_Apps_in_prog_of_higher_pri(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT Hold Application":
                return OnCompleteActivity_EXT_Hold_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT Reactivate App":
                return OnCompleteActivity_EXT_Reactivate_App(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Highest Priority":
                return OnCompleteActivity_Highest_Priority(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Complete Followup":
                return OnCompleteActivity_Complete_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Update Followup":
                return OnCompleteActivity_Update_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Create Followup":
                return OnCompleteActivity_Create_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnFollowup":
                return OnCompleteActivity_OnFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "NTU Finalised":
                return OnCompleteActivity_NTU_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline Finalised":
                return OnCompleteActivity_Decline_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Pipeline_UpForFees":
                return OnCompleteActivity_Pipeline_UpForFees(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Back To Credit Goto":
                return OnCompleteActivity_Back_To_Credit_Goto(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "SystemBackToCredit":
                return OnCompleteActivity_SystemBackToCredit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Perform Valuation":
                return OnCompleteActivity_Perform_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "FL Return Common":
                return OnCompleteActivity_FL_Return_Common(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Disbursement Timer":
                return OnCompleteActivity_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Valuations Request":
                return OnCompleteActivity_Valuations_Request(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Valuations Review":
                return OnCompleteActivity_Valuations_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Create EWork PipelineCase":
                return OnCompleteActivity_Create_EWork_PipelineCase(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT NTU":
                return OnCompleteActivity_EXT_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Continue with Followup":
                return OnCompleteActivity_Continue_with_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reinstate Followup":
                return OnCompleteActivity_Reinstate_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Archive Completed Followup":
                return OnCompleteActivity_Archive_Completed_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "FL Rework Application":
                return OnCompleteActivity_FL_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT DataStore FL Doc Recieved":
                return OnCompleteActivity_EXT_DataStore_FL_Doc_Recieved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Rapid Return":
                return OnCompleteActivity_Rapid_Return(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Retry Instruction":
                return OnCompleteActivity_Retry_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Instruction Failed":
                return OnCompleteActivity_Instruction_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Pipeline Case Create Failed":
                return OnCompleteActivity_Pipeline_Case_Create_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT Reinstate":
                return OnCompleteActivity_EXT_Reinstate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT NTU Final":
                return OnCompleteActivity_EXT_NTU_Final(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Assign Admin":
                return OnCompleteActivity_Assign_Admin(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "ValuationComplete":
                return OnCompleteActivity_ValuationComplete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "RaiseOnComplete":
                return OnCompleteActivity_RaiseOnComplete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Return to Manage Application":
                return OnCompleteActivity_Return_to_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT_ArchiveFollowup":
                return OnCompleteActivity_EXT_ArchiveFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT_ArchiveValuationClone":
                return OnCompleteActivity_EXT_ArchiveValuationClone(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT_CommonArchiveMain":
                return OnCompleteActivity_EXT_CommonArchiveMain(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Rapid Submit to Credit":
                return OnCompleteActivity_Rapid_Submit_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Readvance Post Credit Appro":
                return OnCompleteActivity_Readvance_Post_Credit_Appro(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "NTU PipeLine":
                return OnCompleteActivity_NTU_PipeLine(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Held Over":
                return OnCompleteActivity_Held_Over(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "MoveSwitch":
                return OnCompleteActivity_MoveSwitch(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Return Processor Readvance":
                return OnCompleteActivity_Return_Processor_Readvance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Pipeline Relodge":
                return OnCompleteActivity_Pipeline_Relodge(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Ext Held Over":
                return OnCompleteActivity_Ext_Held_Over(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT Disburse":
                return OnCompleteActivity_EXT_Disburse(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Ext Complete":
                return OnCompleteActivity_Ext_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reinstruct Attorney":
                return OnCompleteActivity_Reinstruct_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Force Disbursement Timer":
                return OnCompleteActivity_Force_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Account Create Failed":
                return OnCompleteActivity_Account_Create_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Create Account For Application":
                return OnCompleteActivity_Create_Account_For_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reload CaseName":
                return OnCompleteActivity_Reload_CaseName(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "SplitVal":
                return OnCompleteActivity_SplitVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnSplitReturn":
                return OnCompleteActivity_OnSplitReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Further Valuation Required":
                return OnCompleteActivity_Further_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnReturnFurtherVal":
                return OnCompleteActivity_OnReturnFurtherVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnReturnReviewVal":
                return OnCompleteActivity_OnReturnReviewVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Valuation Review Required":
                return OnCompleteActivity_Valuation_Review_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnStuck":
                return OnCompleteActivity_OnStuck(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Query Complete":
                return OnCompleteActivity_Query_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline By Credit Timer":
                return OnCompleteActivity_Decline_By_Credit_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Branch Rework Application":
                return OnCompleteActivity_Branch_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnCreateFollowup":
                return OnCompleteActivity_OnCreateFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnFollowupReturn":
                return OnCompleteActivity_OnFollowupReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "ReAssign Commission Consultant":
                return OnCompleteActivity_ReAssign_Commission_Consultant(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "90 Day Timer":
                return OnCompleteActivity_90_Day_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Stale App":
                return OnCompleteActivity_Stale_App(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Create Instance":
                return OnCompleteActivity_Create_Instance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Request Lightstone Valuation":
                return OnCompleteActivity_Request_Lightstone_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Submit To Credit":
                return OnCompleteActivity_Submit_To_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Credit Score Decline":
                return OnCompleteActivity_Credit_Score_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Continue Request Resolved":
                return OnCompleteActivity_Continue_Request_Resolved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "QAIsFL":
                return OnCompleteActivity_QAIsFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "CredIsFL":
                return OnCompleteActivity_CredIsFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Revert to Request at QA":
                return OnCompleteActivity_Revert_to_Request_at_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Revert to Resubmission":
                return OnCompleteActivity_Revert_to_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Revert to Manage Application":
                return OnCompleteActivity_Revert_to_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Return from Opt-Out Super Lo":
                return OnCompleteActivity_Return_from_Opt_Out_Super_Lo(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "CredNotFL":
                return OnCompleteActivity_CredNotFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Return Credit Pre-Approved":
                return OnCompleteActivity_Return_Credit_Pre_Approved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "isFL":
                return OnCompleteActivity_isFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            default:
                return false;
        }
    }

    public DateTime GetActivityTime(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityTime", WorkFlowData, InstanceData, Params, null);
        try
        {
            DateTime returnData = this.GetActivityTimeInternal(messages, WorkFlowData, InstanceData, Params);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityTime", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityTime", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityTime", InstanceData.InstanceID);
        }
    }

    public DateTime GetActivityTimeInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        switch (Params.ActivityName)
        {
            case "1 month timer":
                return GetActivityTime_1_month_timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "2 Months":
                return GetActivityTime_2_Months(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "45 days":
                return GetActivityTime_45_days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "30 Days":
                return GetActivityTime_30_Days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU Timeout":
                return GetActivityTime_NTU_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Timeout":
                return GetActivityTime_Decline_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowup":
                return GetActivityTime_OnFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Timer":
                return GetActivityTime_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Completed Followup":
                return GetActivityTime_Archive_Completed_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline By Credit Timer":
                return GetActivityTime_Decline_By_Credit_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "90 Day Timer":
                return GetActivityTime_90_Day_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            default:
                return DateTime.MinValue;
        }
    }

    public string GetStageTransition(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetStageTransition", WorkFlowData, InstanceData, Params, null);
        try
        {
            string returnData = this.GetStageTransitionInternal(messages, WorkFlowData, InstanceData, Params);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetStageTransition", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetStageTransition", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetStageTransition", InstanceData.InstanceID);
        }
    }

    public string GetStageTransitionInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        switch (Params.ActivityName)
        {
            case "Proceed with Application":
                return GetStageTransition_Proceed_with_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application in Order":
                return GetStageTransition_Application_in_Order(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Send LOA":
                return GetStageTransition_Send_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QA Query":
                return GetStageTransition_QA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Request Resolved":
                return GetStageTransition_Request_Resolved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QA Complete":
                return GetStageTransition_QA_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "New Purchase":
                return GetStageTransition_New_Purchase(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Client Accepts":
                return GetStageTransition_Client_Accepts(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Client Refuse":
                return GetStageTransition_Client_Refuse(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "1 month timer":
                return GetStageTransition_1_month_timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Other Types":
                return GetStageTransition_Other_Types(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query on Application":
                return GetStageTransition_Query_on_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Feedback on Query":
                return GetStageTransition_Feedback_on_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resubmit to Credit":
                return GetStageTransition_Resubmit_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA Received":
                return GetStageTransition_LOA_Received(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA Accepted":
                return GetStageTransition_LOA_Accepted(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Instruct Attorney":
                return GetStageTransition_Instruct_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Review Disbursement Setup":
                return GetStageTransition_Review_Disbursement_Setup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rollback Disbursement":
                return GetStageTransition_Rollback_Disbursement(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resubmit":
                return GetStageTransition_Resubmit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AttAssigned":
                return GetStageTransition_AttAssigned(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Received":
                return GetStageTransition_Application_Received(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Note Comment":
                return GetStageTransition_Note_Comment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Readvance":
                return GetStageTransition_Rapid_Readvance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "2 Months":
                return GetStageTransition_2_Months(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assigned QA":
                return GetStageTransition_Assigned_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "45 days":
                return GetStageTransition_45_days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Send AIP":
                return GetStageTransition_Send_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query on LOA":
                return GetStageTransition_Query_on_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "30 Days":
                return GetStageTransition_30_Days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resend LOA":
                return GetStageTransition_Resend_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Require Arrear Comment":
                return GetStageTransition_Require_Arrear_Comment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Override Check":
                return GetStageTransition_Override_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Translation Complete":
                return GetStageTransition_Translation_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Misc Condition":
                return GetStageTransition_Misc_Condition(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Select Attorney":
                return GetStageTransition_Select_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Att not assigned":
                return GetStageTransition_Att_not_assigned(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "New Business":
                return GetStageTransition_New_Business(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Lending":
                return GetStageTransition_Further_Lending(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assign Processor":
                return GetStageTransition_Assign_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AutoValuation":
                return GetStageTransition_AutoValuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "GotoCredit":
                return GetStageTransition_GotoCredit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AutoGotoQC":
                return GetStageTransition_AutoGotoQC(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resend Instruction":
                return GetStageTransition_Resend_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reassign User":
                return GetStageTransition_Reassign_User(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Perform Further Valuation":
                return GetStageTransition_Perform_Further_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Review Valuation Required":
                return GetStageTransition_Review_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rework Application":
                return GetStageTransition_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Info Request Complete":
                return GetStageTransition_Info_Request_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Final":
                return GetStageTransition_Decline_Final(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Motivate":
                return GetStageTransition_Motivate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Dispute Finalised":
                return GetStageTransition_Dispute_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU Timeout":
                return GetStageTransition_NTU_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Timeout":
                return GetStageTransition_Decline_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate NTU":
                return GetStageTransition_Reinstate_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Decline":
                return GetStageTransition_Reinstate_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return GetStageTransition_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return GetStageTransition_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Incorrect":
                return GetStageTransition_Disbursement_Incorrect(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Lending Calc":
                return GetStageTransition_Further_Lending_Calc(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Apps in prog of higher pri":
                return GetStageTransition_Apps_in_prog_of_higher_pri(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Hold Application":
                return GetStageTransition_EXT_Hold_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Reactivate App":
                return GetStageTransition_EXT_Reactivate_App(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Highest Priority":
                return GetStageTransition_Highest_Priority(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Complete Followup":
                return GetStageTransition_Complete_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Update Followup":
                return GetStageTransition_Update_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create Followup":
                return GetStageTransition_Create_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowup":
                return GetStageTransition_OnFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU Finalised":
                return GetStageTransition_NTU_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Finalised":
                return GetStageTransition_Decline_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Pipeline_UpForFees":
                return GetStageTransition_Pipeline_UpForFees(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Perform Valuation":
                return GetStageTransition_Perform_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Timer":
                return GetStageTransition_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create EWork PipelineCase":
                return GetStageTransition_Create_EWork_PipelineCase(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT NTU":
                return GetStageTransition_EXT_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "FL Rework Application":
                return GetStageTransition_FL_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT DataStore FL Doc Recieved":
                return GetStageTransition_EXT_DataStore_FL_Doc_Recieved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Retry Instruction":
                return GetStageTransition_Retry_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Reinstate":
                return GetStageTransition_EXT_Reinstate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT NTU Final":
                return GetStageTransition_EXT_NTU_Final(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return to Manage Application":
                return GetStageTransition_Return_to_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU PipeLine":
                return GetStageTransition_NTU_PipeLine(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Held Over":
                return GetStageTransition_Held_Over(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Ext Held Over":
                return GetStageTransition_Ext_Held_Over(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Disburse":
                return GetStageTransition_EXT_Disburse(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Ext Complete":
                return GetStageTransition_Ext_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstruct Attorney":
                return GetStageTransition_Reinstruct_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Force Disbursement Timer":
                return GetStageTransition_Force_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create Account For Application":
                return GetStageTransition_Create_Account_For_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query Complete":
                return GetStageTransition_Query_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline By Credit Timer":
                return GetStageTransition_Decline_By_Credit_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Branch Rework Application":
                return GetStageTransition_Branch_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ReAssign Commission Consultant":
                return GetStageTransition_ReAssign_Commission_Consultant(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Stale App":
                return GetStageTransition_Stale_App(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Request Lightstone Valuation":
                return GetStageTransition_Request_Lightstone_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Submit To Credit":
                return GetStageTransition_Submit_To_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Score Decline":
                return GetStageTransition_Credit_Score_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Continue Request Resolved":
                return GetStageTransition_Continue_Request_Resolved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QAIsFL":
                return GetStageTransition_QAIsFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CredIsFL":
                return GetStageTransition_CredIsFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Request at QA":
                return GetStageTransition_Revert_to_Request_at_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Resubmission":
                return GetStageTransition_Revert_to_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Manage Application":
                return GetStageTransition_Revert_to_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            default:
                return string.Empty;
        }
    }

    public string GetActivityMessage(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityMessage", WorkFlowData, InstanceData, Params, null);
        try
        {
            string returnData = this.GetActivityMessageInternal(messages, WorkFlowData, InstanceData, Params);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityMessage", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityMessage", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityMessage", InstanceData.InstanceID);
        }
    }

    public string GetActivityMessageInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params)
    {
        switch (Params.ActivityName)
        {
            case "Proceed with Application":
                return GetActivityMessage_Proceed_with_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application in Order":
                return GetActivityMessage_Application_in_Order(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Send LOA":
                return GetActivityMessage_Send_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QA Query":
                return GetActivityMessage_QA_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Request Resolved":
                return GetActivityMessage_Request_Resolved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QA Complete":
                return GetActivityMessage_QA_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "New Purchase":
                return GetActivityMessage_New_Purchase(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Client Accepts":
                return GetActivityMessage_Client_Accepts(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Client Refuse":
                return GetActivityMessage_Client_Refuse(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "1 month timer":
                return GetActivityMessage_1_month_timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Other Types":
                return GetActivityMessage_Other_Types(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query on Application":
                return GetActivityMessage_Query_on_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Feedback on Query":
                return GetActivityMessage_Feedback_on_Query(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resubmit to Credit":
                return GetActivityMessage_Resubmit_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA Received":
                return GetActivityMessage_LOA_Received(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "LOA Accepted":
                return GetActivityMessage_LOA_Accepted(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Instruct Attorney":
                return GetActivityMessage_Instruct_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Review Disbursement Setup":
                return GetActivityMessage_Review_Disbursement_Setup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rollback Disbursement":
                return GetActivityMessage_Rollback_Disbursement(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resubmit":
                return GetActivityMessage_Resubmit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AttAssigned":
                return GetActivityMessage_AttAssigned(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Application Received":
                return GetActivityMessage_Application_Received(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Note Comment":
                return GetActivityMessage_Note_Comment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Readvance":
                return GetActivityMessage_Rapid_Readvance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "2 Months":
                return GetActivityMessage_2_Months(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assigned QA":
                return GetActivityMessage_Assigned_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "45 days":
                return GetActivityMessage_45_days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Send AIP":
                return GetActivityMessage_Send_AIP(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query on LOA":
                return GetActivityMessage_Query_on_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "30 Days":
                return GetActivityMessage_30_Days(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resend LOA":
                return GetActivityMessage_Resend_LOA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Require Arrear Comment":
                return GetActivityMessage_Require_Arrear_Comment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Override Check":
                return GetActivityMessage_Override_Check(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation in Order":
                return GetActivityMessage_Valuation_in_Order(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Resubmission?":
                return GetActivityMessage_Valuation_Resubmission_(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return Processor Credit":
                return GetActivityMessage_Return_Processor_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return with Approve or Offer":
                return GetActivityMessage_Return_with_Approve_or_Offer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Return Resubmission":
                return GetActivityMessage_Credit_Return_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Return Resub Approve":
                return GetActivityMessage_Credit_Return_Resub_Approve(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Translation Complete":
                return GetActivityMessage_Translation_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Misc Condition":
                return GetActivityMessage_Misc_Condition(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Select Attorney":
                return GetActivityMessage_Select_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Att not assigned":
                return GetActivityMessage_Att_not_assigned(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Workflow":
                return GetActivityMessage_Valuation_Workflow(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit":
                return GetActivityMessage_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return ReAdv Payment FL":
                return GetActivityMessage_Return_ReAdv_Payment_FL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "New Business":
                return GetActivityMessage_New_Business(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Lending":
                return GetActivityMessage_Further_Lending(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Payments":
                return GetActivityMessage_Readvance_Payments(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Payment":
                return GetActivityMessage_Readvance_Payment(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assign Processor":
                return GetActivityMessage_Assign_Processor(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AutoValuation":
                return GetActivityMessage_AutoValuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "GotoCredit":
                return GetActivityMessage_GotoCredit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "AutoGotoQC":
                return GetActivityMessage_AutoGotoQC(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Resend Instruction":
                return GetActivityMessage_Resend_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reassign User":
                return GetActivityMessage_Reassign_User(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Perform Further Valuation":
                return GetActivityMessage_Perform_Further_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Review Valuation Required":
                return GetActivityMessage_Review_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rework Application":
                return GetActivityMessage_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Info Request Complete":
                return GetActivityMessage_Info_Request_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Final":
                return GetActivityMessage_Decline_Final(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Motivate":
                return GetActivityMessage_Motivate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Dispute Finalised":
                return GetActivityMessage_Dispute_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Decline Application":
                return GetActivityMessage_Credit_Decline_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Further Info":
                return GetActivityMessage_Credit_Further_Info(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Dispute":
                return GetActivityMessage_Credit_Dispute(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU Timeout":
                return GetActivityMessage_NTU_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Timeout":
                return GetActivityMessage_Decline_Timeout(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate NTU":
                return GetActivityMessage_Reinstate_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Decline":
                return GetActivityMessage_Reinstate_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return GetActivityMessage_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return GetActivityMessage_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Incorrect":
                return GetActivityMessage_Disbursement_Incorrect(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Lending Calc":
                return GetActivityMessage_Further_Lending_Calc(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Apps in prog of higher pri":
                return GetActivityMessage_Apps_in_prog_of_higher_pri(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Hold Application":
                return GetActivityMessage_EXT_Hold_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Reactivate App":
                return GetActivityMessage_EXT_Reactivate_App(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Highest Priority":
                return GetActivityMessage_Highest_Priority(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Complete Followup":
                return GetActivityMessage_Complete_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Update Followup":
                return GetActivityMessage_Update_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create Followup":
                return GetActivityMessage_Create_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowup":
                return GetActivityMessage_OnFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU Finalised":
                return GetActivityMessage_NTU_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline Finalised":
                return GetActivityMessage_Decline_Finalised(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Pipeline_UpForFees":
                return GetActivityMessage_Pipeline_UpForFees(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Back To Credit Goto":
                return GetActivityMessage_Back_To_Credit_Goto(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "SystemBackToCredit":
                return GetActivityMessage_SystemBackToCredit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Perform Valuation":
                return GetActivityMessage_Perform_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "FL Return Common":
                return GetActivityMessage_FL_Return_Common(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Timer":
                return GetActivityMessage_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuations Request":
                return GetActivityMessage_Valuations_Request(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuations Review":
                return GetActivityMessage_Valuations_Review(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create EWork PipelineCase":
                return GetActivityMessage_Create_EWork_PipelineCase(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT NTU":
                return GetActivityMessage_EXT_NTU(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Continue with Followup":
                return GetActivityMessage_Continue_with_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Followup":
                return GetActivityMessage_Reinstate_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Archive Completed Followup":
                return GetActivityMessage_Archive_Completed_Followup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "FL Rework Application":
                return GetActivityMessage_FL_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT DataStore FL Doc Recieved":
                return GetActivityMessage_EXT_DataStore_FL_Doc_Recieved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Return":
                return GetActivityMessage_Rapid_Return(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Retry Instruction":
                return GetActivityMessage_Retry_Instruction(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Instruction Failed":
                return GetActivityMessage_Instruction_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Pipeline Case Create Failed":
                return GetActivityMessage_Pipeline_Case_Create_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Reinstate":
                return GetActivityMessage_EXT_Reinstate(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT NTU Final":
                return GetActivityMessage_EXT_NTU_Final(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Assign Admin":
                return GetActivityMessage_Assign_Admin(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ValuationComplete":
                return GetActivityMessage_ValuationComplete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "RaiseOnComplete":
                return GetActivityMessage_RaiseOnComplete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return to Manage Application":
                return GetActivityMessage_Return_to_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT_ArchiveFollowup":
                return GetActivityMessage_EXT_ArchiveFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT_ArchiveValuationClone":
                return GetActivityMessage_EXT_ArchiveValuationClone(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT_CommonArchiveMain":
                return GetActivityMessage_EXT_CommonArchiveMain(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Submit to Credit":
                return GetActivityMessage_Rapid_Submit_to_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Post Credit Appro":
                return GetActivityMessage_Readvance_Post_Credit_Appro(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "NTU PipeLine":
                return GetActivityMessage_NTU_PipeLine(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Held Over":
                return GetActivityMessage_Held_Over(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "MoveSwitch":
                return GetActivityMessage_MoveSwitch(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return Processor Readvance":
                return GetActivityMessage_Return_Processor_Readvance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Pipeline Relodge":
                return GetActivityMessage_Pipeline_Relodge(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Ext Held Over":
                return GetActivityMessage_Ext_Held_Over(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "EXT Disburse":
                return GetActivityMessage_EXT_Disburse(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Ext Complete":
                return GetActivityMessage_Ext_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reinstruct Attorney":
                return GetActivityMessage_Reinstruct_Attorney(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Force Disbursement Timer":
                return GetActivityMessage_Force_Disbursement_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Account Create Failed":
                return GetActivityMessage_Account_Create_Failed(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create Account For Application":
                return GetActivityMessage_Create_Account_For_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Reload CaseName":
                return GetActivityMessage_Reload_CaseName(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "SplitVal":
                return GetActivityMessage_SplitVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnSplitReturn":
                return GetActivityMessage_OnSplitReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Further Valuation Required":
                return GetActivityMessage_Further_Valuation_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnReturnFurtherVal":
                return GetActivityMessage_OnReturnFurtherVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnReturnReviewVal":
                return GetActivityMessage_OnReturnReviewVal(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Valuation Review Required":
                return GetActivityMessage_Valuation_Review_Required(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnStuck":
                return GetActivityMessage_OnStuck(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Query Complete":
                return GetActivityMessage_Query_Complete(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Decline By Credit Timer":
                return GetActivityMessage_Decline_By_Credit_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Branch Rework Application":
                return GetActivityMessage_Branch_Rework_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnCreateFollowup":
                return GetActivityMessage_OnCreateFollowup(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowupReturn":
                return GetActivityMessage_OnFollowupReturn(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "ReAssign Commission Consultant":
                return GetActivityMessage_ReAssign_Commission_Consultant(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "90 Day Timer":
                return GetActivityMessage_90_Day_Timer(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Stale App":
                return GetActivityMessage_Stale_App(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Create Instance":
                return GetActivityMessage_Create_Instance(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Request Lightstone Valuation":
                return GetActivityMessage_Request_Lightstone_Valuation(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Submit To Credit":
                return GetActivityMessage_Submit_To_Credit(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Credit Score Decline":
                return GetActivityMessage_Credit_Score_Decline(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Continue Request Resolved":
                return GetActivityMessage_Continue_Request_Resolved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "QAIsFL":
                return GetActivityMessage_QAIsFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CredIsFL":
                return GetActivityMessage_CredIsFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Request at QA":
                return GetActivityMessage_Revert_to_Request_at_QA(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Resubmission":
                return GetActivityMessage_Revert_to_Resubmission(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Revert to Manage Application":
                return GetActivityMessage_Revert_to_Manage_Application(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return from Opt-Out Super Lo":
                return GetActivityMessage_Return_from_Opt_Out_Super_Lo(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "CredNotFL":
                return GetActivityMessage_CredNotFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "Return Credit Pre-Approved":
                return GetActivityMessage_Return_Credit_Pre_Approved(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            case "isFL":
                return GetActivityMessage_isFL(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params);
            default:
                return string.Empty;
        }
    }

    public string GetDynamicRole(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName, string WorkflowName)
    {
        log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetDynamicRole", WorkFlowData, InstanceData, Params, new object[] { RoleName, WorkflowName });
        try
        {
            string returnData = this.GetDynamicRoleInternal(messages, WorkFlowData, InstanceData, Params, RoleName, WorkflowName);
            log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetDynamicRole", WorkFlowData, InstanceData, Params);
            return returnData;
        }
        catch (Exception exception)
        {
            log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetDynamicRole", InstanceData.InstanceID, exception);
            throw exception;
        }
        finally
        {
            log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetDynamicRole", InstanceData.InstanceID);
        }
    }

    public string GetDynamicRoleInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, SAHL.X2.Framework.Common.IX2Params Params, string RoleName, string WorkflowName)
    {
        switch (RoleName.Replace(' ', '_') + "_" + WorkflowName.Replace(' ', '_'))
        {
            case "Branch_Consultant_D_Application_Management":
                return OnGetRole_Application_Management_Branch_Consultant_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "New_Business_Processor_D_Application_Management":
                return OnGetRole_Application_Management_New_Business_Processor_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Registrations_LOA_Admin_D_Application_Management":
                return OnGetRole_Application_Management_Registrations_LOA_Admin_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Registrations_Administrator_D_Application_Management":
                return OnGetRole_Application_Management_Registrations_Administrator_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Credit_Underwriter_D_Application_Management":
                return OnGetRole_Application_Management_Credit_Underwriter_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "FL_Processor_D_Application_Management":
                return OnGetRole_Application_Management_FL_Processor_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "FL_Collections_Admin_D_Application_Management":
                return OnGetRole_Application_Management_FL_Collections_Admin_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Resubmission_Admin_D_Application_Management":
                return OnGetRole_Application_Management_Resubmission_Admin_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Registrations_Supervisor_D_Application_Management":
                return OnGetRole_Application_Management_Registrations_Supervisor_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Registrations_Manager_D_Application_Management":
                return OnGetRole_Application_Management_Registrations_Manager_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Branch_Admin_D_Application_Management":
                return OnGetRole_Application_Management_Branch_Admin_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Translate_Conditions_D_Application_Management":
                return OnGetRole_Application_Management_Translate_Conditions_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "New_Business_Supervisor_D_Application_Management":
                return OnGetRole_Application_Management_New_Business_Supervisor_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "FL_Manager_D_Application_Management":
                return OnGetRole_Application_Management_FL_Manager_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Credit_Manager_D_Application_Management":
                return OnGetRole_Application_Management_Credit_Manager_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Credit_Supervisor_D_Application_Management":
                return OnGetRole_Application_Management_Credit_Supervisor_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "FL_Supervisor_D_Application_Management":
                return OnGetRole_Application_Management_FL_Supervisor_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "QA_Administrator_D_Application_Management":
                return OnGetRole_Application_Management_QA_Administrator_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "Branch_Manager_D_Application_Management":
                return OnGetRole_Application_Management_Branch_Manager_D(messages, (IX2Application_Management_Data)WorkFlowData, InstanceData, Params, RoleName);
            default:
                return string.Empty;
        }
    }

    public IX2WorkFlowDataProvider GetWorkFlowDataProvider()
    {
        return new X2Application_Management_Data();
    }

    #endregion IX2WorkFlow Members
    
     



}

#endregion WorkFlow Application Management