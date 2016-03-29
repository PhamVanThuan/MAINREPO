using System.Collections.Generic;
using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.PersonalLoan.UpdateOfferInformationTypeSpecs
{
    [Subject(typeof(UpdateOfferInformationTypeCommandHandler))]
    public class When_application_with_offer_information_exists : DomainServiceSpec<UpdateOfferInformationTypeCommand, UpdateOfferInformationTypeCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        private static ILookupRepository lookupRepository;

        private static IApplication application;
        Establish context = () =>
        {
            application = An<IApplication>();
            var applicationInformation = An<IApplicationInformation>();
            var applicationInformationType = An<IApplicationInformationType>();
            var objectDictionary = new Dictionary<string, IApplicationInformationType>();
            applicationRepository = An<IApplicationRepository>();
            lookupRepository = An<ILookupRepository>();

            var applicationInformationTypes = An<IEventList<IApplicationInformationType>>();

            objectDictionary.Add(((int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer).ToString(), applicationInformationType);

            command = new UpdateOfferInformationTypeCommand(Param.IsAny<int>(), (int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer);
            handler = new UpdateOfferInformationTypeCommandHandler(applicationRepository, lookupRepository);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            application.WhenToldTo(x => x.GetLatestApplicationInformation()).Return(applicationInformation);
            lookupRepository.WhenToldTo(x => x.ApplicationInformationTypes).Return(applicationInformationTypes);
            applicationInformationType.WhenToldTo(x => x.Description).Return(((int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer).ToString());
            applicationInformationTypes.WhenToldTo(x => x.ObjectDictionary).Return(objectDictionary);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_save = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(application));
        };
    }
}