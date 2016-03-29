using System;
using System.Collections;
using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.AddDetailTypeInstructionSentCommandHandlerSpecs
{
    [Subject(typeof(AddDetailTypeInstructionSentCommandHandler))]
    public class When_instruction_not_sent_details_exists : WithFakes
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

                IDetail detail = An<IDetail>();
                IDetailType instructionNotSentDetailType = An<IDetailType>();
                detail.WhenToldTo(x => x.DetailType).Return(instructionNotSentDetailType);
                detail.WhenToldTo(x => x.DetailDate).Return(new DateTime());
                details = new ReadOnlyEventList<IDetail>(new List<IDetail> { detail });

                IDetailType detailType = An<IDetailType>();
                detailType.WhenToldTo(x => x.Key).Return((int)DetailTypes.InstructionSent);
                IEventList<IDetailType> detailTypes = An<IEventList<IDetailType>>();

                IDictionary<string, IDetailType> dictionary = new Dictionary<string, IDetailType>();
                dictionary.Add(Convert.ToString((int)DetailTypes.InstructionSent), detailType);

                detailTypes.WhenToldTo(x => x.ObjectDictionary).Return(dictionary);

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
        //    {
        //        accountRepository.WasToldTo(x => x.SaveDetail(Param<IDetail>.IsAnything));
        //    };
    }
}