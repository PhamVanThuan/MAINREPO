using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using DomainService2.Workflow.PersonalLoan;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.PersonalLoan.CreateAndOpenPersonalLoanCommandHandlerSpecs
{
    [Subject(typeof(CreateAndOpenPersonalLoanCommandHandler))]
    public class When_the_application_exists : DomainServiceSpec<CreateAndOpenPersonalLoanCommand,CreateAndOpenPersonalLoanCommandHandler>
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
            IAccountSequence accountSequence = An<IAccountSequence>();

            application.WhenToldTo(x => x.ReservedAccount).Return(accountSequence);
            accountSequence.WhenToldTo(x => x.Key).Return(1);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
            applicationUnsecuredLendingRepository.WhenToldTo(x => x.CreateAndOpenPersonalLoan(Param<int>.IsAnything, Param<string>.IsAnything));


            command = new CreateAndOpenPersonalLoanCommand(Param<int>.IsAnything, Param<string>.IsAnything);
            handler = new CreateAndOpenPersonalLoanCommandHandler(applicationUnsecuredLendingRepository, applicationRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_disburse_funds_for_unsecuredlending = () =>
        {
            applicationUnsecuredLendingRepository.WasToldTo(x => x.CreateAndOpenPersonalLoan(Param<int>.IsAnything, Param<string>.IsAnything));
        };

    }
}
