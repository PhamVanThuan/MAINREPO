using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IPreDebtCounsellingAccountDetails : IViewBase
    {
        void BindSnapShotAccount(ISnapShotAccount snapShotAccount, double preDCInstalment, double linkRate, double marketRate, double interestRate, int term,double lifeInstallment,double hocInstallment);

        string Set_DebtCounsellingCancelledHeading_InnerText { set; }

        bool Set_DebtCounsellingInfo_Visibility { set; }
    }
}