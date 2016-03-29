using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.Credit.DisqualifyApplicationForGEPFCommandHandlerSpecs
{
    [Subject(typeof(DisqualifyApplicationForGEPFCommandHandler))]
    public class when_disqualifying_an_application_for_gepf : DomainServiceSpec<DisqualifyApplicationForGEPFCommand, DisqualifyApplicationForGEPFCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected static IApplication application;
        protected static int appKey = 1;

        private Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(appKey)).Return(application);

            command = new DisqualifyApplicationForGEPFCommand(appKey);
            handler = new DisqualifyApplicationForGEPFCommandHandler(applicationRepository);
        };

        private Because of = () =>
        {
            handler.Handle(messages, command);
        };

        private It should_get_the_application = () =>
        {
            applicationRepository.WasToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything));
        };

        private It should_add_the_disqualified_for_gepf_attribute = () =>
        {
            applicationRepository.WasToldTo(x => x.AddApplicationAttributeIfNotExists(application, OfferAttributeTypes.DisqualifiedforGEPF));
        };

        private It should_save_application = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(application));
        };
    }
}