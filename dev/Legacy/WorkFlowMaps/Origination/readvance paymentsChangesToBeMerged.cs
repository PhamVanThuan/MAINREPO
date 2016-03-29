using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
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
using SAHL.Common.DomainMessages;

#region Process

public class Process : IX2Process
{
    public IX2WorkFlow GetWorkFlow(string WorkFlowName)
    {
        switch (WorkFlowName)
        {
            //case "Readvance Payments":
            //    return new X2Readvance_Payments();
            //case "Credit":
            //    return new X2Credit();
            //case "Valuations":
            //    return new X2Valuations();
            //case "Application Management":
            //    return new X2Application_Management();
            //case "Application Capture":
            //    return new X2Application_Capture();
            default:
                return null;
        }
    }

    public string GetDynamicRole(string RoleName, IActiveDataTransaction Tran)
    {
        switch (RoleName)
        {
            default:
                return null;
        }
    }

    #region Process Roles

    #endregion Process Roles
}

#endregion Process

#region WorkFlowData Readvance Payments

public interface IX2Readvance_Payments_Data : IX2WorkFlowDataProvider
{
    Int32 ApplicationKey { get; set; }

    String PreviousState { get; set; }

    Int32 GenericKey { get; set; }

    Int32 EntryPath { get; set; }
}

