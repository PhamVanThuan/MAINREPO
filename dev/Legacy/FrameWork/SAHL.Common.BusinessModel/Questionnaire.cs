using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO
	/// </summary>
	public partial class Questionnaire : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Questionnaire_DAO>, IQuestionnaire
	{
				public Questionnaire(SAHL.Common.BusinessModel.DAO.Questionnaire_DAO Questionnaire) : base(Questionnaire)
		{
			this._DAO = Questionnaire;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.BusinessAreaGenericKey
		/// </summary>
		public Int32 BusinessAreaGenericKey 
		{
			get { return _DAO.BusinessAreaGenericKey; }
			set { _DAO.BusinessAreaGenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.BusinessAreaGenericKeyType
		/// </summary>
		public IGenericKeyType BusinessAreaGenericKeyType 
		{
			get
			{
				if (null == _DAO.BusinessAreaGenericKeyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.BusinessAreaGenericKeyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.BusinessAreaGenericKeyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.BusinessAreaGenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.Questions
		/// </summary>
		private DAOEventList<QuestionnaireQuestion_DAO, IQuestionnaireQuestion, QuestionnaireQuestion> _Questions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.Questions
		/// </summary>
		public IEventList<IQuestionnaireQuestion> Questions
		{
			get
			{
				if (null == _Questions) 
				{
					if(null == _DAO.Questions)
						_DAO.Questions = new List<QuestionnaireQuestion_DAO>();
					_Questions = new DAOEventList<QuestionnaireQuestion_DAO, IQuestionnaireQuestion, QuestionnaireQuestion>(_DAO.Questions);
					_Questions.BeforeAdd += new EventListHandler(OnQuestions_BeforeAdd);					
					_Questions.BeforeRemove += new EventListHandler(OnQuestions_BeforeRemove);					
					_Questions.AfterAdd += new EventListHandler(OnQuestions_AfterAdd);					
					_Questions.AfterRemove += new EventListHandler(OnQuestions_AfterRemove);					
				}
				return _Questions;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.BusinessEventQuestionnaires
		/// </summary>
		private DAOEventList<BusinessEventQuestionnaire_DAO, IBusinessEventQuestionnaire, BusinessEventQuestionnaire> _BusinessEventQuestionnaires;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.BusinessEventQuestionnaires
		/// </summary>
		public IEventList<IBusinessEventQuestionnaire> BusinessEventQuestionnaires
		{
			get
			{
				if (null == _BusinessEventQuestionnaires) 
				{
					if(null == _DAO.BusinessEventQuestionnaires)
						_DAO.BusinessEventQuestionnaires = new List<BusinessEventQuestionnaire_DAO>();
					_BusinessEventQuestionnaires = new DAOEventList<BusinessEventQuestionnaire_DAO, IBusinessEventQuestionnaire, BusinessEventQuestionnaire>(_DAO.BusinessEventQuestionnaires);
					_BusinessEventQuestionnaires.BeforeAdd += new EventListHandler(OnBusinessEventQuestionnaires_BeforeAdd);					
					_BusinessEventQuestionnaires.BeforeRemove += new EventListHandler(OnBusinessEventQuestionnaires_BeforeRemove);					
					_BusinessEventQuestionnaires.AfterAdd += new EventListHandler(OnBusinessEventQuestionnaires_AfterAdd);					
					_BusinessEventQuestionnaires.AfterRemove += new EventListHandler(OnBusinessEventQuestionnaires_AfterRemove);					
				}
				return _BusinessEventQuestionnaires;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.GeneralStatus
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.QuestionnaireQuestionnaireEmails
		/// </summary>
		private DAOEventList<QuestionnaireQuestionnaireEmail_DAO, IQuestionnaireQuestionnaireEmail, QuestionnaireQuestionnaireEmail> _QuestionnaireQuestionnaireEmails;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Questionnaire_DAO.QuestionnaireQuestionnaireEmails
		/// </summary>
		public IEventList<IQuestionnaireQuestionnaireEmail> QuestionnaireQuestionnaireEmails
		{
			get
			{
				if (null == _QuestionnaireQuestionnaireEmails) 
				{
					if(null == _DAO.QuestionnaireQuestionnaireEmails)
						_DAO.QuestionnaireQuestionnaireEmails = new List<QuestionnaireQuestionnaireEmail_DAO>();
					_QuestionnaireQuestionnaireEmails = new DAOEventList<QuestionnaireQuestionnaireEmail_DAO, IQuestionnaireQuestionnaireEmail, QuestionnaireQuestionnaireEmail>(_DAO.QuestionnaireQuestionnaireEmails);
					_QuestionnaireQuestionnaireEmails.BeforeAdd += new EventListHandler(OnQuestionnaireQuestionnaireEmails_BeforeAdd);					
					_QuestionnaireQuestionnaireEmails.BeforeRemove += new EventListHandler(OnQuestionnaireQuestionnaireEmails_BeforeRemove);					
					_QuestionnaireQuestionnaireEmails.AfterAdd += new EventListHandler(OnQuestionnaireQuestionnaireEmails_AfterAdd);					
					_QuestionnaireQuestionnaireEmails.AfterRemove += new EventListHandler(OnQuestionnaireQuestionnaireEmails_AfterRemove);					
				}
				return _QuestionnaireQuestionnaireEmails;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_Questions = null;
			_BusinessEventQuestionnaires = null;
			_QuestionnaireQuestionnaireEmails = null;
			
		}
	}
}


