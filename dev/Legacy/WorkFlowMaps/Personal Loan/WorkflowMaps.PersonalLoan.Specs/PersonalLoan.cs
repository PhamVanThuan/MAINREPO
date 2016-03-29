using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using SAHL.X2.Framework;
using SAHL.X2.Framework.Common;
using SAHL.X2.Framework.Interfaces;
using SAHL.X2.Framework.DataAccess;
using SAHL.X2.Framework.ServiceFacade;
using SAHL.X2.Framework.DataSets;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.Logging;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Logging;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.PersonalLoan;
using DomainService2.IOC;
using X2DomainService.Interface.WorkflowAssignment;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

	#region Process

	public class Process : MarshalByRefObject, IX2Process
	{
		public Process()
		{
            string log4netconfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\log4net.config");
            FileInfo fi = new FileInfo(log4netconfig);
            log4net.Config.XmlConfigurator.Configure(fi);
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
		
		public IX2WorkFlow GetWorkFlow(string WorkFlowName)
		{
			switch(WorkFlowName)
			{
				case "Personal Loans":
					return new X2Personal_Loans();
				default:
					return null;
			}
		}
		
		public string GetDynamicRole(string RoleName, IActiveDataTransaction Tran)
		{
			switch(RoleName)
			{
				default:
					return null;
			}
		}

		#region Process Roles


		#endregion
	}
	
	#endregion
	
	#region WorkFlowData Personal Loans
	
	public interface IX2Personal_Loans_Data : IX2WorkFlowDataProvider
	{
			
		Int32 ApplicationKey { get; set; }
			
		String PreviousState { get; set; }
	
	}
	
	public class X2Personal_Loans_Data : MarshalByRefObject, IX2Personal_Loans_Data
	{
		private bool m_HasChanges = false;
		private Dictionary<string, string> m_dataFields;
		
		public X2Personal_Loans_Data()
		{
			this.m_dataFields = new Dictionary<string, string>();
			this.m_dataFields.Add("applicationkey", "SqlDbType.Int");
			this.m_dataFields.Add("previousstate", "SqlDbType.VarChar");
			
		}
		
		public override object InitializeLifetimeService()
		{
			return null;
		}
	
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
	
		#region IX2WorkFlowDataProvider Members

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
					}
				}
			}
		}

		public Dictionary<string, string> GetDataFields()
		{
			Dictionary<string, string> Data = new Dictionary<string, string>();
			Data.Add( "applicationkey", m_ApplicationKey.ToString());
			if(m_PreviousState != null)
				Data.Add( "previousstate", m_PreviousState.ToString());
			return Data;
		}
		
		public void SetDataField(string FieldName, object value)
		{
			switch (FieldName.ToLower())
			{		
						case "applicationkey":
							ApplicationKey = Convert.ToInt32(value);
							break;
						case "previousstate":
							PreviousState = Convert.ToString(value);
							break;
			}
		}
		
		public object GetDataField(string FieldName)
		{
			switch (FieldName.ToLower())
			{
				case "applicationkey":
					return m_ApplicationKey;
				case "previousstate":
					return m_PreviousState;
			
				default:
					return null;
			}
		}
		
		public Dictionary<string, string> DataFields
		{
			get
			{
				return m_dataFields;
			}
		}
		
		public string DataProviderName
		{
			get
			{
				return "Personal_Loans";
			}
		}
		
		public bool HasChanges
		{
			get
			{
				return m_HasChanges;
			}
		}
		
		public bool Contains(string FieldName)
		{
			switch (FieldName.ToLower())
			{
				case "applicationkey":
					return true;
				case "previousstate":
					return true;
					
				default:
					return false;
			}
		}



	#endregion
}

	#endregion	

	#region WorkFlow Personal Loans

	public class X2Personal_Loans : MarshalByRefObject, IX2WorkFlow
	{

		public X2Personal_Loans()
		{
			// initialise the loader
			var loader = DomainServiceLoader.Instance;
		}
		
		#region States

		public bool OnEnter_Manage_Lead(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Manage_Lead(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnEnter_Manage_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Manage_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			Personal_Loans_Data.PreviousState = "Manage Application";
			return true;
		}

		public bool OnEnter_Credit(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{	
			return true;
		}

		public bool OnExit_Credit(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnEnter_Disbursement(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Disbursement(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			Personal_Loans_Data.PreviousState = "Disbursement";
			return true;
		}

		public bool OnEnter_Archive_Disbursed(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Archive_Disbursed(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnReturn_Archive_Disbursed(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		  return true;
		}

		public bool OnEnter_Common_Rework(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Common_Rework(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnEnter_Declined_by_Credit(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Declined_by_Credit(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnEnter_Common_Return(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Common_Return(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnEnter_Legal_Agreements(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Legal_Agreements(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			Personal_Loans_Data.PreviousState = "Legal Agreements";
			return true;
		}

		public bool OnEnter_Common_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Common_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnEnter_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnEnter_Return_to_State(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Return_to_State(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public string GetForwardStateName_Return_to_State(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			IWorkflowAssignment  wfa = DomainServiceLoader.Instance.Get<IWorkflowAssignment >();
			
			switch(Personal_Loans_Data.PreviousState)
			{
				case "Disbursement":
					wfa.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLSupervisorD, Personal_Loans_Data.ApplicationKey, InstanceData.InstanceID, SAHL.Common.Globals.RoundRobinPointers.PLSupervisor);
					break;
			
				case "Legal Agreements":
					wfa.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLAdminD, Personal_Loans_Data.ApplicationKey, InstanceData.InstanceID, SAHL.Common.Globals.RoundRobinPointers.PLAdmin);	
					break;
			}
				
			return Personal_Loans_Data.PreviousState;
		}

		public bool OnEnter_Archive_NTU_Declines(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnExit_Archive_NTU_Declines(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnReturn_Archive_NTU_Declines(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		  return true;
		}

		#endregion

		#region Activities

		public bool OnStartActivity_Rework_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Rework_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
		return true;
		}

		public string GetStageTransition_Rework_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Rework_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Calculate_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Calculate_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
		return true;
		}

		public string GetStageTransition_Calculate_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		  return string.Empty;
		}

		public string GetActivityMessage_Calculate_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Application_in_Order(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Application_in_Order(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			IPersonalLoan personalloanClient = DomainServiceLoader.Instance.Get<IPersonalLoan>();
			IWorkflowAssignment  wfa = DomainServiceLoader.Instance.Get<IWorkflowAssignment >();
			bool applicationInOrder = false;
		 	applicationInOrder = personalloanClient.CheckCreditSubmissionRules(messages, Personal_Loans_Data.ApplicationKey, true);
			
			if(applicationInOrder)
				Message = wfa.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLCreditAnalystD, Personal_Loans_Data.ApplicationKey, InstanceData.InstanceID, SAHL.Common.Globals.RoundRobinPointers.PLCreditAnalyst);	
			
			return applicationInOrder;
		}

		public string GetStageTransition_Application_in_Order(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Application_in_Order(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Approve(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Approve(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			//set offerinformation to accepted
			var personalLoansHost = DomainServiceLoader.Instance.Get<IPersonalLoan>();
			personalLoansHost.UpdateOfferInformationType(messages, Personal_Loans_Data.ApplicationKey, SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer);
			//do assignment
			var workflowAssigment = DomainServiceLoader.Instance.Get<X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment>();
			workflowAssigment.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLAdminD, Personal_Loans_Data.ApplicationKey, InstanceData.InstanceID, SAHL.Common.Globals.RoundRobinPointers.PLAdmin);	
			
			return true;
		}

		public string GetStageTransition_Approve(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Approve(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Decline(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Decline(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment workflowAssigment = DomainServiceLoader.Instance.Get<X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment>();
			workflowAssigment.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, Personal_Loans_Data.ApplicationKey, InstanceData.InstanceID, SAHL.Common.Globals.RoundRobinPointers.PLConsultant);	
			return true;
		}

		public string GetStageTransition_Decline(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Decline(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Decline_Finalised(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Decline_Finalised(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
			common.UpdateOfferStatus(messages,Personal_Loans_Data.ApplicationKey,(int)SAHL.Common.Globals.OfferStatuses.Declined,-1);
			return true;
		}

		public string GetStageTransition_Decline_Finalised(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Decline_Finalised(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Disburse_Funds(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Disburse_Funds(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
		return true;
		}

		public string GetStageTransition_Disburse_Funds(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Disburse_Funds(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment workflowAssigment = DomainServiceLoader.Instance.Get<X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment>();
			workflowAssigment.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, Personal_Loans_Data.ApplicationKey, InstanceData.InstanceID, SAHL.Common.Globals.RoundRobinPointers.PLConsultant);	
			return true;
		}

		public string GetStageTransition_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Return_to_Manage_Application(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Create_Personal_Loan_Lead(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
		return true;
		}

		public bool OnCompleteActivity_Create_Personal_Loan_Lead(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			IPersonalLoan personalloanClient = DomainServiceLoader.Instance.Get<IPersonalLoan>();
			X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment workflowAssigment = DomainServiceLoader.Instance.Get<X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment>();
			InstanceData.Name = Personal_Loans_Data.ApplicationKey.ToString();
			InstanceData.Subject = personalloanClient.GetInstanceSubjectForPersonalLoan(messages, Personal_Loans_Data.ApplicationKey);
			workflowAssigment.AssignWorkflowRoleForADUser(messages, InstanceData.InstanceID, Params.ADUserName,SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD,Personal_Loans_Data.ApplicationKey,"");
			return true;
		}

		public string GetStageTransition_Create_Personal_Loan_Lead(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Documents_Verified(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			
			return true;
		}

		public bool OnCompleteActivity_Documents_Verified(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			IWorkflowAssignment  wfa = DomainServiceLoader.Instance.Get<IWorkflowAssignment >();
			Message = wfa.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLSupervisorD, Personal_Loans_Data.ApplicationKey, InstanceData.InstanceID, SAHL.Common.Globals.RoundRobinPointers.PLSupervisor);	
			return true;
		}

		public string GetStageTransition_Documents_Verified(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Documents_Verified(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment workflowAssigment = DomainServiceLoader.Instance.Get<X2DomainService.Interface.WorkflowAssignment.IWorkflowAssignment>();
			workflowAssigment.ReactivateUserOrRoundRobinForWorkflowRoleAssignment(messages, SAHL.Common.Globals.GenericKeyTypes.WorkflowRoleType, SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, Personal_Loans_Data.ApplicationKey, InstanceData.InstanceID, SAHL.Common.Globals.RoundRobinPointers.PLConsultant);	
			return true;
		}

		public string GetStageTransition_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Reinstate_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Reinstate_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
		return true;
		}

		public string GetStageTransition_Reinstate_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Reinstate_NTU(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_NTU_Finalised(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
			common.UpdateOfferStatus(messages,Personal_Loans_Data.ApplicationKey,(int)SAHL.Common.Globals.OfferStatuses.NTU,-1);
			return true;
		}

		public bool OnCompleteActivity_NTU_Finalised(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
		return true;
		}

		public string GetStageTransition_NTU_Finalised(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_NTU_Finalised(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_NTU_Timer(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_NTU_Timer(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			ICommon common = DomainServiceLoader.Instance.Get<ICommon>();
			common.UpdateOfferStatus(messages,Personal_Loans_Data.ApplicationKey,(int)SAHL.Common.Globals.OfferStatuses.NTU,-1);
			return true;
		}

		public string GetStageTransition_NTU_Timer(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public DateTime GetActivityTime_NTU_Timer(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return DateTime.Now.AddDays(30);
		}

		public bool OnStartActivity_Send_Documents(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params)
		{
			return true;
		}

		public bool OnCompleteActivity_Send_Documents(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
		return true;
		}

		public string GetStageTransition_Send_Documents(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Send_Documents(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public bool OnStartActivity_Send_Offer(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			IPersonalLoan personalloanClient = DomainServiceLoader.Instance.Get<IPersonalLoan>();
			IWorkflowAssignment  wfa = DomainServiceLoader.Instance.Get<IWorkflowAssignment >();
			bool canSendOffer = false;
		 	canSendOffer = personalloanClient.CheckSendOfferRules(messages, Personal_Loans_Data.ApplicationKey, Params.IgnoreWarning);		
			return canSendOffer;
		}

		public bool OnCompleteActivity_Send_Offer(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params, ref string Message)
		{
			return true;
		}

		public string GetStageTransition_Send_Offer(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		public string GetActivityMessage_Send_Offer(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params)
		{
			return string.Empty;
		}

		#endregion

		#region Roles

		public string OnGetRole_Personal_Loans_PL_Supervisor_D(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, string RoleName)
		{
			string s = string.Empty;
			var workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
			s = workflowAssignment.ResolveWorkflowRoleAssignment(messages, InstanceData.InstanceID, SAHL.Common.Globals.WorkflowRoleTypes.PLSupervisorD, SAHL.Common.Globals.WorkflowRoleTypeGroups.PersonalLoan);
			return s;
		}

		public string OnGetRole_Personal_Loans_PL_Manager_D(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, string RoleName)
		{
			string s = string.Empty;
			var workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
			s = workflowAssignment.ResolveWorkflowRoleAssignment(messages, InstanceData.InstanceID, SAHL.Common.Globals.WorkflowRoleTypes.PLManagerD, SAHL.Common.Globals.WorkflowRoleTypeGroups.PersonalLoan);
			return s;
		}

		public string OnGetRole_Personal_Loans_PL_Consultant_D(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, string RoleName)
		{
			string s = string.Empty;
			var workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
		 s = workflowAssignment.ResolveWorkflowRoleAssignment(messages, InstanceData.InstanceID, SAHL.Common.Globals.WorkflowRoleTypes.PLConsultantD, SAHL.Common.Globals.WorkflowRoleTypeGroups.PersonalLoan);
		 return s;
		}

		public string OnGetRole_Personal_Loans_PL_Credit_Analyst_D(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data ,IX2InstanceData InstanceData, IX2Params Params, string RoleName)
		{
			string s = string.Empty;
			var workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
		 s = workflowAssignment.ResolveWorkflowRoleAssignment(messages, InstanceData.InstanceID, SAHL.Common.Globals.WorkflowRoleTypes.PLCreditAnalystD, SAHL.Common.Globals.WorkflowRoleTypeGroups.PersonalLoan);
		 return s;
		}

		public string OnGetRole_Personal_Loans_PL_Admin_D(IDomainMessageCollection messages, IX2Personal_Loans_Data Personal_Loans_Data , IX2InstanceData InstanceData, IX2Params Params, string RoleName)
		{
			string s = string.Empty;
			var workflowAssignment = DomainServiceLoader.Instance.Get<IWorkflowAssignment>();
			s = workflowAssignment.ResolveWorkflowRoleAssignment(messages, InstanceData.InstanceID, SAHL.Common.Globals.WorkflowRoleTypes.PLAdminD, SAHL.Common.Globals.WorkflowRoleTypeGroups.PersonalLoan);
			return s;
		}

		#endregion

		#region IX2WorkFlow Members

		[CoverageExclude]
		public bool OnEnterState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnEnterState", WorkFlowData, InstanceData, Params, null);
			try
			{
				bool returnData = this.OnEnterStateInternal(messages, WorkFlowData, InstanceData, Params);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnEnterState", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnEnterState", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnEnterState", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public bool OnEnterStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			switch(Params.StateName)
			{
				case "Manage Lead":
					return OnEnter_Manage_Lead(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Manage Application":
					return OnEnter_Manage_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Credit":
					return OnEnter_Credit(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Disbursement":
					return OnEnter_Disbursement(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Archive Disbursed":
					return OnEnter_Archive_Disbursed(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Common Rework":
					return OnEnter_Common_Rework(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Declined by Credit":
					return OnEnter_Declined_by_Credit(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Common Return":
					return OnEnter_Common_Return(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Legal Agreements":
					return OnEnter_Legal_Agreements(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Common NTU":
					return OnEnter_Common_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU":
					return OnEnter_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Return to State":
					return OnEnter_Return_to_State(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Archive NTU Declines":
					return OnEnter_Archive_NTU_Declines(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
					
				default:
					return false;
			}
		}

		[CoverageExclude]
		public bool OnExitState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnExitState", WorkFlowData, InstanceData, Params, null);
			try
			{
				bool returnData = this.OnExitStateInternal(messages, WorkFlowData, InstanceData, Params);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnExitState", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnExitState", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnExitState", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public bool OnExitStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			switch(Params.StateName)
			{
				case "Manage Lead":
					return OnExit_Manage_Lead(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Manage Application":
					return OnExit_Manage_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Credit":
					return OnExit_Credit(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Disbursement":
					return OnExit_Disbursement(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Archive Disbursed":
					return OnExit_Archive_Disbursed(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Common Rework":
					return OnExit_Common_Rework(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Declined by Credit":
					return OnExit_Declined_by_Credit(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Common Return":
					return OnExit_Common_Return(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Legal Agreements":
					return OnExit_Legal_Agreements(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Common NTU":
					return OnExit_Common_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU":
					return OnExit_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Return to State":
					return OnExit_Return_to_State(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Archive NTU Declines":
					return OnExit_Archive_NTU_Declines(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
								
				default:
					return false;
			}
		}

		[CoverageExclude]
		public bool OnReturnState(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnReturnState", WorkFlowData, InstanceData, Params, null);
			try
			{
				bool returnData = this.OnReturnStateInternal(messages, WorkFlowData, InstanceData, Params);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnReturnState", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnReturnState", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnReturnState", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public bool OnReturnStateInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			switch(Params.StateName)
			{
				case "Archive Disbursed":
					return OnReturn_Archive_Disbursed(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Archive NTU Declines":
					return OnReturn_Archive_NTU_Declines(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
	
					default: 
						return false;
				}
			}

		[CoverageExclude]
		public string GetForwardStateName(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetForwardStateName", WorkFlowData, InstanceData, Params, null);
			try
			{
				string returnData = this.GetForwardStateNameInternal(messages, WorkFlowData, InstanceData, Params);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetForwardStateName", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetForwardStateName", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetForwardStateName", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public string GetForwardStateNameInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			switch(Params.StateName)
			{
				case "Return to State":
					return GetForwardStateName_Return_to_State(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				
				default:
					return string.Empty;
			}
		}

		[CoverageExclude]
		public bool OnStartActivity(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnStartActivity", WorkFlowData, InstanceData, Params, null);
			try
			{
				bool returnData = this.OnStartActivityInternal(messages, WorkFlowData, InstanceData, Params);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnStartActivity", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnStartActivity", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnStartActivity", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public bool OnStartActivityInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			switch(Params.ActivityName)
			{
				case "Rework Application":
					return OnStartActivity_Rework_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Calculate Application":
					return OnStartActivity_Calculate_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Application in Order":
					return OnStartActivity_Application_in_Order(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Approve":
					return OnStartActivity_Approve(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Decline":
					return OnStartActivity_Decline(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Decline Finalised":
					return OnStartActivity_Decline_Finalised(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Disburse Funds":
					return OnStartActivity_Disburse_Funds(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Return to Manage Application":
					return OnStartActivity_Return_to_Manage_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Create Personal Loan Lead":
					return OnStartActivity_Create_Personal_Loan_Lead(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Documents Verified":
					return OnStartActivity_Documents_Verified(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU":
					return OnStartActivity_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Reinstate NTU":
					return OnStartActivity_Reinstate_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU Finalised":
					return OnStartActivity_NTU_Finalised(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU Timer":
					return OnStartActivity_NTU_Timer(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Send Documents":
					return OnStartActivity_Send_Documents(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Send Offer":
					return OnStartActivity_Send_Offer(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				
				default:
					return false;
			}
		}

		[CoverageExclude]
		public bool OnCompleteActivity(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, ref string AlertMessage)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnCompleteActivity", WorkFlowData, InstanceData, Params, new object[] {AlertMessage});
			try
			{
				bool returnData = this.OnCompleteActivityInternal(messages, WorkFlowData, InstanceData, Params, ref AlertMessage);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnCompleteActivity", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnCompleteActivity", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "OnCompleteActivity", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public bool OnCompleteActivityInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, ref string AlertMessage)
		{
			switch(Params.ActivityName)
			{
				case "Rework Application":
					return OnCompleteActivity_Rework_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Calculate Application":
					return OnCompleteActivity_Calculate_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Application in Order":
					return OnCompleteActivity_Application_in_Order(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Approve":
					return OnCompleteActivity_Approve(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Decline":
					return OnCompleteActivity_Decline(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Decline Finalised":
					return OnCompleteActivity_Decline_Finalised(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Disburse Funds":
					return OnCompleteActivity_Disburse_Funds(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Return to Manage Application":
					return OnCompleteActivity_Return_to_Manage_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Create Personal Loan Lead":
					return OnCompleteActivity_Create_Personal_Loan_Lead(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Documents Verified":
					return OnCompleteActivity_Documents_Verified(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "NTU":
					return OnCompleteActivity_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Reinstate NTU":
					return OnCompleteActivity_Reinstate_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "NTU Finalised":
					return OnCompleteActivity_NTU_Finalised(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "NTU Timer":
					return OnCompleteActivity_NTU_Timer(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Send Documents":
					return OnCompleteActivity_Send_Documents(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				case "Send Offer":
					return OnCompleteActivity_Send_Offer(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, ref AlertMessage);
				
				default:
					return false;
			}
		}

		[CoverageExclude]
		public DateTime GetActivityTime(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityTime", WorkFlowData, InstanceData, Params, null);
			try
			{
				DateTime returnData = this.GetActivityTimeInternal(messages, WorkFlowData, InstanceData, Params);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityTime", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityTime", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityTime", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public DateTime GetActivityTimeInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			switch(Params.ActivityName)
			{
				case "NTU Timer":
					return GetActivityTime_NTU_Timer(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
					
				default:
					return DateTime.MinValue;
			}
		}

		[CoverageExclude]
		public string GetStageTransition(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetStageTransition", WorkFlowData, InstanceData, Params, null);
			try
			{
				string returnData = this.GetStageTransitionInternal(messages, WorkFlowData, InstanceData, Params);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetStageTransition", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetStageTransition", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetStageTransition", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public string GetStageTransitionInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			switch(Params.ActivityName)
			{
				case "Rework Application":
					return GetStageTransition_Rework_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Calculate Application":
					return GetStageTransition_Calculate_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Application in Order":
					return GetStageTransition_Application_in_Order(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Approve":
					return GetStageTransition_Approve(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Decline":
					return GetStageTransition_Decline(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Decline Finalised":
					return GetStageTransition_Decline_Finalised(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Disburse Funds":
					return GetStageTransition_Disburse_Funds(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Return to Manage Application":
					return GetStageTransition_Return_to_Manage_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Create Personal Loan Lead":
					return GetStageTransition_Create_Personal_Loan_Lead(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Documents Verified":
					return GetStageTransition_Documents_Verified(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU":
					return GetStageTransition_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Reinstate NTU":
					return GetStageTransition_Reinstate_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU Finalised":
					return GetStageTransition_NTU_Finalised(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU Timer":
					return GetStageTransition_NTU_Timer(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Send Documents":
					return GetStageTransition_Send_Documents(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Send Offer":
					return GetStageTransition_Send_Offer(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
						
				default:
					return string.Empty;
			}
		}

		[CoverageExclude]
		public string GetActivityMessage(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityMessage", WorkFlowData, InstanceData, Params, null);
			try
			{
				string returnData = this.GetActivityMessageInternal(messages, WorkFlowData, InstanceData, Params);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityMessage", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityMessage", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetActivityMessage", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public string GetActivityMessageInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params)
		{
			switch(Params.ActivityName)
			{
				case "Rework Application":
					return GetActivityMessage_Rework_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Calculate Application":
					return GetActivityMessage_Calculate_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Application in Order":
					return GetActivityMessage_Application_in_Order(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Approve":
					return GetActivityMessage_Approve(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Decline":
					return GetActivityMessage_Decline(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Decline Finalised":
					return GetActivityMessage_Decline_Finalised(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Disburse Funds":
					return GetActivityMessage_Disburse_Funds(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Return to Manage Application":
					return GetActivityMessage_Return_to_Manage_Application(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Documents Verified":
					return GetActivityMessage_Documents_Verified(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU":
					return GetActivityMessage_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Reinstate NTU":
					return GetActivityMessage_Reinstate_NTU(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "NTU Finalised":
					return GetActivityMessage_NTU_Finalised(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Send Documents":
					return GetActivityMessage_Send_Documents(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
				case "Send Offer":
					return GetActivityMessage_Send_Offer(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params);
					
				default:
					return string.Empty;
			}
		}

		[CoverageExclude]
		public string GetDynamicRole(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, string RoleName, string WorkflowName)
		{
			log4netX2Logging.LogOnEnterWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetDynamicRole", WorkFlowData, InstanceData, Params, new object[] {RoleName, WorkflowName});
			try
			{
				string returnData = this.GetDynamicRoleInternal(messages, WorkFlowData, InstanceData, Params, RoleName, WorkflowName);
				log4netX2Logging.LogOnWorkflowSuccess(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetDynamicRole", WorkFlowData, InstanceData, Params);
				return returnData;
			}
			catch(Exception exception)
			{
				log4netX2Logging.LogOnWorkflowException(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetDynamicRole", InstanceData.InstanceID, exception);
				throw;
			}
			finally
			{
				log4netX2Logging.LogOnExitWorkflow(SAHL.X2.Framework.Logging.LogSettings.Default.WorkflowLog, "GetDynamicRole", InstanceData.InstanceID);
			}
		}

		[CoverageExclude]
		public string GetDynamicRoleInternal(IDomainMessageCollection messages, object WorkFlowData, IX2InstanceData InstanceData, IX2Params Params, string RoleName, string WorkflowName)
		{
			switch(RoleName.Replace(' ', '_')+"_"+WorkflowName.Replace(' ', '_'))
			{
				case "PL_Supervisor_D_Personal_Loans":
					return OnGetRole_Personal_Loans_PL_Supervisor_D(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, RoleName);
				case "PL_Manager_D_Personal_Loans":
					return OnGetRole_Personal_Loans_PL_Manager_D(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, RoleName);
				case "PL_Consultant_D_Personal_Loans":
					return OnGetRole_Personal_Loans_PL_Consultant_D(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, RoleName);
				case "PL_Credit_Analyst_D_Personal_Loans":
					return OnGetRole_Personal_Loans_PL_Credit_Analyst_D(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, RoleName);
				case "PL_Admin_D_Personal_Loans":
					return OnGetRole_Personal_Loans_PL_Admin_D(messages, (IX2Personal_Loans_Data)WorkFlowData, InstanceData, Params, RoleName);
				
				default:
					return string.Empty;
			}
		}

		[CoverageExclude]
		public IX2WorkFlowDataProvider GetWorkFlowDataProvider()
		{
			return new X2Personal_Loans_Data();
		}

		#endregion
	}

	#endregion
	
