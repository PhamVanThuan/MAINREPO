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
	/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO
	/// </summary>
	public partial class QuestionnaireQuestionAnswer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO>, IQuestionnaireQuestionAnswer
	{
				public QuestionnaireQuestionAnswer(SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO QuestionnaireQuestionAnswer) : base(QuestionnaireQuestionAnswer)
		{
			this._DAO = QuestionnaireQuestionAnswer;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO.Sequence
		/// </summary>
		public Int32 Sequence 
		{
			get { return _DAO.Sequence; }
			set { _DAO.Sequence = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO.Answer
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
		/// SAHL.Common.BusinessModel.DAO.QuestionnaireQuestionAnswer_DAO.QuestionnaireQuestion
		/// </summary>
		public IQuestionnaireQuestion QuestionnaireQuestion 
		{
			get
			{
				if (null == _DAO.QuestionnaireQuestion) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IQuestionnaireQuestion, QuestionnaireQuestion_DAO>(_DAO.QuestionnaireQuestion);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.QuestionnaireQuestion = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.QuestionnaireQuestion = (QuestionnaireQuestion_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


