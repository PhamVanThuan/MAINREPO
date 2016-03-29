using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using DomainService2.Workflow.PersonalLoan;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.PersonalLoan.ReturnDisbursedPersonalLoanToApplicationCommandHandlerSpecs
{
    [Subject(typeof(ReturnDisbursedPersonalLoanToApplicationCommandHandler))]
    public class When_the_application_has_been_disbursed_and_account_exists : DomainServiceSpec<ReturnDisbursedPersonalLoanToApplicationCommand, ReturnDisbursedPersonalLoanToApplicationCommandHandler>
    {
        private static IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;
        private static IApplicationRepository applicationRepository;
        private static ICommonRepository commonRepository;


        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            applicationUnsecuredLendingRepository = An<IApplicationUnsecuredLendingRepository>();
            applicationRepository = An<IApplicationRepository>();

            IApplication application = An<IApplication>();
            IAccount account = An<IAccount>();

            application.WhenToldTo(x => x.Account).Return(account);
            account.WhenToldTo(x => x.Key).Return(1);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
            applicationUnsecuredLendingRepository.WhenToldTo(x => x.ReturnDisbursedPersonalLoanToApplication(Param<int>.IsAnything));


            command = new ReturnDisbursedPersonalLoanToApplicationCommand(Param<int>.IsAnything);
            handler = new ReturnDisbursedPersonalLoanToApplicationCommandHandler(applicationUnsecuredLendingRepository, applicationRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_disburse_funds_for_unsecuredlending = () =>
        {
            applicationUnsecuredLendingRepository.WasToldTo(x => x.ReturnDisbursedPersonalLoanToApplication(Param<int>.IsAnything));
        };
    }
}
