using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.UpdateAccountStatusCommandHandleSpecs
{
    [Subject(typeof(UpdateAccountStatusCommandHandler))]
    public class When_update_account_status_with_no_validation : DomainServiceSpec<UpdateAccountStatusCommand, UpdateAccountStatusCommandHandler>
    {
        protected static IAccountRepository accRepository;

        Establish context = () =>
        {
            accRepository = An<IAccountRepository>();

            command = new UpdateAccountStatusCommand(Param.IsAny<int>(), Param.IsAny<int>());
            handler = new UpdateAccountStatusCommandHandler(accRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_repository_method_to_update_account_status = () =>
        {
            accRepository.WasToldTo(x => x.UpdateAccountStatusWithNoValidation(Param.IsAny<int>(), Param.IsAny<int>()));
        };
    }
}