using Machine.Fakes;
using Machine.Specifications;
using SAHL.Batch.Common.ServiceContracts;
using SAHL.Batch.Service.MessageProcessors;
using SAHL.Batch.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Service.Test.CreateCapitecApplicationProcessorSpecs
{
    class when_there_is_a_generic_exception : WithFakes
    {
        static CreateCapitecApplicationProcessor processor;
        static ICapitecClientService capitecServiceClient;
        static CapitecApplicationMessage capitecApplicationMessage;
        static bool result;
        static Exception exception;

        Establish context = () =>
        {
            capitecApplicationMessage = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp());

            capitecServiceClient = An<ICapitecClientService>();
            capitecServiceClient.WhenToldTo(x => x.CreateApplication(capitecApplicationMessage.CapitecApplication, capitecApplicationMessage.Id)).Return(() =>
            {
                throw new NotSupportedException("This method is not supported.");
                return true;
            });

            processor = new CreateCapitecApplicationProcessor(capitecServiceClient);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => { result = processor.Process(capitecApplicationMessage); });
        };


        It should_throw_an_exception = () =>
        {
            exception.ShouldBeOfType(typeof(NotSupportedException));
        };
    }
}
