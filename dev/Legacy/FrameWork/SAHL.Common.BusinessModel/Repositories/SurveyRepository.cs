using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using System.Data;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common;
using System.Data.SqlClient;
using SAHL.Common.CacheData;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using System.Linq;

namespace SAHL.Common.BusinessModel.Repositories
{
	[FactoryType(typeof(ISurveyRepository))]
	public class SurveyRepository : AbstractRepositoryBase, ISurveyRepository
	{
		public SurveyRepository()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public IClientQuestionnaire GetClientQuestionnaireByKey(int key)
		{
			return base.GetByKey<IClientQuestionnaire, ClientQuestionnaire_DAO>(key);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public IClientQuestionnaire GetClientQuestionnaireByGUID(string guid)
		{
			using (IDbConnection con = Helper.GetSQLDBConnection())
			{
				string sql = string.Format(UIStatementRepository.GetStatement("Repositories.SurveyRepository", "GetClientQuestionnaireByGUID"), guid);
				SimpleQuery<ClientQuestionnaire_DAO> query = new SimpleQuery<ClientQuestionnaire_DAO>(QueryLanguage.Sql, sql);
				query.AddSqlReturnDefinition(typeof(ClientQuestionnaire_DAO), "CQ");
				ClientQuestionnaire_DAO[] clientQuestionnaires = query.Execute();

				if (clientQuestionnaires == null || clientQuestionnaires.Length == 0)
					return null;

				return new ClientQuestionnaire(clientQuestionnaires[0]);
			}
		}

		public void RemoveClientQuestionnaireByGUID(string guid)
		{
			string query = string.Format("GUID='{0}'", guid);
			ClientQuestionnaire_DAO.DeleteAll(query);
			if (ValidationHelper.PrincipalHasValidationErrors())
				throw new DomainValidationException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IClientQuestionnaire CreateEmptyClientQuestionnaire()
		{
			return base.CreateEmpty<IClientQuestionnaire, ClientQuestionnaire_DAO>();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cq"></param>
		public void SaveClientQuestionnaire(IClientQuestionnaire cq)
		{
			base.Save<IClientQuestionnaire, ClientQuestionnaire_DAO>(cq);
		}

		public IQuestionnaire GetQuestionnaireByKey(int key)
		{
			return base.GetByKey<IQuestionnaire, Questionnaire_DAO>(key);
		}

		public IQuestionnaireQuestion GetQuestionnaireQuestionByKey(int key)
		{
			return base.GetByKey<IQuestionnaireQuestion, QuestionnaireQuestion_DAO>(key);
		}

		public IClientAnswer CreateEmptyClientAnswer()
		{
			return base.CreateEmpty<IClientAnswer, ClientAnswer_DAO>();
		}

		public IClientAnswerValue CreateEmptyClientAnswerValue()
		{
			return base.CreateEmpty<IClientAnswerValue, ClientAnswerValue_DAO>();
		}

		public IAnswer GetAnswerByKey(int key)
		{
			return base.GetByKey<IAnswer, Answer_DAO>(key);
		}

		//public IEventList<IClientQuestionnaire> GetQuestionnaireByBusiness(int legalEntityKey)
		//{
		//    string HQL = "from ClientQuestionnaire_DAO cq where cq.LegalEntity.Key = ?";

		//    SimpleQuery<ClientQuestionnaire_DAO> q = new SimpleQuery<ClientQuestionnaire_DAO>(HQL, legalEntityKey);

		//    ClientQuestionnaire_DAO[] res = q.Execute();

		//    IEventList<IClientQuestionnaire> list = new DAOEventList<ClientQuestionnaire_DAO, IClientQuestionnaire, SAHL.Common.BusinessModel.ClientQuestionnaire>(res);
		//    return list;
		//}

		public IEventList<IClientQuestionnaire> GetClientQuestionnairesByLegalEntityKey(int legalEntityKey)
		{
			return GetClientQuestionnairesByLegalEntityKey(legalEntityKey, ClientSurveyStatus.All, false);
		}
		public IEventList<IClientQuestionnaire> GetClientQuestionnairesByLegalEntityKey(int legalEntityKey, Globals.ClientSurveyStatus clientSurveyStatus)
		{
			return GetClientQuestionnairesByLegalEntityKey(legalEntityKey, clientSurveyStatus, false);
		}
		public IEventList<IClientQuestionnaire> GetClientQuestionnairesByLegalEntityKey(int legalEntityKey, Globals.ClientSurveyStatus clientSurveyStatus, bool latestFirst)
		{
			string SQL = "select cq.* from [2am].survey.ClientQuestionnaire cq (nolock)";

			string leftJoinClause = " left join [2am].survey.ClientAnswer ca (nolock) on ca.ClientQuestionnaireKey = cq.ClientQuestionnaireKey";
			string groupByClause = " group by cq.ClientQuestionnaireKey,cq.BusinessEventQuestionnaireKey,cq.DatePresented,cq.ADUserKey,cq.GenericKey,cq.GenericKeyTypeKey,cq.DateReceived,cq.LegalEntityKey,cq.[GUID]";
			string whereClause = " where cq.LegalEntityKey = ?";

			switch (clientSurveyStatus)
			{
				case ClientSurveyStatus.All:
					SQL += whereClause;
					break;
				case ClientSurveyStatus.Unanswered:
					SQL += whereClause;
					SQL += " and cq.DateReceived is null";
					break;
				case ClientSurveyStatus.Answered:
				case ClientSurveyStatus.Rejected:
					SQL += leftJoinClause;
					SQL += whereClause;
					SQL += " and cq.DateReceived is not null";
					SQL += groupByClause;
					if (clientSurveyStatus == ClientSurveyStatus.Answered)
						SQL += " having count(ca.ClientQuestionnaireKey) > 0";
					else
						SQL += " having count(ca.ClientQuestionnaireKey) <= 0";
					break;
				default:
					break;
			}

			if (latestFirst)
			{
				if (clientSurveyStatus == ClientSurveyStatus.Unanswered)
					SQL += " ORDER BY cq.DatePresented desc";
				else
					SQL += " ORDER BY cq.DateReceived desc";
			}


			SimpleQuery<ClientQuestionnaire_DAO> q = new SimpleQuery<ClientQuestionnaire_DAO>(QueryLanguage.Sql, SQL, legalEntityKey);
			q.AddSqlReturnDefinition(typeof(ClientQuestionnaire_DAO), "C");
			ClientQuestionnaire_DAO[] res = q.Execute();
			IEventList<IClientQuestionnaire> list = new DAOEventList<ClientQuestionnaire_DAO, IClientQuestionnaire, ClientQuestionnaire>(res);
			return new EventList<IClientQuestionnaire>(list);
		}

		public IEventList<IClientQuestionnaire> GetClientQuestionnairesByGenericKey(int genericKey, int genericKeyTypeKey)
		{
			return GetClientQuestionnairesByGenericKey(genericKey, genericKeyTypeKey, ClientSurveyStatus.All, false);
		}
		public IEventList<IClientQuestionnaire> GetClientQuestionnairesByGenericKey(int genericKey, int genericKeyTypeKey, Globals.ClientSurveyStatus clientSurveyStatus)
		{
			return GetClientQuestionnairesByGenericKey(genericKey, genericKeyTypeKey, clientSurveyStatus, false);
		}
		public IEventList<IClientQuestionnaire> GetClientQuestionnairesByGenericKey(int genericKey, int genericKeyTypeKey, Globals.ClientSurveyStatus clientSurveyStatus, bool latestFirst)
		{
			string SQL = "select cq.* from [2am].survey.ClientQuestionnaire cq (nolock)";

			string leftJoinClause = " left join [2am].survey.ClientAnswer ca (nolock) on ca.ClientQuestionnaireKey = cq.ClientQuestionnaireKey";
			string groupByClause = " group by cq.ClientQuestionnaireKey,cq.BusinessEventQuestionnaireKey,cq.DatePresented,cq.ADUserKey,cq.GenericKey,cq.GenericKeyTypeKey,cq.DateReceived,cq.LegalEntityKey,cq.[GUID]";
			string whereClause = " where cq.GenericKey = ? AND cq.GenericKeyTypeKey = ?";

			switch (clientSurveyStatus)
			{
				case ClientSurveyStatus.All:
					SQL += whereClause;
					break;
				case ClientSurveyStatus.Unanswered:
					SQL += whereClause;
					SQL += " and cq.DateReceived is null";
					break;
				case ClientSurveyStatus.Answered:
				case ClientSurveyStatus.Rejected:
					SQL += leftJoinClause;
					SQL += whereClause;
					SQL += " and cq.DateReceived is not null";
					SQL += groupByClause;
					if (clientSurveyStatus == ClientSurveyStatus.Answered)
						SQL += " having count(ca.ClientQuestionnaireKey) > 0";
					else
						SQL += " having count(ca.ClientQuestionnaireKey) <= 0";
					break;
				default:
					break;
			}

			if (latestFirst)
			{
				if (clientSurveyStatus == ClientSurveyStatus.Unanswered)
					SQL += " ORDER BY cq.DatePresented desc";
				else
					SQL += " ORDER BY cq.DateReceived desc";
			}


			SimpleQuery<ClientQuestionnaire_DAO> q = new SimpleQuery<ClientQuestionnaire_DAO>(QueryLanguage.Sql, SQL, genericKey, genericKeyTypeKey);
			q.AddSqlReturnDefinition(typeof(ClientQuestionnaire_DAO), "C");
			ClientQuestionnaire_DAO[] res = q.Execute();
			IEventList<IClientQuestionnaire> list = new DAOEventList<ClientQuestionnaire_DAO, IClientQuestionnaire, ClientQuestionnaire>(res);
			return new EventList<IClientQuestionnaire>(list);
		}

		/// <summary>
		/// Get Business Event Questionnaire By BusinessEventQuestionnaireKey
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public IBusinessEventQuestionnaire GetBusinessEventQuestionnaireByKey(int key)
		{
			BusinessEventQuestionnaire_DAO businessEventQuestionnaire = BusinessEventQuestionnaire_DAO.TryFind(key);
			if (businessEventQuestionnaire != null)
			{
				IBusinessModelTypeMapper typeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return typeMapper.GetMappedType<IBusinessEventQuestionnaire, BusinessEventQuestionnaire_DAO>(businessEventQuestionnaire);
			}
			return null;
		}

		public IList<IClientAnswer> GetClientAnswersForQuestion(IClientQuestionnaire clientQuestionnaire, IQuestionnaireQuestion questionnaireQuestion)
		{
			IList<IClientAnswer> clientAnswers = new List<IClientAnswer>();

			foreach (IClientAnswer clientAnswer in clientQuestionnaire.ClientAnswers)
			{
				if (clientAnswer.QuestionnaireQuestion.Key == questionnaireQuestion.Key)
				{
					clientAnswers.Add(clientAnswer);
				}
			}

			return clientAnswers;
		}

		public List<SurveyBindableObject> GetUnansweredandAdHocClientQuestionnairesByGenericKey(int genericKey, int genericKeyTypeKey)
		{
			List<SurveyBindableObject> sboList = new List<SurveyBindableObject>();

			DataTable dtQuestionnaires = new DataTable();
			using (IDbConnection con = Helper.GetSQLDBConnection())
			{
				string qry = UIStatementRepository.GetStatement("Repositories.SurveyRepository", "GetCaptureList");
				// Create a collection
				ParameterCollection Parameters = new ParameterCollection();
				//Add the required parameters
				Helper.AddIntParameter(Parameters, "@GenericKey", genericKey);
				Helper.AddIntParameter(Parameters, "@GenericKeyTypeKey", genericKeyTypeKey);
				Helper.FillFromQuery(dtQuestionnaires, qry, con, Parameters);
			}

			if (dtQuestionnaires != null && dtQuestionnaires.Rows.Count > 0)
			{
				foreach (DataRow dr in dtQuestionnaires.Rows)
				{
					SurveyBindableObject sbo = new SurveyBindableObject(
						Convert.ToInt32(dr["QuestionnaireKey"]),
						dr["QuestionnaireDescription"].ToString(),
						dr["BusinessEventKey"] == null ? 0 : Convert.ToInt32(dr["BusinessEventKey"]),
						dr["BusinessEventDescription"].ToString(),
						dr["ClientQuestionnaireKey"] == null ? 0 : Convert.ToInt32(dr["ClientQuestionnaireKey"]),
						dr["BusinessEventQuestionnaireKey"] == null ? 0 : Convert.ToInt32(dr["BusinessEventQuestionnaireKey"]),
						dr["DatePresented"] == null ? "" : dr["DatePresented"].ToString(),
						dr["ADUserKey"] == null ? 0 : Convert.ToInt32(dr["ADUserKey"]),
						dr["GenericKey"] == null ? 0 : Convert.ToInt32(dr["GenericKey"]),
						dr["GenericKeyTypeKey"] == null ? 0 : Convert.ToInt32(dr["GenericKeyTypeKey"]),
						dr["GenericKeyDescription"].ToString(),
						dr["DateReceived"] == null ? "" : dr["DateReceived"].ToString(),
						dr["LegalEntityKey"] == null ? 0 : Convert.ToInt32(dr["LegalEntityKey"]),
						dr["LegalEntityName"].ToString(),
						dr["ClientSurveyStatus"].ToString(),
						dr["GUID"].ToString());

					sboList.Add(sbo);
				}
			}

			return sboList;
		}

		/// <summary>
		/// Sends a Client Survey to the legal entity's email address.
		/// </summary>
		/// <param name="businessEventQuestionnaireKey">Business Event Questionnaire Key</param>
		/// <param name="applicationKey">Application Key</param>
		/// <param name="adUserName"></param>
		/// <param name="externalEmail">Indicates whether this is to be sent to an Internal or External email address</param>
		public void SendClientSurveyEmail(int businessEventQuestionnaireKey, int applicationKey, string adUserName, bool externalEmail)
		{
			if (externalEmail)
			{
				SendClientSurveyExternalEmail(businessEventQuestionnaireKey, applicationKey, adUserName);
			}
			else
			{
				SendClientSurveyInternalEmail(businessEventQuestionnaireKey, applicationKey, adUserName);
			}
		}

		
        /// <summary>
        /// Send Client survey Internal Email
        /// </summary>
        /// <param name="businessEventQuestionnaireKey"></param>
        /// <param name="applicationKey"></param>
        /// <param name="adUserName"></param>
		private void SendClientSurveyInternalEmail(int businessEventQuestionnaireKey, int applicationKey, string adUserName)
		{
			IApplication application = AppRepo.GetApplicationByKey(applicationKey);
			int accountKey = application.ReservedAccount.Key;

			string legalEntityName = application.GetLegalName(LegalNameFormat.Full);

			//Create the Client Survey
			IQuestionnaire questionnaire = CreateClientSurvey(businessEventQuestionnaireKey, accountKey, (int)GenericKeyTypes.Account, adUserName).BusinessEventQuestionnaire.Questionnaire;

			//Since this is an internal email, let's get the HTML Internal email to send
			IQuestionnaireEmail questionnaireEmail = null;
			foreach (IQuestionnaireQuestionnaireEmail questionnaireQuestionnaireEmail in questionnaire.QuestionnaireQuestionnaireEmails)
			{
				if (questionnaireQuestionnaireEmail.QuestionnaireEmail.ContentType.Key == (int)ContentTypes.HTML && questionnaireQuestionnaireEmail.InternalEmail == 1)
				{
					questionnaireEmail = questionnaireQuestionnaireEmail.QuestionnaireEmail;
					break;
				}
			}

			if (questionnaireEmail == null)
			{
				throw new NullReferenceException(String.Format("There is no internal email template found for Questionnaire ({0})", questionnaire.Description));
			}

			string from = CTRLRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ClientSurvey.ClientSurveyMailFrom).ControlText;
			string to = CTRLRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ClientSurvey.ClientSurveyMailTo).ControlText;
			string subject = CompileEmailSubject(questionnaireEmail.EmailSubject, questionnaire.Description, accountKey);
			string body = CompileEmailBody(questionnaireEmail.EmailBody, legalEntityName, String.Empty, Guid.NewGuid().ToString());

			//Send the survey to the group

			MSGSvc.SendEmailInternal(from, to, null, null, subject, body, true);
		}

		/// <summary>
		/// Send Client Survey External Email
		/// </summary>
		/// <param name="businessEventQuestionnaireKey"></param>
		/// <param name="applicationKey"></param>
        /// <param name="adUserName"></param>
		private void SendClientSurveyExternalEmail(int businessEventQuestionnaireKey, int applicationKey, string adUserName)
		{
			string subject = String.Empty;
			string body = String.Empty;
			string from = CTRLRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ClientSurvey.ClientSurveyMailFrom).ControlText;
			string url = CTRLRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ClientSurvey.ClientSurveyURL).ControlText;

