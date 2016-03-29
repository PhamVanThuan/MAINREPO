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
	/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO
	/// </summary>
	public partial class QuestionAnswer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO>, IQuestionAnswer
	{
				public QuestionAnswer(SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO QuestionAnswer) : base(QuestionAnswer)
		{
			this._DAO = QuestionAnswer;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.Sequence
		/// </summary>
		public Int32 Sequence 
		{
			get { return _DAO.Sequence; }
			set { _DAO.Sequence = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.QuestionAnswerQuestionnaires
		/// </summary>
		private DAOEventList<QuestionAnswerQuestionnaire_DAO, IQuestionAnswerQuestionnaire, QuestionAnswerQuestionnaire> _QuestionAnswerQuestionnaires;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.QuestionAnswerQuestionnaires
		/// </summary>
		public IEventList<IQuestionAnswerQuestionnaire> QuestionAnswerQuestionnaires
		{
			get
			{
				if (null == _QuestionAnswerQuestionnaires) 
				{
					if(null == _DAO.QuestionAnswerQuestionnaires)
						_DAO.QuestionAnswerQuestionnaires = new List<QuestionAnswerQuestionnaire_DAO>();
					_QuestionAnswerQuestionnaires = new DAOEventList<QuestionAnswerQuestionnaire_DAO, IQuestionAnswerQuestionnaire, QuestionAnswerQuestionnaire>(_DAO.QuestionAnswerQuestionnaires);
					_QuestionAnswerQuestionnaires.BeforeAdd += new EventListHandler(OnQuestionAnswerQuestionnaires_BeforeAdd);					
					_QuestionAnswerQuestionnaires.BeforeRemove += new EventListHandler(OnQuestionAnswerQuestionnaires_BeforeRemove);					
					_QuestionAnswerQuestionnaires.AfterAdd += new EventListHandler(OnQuestionAnswerQuestionnaires_AfterAdd);					
					_QuestionAnswerQuestionnaires.AfterRemove += new EventListHandler(OnQuestionAnswerQuestionnaires_AfterRemove);					
				}
				return _QuestionAnswerQuestionnaires;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.Answer
		/// </summary>
		public IAnswer Answer 
		{
			get
			{
				if (null == _DAO.Answer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAnswer, Answer_DAO>(_DAO.Answer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Answer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Answer = (Answer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.Question
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
		public override void Refresh()
		{
			base.Refresh();
			_QuestionAnswerQuestionnaires = null;
			
		}
	}
}


