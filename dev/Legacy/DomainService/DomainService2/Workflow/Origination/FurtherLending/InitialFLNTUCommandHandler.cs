using DomainService2.SharedServices;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class InitialFLNTUCommandHandler : IHandlesDomainServiceCommand<InitialFLNTUCommand>
    {
        private IX2WorkflowService x2WorkflowService;
        private IApplicationRepository applicationRepository;
        private IOrganisationStructureRepository organisationStructureRepository;
        private ISAHLPrincipalCacheProvider principalCacheProvider;

        public InitialFLNTUCommandHandler(IX2WorkflowService x2WorkflowService, IApplicationRepository applicationRepository,
                                            IOrganisationStructureRepository organisationStructureRepository, ISAHLPrincipalCacheProvider principalCacheProvider)
        {
            this.x2WorkflowService = x2WorkflowService;
            this.applicationRepository = applicationRepository;
            this.organisationStructureRepository = organisationStructureRepository;
            this.principalCacheProvider = principalCacheProvider;
        }

        public void Handle(IDomainMessageCollection messages, InitialFLNTUCommand command)
        {
            if (!this.x2WorkflowService.HasInstancePerformedActivity(command.InstanceID, Constants.WorkFlowExternalActivity.ApplicationReceived))
            {
                ISAHLPrincipalCache SPC = this.principalCacheProvider.GetPrincipalCache();
                SPC.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);
                try
                {
                    IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
                    IADUser adUser = organisationStructureRepository.GetAdUserForAdUserName(command.ADUser);
                    application.AddRole((int)OfferRoleTypes.FLProcessorD, adUser.LegalEntity);
                    applicationRepository.SaveApplication(application);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (SPC != null)
                    {
                        SPC.ExclusionSets.Remove(RuleExclusionSets.LegalEntityExcludeAll);
                    }
                }
            }
        }
    }
}