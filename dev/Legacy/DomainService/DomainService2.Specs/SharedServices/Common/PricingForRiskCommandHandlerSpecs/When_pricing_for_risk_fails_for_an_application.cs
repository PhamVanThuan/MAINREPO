using System;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.SharedServices.PricingForRiskCommandHandlerSpecs
{
    [Subject(typeof(PricingForRiskCommandHandler))]
    public class When_pricing_for_risk_fails_for_an_application : DomainServiceSpec<PricingForRiskCommand, PricingForRiskCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected static IApplication application;
        protected static Exception exception;

        // Arrange
        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
            application.WhenToldTo(x => x.PricingForRisk()).Throw(new Exception());

            command = new PricingForRiskCommand(Param<int>.IsAnything);
            handler = new PricingForRiskCommandHandler(applicationRepository);
        };

        // Act
        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        // Assert
        It should_return_an_exception_and_not_save_the_application = () =>
        {
            exception.ShouldNotBeNull();
            applicationRepository.WasNotToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));
        };
    }
}