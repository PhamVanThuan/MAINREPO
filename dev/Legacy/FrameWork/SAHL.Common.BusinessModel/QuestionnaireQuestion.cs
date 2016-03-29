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
	/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO
	/// </summary>
	public partial class QuestionnaireQuestion : BusinessModelBase<SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO>, IQuestionnaireQuestion
	{
				public QuestionnaireQuestion(SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO QuestionnaireQuestion) : base(QuestionnaireQuestion)
		{
			this._DAO = QuestionnaireQuestion;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.Sequence
		/// </summary>
		public Int32 Sequence 
		{
			get { return _DAO.Sequence; }
			set { _DAO.Sequence = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.QuestionAnswers
		/// </summary>
		private DAOEventList<QuestionnaireQuestionAnswer_DAO, IQuestionnaireQuestionAnswer, QuestionnaireQuestionAnswer> _QuestionAnswers;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.QuestionAnswers
		/// </summary>
		public IEventList<IQuestionnaireQuestionAnswer> QuestionAnswers
		{
			get
			{
				if (null == _QuestionAnswers) 
				{
					if(null == _DAO.QuestionAnswers)
						_DAO.QuestionAnswers = new List<QuestionnaireQuestionAnswer_DAO>();
					_QuestionAnswers = new DAOEventList<QuestionnaireQuestionAnswer_DAO, IQuestionnaireQuestionAnswer, QuestionnaireQuestionAnswer>(_DAO.QuestionAnswers);
					_QuestionAnswers.BeforeAdd += new EventListHandler(OnQuestionAnswers_BeforeAdd);					
					_QuestionAnswers.BeforeRemove += new EventListHandler(OnQuestionAnswers_BeforeRemove);					
					_QuestionAnswers.AfterAdd += new EventListHandler(OnQuestionAnswers_AfterAdd);					
					_QuestionAnswers.AfterRemove += new EventListHandler(OnQuestionAnswers_AfterRemove);					
				}
				return _QuestionAnswers;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.Question
		/// </summary>
		public IQuestion Question 
		{
			get
			{
				if (null == _DAO.Question) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IQuestion, Question_DAO>(_DAO.Question);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Question = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Question = (Question_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestion_DAO.Questionnaire
		/// </summary>
		public IQuestionnaire Questionnaire 
		{
			get
			{
				if (null == _DAO.Questionnaire) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IQuestionnaire, Questionnaire_DAO>(_DAO.Questionnaire);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Questionnaire = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Questionnaire = (Questionnaire_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_QuestionAnswers = null;
			
		}
	}
}


