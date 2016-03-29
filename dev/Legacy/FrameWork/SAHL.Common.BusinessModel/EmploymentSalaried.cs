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
	/// Derived from the Employment_DAO and is used to represent an Employment type of Salaried.
	/// </summary>
	public partial class EmploymentSalaried : Employment, IEmploymentSalaried
	{
		protected new SAHL.Common.BusinessModel.DAO.EmploymentSalaried_DAO _DAO;
		public EmploymentSalaried(SAHL.Common.BusinessModel.DAO.EmploymentSalaried_DAO EmploymentSalaried) : base(EmploymentSalaried)
		{
			this._DAO = EmploymentSalaried;
		}
	}
}


