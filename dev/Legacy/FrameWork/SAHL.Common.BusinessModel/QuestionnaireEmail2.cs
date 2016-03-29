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
	/// SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO
	/// </summary>
	public partial class QuestionnaireEmail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.QuestionnaireEmail_DAO>, IQuestionnaireEmail
	{
		protected void OnQuestionnaireQuestionnaireEmails_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnQuestionnaireQuestionnaireEmails_AfterAdd(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnQuestionnaireQuestionnaireEmails_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnQuestionnaireQuestionnaireEmails_BeforeAdd(ICancelDomainArgs args, object Item)
		{
		}
	}
}