public class X2Readvance_Payments_Data : IX2Readvance_Payments_Data
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

    private Int32 m_EntryPath;

    public Int32 EntryPath
    {
        get
        {
            return m_EntryPath;
        }
        set
        {
            m_HasChanges = true;
            m_EntryPath = value;
        }
    }

    #region IX2WorkFlowDataProvider Members

    public void LoadData(IActiveDataTransaction Tran, Int64 InstanceID)
    {
        SqlDataAdapter SDA = null;
        DataTable WorkFlowData = new DataTable();
        try
        {
            WorkerHelper.FillFromQuery(WorkFlowData, "select * from [X2DATA].Readvance_Payments (nolock) where InstanceID = " + InstanceID, Tran.Context, null);
            if (WorkFlowData.Rows.Count > 0)
            {
                if (WorkFlowData.Rows[0]["ApplicationKey"] != DBNull.Value)
                    m_ApplicationKey = Convert.ToInt32(WorkFlowData.Rows[0]["ApplicationKey"]);
                if (WorkFlowData.Rows[0]["PreviousState"] != DBNull.Value)
                    m_PreviousState = Convert.ToString(WorkFlowData.Rows[0]["PreviousState"]);
                if (WorkFlowData.Rows[0]["GenericKey"] != DBNull.Value)
                    m_GenericKey = Convert.ToInt32(WorkFlowData.Rows[0]["GenericKey"]);
                if (WorkFlowData.Rows[0]["EntryPath"] != DBNull.Value)
                    m_EntryPath = Convert.ToInt32(WorkFlowData.Rows[0]["EntryPath"]);
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
                    case "entrypath":
                        EntryPath = Convert.ToInt32(Fields[Keys[i]]);
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
            WorkerHelper.AddParameter(Parameters, "@P3", SqlDbType.Int, ParameterDirection.Input, m_EntryPath);
            WorkerHelper.ExecuteNonQuery(Tran.Context, "update [X2DATA].[Readvance_Payments] with (rowlock) set [ApplicationKey] = @P0, [PreviousState] = @P1, [GenericKey] = @P2, [EntryPath] = @P3 where InstanceID = '" + InstanceID + "'", Parameters);
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
        WorkerHelper.AddParameter(Parameters, "@P3", SqlDbType.Int, ParameterDirection.Input, m_EntryPath);
        WorkerHelper.ExecuteNonQuery(Tran.Context, "insert into [X2DATA].[Readvance_Payments] values( " + InstanceID + ", @P0, @P1, @P2, @P3)", Parameters);
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
            case "entrypath":
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
            case "entrypath":
                return m_EntryPath.ToString();
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
        Data.Add("entrypath", m_EntryPath.ToString());
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

#endregion WorkFlowData Readvance Payments
#region WorkFlow Readvance Payments

public class X2Readvance_Payments : IX2WorkFlow
{
    #region States

    /// <summary>
    /// Called when the Rapid Decision state is entered.
    /// </summary>
    public bool OnEnter_Rapid_Decision(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        // string User = string.Empty;
        // WorkflowAssignment.ReactiveUserOrRoundRobin(Params.Tran, "FL Supervisor D", Readvance_Payments_Data.ApplicationKey, 155, InstanceData.InstanceID, "Rapid Decision", SAHL.Common.Constants.WorkFlowProcessName.Origination);
        return true;
    }

    /// <summary>
    /// Called when the Rapid Decision state is exited.
    /// </summary>
    public bool OnExit_Rapid_Decision(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        Readvance_Payments_Data.PreviousState = "Rapid Decision";
        return true;
    }

    /// <summary>
    /// Called when the Contact Client state is entered.
    /// </summary>
    public bool OnEnter_Contact_Client(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        // look for prev FL Proc Drole in prev map
        string s = string.Empty;

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(messages, (long)InstanceData.SourceInstanceID, "FL Processor D", 157, out s);

        if (!string.IsNullOrEmpty(s))
        {
            WorkflowAssignment.ReassignCaseToUserByProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, s, 157, 857, "Contact Client", SAHL.Common.Globals.Process.Origination);
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "Contact Client", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }


        return true;
    }

    /// <summary>
    /// Called when the Contact Client state is exited.
    /// </summary>
    public bool OnExit_Contact_Client(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        Readvance_Payments_Data.PreviousState = "Contact Client";
        return true;
    }

    /// <summary>
    /// Called when the Send Schedule state is entered.
    /// </summary>
    public bool OnEnter_Send_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Send Schedule state is exited.
    /// </summary>
    public bool OnExit_Send_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        Readvance_Payments_Data.PreviousState = "Send Schedule";
        return true;
    }

    /// <summary>
    /// Called when the Awaiting Schedule state is entered.
    /// </summary>
    public bool OnEnter_Awaiting_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Awaiting Schedule state is exited.
    /// </summary>
    public bool OnExit_Awaiting_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        Readvance_Payments_Data.PreviousState = "Awaiting Schedule";
        return true;
    }

    public bool OnEnter_Setup_Payment(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Setup Payment state is exited.
    /// </summary>
    public bool OnExit_Setup_Payment(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        Readvance_Payments_Data.PreviousState = "Setup Payment";
        return true;
    }

    /// <summary>
    /// Called when the Disburse Funds state is entered.
    /// </summary>
    public bool OnEnter_Disburse_Funds(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Disburse Funds state is exited.
    /// </summary>
    public bool OnExit_Disburse_Funds(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        Readvance_Payments_Data.PreviousState = "Disburse Funds";
        return true;
    }

    /// <summary>
    /// Called when the Folder Archive state is entered.
    /// </summary>
    public bool OnEnter_Folder_Archive(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IApplicationManagement appman = DomainServiceLoader.Instance.Get<IApplicationManagement>();
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        common.SetOfferEndDate(messages, Readvance_Payments_Data.ApplicationKey);
        common.UpdateOfferStatus(messages, Readvance_Payments_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Accepted, (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer);
        appman.RemoveRegistrationProcessDetailTypes(messages, Readvance_Payments_Data.ApplicationKey);

        appman.SetAccountStatusToApplicationPriorToInstructAttorney(messages, Readvance_Payments_Data.ApplicationKey, Params.ADUserName);
        appman.RemoveRegistrationProcessDetailTypes(messages, Readvance_Payments_Data.ApplicationKey);
        appman.RemoveAccountFromRegMail(messages, Readvance_Payments_Data.ApplicationKey);

        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        return true;
    }

    /// <summary>
    /// Called when the Folder Archive state is exited.
    /// </summary>
    public bool OnExit_Folder_Archive(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Folder Archive state is about to be archived.
    /// </summary>
    public bool OnReturn_Folder_Archive(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Next Step state is entered.
    /// </summary>
    public bool OnEnter_Next_Step(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Next Step state is exited.
    /// </summary>
    public bool OnExit_Next_Step(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Disbursed state is entered.
    /// </summary>
    public bool OnEnter_Disbursed(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Disbursed state is exited.
    /// </summary>
    public bool OnExit_Disbursed(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        Readvance_Payments_Data.PreviousState = "Disbursed";
        return true;
    }

    /// <summary>
    /// Called when the Archive Further Loan state is entered.
    /// </summary>
    public bool OnEnter_Archive_Further_Loan(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        return true;
    }

    /// <summary>
    /// Called when the Archive Further Loan state is exited.
    /// </summary>
    public bool OnExit_Archive_Further_Loan(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Further Loan state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Further_Loan(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Reassign state is entered.
    /// </summary>
    public bool OnEnter_Common_Reassign(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Reassign state is exited.
    /// </summary>
    public bool OnExit_Common_Reassign(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common NTU state is entered.
    /// </summary>
    public bool OnEnter_Common_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common NTU state is exited.
    /// </summary>
    public bool OnExit_Common_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Decline state is entered.
    /// </summary>
    public bool OnEnter_Common_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Decline state is exited.
    /// </summary>
    public bool OnExit_Common_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the NTU state is entered.
    /// </summary>
    public bool OnEnter_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Readvance_Payments_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1);
        return true;
    }

    /// <summary>
    /// Called when the NTU state is exited.
    /// </summary>
    public bool OnExit_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        //Readvance_Payments_Data.PreviousState = "NTU";
        return true;
    }

    /// <summary>
    /// Called when the Decline state is entered.
    /// </summary>
    public bool OnEnter_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Readvance_Payments_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Declined, -1);
        return true;
    }

    /// <summary>
    /// Called when the Decline state is exited.
    /// </summary>
    public bool OnExit_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        //Readvance_Payments_Data.PreviousState = "Decline";
        return true;
    }

    /// <summary>
    /// Called when the Return to sender state is entered.
    /// </summary>
    public bool OnEnter_Return_to_sender(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Readvance_Payments_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open, -1);

        return true;
    }

    /// <summary>
    /// Called when the Return to sender state is exited.
    /// </summary>
    public bool OnExit_Return_to_sender(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Return to sender state is autoforwarded.
    /// </summary>
    public IX2ReturnData OnForwardState_Return_to_sender(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return Readvance_Payments_Data.PreviousState;
    }

    /// <summary>
    /// Called when the Archive Decline state is entered.
    /// </summary>
    public bool OnEnter_Archive_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();

        furtherLending.RemoveDetailTypes(messages, Readvance_Payments_Data.ApplicationKey);
        furtherLending.ArchiveFLRelatedCases(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, Params.ADUserName);

        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Readvance_Payments_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Declined, -1);

        return true;
    }

    /// <summary>
    /// Called when the Archive Decline state is exited.
    /// </summary>
    public bool OnExit_Archive_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Decline state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive NTU state is entered.
    /// </summary>
    public bool OnEnter_Archive_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        furtherLending.RemoveDetailTypes(messages, Readvance_Payments_Data.ApplicationKey);

        furtherLending.ArchiveFLRelatedCases(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, Params.ADUserName);

        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Readvance_Payments_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1);

        return true;
    }

    /// <summary>
    /// Called when the Archive NTU state is exited.
    /// </summary>
    public bool OnExit_Archive_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive NTU state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Followup state is entered.
    /// </summary>
    public bool OnEnter_Common_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Followup state is exited.
    /// </summary>
    public bool OnExit_Common_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Followup Hold state is entered.
    /// </summary>
    public bool OnEnter_Followup_Hold(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Followup Hold state is exited.
    /// </summary>
    public bool OnExit_Followup_Hold(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Followup Complete state is entered.
    /// </summary>
    public bool OnEnter_Followup_Complete(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);
        return true;
    }

    /// <summary>
    /// Called when the Followup Complete state is exited.
    /// </summary>
    public bool OnExit_Followup_Complete(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Followup Complete state is about to be archived.
    /// </summary>
    public bool OnReturn_Followup_Complete(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Ready To Followup state is entered.
    /// </summary>
    public bool OnEnter_Ready_To_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Ready To Followup state is exited.
    /// </summary>
    public bool OnExit_Ready_To_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Archive Followup state is entered.
    /// </summary>
    public bool OnEnter_Common_Archive_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Archive Followup state is exited.
    /// </summary>
    public bool OnExit_Common_Archive_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Archive(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);
        return true;
    }

    /// <summary>
    /// Called when the Archive state is exited.
    /// </summary>
    public bool OnExit_Archive(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Return to Processor state is entered.
    /// </summary>
    public bool OnEnter_Common_Return_to_Processor(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Common Return to Processor state is exited.
    /// </summary>
    public bool OnExit_Common_Return_to_Processor(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Processor state is entered.
    /// </summary>
    public bool OnEnter_Archive_Processor(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        // get current user assigned to this case, get source id, assign user to source id in WA as this case is heading back to AppMan
        //WorkflowAssignment.ReassignParentMapToCurrentUser(Params.Tran, InstanceData.InstanceID, (Int64)InstanceData.SourceInstanceID, Readvance_Payments_Data.ApplicationKey, "Archive Processor", SAHL.Common.Constants.WorkFlowProcessName.Origination);

        string ADUserName = WorkflowAssignment.GetLatestUserAcrossInstances(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, 157, "FL Processor D", "Archive Processor", SAHL.Common.Globals.Process.Origination);
        if (ADUserName == null || ADUserName == string.Empty || ADUserName.Length == 0)
        {
            //WorkflowAssignment.X2RoundRobinForGivenOSKeys(Params.Tran, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, (Int64)InstanceData.SourceInstanceID, "Archive Processor", SAHL.Common.Constants.WorkFlowProcessName.Origination);
            WorkflowAssignment.X2RoundRobinForPointerDescription(messages, InstanceData.SourceInstanceID.Value, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor, Readvance_Payments_Data.ApplicationKey, "FL Processor D", "Archive Processor", SAHL.Common.Globals.Process.Origination);
        }
        else
        {
            WorkflowAssignment.ReassignCaseToUser(messages, InstanceData.SourceInstanceID.Value, Readvance_Payments_Data.ApplicationKey, ADUserName, 157, 857, "Archive Processor");
        }

        return true;
    }

    /// <summary>
    /// Called when the Archive Processor state is exited.
    /// </summary>
    public bool OnExit_Archive_Processor(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    /// <summary>
    /// Called when the Archive Processor state is about to be archived.
    /// </summary>
    public bool OnReturn_Archive_Processor(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Readvance_Create_Fail(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Readvance_Create_Fail(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_State29(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnExit_State29(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_FollowupReturn(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnExit_FollowupReturn(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public IX2ReturnData OnForwardState_FollowupReturn(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return Readvance_Payments_Data.PreviousState;
    }

    public bool OnEnter_Deed_of_Surety_Instruction(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        List<string> DYs = new List<string>();
        DYs.Add("RV Admin D");
        DYs.Add("FL Processor D");
        DYs.Add("FL Manager D");
        DYs.Add("FL Supervisor D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);
        WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "RV Admin D", Readvance_Payments_Data.ApplicationKey, 595, InstanceData.InstanceID, "Deed of Surety Instruction", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.RVAdmin);

        return true;
    }

    public bool OnExit_Deed_of_Surety_Instruction(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Common_Lightstone_AVM(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Common_Lightstone_AVM(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Archive_FL_SuperLo_OptOut(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Archive_FL_SuperLo_OptOut(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnReturn_Archive_FL_SuperLo_OptOut(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Common_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Common_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_Require_SuperLo_Opt_Out(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnExit_Require_SuperLo_Opt_Out(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnEnter_OptOut_Return(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnExit_OptOut_Return(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public IX2ReturnData OnForwardState_OptOut_Return(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return Readvance_Payments_Data.PreviousState;
    }

    #endregion States

    #region Activities

    public bool OnStartActivity_Readvance_Payment(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Readvance_Payment(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        Readvance_Payments_Data.EntryPath = 1;

        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Readvance_Payment(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Inform_Client(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Inform_Client(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.CheckInformClientRules(messages, Readvance_Payments_Data.ApplicationKey, Params.IgnoreWarning);
    }

    /// <summary>
    ///  Inform Client.
    /// </summary>
    public string GetStageTransition_Inform_Client(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Inform Client");
    }

    public string GetActivityMessage_Inform_Client(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Further_Advance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.IsFurtherAdvance(messages, Readvance_Payments_Data.ApplicationKey);
    }

    public bool OnCompleteActivity_Further_Advance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        DYs.Add("RV Manager D");
        DYs.Add("RV Admin D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "Send Schedule", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        return true;
    }

    /// <summary>
    ///  Further Advance.
    /// </summary>
    public string GetStageTransition_Further_Advance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Further_Advance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Send_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Send_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Send Schedule.
    /// </summary>
    public string GetStageTransition_Send_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Send Schedule");
    }

    public string GetActivityMessage_Send_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Receive_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Receive_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Receive Schedule.
    /// </summary>
    public string GetStageTransition_Receive_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Receive Schedule");
    }

    public string GetActivityMessage_Receive_Schedule(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Payment_Prepared(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Payment_Prepared(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        // deactivate workflow asignment records
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        // round robin
        WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeysByProcess(messages, "FL Supervisor D", Readvance_Payments_Data.ApplicationKey, null, InstanceData.InstanceID, "Disburse Funds", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisorDisburseFunds);

        return true;
    }

    /// <summary>
    ///  Payment Prepared.
    /// </summary>
    public string GetStageTransition_Payment_Prepared(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Payment Prepared");
    }

    public string GetActivityMessage_Payment_Prepared(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Approve_Rapid(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Approve_Rapid(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.UpdateOfferStatus(messages, Readvance_Payments_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open, (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer);

        // Deactivate the supervisor (deactivate all then wake up fl prod D
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        string s = "";
        WorkflowAssignment.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(messages, InstanceData.SourceInstanceID.Value, "FL Processor D", 157, out s);
        if (!string.IsNullOrEmpty(s))
        {
            WorkflowAssignment.ReassignCaseToUser(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, s, 157, 857, "Rapid Decision");
        }
        else
        {
            string User = string.Empty;
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }
        ///////////////////////////////////////////////////////////
        string ADUserName = WorkflowAssignment.GetLastUserToWorkOnCaseAcrossInstances(messages, InstanceData.InstanceID, (Int64)InstanceData.SourceInstanceID, 921, "FL Supervisor D", "Readvance Payments");
        if (!string.IsNullOrEmpty(ADUserName))
        {
            WorkflowAssignment.ReassignCaseToUser(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, ADUserName, 155, 921, "Rapid Decision");
        }

        return true;
    }

    /// <summary>
    ///  Approve Rapid.
    /// </summary>
    public string GetStageTransition_Approve_Rapid(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Approve Rapid");
    }

    public string GetActivityMessage_Approve_Rapid(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Disbursement_Complete(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Disbursement_Complete(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();

        if (furtherLending.CheckCanDisburseReadvanceRules(messages, Readvance_Payments_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return common.AddDetailType(messages, Readvance_Payments_Data.ApplicationKey, Params.ADUserName, SAHL.Common.Globals.DetailTypes.NewLegalAgreementSigned.ToString());
        }
        return false;
    }

    /// <summary>
    ///  Disbursement Complete.
    /// </summary>
    public string GetStageTransition_Disbursement_Complete(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Disbursement Complete");
    }

    public string GetActivityMessage_Disbursement_Complete(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Refer_to_Credit(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Refer_to_Credit(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Refer to Credit.
    /// </summary>
    public string GetStageTransition_Refer_to_Credit(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Refer to Credit");
    }

    public string GetActivityMessage_Refer_to_Credit(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Further_Loan(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.IsFurtherLoan(messages, Readvance_Payments_Data.ApplicationKey);
    }

    public bool OnCompleteActivity_Further_Loan(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  Further Loan.
    /// </summary>
    public string GetStageTransition_Further_Loan(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Further_Loan(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Rollback_Disbursement(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Rollback_Disbursement(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        if (furtherLending.CanRollbackReadvanceCorrectionTransaction(messages, Readvance_Payments_Data.ApplicationKey))
        {
            ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
            if (common.GetOfferType(messages, Readvance_Payments_Data.ApplicationKey) == (int)SAHL.Common.Globals.OfferTypes.FurtherAdvance)
            {
                common.DeleteDetail(messages, Readvance_Payments_Data.ApplicationKey, SAHL.Common.Globals.DetailTypes.NewLegalAgreementSigned.ToString());
            }
        }
        return true;
    }

    /// <summary>
    ///  Rollback Disbursement.
    /// </summary>
    public string GetStageTransition_Rollback_Disbursement(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Rollback Disbursement");
    }

    public string GetActivityMessage_Rollback_Disbursement(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_12hrs(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_12hrs(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_12hrs(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_12hrs(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for 12hrs.
    /// </summary>
    public DateTime GetActivityTime_12hrs(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return (DateTime.Now.AddHours(12));
    }

    public bool OnStartActivity_Approve_App___FA___FL(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Approve_App___FA___FL(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        Readvance_Payments_Data.EntryPath = 3;
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Approve_App___FA___FL(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Reassign_User(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Reassign_User(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Reassign_User(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        if (null != Params.Data)
            return Params.Data.ToString();
        return string.Empty;
    }

    public string GetActivityMessage_Reassign_User(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_NTU_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_NTU_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    /// <summary>
    ///  NTU Timeout.
    /// </summary>
    public string GetStageTransition_NTU_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_NTU_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for NTU Timeout.
    /// </summary>
    public DateTime GetActivityTime_NTU_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return (DateTime.Now.AddDays(30));
    }

    public bool OnStartActivity_Decline_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Decline_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Decline_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Decline_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for Decline Timeout.
    /// </summary>
    public DateTime GetActivityTime_Decline_Timeout(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return (DateTime.Now.AddDays(30));
    }

    public bool OnStartActivity_Reinstate_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Reinstate_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        List<string> dys = new List<string>();
        dys.Add("FL Processor D");
        dys.Add("FL Supervisor D");
        dys.Add("FL Manager D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, dys);

        if (Readvance_Payments_Data.PreviousState.ToUpper() == "RAPID DECISION")
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Supervisor D", Readvance_Payments_Data.ApplicationKey, 155, InstanceData.InstanceID, "Reinstate NTU", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor);
        }
        else
        {
            string ADUserName = WorkflowAssignment.GetLatestUserAcrossInstances(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, 157, "FL Processor D", "NTU", SAHL.Common.Globals.Process.Origination);
            if (ADUserName != null)
            {
                WorkflowAssignment.ReassignCaseToUser(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, ADUserName, 157, 857, "NTU");
            }
            else
            {
                //WorkflowAssignment.X2RoundRobinForGivenOSKeys(Params.Tran, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "NTU", SAHL.Common.Constants.WorkFlowProcessName.Origination);
                WorkflowAssignment.X2RoundRobinForPointerDescription(messages, InstanceData.InstanceID, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor, Readvance_Payments_Data.ApplicationKey, "FL Processor D", "NTU", SAHL.Common.Globals.Process.Origination);
            }
        }

        return true;
    }

    /// <summary>
    ///  Reinstate NTU.
    /// </summary>
    public string GetStageTransition_Reinstate_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Reinstate NTU");
    }

    public string GetActivityMessage_Reinstate_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Reinstate_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Reinstate_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Supervisor D", Readvance_Payments_Data.ApplicationKey, 155, InstanceData.InstanceID, "Reinstate Decline", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor);
        return true;
    }

    /// <summary>
    ///  Reinstate Decline.
    /// </summary>
    public string GetStageTransition_Reinstate_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Reinstate Decline");
    }

    public string GetActivityMessage_Reinstate_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        string ADUserName = WorkflowAssignment.GetLatestUserAcrossInstances(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, 157, "FL Processor D", "Decline", SAHL.Common.Globals.Process.Origination);
        if (!string.IsNullOrEmpty(ADUserName))
        {
            WorkflowAssignment.ReassignCaseToUser(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, ADUserName, 157, 857, "Decline");
        }
        else
        {
            //WorkflowAssignment.X2RoundRobinForGivenOSKeys(Params.Tran, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "Decline", SAHL.Common.Constants.WorkFlowProcessName.Origination);
            WorkflowAssignment.X2RoundRobinForPointerDescription(messages, InstanceData.InstanceID, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor, Readvance_Payments_Data.ApplicationKey, "FL Processor D", "Decline", SAHL.Common.Globals.Process.Origination);
        }

        ///////////////////////////////////////////////////////////
        ADUserName = WorkflowAssignment.GetLastUserToWorkOnCaseAcrossInstances(messages, InstanceData.InstanceID, (long)InstanceData.SourceInstanceID, 921, "FL Supervisor D", "Readvance Payments");
        if (!string.IsNullOrEmpty(ADUserName))
        {
            WorkflowAssignment.ReassignCaseToUser(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, ADUserName, 155, 921, "Rapid Decision");
        }

        return true;
    }

    /// <summary>
    ///  Decline.
    /// </summary>
    public string GetStageTransition_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Decline");
    }

    public string GetActivityMessage_Decline(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");

        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        WorkflowAssignment.DeActiveUsersForInstance(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs);

        string ADUserName = WorkflowAssignment.GetLatestUserAcrossInstances(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, 157, "FL Processor D", "NTU", SAHL.Common.Globals.Process.Origination);
        if (!string.IsNullOrEmpty(ADUserName))
        {
            WorkflowAssignment.ReassignCaseToUser(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, ADUserName, 157, 857, "NTU");
        }
        else
        {
            //WorkflowAssignment.X2RoundRobinForGivenOSKeys(Params.Tran, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "NTU", SAHL.Common.Constants.WorkFlowProcessName.Origination);
            WorkflowAssignment.X2RoundRobinForPointerDescription(messages, InstanceData.InstanceID, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor, Readvance_Payments_Data.ApplicationKey, "FL Processor D", "NTU", SAHL.Common.Globals.Process.Origination);
        }

        return true;
    }

    /// <summary>
    ///  NTU.
    /// </summary>
    public string GetStageTransition_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("NTU");
    }

    public string GetActivityMessage_NTU(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Complete_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Complete_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Complete_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Update_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Update_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Update_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Create_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Create_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();

        string CaseName = common.GetCaseName(messages, Readvance_Payments_Data.ApplicationKey);
        InstanceData.Subject = CaseName;
        InstanceData.Name = Readvance_Payments_Data.ApplicationKey.ToString();
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Create_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnFollowup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_OnFollowup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnFollowup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for OnFollowup.
    /// </summary>
    public DateTime GetActivityTime_OnFollowup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        return common.GetFollowupTime(messages, Readvance_Payments_Data.GenericKey);
    }

    public bool OnStartActivity_Continue_with_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Continue_with_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Continue_with_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Reinstate_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Reinstate_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Reinstate_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Archive_Complete_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Archive_Complete_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Archive_Complete_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    /// <summary>
    /// Called to determine the time value for Archive Complete Followup.
    /// </summary>
    public DateTime GetActivityTime_Archive_Complete_Followup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return (DateTime.Now.AddDays(10));
    }

    public bool OnStartActivity_Decline_Final(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Decline_Final(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.CheckNTUDeclineFinalRules(messages, Readvance_Payments_Data.ApplicationKey, Params.IgnoreWarning);
    }

    /// <summary>
    ///  Decline Final.
    /// </summary>
    public string GetStageTransition_Decline_Final(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Decline Final");
    }

    public string GetActivityMessage_Decline_Final(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_NTU_Final(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_NTU_Final(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.CheckNTUDeclineFinalRules(messages, Readvance_Payments_Data.ApplicationKey, Params.IgnoreWarning);
    }

    public string GetStageTransition_NTU_Final(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("NTU Final");
    }

    public string GetActivityMessage_NTU_Final(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Decline_by_Credit(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Decline_by_Credit(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Decline_by_Credit(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_EXT_ArchiveFollowup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_EXT_ArchiveFollowup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_EXT_ArchiveFollowup(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Readvance_Post_Credit_Appro(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Readvance_Post_Credit_Appro(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        Readvance_Payments_Data.EntryPath = 2;
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Readvance_Post_Credit_Appro(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        common.ArchiveV3ITCForApplication(messages, Readvance_Payments_Data.ApplicationKey);
        common.UpdateOfferStatus(messages, Readvance_Payments_Data.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.Open, -1);
        if (messages.HasErrorMessages)
            return false;
        else
            return true;
    }

    public string GetStageTransition_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Return from Readvance Payments");
    }

    public string GetActivityMessage_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_12_Hour_Override(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_12_Hour_Override(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_12_Hour_Override(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Sys_Post_Cred_Appro(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        if (Readvance_Payments_Data.EntryPath == 2)
            return true;
        return false;
    }

    public bool OnCompleteActivity_Sys_Post_Cred_Appro(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        int applicationKey = 0;

        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IWorkflowAssignment WorkflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        applicationKey = common.GetApplicationKeyFromSourceInstanceID(messages, InstanceData.InstanceID);
        Readvance_Payments_Data.ApplicationKey = applicationKey;

        InstanceData.Name = applicationKey.ToString();

        InstanceData.Subject = common.GetCaseName(messages, applicationKey);
        // look for prev FL Proc Drole in prev map
        string s = string.Empty;
        WorkflowAssignment.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(messages, (long)InstanceData.SourceInstanceID, "FL Processor D", 157, out s);
        if (!string.IsNullOrEmpty(s))
        {
            WorkflowAssignment.ReassignCaseToUserByProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, s, 157, 857, "Setup Payment", SAHL.Common.Globals.Process.Origination);
        }
        else
        {
            WorkflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }

        string ADUserName = WorkflowAssignment.GetLastUserToWorkOnCaseAcrossInstances(messages, InstanceData.InstanceID, (Int64)InstanceData.SourceInstanceID, 921, "FL Supervisor D", "Readvance Payments");
        if (!string.IsNullOrEmpty(ADUserName))
        {
            WorkflowAssignment.ReassignCaseToUserByProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, ADUserName, 155, 921, "Rapid Decision", SAHL.Common.Globals.Process.Origination);
        }

        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Sys_Post_Cred_Appro(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Sys_Post_Readvance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        if (Readvance_Payments_Data.EntryPath == 1)
            return true;
        return false;
    }

    public bool OnCompleteActivity_Sys_Post_Readvance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        int applicationKey = 0;

        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        applicationKey = common.GetApplicationKeyFromSourceInstanceID(messages, InstanceData.InstanceID);
        Readvance_Payments_Data.ApplicationKey = applicationKey;
        InstanceData.Name = applicationKey.ToString();
        InstanceData.Subject = common.GetCaseName(messages, applicationKey);

        string s = string.Empty;
        string User = string.Empty;
        workflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Supervisor D", Readvance_Payments_Data.ApplicationKey, 155, InstanceData.InstanceID, "Rapid Decision", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor);

        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Sys_Post_Readvance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Sys_Appro_FL_and_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        if (Readvance_Payments_Data.EntryPath == 3)
            return true;
        return false;
    }

    public bool OnCompleteActivity_Sys_Appro_FL_and_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        int applicationKey = 0;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        applicationKey = common.GetApplicationKeyFromSourceInstanceID(messages, InstanceData.InstanceID);
        Readvance_Payments_Data.ApplicationKey = applicationKey;
        InstanceData.Name = applicationKey.ToString();
        InstanceData.Subject = common.GetCaseName(messages, applicationKey);

        return true;
    }

    public string GetStageTransition_Sys_Appro_FL_and_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Sys_Appro_FL_and_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Retry_Readvance_Payment(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Retry_Readvance_Payment(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        int applicationKey = 0;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        applicationKey = common.GetApplicationKeyFromSourceInstanceID(messages, InstanceData.InstanceID);
        Readvance_Payments_Data.ApplicationKey = applicationKey;
        InstanceData.Name = applicationKey.ToString();
        InstanceData.Subject = common.GetCaseName(messages, applicationKey);
        string User = string.Empty;
        workflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Supervisor D", Readvance_Payments_Data.ApplicationKey, 155, InstanceData.InstanceID, "Rapid Decision", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor);

        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Retry_Readvance_Payment(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Retry_Post_Cred_Create(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Retry_Post_Cred_Create(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        int applicationKey = 0;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        applicationKey = common.GetApplicationKeyFromSourceInstanceID(messages, InstanceData.InstanceID);
        Readvance_Payments_Data.ApplicationKey = applicationKey;
        InstanceData.Name = applicationKey.ToString();
        InstanceData.Subject = common.GetCaseName(messages, applicationKey);

        // look for prev FL Proc Drole in prev map
        string s = string.Empty;
        workflowAssignment.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(messages, (Int64)InstanceData.SourceInstanceID, "FL Processor D", 157, out s);
        if (!string.IsNullOrEmpty(s))
        {
            workflowAssignment.ReassignCaseToUserByProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, s, 157, 857, "Setup Payment", SAHL.Common.Globals.Process.Origination);
        }
        else
        {
            workflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }

        ///////////////////////////////////////////////////////////
        string ADUserName = workflowAssignment.GetLastUserToWorkOnCaseAcrossInstances(messages, InstanceData.InstanceID, (Int64)InstanceData.SourceInstanceID, 921, "FL Supervisor D", "Readvance Payments");
        if (!string.IsNullOrEmpty(ADUserName))
        {
            workflowAssignment.ReassignCaseToUserByProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, ADUserName, 155, 921, "Rapid Decision", SAHL.Common.Globals.Process.Origination);
        }

        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_Retry_Post_Cred_Create(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Retry_Appro_FL_and_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Retry_Appro_FL_and_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        int applicationKey = 0;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        applicationKey = common.GetApplicationKeyFromSourceInstanceID(messages, InstanceData.InstanceID);
        Readvance_Payments_Data.ApplicationKey = applicationKey;
        InstanceData.Name = applicationKey.ToString();
        InstanceData.Subject = common.GetCaseName(messages, applicationKey);

        return true;
    }

    public string GetStageTransition_Retry_Appro_FL_and_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Retry Appro FL and FA");
    }

    public string GetActivityMessage_Retry_Appro_FL_and_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnFollowUp(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        string s = string.Empty;
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        InstanceData.Subject = common.GetCaseName(messages, Readvance_Payments_Data.ApplicationKey);
        InstanceData.Name = Readvance_Payments_Data.ApplicationKey.ToString();
        return true;
    }

    public bool OnCompleteActivity_OnFollowUp(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        workflowAssignment.CloneActiveSecurityFromInstanceForInstance(messages, InstanceData.ParentInstanceID, InstanceData.InstanceID);
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnFollowUp(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_OnFollowUpReturn(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_OnFollowUpReturn(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    // You must select and business stage transition to provide code behind.
    public string GetActivityMessage_OnFollowUpReturn(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Rapid_Readvance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.IsReadvanceAdvance(messages, Readvance_Payments_Data.ApplicationKey);
    }

    public bool OnCompleteActivity_Rapid_Readvance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        DYs.Add("RV Manager D");
        DYs.Add("RV Admin D");
        workflowAssignment.DeActiveUsersForInstanceAndProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs, SAHL.Common.Globals.Process.Origination);

        string s = "";
        workflowAssignment.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct(messages, (Int64)InstanceData.SourceInstanceID, "FL Processor D", 157, out s);
        if (!string.IsNullOrEmpty(s))
        {
            workflowAssignment.ReassignCaseToUserByProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, s, 157, 857, "Setup Payment", SAHL.Common.Globals.Process.Origination);
        }
        else
        {
            string User = string.Empty;
            workflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        }

        ///////////////////////////////////////////////////////////
        string ADUserName = workflowAssignment.GetLastUserToWorkOnCaseAcrossInstances(messages, InstanceData.InstanceID, (Int64)InstanceData.SourceInstanceID, 921, "FL Supervisor D", "Readvance Payments");
        if (!string.IsNullOrEmpty(ADUserName))
        {
            workflowAssignment.ReassignCaseToUserByProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, ADUserName, 155, 921, "Rapid Decision", SAHL.Common.Globals.Process.Origination);
        }
        return true;
    }

    public string GetStageTransition_Rapid_Readvance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Rapid_Readvance(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Surety_check_for_Rapid(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.CheckSuretyForReAdvanceRules(messages, Readvance_Payments_Data.ApplicationKey, false);
    }

    public bool OnCompleteActivity_Surety_check_for_Rapid(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Surety_check_for_Rapid(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Surety check for Rapid");
    }

    public string GetActivityMessage_Surety_check_for_Rapid(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Surety_Check_for_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.CheckSuretyForReAdvanceRules(messages, Readvance_Payments_Data.ApplicationKey, false);
    }

    public bool OnCompleteActivity_Surety_Check_for_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        return true;
    }

    public string GetStageTransition_Surety_Check_for_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return ("Surety Check for FA");
    }

    public string GetActivityMessage_Surety_Check_for_FA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Suretyship_Signed(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Suretyship_Signed(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        furtherLending.SuretySignedConfirmed(messages, Readvance_Payments_Data.ApplicationKey);
        return false;
    }

    public string GetStageTransition_Suretyship_Signed(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Suretyship_Signed(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Request_Lightstone_Valuation(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Request_Lightstone_Valuation(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
        if (!common.CheckPropertyExists(messages, Readvance_Payments_Data.ApplicationKey))
        {
            return false;
        }

        if (!common.CheckLightStoneValuationRules(messages, Readvance_Payments_Data.ApplicationKey, Params.IgnoreWarning))
        {
            return false;
        }

        common.DoLightStoneValuationForWorkFlow(messages, Readvance_Payments_Data.ApplicationKey, Params.ADUserName);
        return true;

    }

    public string GetStageTransition_Request_Lightstone_Valuation(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Request_Lightstone_Valuation(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        //bool b = false;
        //IX2ReturnData ret = FL.SuperLoOptOutCheck(Readvance_Payments_Data.ApplicationKey, Params.IgnoreWarning, out b);
        //if (!b)
        // return ret;

        //return true;
        return true;
    }

    public bool OnCompleteActivity_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.OptOutSuperLo(messages, Readvance_Payments_Data.ApplicationKey, Params.ADUserName);
    }

    public string GetStageTransition_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Cancel_Opt_Out_Request(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Cancel_Opt_Out_Request(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.CheckSuperLoOptOutRequiredRules(messages, Readvance_Payments_Data.ApplicationKey, false);
    }

    public string GetStageTransition_Cancel_Opt_Out_Request(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Cancel_Opt_Out_Request(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Require_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.CheckSuperLoOptOutRequiredRules(messages, Readvance_Payments_Data.ApplicationKey, false);
    }

    public bool OnCompleteActivity_Require_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();

        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        workflowAssignment.DeActiveUsersForInstanceAndProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs, SAHL.Common.Globals.Process.Origination);

        workflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Supervisor D", Readvance_Payments_Data.ApplicationKey, 155, InstanceData.InstanceID, "Require SuperLo Opt-Out", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLSupervisor);
        workflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Manager D", Readvance_Payments_Data.ApplicationKey, 174, InstanceData.InstanceID, "Require SuperLo Opt-Out", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLManager);
        return true;
    }

    public string GetStageTransition_Require_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Require_Opt_Out_SuperLo(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Further_Advance_Below_LAA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        IFL furtherLending = DomainServiceLoader.Instance.Get<IFL>();
        return furtherLending.CheckIsFurtherAdvanceBelowLAARules(messages, Readvance_Payments_Data.ApplicationKey, false);
    }

    public bool OnCompleteActivity_Further_Advance_Below_LAA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        List<string> DYs = new List<string>();
        DYs.Add("FL Processor D");
        DYs.Add("FL Supervisor D");
        DYs.Add("FL Manager D");
        DYs.Add("RV Manager D");
        DYs.Add("RV Admin D");
        workflowAssignment.DeActiveUsersForInstanceAndProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs, SAHL.Common.Globals.Process.Origination);
        workflowAssignment.ReactiveUserOrRoundRobinForOSKeyByProcess(messages, "FL Processor D", Readvance_Payments_Data.ApplicationKey, 157, InstanceData.InstanceID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        return true;
    }

    public string GetStageTransition_Further_Advance_Below_LAA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Further_Advance_Below_LAA(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public bool OnStartActivity_Disbursement_Incorrect(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return true;
    }

    public bool OnCompleteActivity_Disbursement_Incorrect(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, ref string Message)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        List<string> DYs = new List<string>();
        DYs.Add("FL Supervisor D");
        workflowAssignment.DeActiveUsersForInstanceAndProcess(messages, InstanceData.InstanceID, Readvance_Payments_Data.ApplicationKey, DYs, SAHL.Common.Globals.Process.Origination);
        workflowAssignment.ReactiveUserOrRoundRobinForOSKeysByProcess(messages, "FL Processor D", Readvance_Payments_Data.ApplicationKey, null, InstanceData.InstanceID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor);
        return true;
    }

    public string GetStageTransition_Disbursement_Incorrect(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    public string GetActivityMessage_Disbursement_Incorrect(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params)
    {
        return string.Empty;
    }

    #endregion Activities

    #region Roles

    public string OnGetRole_Readvance_Payments_FL_Processor_D(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, string RoleName)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = workflowAssignment.ResolveDynamicRoleToUserName(messages, "FL Processor D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Readvance_Payments_FL_Supervisor_D(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, string RoleName)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = workflowAssignment.ResolveDynamicRoleToUserName(messages, "FL Supervisor D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Readvance_Payments_FL_Manager_D(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, string RoleName)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = workflowAssignment.ResolveDynamicRoleToUserName(messages, "FL Manager D", InstanceData.InstanceID);
        return s;
    }

    public string OnGetRole_Readvance_Payments_RV_Admin_D(IDomainMessageCollection messages, IX2Readvance_Payments_Data Readvance_Payments_Data, IX2InstanceData InstanceData, IX2Params Params, string RoleName)
    {
        IWorkflowAssignment workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
        string s = string.Empty;
        s = workflowAssignment.ResolveDynamicRoleToUserName(messages, "RV Admin D", InstanceData.InstanceID);
        return s;
    }

    #endregion Roles

    #region IX2WorkFlow Members

    public bool OnEnterState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
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

    public bool OnEnterStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
    {
        switch (Params.StateName)
        {
            case "Rapid Decision":
                return OnEnter_Rapid_Decision(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Contact Client":
                return OnEnter_Contact_Client(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Send Schedule":
                return OnEnter_Send_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Awaiting Schedule":
                return OnEnter_Awaiting_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Setup Payment":
                return OnEnter_Setup_Payment(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disburse Funds":
                return OnEnter_Disburse_Funds(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Folder Archive":
                return OnEnter_Folder_Archive(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Next Step":
                return OnEnter_Next_Step(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disbursed":
                return OnEnter_Disbursed(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Further Loan":
                return OnEnter_Archive_Further_Loan(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Reassign":
                return OnEnter_Common_Reassign(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common NTU":
                return OnEnter_Common_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Decline":
                return OnEnter_Common_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return OnEnter_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return OnEnter_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Return to sender":
                return OnEnter_Return_to_sender(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Decline":
                return OnEnter_Archive_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive NTU":
                return OnEnter_Archive_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Followup":
                return OnEnter_Common_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Followup Hold":
                return OnEnter_Followup_Hold(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Followup Complete":
                return OnEnter_Followup_Complete(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Ready To Followup":
                return OnEnter_Ready_To_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Archive Followup":
                return OnEnter_Common_Archive_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive":
                return OnEnter_Archive(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Return to Processor":
                return OnEnter_Common_Return_to_Processor(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Processor":
                return OnEnter_Archive_Processor(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Create Fail":
                return OnEnter_Readvance_Create_Fail(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "State29":
                return OnEnter_State29(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "FollowupReturn":
                return OnEnter_FollowupReturn(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Deed of Surety Instruction":
                return OnEnter_Deed_of_Surety_Instruction(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Lightstone AVM":
                return OnEnter_Common_Lightstone_AVM(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive FL SuperLo OptOut":
                return OnEnter_Archive_FL_SuperLo_OptOut(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Opt-Out SuperLo":
                return OnEnter_Common_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Require SuperLo Opt-Out":
                return OnEnter_Require_SuperLo_Opt_Out(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OptOut Return":
                return OnEnter_OptOut_Return(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            default:
                return false;
        }
    }

    public bool OnExitState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
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

    public bool OnExitStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
    {
        switch (Params.StateName)
        {
            case "Rapid Decision":
                return OnExit_Rapid_Decision(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Contact Client":
                return OnExit_Contact_Client(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Send Schedule":
                return OnExit_Send_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Awaiting Schedule":
                return OnExit_Awaiting_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Setup Payment":
                return OnExit_Setup_Payment(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disburse Funds":
                return OnExit_Disburse_Funds(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Folder Archive":
                return OnExit_Folder_Archive(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Next Step":
                return OnExit_Next_Step(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disbursed":
                return OnExit_Disbursed(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Further Loan":
                return OnExit_Archive_Further_Loan(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Reassign":
                return OnExit_Common_Reassign(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common NTU":
                return OnExit_Common_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Decline":
                return OnExit_Common_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return OnExit_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return OnExit_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Return to sender":
                return OnExit_Return_to_sender(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Decline":
                return OnExit_Archive_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive NTU":
                return OnExit_Archive_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Followup":
                return OnExit_Common_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Followup Hold":
                return OnExit_Followup_Hold(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Followup Complete":
                return OnExit_Followup_Complete(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Ready To Followup":
                return OnExit_Ready_To_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Archive Followup":
                return OnExit_Common_Archive_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive":
                return OnExit_Archive(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Return to Processor":
                return OnExit_Common_Return_to_Processor(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Processor":
                return OnExit_Archive_Processor(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Create Fail":
                return OnExit_Readvance_Create_Fail(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "State29":
                return OnExit_State29(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "FollowupReturn":
                return OnExit_FollowupReturn(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Deed of Surety Instruction":
                return OnExit_Deed_of_Surety_Instruction(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Lightstone AVM":
                return OnExit_Common_Lightstone_AVM(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive FL SuperLo OptOut":
                return OnExit_Archive_FL_SuperLo_OptOut(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Common Opt-Out SuperLo":
                return OnExit_Common_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Require SuperLo Opt-Out":
                return OnExit_Require_SuperLo_Opt_Out(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OptOut Return":
                return OnExit_OptOut_Return(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            default:
                return false;
        }
    }

    public bool OnReturnState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
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

    public bool OnReturnStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
    {
        switch (Params.StateName)
        {
            case "Folder Archive":
                return OnReturn_Folder_Archive(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Further Loan":
                return OnReturn_Archive_Further_Loan(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Decline":
                return OnReturn_Archive_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive NTU":
                return OnReturn_Archive_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Followup Complete":
                return OnReturn_Followup_Complete(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive":
                return OnReturn_Archive(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Processor":
                return OnReturn_Archive_Processor(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive FL SuperLo OptOut":
                return OnReturn_Archive_FL_SuperLo_OptOut(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            default: return false;
        }
    }

    public string GetForwardStateName(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
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

    public string GetForwardStateNameInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
    {
        switch (Params.StateName)
        {
            case "Return to sender":
                return GetForwardStateName_Return_to_sender(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "FollowupReturn":
                return GetForwardStateName_FollowupReturn(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OptOut Return":
                return GetForwardStateName_OptOut_Return(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            default:
                return string.Empty;
        }
    }

    public bool OnStartActivity(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
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

    public bool OnStartActivityInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
    {
        switch (Params.ActivityName)
        {
            case "Readvance Payment":
                return OnStartActivity_Readvance_Payment(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Inform Client":
                return OnStartActivity_Inform_Client(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Advance":
                return OnStartActivity_Further_Advance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Send Schedule":
                return OnStartActivity_Send_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Receive Schedule":
                return OnStartActivity_Receive_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Payment Prepared":
                return OnStartActivity_Payment_Prepared(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Approve Rapid":
                return OnStartActivity_Approve_Rapid(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Complete":
                return OnStartActivity_Disbursement_Complete(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Refer to Credit":
                return OnStartActivity_Refer_to_Credit(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Loan":
                return OnStartActivity_Further_Loan(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Rollback Disbursement":
                return OnStartActivity_Rollback_Disbursement(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "12hrs":
                return OnStartActivity_12hrs(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Approve App - FA & FL":
                return OnStartActivity_Approve_App___FA___FL(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reassign User":
                return OnStartActivity_Reassign_User(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU Timeout":
                return OnStartActivity_NTU_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline Timeout":
                return OnStartActivity_Decline_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate NTU":
                return OnStartActivity_Reinstate_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Decline":
                return OnStartActivity_Reinstate_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return OnStartActivity_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return OnStartActivity_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Complete Followup":
                return OnStartActivity_Complete_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Update Followup":
                return OnStartActivity_Update_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Create Followup":
                return OnStartActivity_Create_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowup":
                return OnStartActivity_OnFollowup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Continue with Followup":
                return OnStartActivity_Continue_with_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Followup":
                return OnStartActivity_Reinstate_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Complete Followup":
                return OnStartActivity_Archive_Complete_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline Final":
                return OnStartActivity_Decline_Final(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU Final":
                return OnStartActivity_NTU_Final(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline by Credit":
                return OnStartActivity_Decline_by_Credit(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "EXT_ArchiveFollowup":
                return OnStartActivity_EXT_ArchiveFollowup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Post Credit Appro":
                return OnStartActivity_Readvance_Post_Credit_Appro(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Return to Manage Application":
                return OnStartActivity_Return_to_Manage_Application(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "12 Hour Override":
                return OnStartActivity_12_Hour_Override(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Sys Post Cred Appro":
                return OnStartActivity_Sys_Post_Cred_Appro(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Sys Post Readvance":
                return OnStartActivity_Sys_Post_Readvance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Sys Appro FL and FA":
                return OnStartActivity_Sys_Appro_FL_and_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Retry Readvance Payment":
                return OnStartActivity_Retry_Readvance_Payment(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Retry Post Cred Create":
                return OnStartActivity_Retry_Post_Cred_Create(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Retry Appro FL and FA":
                return OnStartActivity_Retry_Appro_FL_and_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowUp":
                return OnStartActivity_OnFollowUp(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowUpReturn":
                return OnStartActivity_OnFollowUpReturn(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Readvance":
                return OnStartActivity_Rapid_Readvance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Surety check for Rapid":
                return OnStartActivity_Surety_check_for_Rapid(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Surety Check for FA":
                return OnStartActivity_Surety_Check_for_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Suretyship Signed":
                return OnStartActivity_Suretyship_Signed(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Request Lightstone Valuation":
                return OnStartActivity_Request_Lightstone_Valuation(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Opt-Out SuperLo":
                return OnStartActivity_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Cancel Opt-Out Request":
                return OnStartActivity_Cancel_Opt_Out_Request(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Require Opt-Out SuperLo":
                return OnStartActivity_Require_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Advance Below LAA":
                return OnStartActivity_Further_Advance_Below_LAA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Incorrect":
                return OnStartActivity_Disbursement_Incorrect(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            default:
                return false;
        }
    }

    public bool OnCompleteActivity(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, ref string AlertMessage)
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

    public bool OnCompleteActivityInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, ref string AlertMessage)
    {
        switch (Params.ActivityName)
        {
            case "Readvance Payment":
                return OnCompleteActivity_Readvance_Payment(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Inform Client":
                return OnCompleteActivity_Inform_Client(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Further Advance":
                return OnCompleteActivity_Further_Advance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Send Schedule":
                return OnCompleteActivity_Send_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Receive Schedule":
                return OnCompleteActivity_Receive_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Payment Prepared":
                return OnCompleteActivity_Payment_Prepared(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Approve Rapid":
                return OnCompleteActivity_Approve_Rapid(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Disbursement Complete":
                return OnCompleteActivity_Disbursement_Complete(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Refer to Credit":
                return OnCompleteActivity_Refer_to_Credit(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Further Loan":
                return OnCompleteActivity_Further_Loan(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Rollback Disbursement":
                return OnCompleteActivity_Rollback_Disbursement(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "12hrs":
                return OnCompleteActivity_12hrs(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Approve App - FA & FL":
                return OnCompleteActivity_Approve_App___FA___FL(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reassign User":
                return OnCompleteActivity_Reassign_User(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "NTU Timeout":
                return OnCompleteActivity_NTU_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline Timeout":
                return OnCompleteActivity_Decline_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reinstate NTU":
                return OnCompleteActivity_Reinstate_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reinstate Decline":
                return OnCompleteActivity_Reinstate_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline":
                return OnCompleteActivity_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "NTU":
                return OnCompleteActivity_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Complete Followup":
                return OnCompleteActivity_Complete_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Update Followup":
                return OnCompleteActivity_Update_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Create Followup":
                return OnCompleteActivity_Create_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnFollowup":
                return OnCompleteActivity_OnFollowup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Continue with Followup":
                return OnCompleteActivity_Continue_with_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Reinstate Followup":
                return OnCompleteActivity_Reinstate_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Archive Complete Followup":
                return OnCompleteActivity_Archive_Complete_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline Final":
                return OnCompleteActivity_Decline_Final(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "NTU Final":
                return OnCompleteActivity_NTU_Final(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Decline by Credit":
                return OnCompleteActivity_Decline_by_Credit(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "EXT_ArchiveFollowup":
                return OnCompleteActivity_EXT_ArchiveFollowup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Readvance Post Credit Appro":
                return OnCompleteActivity_Readvance_Post_Credit_Appro(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Return to Manage Application":
                return OnCompleteActivity_Return_to_Manage_Application(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "12 Hour Override":
                return OnCompleteActivity_12_Hour_Override(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Sys Post Cred Appro":
                return OnCompleteActivity_Sys_Post_Cred_Appro(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Sys Post Readvance":
                return OnCompleteActivity_Sys_Post_Readvance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Sys Appro FL and FA":
                return OnCompleteActivity_Sys_Appro_FL_and_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Retry Readvance Payment":
                return OnCompleteActivity_Retry_Readvance_Payment(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Retry Post Cred Create":
                return OnCompleteActivity_Retry_Post_Cred_Create(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Retry Appro FL and FA":
                return OnCompleteActivity_Retry_Appro_FL_and_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnFollowUp":
                return OnCompleteActivity_OnFollowUp(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "OnFollowUpReturn":
                return OnCompleteActivity_OnFollowUpReturn(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Rapid Readvance":
                return OnCompleteActivity_Rapid_Readvance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Surety check for Rapid":
                return OnCompleteActivity_Surety_check_for_Rapid(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Surety Check for FA":
                return OnCompleteActivity_Surety_Check_for_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Suretyship Signed":
                return OnCompleteActivity_Suretyship_Signed(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Request Lightstone Valuation":
                return OnCompleteActivity_Request_Lightstone_Valuation(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Opt-Out SuperLo":
                return OnCompleteActivity_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Cancel Opt-Out Request":
                return OnCompleteActivity_Cancel_Opt_Out_Request(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Require Opt-Out SuperLo":
                return OnCompleteActivity_Require_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Further Advance Below LAA":
                return OnCompleteActivity_Further_Advance_Below_LAA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            case "Disbursement Incorrect":
                return OnCompleteActivity_Disbursement_Incorrect(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
            default:
                return false;
        }
    }

    public DateTime GetActivityTime(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
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

    public DateTime GetActivityTimeInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
    {
        switch (Params.ActivityName)
        {
            case "12hrs":
                return GetActivityTime_12hrs(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU Timeout":
                return GetActivityTime_NTU_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline Timeout":
                return GetActivityTime_Decline_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowup":
                return GetActivityTime_OnFollowup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Complete Followup":
                return GetActivityTime_Archive_Complete_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            default:
                return DateTime.MinValue;
        }
    }

    public string GetStageTransition(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
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

    public string GetStageTransitionInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
    {
        switch (Params.ActivityName)
        {
            case "Inform Client":
                return GetStageTransition_Inform_Client(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Advance":
                return GetStageTransition_Further_Advance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Send Schedule":
                return GetStageTransition_Send_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Receive Schedule":
                return GetStageTransition_Receive_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Payment Prepared":
                return GetStageTransition_Payment_Prepared(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Approve Rapid":
                return GetStageTransition_Approve_Rapid(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Complete":
                return GetStageTransition_Disbursement_Complete(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Refer to Credit":
                return GetStageTransition_Refer_to_Credit(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Loan":
                return GetStageTransition_Further_Loan(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Rollback Disbursement":
                return GetStageTransition_Rollback_Disbursement(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "12hrs":
                return GetStageTransition_12hrs(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reassign User":
                return GetStageTransition_Reassign_User(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU Timeout":
                return GetStageTransition_NTU_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline Timeout":
                return GetStageTransition_Decline_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate NTU":
                return GetStageTransition_Reinstate_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Decline":
                return GetStageTransition_Reinstate_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return GetStageTransition_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return GetStageTransition_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline Final":
                return GetStageTransition_Decline_Final(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU Final":
                return GetStageTransition_NTU_Final(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Sys Appro FL and FA":
                return GetStageTransition_Sys_Appro_FL_and_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Retry Appro FL and FA":
                return GetStageTransition_Retry_Appro_FL_and_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Readvance":
                return GetStageTransition_Rapid_Readvance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Surety check for Rapid":
                return GetStageTransition_Surety_check_for_Rapid(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Surety Check for FA":
                return GetStageTransition_Surety_Check_for_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Suretyship Signed":
                return GetStageTransition_Suretyship_Signed(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Request Lightstone Valuation":
                return GetStageTransition_Request_Lightstone_Valuation(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Opt-Out SuperLo":
                return GetStageTransition_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Cancel Opt-Out Request":
                return GetStageTransition_Cancel_Opt_Out_Request(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Require Opt-Out SuperLo":
                return GetStageTransition_Require_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Advance Below LAA":
                return GetStageTransition_Further_Advance_Below_LAA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Incorrect":
                return GetStageTransition_Disbursement_Incorrect(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            default:
                return string.Empty;
        }
    }

    public string GetActivityMessage(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
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

    public string GetActivityMessageInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
    {
        switch (Params.ActivityName)
        {
            case "Readvance Payment":
                return GetActivityMessage_Readvance_Payment(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Inform Client":
                return GetActivityMessage_Inform_Client(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Advance":
                return GetActivityMessage_Further_Advance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Send Schedule":
                return GetActivityMessage_Send_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Receive Schedule":
                return GetActivityMessage_Receive_Schedule(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Payment Prepared":
                return GetActivityMessage_Payment_Prepared(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Approve Rapid":
                return GetActivityMessage_Approve_Rapid(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Complete":
                return GetActivityMessage_Disbursement_Complete(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Refer to Credit":
                return GetActivityMessage_Refer_to_Credit(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Loan":
                return GetActivityMessage_Further_Loan(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Rollback Disbursement":
                return GetActivityMessage_Rollback_Disbursement(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "12hrs":
                return GetActivityMessage_12hrs(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Approve App - FA & FL":
                return GetActivityMessage_Approve_App___FA___FL(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reassign User":
                return GetActivityMessage_Reassign_User(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU Timeout":
                return GetActivityMessage_NTU_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline Timeout":
                return GetActivityMessage_Decline_Timeout(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate NTU":
                return GetActivityMessage_Reinstate_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Decline":
                return GetActivityMessage_Reinstate_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline":
                return GetActivityMessage_Decline(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU":
                return GetActivityMessage_NTU(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Complete Followup":
                return GetActivityMessage_Complete_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Update Followup":
                return GetActivityMessage_Update_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Create Followup":
                return GetActivityMessage_Create_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowup":
                return GetActivityMessage_OnFollowup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Continue with Followup":
                return GetActivityMessage_Continue_with_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Reinstate Followup":
                return GetActivityMessage_Reinstate_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Archive Complete Followup":
                return GetActivityMessage_Archive_Complete_Followup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline Final":
                return GetActivityMessage_Decline_Final(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "NTU Final":
                return GetActivityMessage_NTU_Final(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Decline by Credit":
                return GetActivityMessage_Decline_by_Credit(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "EXT_ArchiveFollowup":
                return GetActivityMessage_EXT_ArchiveFollowup(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Readvance Post Credit Appro":
                return GetActivityMessage_Readvance_Post_Credit_Appro(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Return to Manage Application":
                return GetActivityMessage_Return_to_Manage_Application(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "12 Hour Override":
                return GetActivityMessage_12_Hour_Override(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Sys Post Cred Appro":
                return GetActivityMessage_Sys_Post_Cred_Appro(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Sys Post Readvance":
                return GetActivityMessage_Sys_Post_Readvance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Sys Appro FL and FA":
                return GetActivityMessage_Sys_Appro_FL_and_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Retry Readvance Payment":
                return GetActivityMessage_Retry_Readvance_Payment(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Retry Post Cred Create":
                return GetActivityMessage_Retry_Post_Cred_Create(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Retry Appro FL and FA":
                return GetActivityMessage_Retry_Appro_FL_and_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowUp":
                return GetActivityMessage_OnFollowUp(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "OnFollowUpReturn":
                return GetActivityMessage_OnFollowUpReturn(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Rapid Readvance":
                return GetActivityMessage_Rapid_Readvance(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Surety check for Rapid":
                return GetActivityMessage_Surety_check_for_Rapid(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Surety Check for FA":
                return GetActivityMessage_Surety_Check_for_FA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Suretyship Signed":
                return GetActivityMessage_Suretyship_Signed(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Request Lightstone Valuation":
                return GetActivityMessage_Request_Lightstone_Valuation(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Opt-Out SuperLo":
                return GetActivityMessage_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Cancel Opt-Out Request":
                return GetActivityMessage_Cancel_Opt_Out_Request(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Require Opt-Out SuperLo":
                return GetActivityMessage_Require_Opt_Out_SuperLo(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Further Advance Below LAA":
                return GetActivityMessage_Further_Advance_Below_LAA(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            case "Disbursement Incorrect":
                return GetActivityMessage_Disbursement_Incorrect(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params);
            default:
                return string.Empty;
        }
    }

    public string GetDynamicRole(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, string RoleName, string WorkflowName)
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

    public string GetDynamicRoleInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, string RoleName, string WorkflowName)
    {
        switch (RoleName.Replace(' ', '_') + "_" + WorkflowName.Replace(' ', '_'))
        {
            case "FL_Processor_D_Readvance_Payments":
                return OnGetRole_Readvance_Payments_FL_Processor_D(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "FL_Supervisor_D_Readvance_Payments":
                return OnGetRole_Readvance_Payments_FL_Supervisor_D(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "FL_Manager_D_Readvance_Payments":
                return OnGetRole_Readvance_Payments_FL_Manager_D(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, RoleName);
            case "RV_Admin_D_Readvance_Payments":
                return OnGetRole_Readvance_Payments_RV_Admin_D(messages, (IX2Readvance_Payments_Data)WorkFlowData, InstanceData, Params, RoleName);
            default:
                return string.Empty;
        }
    }

    public IX2WorkFlowDataProvider GetWorkFlowDataProvider()
    {
        return new X2Readvance_Payments_Data();
    }

    #endregion IX2WorkFlow Members
}

#endregion WorkFlow Readvance Payments
