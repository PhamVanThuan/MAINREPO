using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.OptOutNonPerformingLoanCommandHandlerSpecs
{
    public class When_not_a_non_performing_loan : DomainServiceSpec<OptOutNonPerformingLoanCommand, OptOutNonPerformingLoanCommandHandler>
    {
        static IAccountRepository accountRep;
        static ICommonRepository commonRepository;
        static int accountKey = 9999;
        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            accountRep = An<IAccountRepository>();
            IFinancialServiceRepository finRepo = An<IFinancialServiceRepository>();
            finRepo.WhenToldTo(x => x.IsLoanNonPerforming(accountKey))
                .Return(false);

            command = new OptOutNonPerformingLoanCommand(accountKey);
            handler = new OptOutNonPerformingLoanCommandHandler(accountRep, finRepo, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_opt_out_the_account = () =>
        {
            accountRep.WasNotToldTo(x => x.OptOutNonPerforming(accountKey));
        };
    }
}