using Machine.Fakes;
using Machine.Specifications;
using SAHL.Batch.Common.Messages;
using SAHL.Batch.Common.ServiceContracts;
using SAHL.Batch.Service.MessageProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Service.Test.CreateCapitecApplicationProcessorSpecs
{
    public class when_there_is_a_domain_validation_exception : WithFakes
    {
        static CreateCapitecApplicationProcessor processor;
        static ICapitecClientService capitecServiceClient;
        static CapitecApplicationMessage capitecApplicationMessage;
        static bool result;

        Establish context = () =>
        {
            capitecApplicationMessage = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp());

            capitecServiceClient = An<ICapitecClientService>();
            capitecServiceClient.WhenToldTo(x => x.CreateApplication(capitecApplicationMessage.CapitecApplication, capitecApplicationMessage.Id)).Return(false);
            processor = new CreateCapitecApplicationProcessor(capitecServiceClient);
        };

        Because of = () =>
        {
            result = processor.Process(capitecApplicationMessage);
        };

        It should_ask_the_Capitec_Service_Client_to_create_application = () =>
        {
            capitecServiceClient.WasToldTo(x => x.CreateApplication(capitecApplicationMessage.CapitecApplication, capitecApplicationMessage.Id));
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}
