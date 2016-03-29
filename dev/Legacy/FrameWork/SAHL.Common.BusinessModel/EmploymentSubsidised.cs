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
	/// SAHL.Common.BusinessModel.DAO.EmploymentSubsidised_DAO
	/// </summary>
	public partial class EmploymentSubsidised : Employment, IEmploymentSubsidised
	{
		protected new SAHL.Common.BusinessModel.DAO.EmploymentSubsidised_DAO _DAO;
		public EmploymentSubsidised(SAHL.Common.BusinessModel.DAO.EmploymentSubsidised_DAO EmploymentSubsidised) : base(EmploymentSubsidised)
		{
			this._DAO = EmploymentSubsidised;
		}
	}
}