			//create a new GUID
			string guid = System.Guid.NewGuid().ToString();
			IApplication application = AppRepo.GetApplicationByKey(applicationKey);

			if (application.ReservedAccount == null)
			{
				throw new Exception("Reserved Account cannot be null");
			}

			int accountKey = application.ReservedAccount.Key;

			ILegalEntity legalEntity = null;
			//Get the To Mailing address
			string to = String.Empty;
			if (application.ApplicationMailingAddresses != null && application.ApplicationMailingAddresses.Count > 0)
			{
				to = application.ApplicationMailingAddresses[0].LegalEntity.EmailAddress + ";";
				legalEntity = application.ApplicationMailingAddresses[0].LegalEntity;
			}

			if (legalEntity == null)
			{
				throw new Exception("Legal Entity cannot be null");
			}

			//Create the Client Survey
			IQuestionnaire questionnaire = CreateClientSurvey(businessEventQuestionnaireKey, accountKey, (int)SAHL.Common.Globals.GenericKeyTypes.Account, adUserName, guid, legalEntity).BusinessEventQuestionnaire.Questionnaire;

			//Since this is an internal email, let's get the HTML Internal email to send
			IQuestionnaireEmail questionnaireEmail = null;
			foreach (IQuestionnaireQuestionnaireEmail questionnaireQuestionnaireEmail in questionnaire.QuestionnaireQuestionnaireEmails)
			{
                if (questionnaireQuestionnaireEmail.QuestionnaireEmail.ContentType.Key == (int)ContentTypes.HTML && questionnaireQuestionnaireEmail.InternalEmail == 0)
				{
					questionnaireEmail = questionnaireQuestionnaireEmail.QuestionnaireEmail;
					break;
				}
			}

