using BuildingBlocks.Services.Contracts;
using ObjectMaps.Presenters.PersonalLoans;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanDisbursement : PersonalLoanDisbursementControls
    {
        private readonly IWatiNService watinService;

        public PersonalLoanDisbursement()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void ClickConfirm()
        {
            watinService.HandleConfirmationPopup(base.btnConfirm);
        }
    }
}