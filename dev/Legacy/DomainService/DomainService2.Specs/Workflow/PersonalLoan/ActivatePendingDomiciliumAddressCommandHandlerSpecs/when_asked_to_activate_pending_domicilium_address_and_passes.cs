using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Workflow.PersonalLoan.ActivatePendingDomiciliumAddressCommandHandlerSpecs
{
    [Subject(typeof(ActivatePendingDomiciliumAddressCommandHandler))]
    public class when_asked_to_activate_pending_domicilium_address_and_passes : DomainServiceSpec<ActivatePendingDomiciliumAddressCommand, ActivatePendingDomiciliumAddressCommandHandler>
    {
        static IApplicationRepository applicationRepository;

        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            command = new ActivatePendingDomiciliumAddressCommand(Param.IsAny<int>());
            handler = new ActivatePendingDomiciliumAddressCommandHandler(applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages,command);
        };

        It should_tell_application_repository_to_activate_pending_domicilium_addresses = () =>
        {
            applicationRepository.WasToldTo(x => x.ActivatePendingDomiciliumAddress(Param.IsAny<int>()));
        };
    }
}
