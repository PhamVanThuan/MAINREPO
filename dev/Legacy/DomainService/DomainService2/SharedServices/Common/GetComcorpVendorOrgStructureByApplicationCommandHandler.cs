using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class GetComcorpVendorOrgStructureByApplicationCommandHandler : IHandlesDomainServiceCommand<GetComcorpVendorOrgStructureByApplicationCommand>
    {
        private IApplicationReadOnlyRepository applicationReadOnlyRepository;

        public GetComcorpVendorOrgStructureByApplicationCommandHandler(IApplicationReadOnlyRepository applicationReadOnlyRepository)
        {
            this.applicationReadOnlyRepository = applicationReadOnlyRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetComcorpVendorOrgStructureByApplicationCommand command)
        {
            IApplication application = this.applicationReadOnlyRepository.GetApplicationByKey(command.ApplicationKey);
            IVendor vendor = application.GetComcorpVendor();

            if (vendor != null)
            {
                command.Result = vendor.OrganisationStructure.Key;
            }
            else
            {
                command.Result = -1;
            }
        }
    }
}