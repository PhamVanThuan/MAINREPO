//using Machine.Fakes;
//using Machine.Specifications;
//using SAHL.Batch.Common.MessageForwarding;
//using SAHL.Batch.Common.Messages;
//using SAHL.Batch.Service.Registries;
//using SAHL.Services.Capitec.Models.Shared.Capitec;
//using StructureMap;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SAHL.Batch.Service.Test.MessageForwardingQueuedHandlerSpecs
//{
//    public class when_receiving_a_CreateCapitecApplicationRequest : WithFakes
//    {
//        static IForwardingQueuedHandler<CreateCapitecApplicationRequest, CapitecApplicationMessage> handler;
//        static IForwardingMessageBus forwardingMessageBus;
        
//        static CapitecApplicationMessage transformedMessage;
//        static bool transformFuncWasCalled;

//        static CapitecApplicationMessage TransformFunc(CreateCapitecApplicationRequest request)
//        {
//            transformFuncWasCalled = true;
//            return transformedMessage;
//        }

//        Establish context = () =>
//        {
//            forwardingMessageBus = An<IForwardingMessageBus>();
//            handler = new MessageForwardingQueuedHandler<CreateCapitecApplicationRequest, CapitecApplicationMessage>(forwardingMessageBus);
//            handler.Transform(TransformFunc);
//        };

//        Because of = () =>
//        {
//            var message = new CreateCapitecApplicationRequest(null, Guid.NewGuid());
//            handler.Subscribe(message);
//        };

//        It should_use_the_transform_function_to_transform_the_message = () =>
//        {
//            transformFuncWasCalled.ShouldBeTrue();
//        };

//        It should_publish_a_CapitecApplicationMessage = () =>
//        {
//            forwardingMessageBus.WasToldTo(x => x.Publish(transformedMessage));
//        };
//    }
//}
