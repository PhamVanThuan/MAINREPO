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
	/// Derived from the Application_DAO and instantiated to represent a Short Term Loan Application.     
		/// DiscriminatorValue = "11"
	/// </summary>
	public partial interface IApplicationShortTermLoan : IEntityValidation, IBusinessModelObject, IApplication
	{
	}
}


