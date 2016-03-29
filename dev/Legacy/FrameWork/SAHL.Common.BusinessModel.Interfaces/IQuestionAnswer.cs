using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO
	/// </summary>
	public partial interface IQuestionAnswer : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.Sequence
		/// </summary>
		System.Int32 Sequence
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.QuestionAnswerQuestionnaires
		/// </summary>
		IEventList<IQuestionAnswerQuestionnaire> QuestionAnswerQuestionnaires
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.Answer
		/// </summary>
		IAnswer Answer
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.Question
		/// </summary>
		IQuestion Question
		{
			get;
			set;
		}
	}
}


