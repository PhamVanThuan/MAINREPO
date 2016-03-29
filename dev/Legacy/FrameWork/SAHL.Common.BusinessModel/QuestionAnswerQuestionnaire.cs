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
	/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO
	/// </summary>
	public partial class QuestionAnswerQuestionnaire : BusinessModelBase<SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO>, IQuestionAnswerQuestionnaire
	{
				public QuestionAnswerQuestionnaire(SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO QuestionAnswerQuestionnaire) : base(QuestionAnswerQuestionnaire)
		{
			this._DAO = QuestionAnswerQuestionnaire;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.Sequence
		/// </summary>
		public Int32 Sequence 
		{
			get { return _DAO.Sequence; }
			set { _DAO.Sequence = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.Questionnaire
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.QuestionAnswer
		/// </summary>
		public IQuestionAnswer QuestionAnswer 
		{
			get
			{
				if (null == _DAO.QuestionAnswer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IQuestionAnswer, QuestionAnswer_DAO>(_DAO.QuestionAnswer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.QuestionAnswer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.QuestionAnswer = (QuestionAnswer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


