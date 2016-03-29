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
	/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO
	/// </summary>
	public partial interface IQuestionAnswerQuestionnaire : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.Sequence
		/// </summary>
		System.Int32 Sequence
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.GeneralStatus
		/// </summary>
		IGeneralStatus GeneralStatus
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.Questionnaire
		/// </summary>
		IQuestionnaire Questionnaire
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswerQuestionnaire_DAO.QuestionAnswer
		/// </summary>
		IQuestionAnswer QuestionAnswer
		{
			get;
			set;
		}
	}
}


