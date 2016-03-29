using NUnit.Framework;
using SAHL.Core.SystemMessages;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Common.Helpers
{
    public static class SystemMessageCollectionExtensions
    {
        public static void WithoutMessages(this ISystemMessageCollection messages, SystemMessageSeverityEnum severity)
        {
            var filteredMessages = SystemMessageCollection.Empty();
            filteredMessages.AddMessages(messages.AllMessages.Where(x => x.Severity == severity));
            filteredMessages.WithoutMessages();
        }

        public static void WithoutMessages(this ISystemMessageCollection messages)
        {
            var messageList = new List<string>();
            foreach (var message in messages.AllMessages)
            {
                messageList.Add(message.Message);
            }
            Assert.AreEqual(0, messageList.Count(), "The query returned messages: {0}", string.Join(" | ", messageList));
        }
    }
}