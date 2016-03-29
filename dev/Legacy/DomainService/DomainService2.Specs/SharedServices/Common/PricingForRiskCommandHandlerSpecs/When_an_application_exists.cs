using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.PricingForRiskCommandHandlerSpecs
{
    [Subject(typeof(PricingForRiskCommandHandler))]
    public class When_an_application_exists : DomainServiceSpec<PricingForRiskCommand, PricingForRiskCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected static IApplication application;

        // Arrange
        Establish context = () =>
            {
                applicationRepository = An<IApplicationRepository>();
                application = An<IApplication>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
                application.WhenToldTo(x => x.PricingForRisk());
                applicationRepository.WhenToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));

                command = new PricingForRiskCommand(Param<int>.IsAnything);
                handler = new PricingForRiskCommandHandler(applicationRepository);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_calc_pricing_for_risk_and_save_the_application = () =>
            {
                application.WasToldTo(x => x.PricingForRisk());
                applicationRepository.WasToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));
            };
    }
}