using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Data;
using SAHL.Common.BusinessModel.DAO;
using System.Text.RegularExpressions;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Common.Globals;

namespace SAHL.Common.UI.CBOSecurityFilters
{
    /// <summary>
    /// This is a negative filter
    /// This filter is applied to all WF states in Origination except Application Management
    /// The filter will remove access to Add and Remove Legal Entity actions from 
    /// FL Application Processor, FL Admin, FL Manager and FL Supervisor 
    /// </summary>
    public class FurtherLendingFilter : GenericRegExFilter
    {
        public FurtherLendingFilter()
            : base()
        {
            //Allow Further Lending to Add/remove Legal Entities
            //This should only be allowed at Manage Application STATE, for a limited list of users/groups
            if (_currentPrincipal.IsInRole("FL Application Processor") || _currentPrincipal.IsInRole("FL Admin") || _currentPrincipal.IsInRole("FL Manager") || _currentPrincipal.IsInRole("FL Supervisor"))
            {
                //Not sure if we can get away from checking the State here...
                if (_instanceNode != null && _instanceNode.Instance != null
                    && _instanceNode.Instance.State != null
                    && String.Compare(_instanceNode.Instance.State.Name, "Manage Application", true) != 0)
                {
                    _filters.Add(new Regex("Add Legal Entity", RegexOptions.IgnoreCase));
                    _filters.Add(new Regex("Remove Legal Entity", RegexOptions.IgnoreCase));
                }
            }
        }
    }
}
