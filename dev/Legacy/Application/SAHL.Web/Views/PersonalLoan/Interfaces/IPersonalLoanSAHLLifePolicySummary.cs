using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IPersonalLoanSAHLLifePolicySummary : IViewBase
    {
        void BindInsurerName(string insurerName);
        void BindAccountSummary(IAccountCreditProtectionPlan creditProtectionAccount);
    }
}