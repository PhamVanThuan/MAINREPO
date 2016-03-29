using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;
using SAHL.Communication;
using SAHL.Shared.Messages;
using SAHL.Shared.Messages.PersonalLoanLead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.Service.Specs.LeadsImporter
{
    public class when_given_a_collection_personal_loan_leads_to_publish : WithFakes
    {
        static IMessageBus messageBus;
        static IBatchPublisher publisher;
        static IEnumerable<PersonalLoanLead> messages;
        static IBatchService batchService;

        Establish context = () =>
        {
            messageBus = An<IMessageBus>();
            batchService = An<IBatchService>();
            publisher = new BatchPublisher(messageBus, new MessageBusDefaultConfiguration());
            messages = new List<PersonalLoanLead> { new PersonalLoanLead() { IdNumber = "830921" }, new PersonalLoanLead() { IdNumber = "22222222" } };
        };

        Because of = () =>
        {
            publisher.Publish(messages, batchService);
        };

        It should_publish_all_messages = () =>
        {
            messageBus.WasToldTo(x => x.Publish(Param.IsAny<PersonalLoanLeadMessage>())).Times(messages.Count());
        };
    }
}