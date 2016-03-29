using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.DebtCounselling
{
    public class GetEworkDataForLossControlCaseCommandHandler : IHandlesDomainServiceCommand<GetEworkDataForLossControlCaseCommand>
    {
        private IDebtCounsellingRepository debtCounsellingRepository;

        public GetEworkDataForLossControlCaseCommandHandler(IDebtCounsellingRepository debtCounsellingRepository)
        {
            this.debtCounsellingRepository = debtCounsellingRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetEworkDataForLossControlCaseCommand command)
        {
            string eFolderID = string.Empty;
            string eStageName = string.Empty;
            IADUser eADUser = null;

            debtCounsellingRepository.GetEworkDataForLossControlCase(command.AccountKey, out eStageName, out eFolderID, out eADUser);

            command.eFolderID = eFolderID;
            command.eStageName = eStageName;
            command.eADUserName = eADUser != null ? eADUser.ADUserName : "";
        }
    }
}