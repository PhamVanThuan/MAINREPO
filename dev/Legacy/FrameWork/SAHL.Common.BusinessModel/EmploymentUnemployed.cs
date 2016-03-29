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
	/// Derived from Employment_DAO and is used to represent an Employment type of Unemployed.
	/// </summary>
	public partial class EmploymentUnemployed : Employment, IEmploymentUnemployed
	{
		protected new SAHL.Common.BusinessModel.DAO.EmploymentUnemployed_DAO _DAO;
		public EmploymentUnemployed(SAHL.Common.BusinessModel.DAO.EmploymentUnemployed_DAO EmploymentUnemployed) : base(EmploymentUnemployed)
		{
			this._DAO = EmploymentUnemployed;
		}
	}
}


