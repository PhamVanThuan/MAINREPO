using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when_copying_a_message_collection_to_a_domain_message_array : WithFakes
    {
        private static MessageCollection messageCollection;
        private static SystemMessage errorMessage;
        private static SystemMessage infoMessage;
        private static SystemMessage warningMessage;
        private static IEnumerable<SystemMessage> messages;
        private static IDomainMessage[] array = new IDomainMessage[3];

        Establish context = () =>
        {
            messageCollection = new MessageCollection();
            errorMessage = new SystemMessage("error", SystemMessageSeverityEnum.Error);
            infoMessage = new SystemMessage("info", SystemMessageSeverityEnum.Info);
            warningMessage = new SystemMessage("warning", SystemMessageSeverityEnum.Warning);
            messages = new[] { errorMessage, infoMessage, warningMessage };
            messageCollection.AddMessages(messages);
        };

        Because of = () =>
        {
            messageCollection.CopyTo(array, 0);
        };

        It should_copy_the_system_messages_to_the_array = () =>
        {
            array.Length.ShouldEqual(3);
        };

        It should_correctly_cast_an_error_system_message = () =>
        {
            array[0].ShouldBeOfType(typeof(Error));
        };

        It should_correctly_cast_an_info_system_message = () =>
        {
            array[1].ShouldBeOfType(typeof(Information));
        };

        It should_correctly_cast_a_warning_system_message = () =>
        {
            array[2].ShouldBeOfType(typeof(Warning));
        };
    }
}