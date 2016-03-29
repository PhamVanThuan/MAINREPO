using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IUnsecuredLoanSummary : IViewBase
    {
        void BindAccountSummary(IAccountPersonalLoan accountPersonalLoan);

        void BindLifePolicyClaimPending(string claimDate);
    }
}