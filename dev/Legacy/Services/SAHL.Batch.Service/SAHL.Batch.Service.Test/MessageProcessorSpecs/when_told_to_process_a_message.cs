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

namespace SAHL.Batch.Service.Test.MessageProcessorSpecs
{
    public class when_told_to_process_a_message : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<CreateCapitecApplicationProcessor> mock;
        private static CapitecApplicationMessage capitecApplicationMessage;
        private static bool status;

        Establish context = () =>
            {
                capitecApplicationMessage = new CapitecApplicationMessage(CapitecApplicationStubs.CreateDummyNewPurchaseApp());
                mock = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<CreateCapitecApplicationProcessor>();
                mock.Get<ICapitecClientService>().WhenToldTo(x => x.CreateApplication(capitecApplicationMessage.CapitecApplication, capitecApplicationMessage.Id)).Return(true);
            };

        Because of = () =>
            {
                 status = mock.ClassUnderTest.Process(capitecApplicationMessage);
            };

        It should_return_true= () =>
            {
                status.ShouldBeTrue();
            };


    }
}
