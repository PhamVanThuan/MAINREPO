using System;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.ValuationRequiredCommandHandlerSpecs
{
    [Subject(typeof(ValuationRequiredCommandHandler))]
    public class When_furtheradvance_valuation_rqd : WithFakes
    {
        static ValuationRequiredCommand command;
        static ValuationRequiredCommandHandler handler;
        static IDomainMessageCollection messages;

        Establish context = () =>
            {
                IApplicationRepository applicationRepository = An<IApplicationRepository>();
                IApplicationMortgageLoan appMortgageLoan = An<IApplicationMortgageLoan>();
                IApplicationType appType = An<IApplicationType>();
                IValuation valuation = An<IValuation>();
                IProperty property = An<IProperty>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(appMortgageLoan);

                appMortgageLoan.WhenToldTo(x => x.ApplicationType).Return(appType);
                appType.WhenToldTo(x => x.Key).Return((int)OfferTypes.FurtherAdvance);
                appMortgageLoan.WhenToldTo(x => x.Property).Return(property);
                property.WhenToldTo(x => x.LatestCompleteValuation).Return(valuation);

                valuation.WhenToldTo(x => x.ValuationDate).Return(DateTime.Now.AddYears(-4));

                messages = new DomainMessageCollection();
                command = new ValuationRequiredCommand(1);
                handler = new ValuationRequiredCommandHandler(applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_true = () =>
            {
                command.Result.ShouldBeTrue();
            };
    }
}