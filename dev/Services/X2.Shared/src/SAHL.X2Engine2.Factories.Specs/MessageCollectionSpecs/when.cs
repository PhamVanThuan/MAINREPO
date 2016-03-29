using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Core.SystemMessages;

namespace SAHL.X2Engine2.Factories.Specs.MessageCollectionSpecs
{
    public class when : WithFakes
    {
        private static MessageCollection messageCollection;
        private static SystemMessage infoMessage;
        private static SystemMessage infoMessage1;
        private static SystemMessage infoMessage2;
        private static SystemMessage warningMessage;
        private static SystemMessage errorMessage;
        private static IEnumerable<SystemMessage> messages;
        private static ReadOnlyCollection<IDomainMessage> collection;

        Establish context = () =>
        {
            messageCollection = new MessageCollection();
            infoMessage = new SystemMessage("info_1", SystemMessageSeverityEnum.Info);
            infoMessage1 = new SystemMessage("info_2", SystemMessageSeverityEnum.Info);
            infoMessage2 = new SystemMessage("info_3", SystemMessageSeverityEnum.Info);
            warningMessage = new SystemMessage("warning", SystemMessageSeverityEnum.Warning);
            errorMessage = new SystemMessage("error", SystemMessageSeverityEnum.Error);
            messages = new[] { infoMessage, infoMessage1, infoMessage2, warningMessage, errorMessage };
            messageCollection.AddMessages(messages);
        };

        Because of = () =>
        {
            collection = messageCollection.InfoMessages;
        };

        It should_populate_the_collection_with_the_info_messages = () =>
        {
            collection.Count(x => x.MessageType == DomainMessageType.Info).ShouldEqual(3);
        };

        It should_not_contain_any_error_messages = () =>
        {
            collection.Count(x => x.MessageType == DomainMessageType.Error).ShouldEqual(0);
        };

        It should_not_contain_any_warning_messages = () =>
        {
            collection.Count(x => x.MessageType == DomainMessageType.Warning).ShouldEqual(0);
        };

        It should_only_contain_info_messages = () =>
        {
            collection.Count.ShouldEqual(3);
        };
    }
}