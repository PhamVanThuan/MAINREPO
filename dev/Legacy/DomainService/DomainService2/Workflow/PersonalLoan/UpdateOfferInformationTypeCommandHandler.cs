using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.PersonalLoan
{
    public class UpdateOfferInformationTypeCommandHandler : IHandlesDomainServiceCommand<UpdateOfferInformationTypeCommand>
    {
        private IApplicationRepository applicationRepository;
        private ILookupRepository lookupRepository;

        public UpdateOfferInformationTypeCommandHandler(IApplicationRepository applicationRepository, ILookupRepository lookupRepository)
        {
            this.applicationRepository = applicationRepository;
            this.lookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateOfferInformationTypeCommand command)
        {
            IApplication app = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            app.GetLatestApplicationInformation().ApplicationInformationType = lookupRepository.ApplicationInformationTypes.ObjectDictionary[command.ApplicationInformationTypeKey.ToString()];
            applicationRepository.SaveApplication(app);
        }
    }
}