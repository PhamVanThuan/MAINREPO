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
	/// SAHL.Common.BusinessModel.DAO.Question_DAO
	/// </summary>
	public partial class Question : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Question_DAO>, IQuestion
	{
				public Question(SAHL.Common.BusinessModel.DAO.Question_DAO Question) : base(Question)
		{
			this._DAO = Question;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Question_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Question_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Question_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.Question_DAO.QuestionnaireQuestions
		/// </summary>
		private DAOEventList<QuestionnaireQuestion_DAO, IQuestionnaireQuestion, QuestionnaireQuestion> _QuestionnaireQuestions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Question_DAO.QuestionnaireQuestions
		/// </summary>
		public IEventList<IQuestionnaireQuestion> QuestionnaireQuestions
		{
			get
			{
				if (null == _QuestionnaireQuestions) 
				{
					if(null == _DAO.QuestionnaireQuestions)
						_DAO.QuestionnaireQuestions = new List<QuestionnaireQuestion_DAO>();
					_QuestionnaireQuestions = new DAOEventList<QuestionnaireQuestion_DAO, IQuestionnaireQuestion, QuestionnaireQuestion>(_DAO.QuestionnaireQuestions);
					_QuestionnaireQuestions.BeforeAdd += new EventListHandler(OnQuestionnaireQuestions_BeforeAdd);					
					_QuestionnaireQuestions.BeforeRemove += new EventListHandler(OnQuestionnaireQuestions_BeforeRemove);					
					_QuestionnaireQuestions.AfterAdd += new EventListHandler(OnQuestionnaireQuestions_AfterAdd);					
					_QuestionnaireQuestions.AfterRemove += new EventListHandler(OnQuestionnaireQuestions_AfterRemove);					
				}
				return _QuestionnaireQuestions;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_QuestionnaireQuestions = null;
			
		}
	}
}


