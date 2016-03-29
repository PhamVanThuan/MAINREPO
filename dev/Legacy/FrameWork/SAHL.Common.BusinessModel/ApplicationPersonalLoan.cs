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
    /// Derived from the Application. Implements IApplicationPersonalLoan Instantiated to represent a Personal Loan Application. 
    /// </summary>
    public partial class ApplicationPersonalLoan : Application, IApplicationPersonalLoan
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationPersonalLoan_DAO _DAO;

        public ApplicationPersonalLoan(SAHL.Common.BusinessModel.DAO.ApplicationPersonalLoan_DAO ApplicationPersonalLoan)
            : base(ApplicationPersonalLoan)
        {
            this._DAO = ApplicationPersonalLoan;
        }
    }
}


