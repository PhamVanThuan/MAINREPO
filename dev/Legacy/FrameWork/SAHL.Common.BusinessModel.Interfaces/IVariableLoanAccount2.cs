using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
	public partial interface IVariableLoanAccount : IEntityValidation, IAccount, IMortgageLoanAccount
	{

    }
}