			string legalName = application.GetLegalName(LegalNameFormat.InitialsOnly);

			subject = CompileEmailSubject(questionnaireEmail.EmailSubject, questionnaire.Description, accountKey);
			body = CompileEmailBody(questionnaireEmail.EmailBody, legalName, url, guid);

			//Send the survey to the group
            MSGSvc.SendEmailExternal(accountKey, from, to, String.Empty, String.Empty, subject, body, String.Empty, String.Empty, String.Empty, ContentTypes.HTML);
		}

		/// <summary>
		/// Create the Client Survey and return the description
		/// </summary>
		/// <param name="businessEventQuestionnaireKey"></param>
		/// <param name="genericKey"></param>
        /// <param name="gkTypeKey"></param>
        /// <param name="ADUserName"></param>
		/// <returns>The Description of the Questionnaire</returns>
		private IClientQuestionnaire CreateClientSurvey(int businessEventQuestionnaireKey, int genericKey,int gkTypeKey, string ADUserName)
		{
			return CreateClientSurvey(businessEventQuestionnaireKey, genericKey, gkTypeKey, ADUserName, null, null);
		}

		/// <summary>
        /// Create the Client Survey and return the description
		/// </summary>
		/// <param name="businessEventQuestionnaireKey"></param>
		/// <param name="gKey"></param>
		/// <param name="gkTypeKey"></param>
		/// <param name="adUserName"></param>
		/// <param name="guid"></param>
		/// <param name="legalEntity"></param>
        /// <returns>The Description of the Questionnaire</returns>
		private IClientQuestionnaire CreateClientSurvey(int businessEventQuestionnaireKey, int gKey, int gkTypeKey, string adUserName, string guid, ILegalEntity legalEntity)
		{
			//Create a Client Survey
			IClientQuestionnaire clientQuestionnaire = CreateEmptyClientQuestionnaire();
			IBusinessEventQuestionnaire businessEventQuestionnaire = GetBusinessEventQuestionnaireByKey(businessEventQuestionnaireKey);
			clientQuestionnaire.BusinessEventQuestionnaire = businessEventQuestionnaire;
			clientQuestionnaire.DatePresented = DateTime.Now;
			clientQuestionnaire.ADUser = OSRepo.GetAdUserForAdUserName(adUserName);
			clientQuestionnaire.GenericKey = gKey;
			clientQuestionnaire.GenericKeyType = LKRepo.GenericKeyType.ObjectDictionary[Convert.ToString((int)gkTypeKey)];
			if (!String.IsNullOrEmpty(guid))
			{
				clientQuestionnaire.GUID = guid;
			}
			if (legalEntity != null)
			{
				clientQuestionnaire.LegalEntity = legalEntity;
			}

			SaveClientQuestionnaire(clientQuestionnaire);

			return clientQuestionnaire;
		}

