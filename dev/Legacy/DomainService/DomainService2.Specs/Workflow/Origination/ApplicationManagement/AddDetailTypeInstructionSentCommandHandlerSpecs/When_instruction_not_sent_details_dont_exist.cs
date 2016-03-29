using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.AddDetailTypeInstructionSentCommandHandlerSpecs
{
    [Subject(typeof(AddDetailTypeInstructionSentCommandHandler))]
    public class When_instruction_not_sent_details_dont_exist : WithFakes
    {
        protected static AddDetailTypeInstructionSentCommand command;
        protected static AddDetailTypeInstructionSentCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static ICastleTransactionsService castleTransactionsService;
        protected static IReadOnlyEventList<IDetail> details;

        // Arrange
        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            castleTransactionsService = An<ICastleTransactionsService>();
            messages = An<IDomainMessageCollection>();

            IApplication application = An<IApplication>();
            IAccountSequence accountSequence = An<IAccountSequence>();
            accountSequence.WhenToldTo(x => x.Key).Return(1);
            application.WhenToldTo(x => x.ReservedAccount).Return(accountSequence);
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            details = new ReadOnlyEventList<IDetail>();

            command = new AddDetailTypeInstructionSentCommand(Param<int>.IsAnything);
            handler = new AddDetailTypeInstructionSentCommandHandler(applicationRepository, castleTransactionsService);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        //// Assert
        //It should_update_and_save_each_detail_that_exists = () =>
        //{
        //    accountRepository.WasNotToldTo(x => x.SaveDetail(Param<IDetail>.IsAnything));
        //};
    }
}