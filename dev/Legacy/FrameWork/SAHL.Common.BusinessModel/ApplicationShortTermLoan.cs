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
	/// Derived from the Application_DAO and instantiated to represent a Short Term Loan Application.     
    /// DiscriminatorValue = "11"
	/// </summary>
	public partial class ApplicationShortTermLoan : Application, IApplicationShortTermLoan
	{
		protected new SAHL.Common.BusinessModel.DAO.ApplicationShortTermLoan_DAO _DAO;
		public ApplicationShortTermLoan(SAHL.Common.BusinessModel.DAO.ApplicationShortTermLoan_DAO ApplicationShortTermLoan) : base(ApplicationShortTermLoan)
		{
			this._DAO = ApplicationShortTermLoan;
		}
	}
}

