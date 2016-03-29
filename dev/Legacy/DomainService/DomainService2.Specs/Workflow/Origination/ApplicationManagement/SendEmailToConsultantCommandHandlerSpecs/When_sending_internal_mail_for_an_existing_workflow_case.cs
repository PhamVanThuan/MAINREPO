using System.Collections.Generic;
using System.Data;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SendEmailToConsultantCommandHandlerSpecs
{
    [Subject(typeof(SendEmailToConsultantForQueryCommandHandler))]
    public class When_sending_internal_mail_for_an_existing_workflow_case : WithFakes
    {
        protected static SendEmailToConsultantForQueryCommand command;
        protected static SendEmailToConsultantForQueryCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IMessageService messageService;
        protected static IReasonRepository reasonRepository;
        protected static IX2Repository x2Repository;

        // Arrange
        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                x2Repository = An<IX2Repository>();
                reasonRepository = An<IReasonRepository>();
                messageService = An<IMessageService>();

                IInstance instance = An<IInstance>();
                var subject = "subject";
                instance.WhenToldTo(x => x.Subject).Return(subject);
                x2Repository.WhenToldTo(x => x.GetInstanceByKey(Param<int>.IsAnything)).Return(instance);

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(new DataColumn("EMailAddress", typeof(string)));
                dataTable.Rows.Add(new object[] { "test@sahomeloans.com" });

                x2Repository.WhenToldTo(x => x.GetCurrentConsultantAndAdmin(Param<long>.IsAnything)).Return(dataTable);

                IReason reason = An<IReason>();
                IReasonDefinition reasonDefinition = An<IReasonDefinition>();
                IReasonDescription reasonDescription = An<IReasonDescription>();

                var description = "description";
                var comment = "comment";

                reason.WhenToldTo(x => x.Comment).Return(comment);
                reasonDescription.WhenToldTo(x => x.Description).Return(description);
                reasonDefinition.WhenToldTo(x => x.ReasonDescription).Return(reasonDescription);
                reason.WhenToldTo(x => x.ReasonDefinition).Return(reasonDefinition);

                List<IReason> reasonList = new List<IReason>();
                reasonList.Add(reason);
                IReadOnlyEventList<IReason> reasons = new ReadOnlyEventList<IReason>(reasonList);

                reasonRepository.WhenToldTo(x => x.GetReasonByGenericKeyAndReasonGroupTypeKey(Param<int>.IsAnything, Param<int>.IsAnything)).Return(reasons);

                messageService.WhenToldTo(x => x.SendEmailInternal(Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything)).Return(true);

                command = new SendEmailToConsultantForQueryCommand(Param<int>.IsAnything, Param<long>.IsAnything, Param<int>.IsAnything);
                handler = new SendEmailToConsultantForQueryCommandHandler(reasonRepository, x2Repository, messageService);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_have_sent_an_internal_mail = () =>
            {
                messageService.WasToldTo(x => x.SendEmailInternal(Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));
            };
    }
}