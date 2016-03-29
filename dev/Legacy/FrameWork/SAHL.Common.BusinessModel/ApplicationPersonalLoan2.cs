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
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.Globals;
using System.Collections;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Used to populate business rules and supporting functions related to Personal Loan
    /// </summary>
    public partial class ApplicationPersonalLoan : Application, IApplicationPersonalLoan
    {

        /// <summary>
        /// Populates rules that are present in the list of rules.
        /// </summary>
        /// <param name="Rules">List<string></param>
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
        }
    }
}