		/// <summary>
		/// Compile the Email Subject
		/// </summary>
		/// <param name="subjectContent"></param>
		/// <param name="questionnaireDescription"></param>
		/// <param name="accountKey"></param>
		/// <returns></returns>
		private string CompileEmailSubject(string subjectContent, string questionnaireDescription, int accountKey)
		{
			return String.Format(subjectContent, questionnaireDescription, accountKey);
		}

		/// <summary>
		/// Compile Email Body
		/// </summary>
		/// <param name="bodyContent"></param>
		/// <param name="legalName"></param>
		/// <param name="url"></param>
		/// <param name="guid"></param>
		/// <returns></returns>
		private string CompileEmailBody(string bodyContent, string legalName, string url, string guid)
		{
			return String.Format(bodyContent, legalName, url, guid);
		}

		public void SendSurveyToClient(int qKey, int leKey, int gKey, int gkTypekey, string ADUserName)
		{
			SendSurveyToClient(qKey, leKey, gKey, gkTypekey, ADUserName, null);
		}

		public void SendSurveyToClient(int qKey, int leKey, int gKey, int gkTypeKey, string ADUserName, IBusinessEvent be)
		{
			//get objects from the keys
			IQuestionnaire q = GetQuestionnaireByKey(qKey);
			ILegalEntity le = LERepo.GetLegalEntityByKey(leKey);

			//setup additional inputs

			//this should be created on a single server, default item in the db
			string guID = Guid.NewGuid().ToString();

			//use no business event if one was not passed in
			int beqKey = -1;
			if (be == null)
			{
				foreach (IBusinessEventQuestionnaire beq in q.BusinessEventQuestionnaires)
				{
					if (beq.BusinessEvent == null)
					{
						beqKey = beq.Key;
						break;
					}
				}
			}
			else
			{
				beqKey = be.Key;
			}

			//Create IClientQuestionnaire
			IClientQuestionnaire cq = CreateClientSurvey(beqKey, gKey, gkTypeKey, ADUserName, guID, le);

			// Get control values
			string from = CTRLRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ClientSurvey.ClientSurveyMailFrom).ControlText;
			string url = CTRLRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ClientSurvey.ClientSurveyURL).ControlText;

			//We have an IClientQuestionnaire, get the QuestionnaireEmail content for the correspondence
			//GetClient mail content from Client Questionnaire
			string subject = null;
			string body = null;

            GetEmailContentForSurvey(cq.BusinessEventQuestionnaire.Questionnaire, ContentTypes.HTML, ClientSurveyInternalEmails.ExternalMail, out subject, out body);

			// Format the subject and the body
			subject = string.Format(subject, cq.BusinessEventQuestionnaire.Questionnaire.Description, gKey);
			body = string.Format(body, le.DisplayName, url, guID);

			//send the mail
			MSGSvc.SendEmailExternal(gKey, from, le.EmailAddress, String.Empty, String.Empty, subject, body, String.Empty, String.Empty, String.Empty, ContentTypes.HTML);
		}

		private void GetEmailContentForSurvey(IQuestionnaire q, ContentTypes cscType , ClientSurveyInternalEmails csiMail, out string subject, out string body)
		{
			subject = String.Empty;
			body = String.Empty;

			//Since this is an internal email, let's get the HTML Internal email to send
			IQuestionnaireEmail questionnaireEmail = null;
			foreach (IQuestionnaireQuestionnaireEmail questionnaireQuestionnaireEmail in q.QuestionnaireQuestionnaireEmails)
			{
                if (questionnaireQuestionnaireEmail.QuestionnaireEmail.ContentType.Key == (int)ContentTypes.HTML && questionnaireQuestionnaireEmail.InternalEmail == (int)csiMail)
				{
					questionnaireEmail = questionnaireQuestionnaireEmail.QuestionnaireEmail;
					break;
				}
			}

			if (questionnaireEmail != null)
			{
				subject = questionnaireEmail.EmailSubject;
				body = questionnaireEmail.EmailBody;
			}

			return;
		}

		private IControlRepository _ctrlRepo;
		private IControlRepository CTRLRepo
		{
			get
			{
				if (_ctrlRepo == null)
					_ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();

				return _ctrlRepo;
			}
		}

		private ILookupRepository _lkRepo;
		private ILookupRepository LKRepo
		{
			get
			{
				if (_lkRepo == null)
					_lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

				return _lkRepo;
			}
		}

		private IOrganisationStructureRepository _osRepo;
		private IOrganisationStructureRepository OSRepo
		{
			get
			{
				if (_osRepo == null)
					_osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

				return _osRepo;
			}
		}

		private ILegalEntityRepository _leRepo;
		private ILegalEntityRepository LERepo
		{
			get
			{
				if (_leRepo == null)
					_leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

				return _leRepo;
			}
		}

		private IApplicationRepository _appRepo;
		private IApplicationRepository AppRepo
		{
			get
			{
				if (_appRepo == null)
					_appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

				return _appRepo;
			}
		}

		private IMessageService _msgService;
		private IMessageService MSGSvc
		{
			get
			{
				if (_msgService == null)
					_msgService = ServiceFactory.GetService<IMessageService>();

				return _msgService;
			}
		}
	}
}
