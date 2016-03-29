using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IVariFixLoanAccount : IEntityValidation, IAccount, IMortgageLoanAccount
    {
        IMortgageLoan FixedSecuredMortgageLoan
        {
            get;
        }


    }
}
