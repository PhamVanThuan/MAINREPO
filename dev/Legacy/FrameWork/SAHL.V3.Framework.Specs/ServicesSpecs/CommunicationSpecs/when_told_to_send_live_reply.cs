using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.V3.Framework.Services;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.ServicesSpecs.CommunicationSpecs
{
    public class when_told_to_send_live_reply : WithFakes
    {
        private static AutoMocker<CommunicationService> autoMocker = new NSubstituteAutoMocker<CommunicationService>();
        private static SendComcorpLiveReplyCommand command;
        private static ISystemMessageCollection messages;

        Establish context = () =>
            {
                ICommunicationsServiceClient client = An<ICommunicationsServiceClient>();
                autoMocker.Inject(client);
            };

        Because of = () =>
            {
                messages = autoMocker.ClassUnderTest.SendComcorpLiveReply(Guid.NewGuid(), "test", "123", "123ref", "event", DateTime.Now, 1, 10, 10, 10);
            };

        It should_suceed = () =>
            {
                messages.HasErrors.ShouldBeFalse();
            };


    }
}
