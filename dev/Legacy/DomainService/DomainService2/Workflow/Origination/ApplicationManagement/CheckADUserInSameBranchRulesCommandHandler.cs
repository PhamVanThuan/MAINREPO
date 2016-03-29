using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class CheckADUserInSameBranchRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckADUserInSameBranchRulesCommand>
    {
        protected IOrganisationStructureRepository orgStructureRepository;
        protected IApplicationRepository applicationRepository;

        public CheckADUserInSameBranchRulesCommandHandler(ICommandHandler commandHandler, IOrganisationStructureRepository orgStructureRepository, IApplicationRepository applicationRepository)
            : base(commandHandler)
        {
            this.applicationRepository = applicationRepository;
            this.orgStructureRepository = orgStructureRepository;
        }

        public override void SetupRule()
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            IADUser adUser = orgStructureRepository.GetAdUserForAdUserName(command.ADUserName);
            command.RuleParameters = new object[] { application, adUser };
        }
    }
}