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
		
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.QuestionAnswerQuestionnaires
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnQuestionAnswerQuestionnaires_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.QuestionAnswerQuestionnaires
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnQuestionAnswerQuestionnaires_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.QuestionAnswerQuestionnaires
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnQuestionAnswerQuestionnaires_AfterAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.QuestionAnswer_DAO.QuestionAnswerQuestionnaires
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnQuestionAnswerQuestionnaires_AfterRemove(ICancelDomainArgs args, object Item)
		{

		}
	}
}


