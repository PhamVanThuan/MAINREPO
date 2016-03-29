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
	/// Derived from Employment_DAO and is used to represent an Employment type of Self Employed.
	/// </summary>
	public partial class EmploymentSelfEmployed : Employment, IEmploymentSelfEmployed
	{
		protected new SAHL.Common.BusinessModel.DAO.EmploymentSelfEmployed_DAO _DAO;
		public EmploymentSelfEmployed(SAHL.Common.BusinessModel.DAO.EmploymentSelfEmployed_DAO EmploymentSelfEmployed) : base(EmploymentSelfEmployed)
		{
			this._DAO = EmploymentSelfEmployed;
		}
	}
}


