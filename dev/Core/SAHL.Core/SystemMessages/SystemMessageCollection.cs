using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.SystemMessages
{
    [Serializable]
    [JsonObject]
    public class SystemMessageCollection : SAHL.Core.SystemMessages.ISystemMessageCollection
    {
        [JsonProperty]
        private List<ISystemMessage> systemMessages;

        [JsonConstructor]
        public SystemMessageCollection(List<ISystemMessage> AllMessages)
        {
            this.systemMessages = new List<ISystemMessage>(AllMessages);
        }

        public SystemMessageCollection()
            : this(new List<ISystemMessage>())
        {
        }

        public bool HasErrors
        {
            get
            {
                return this.systemMessages.Any(x => x.Severity == SystemMessageSeverityEnum.Error || x.Severity == SystemMessageSeverityEnum.Exception);
            }
        }

        public bool HasExceptions
        {
            get
            {
                return this.systemMessages.Any(x => x.Severity == SystemMessageSeverityEnum.Exception);
            }
        }

        public bool HasWarnings
        {
            get
            {
                return this.systemMessages.Any(x => x.Severity == SystemMessageSeverityEnum.Warning);
            }
        }

        public bool HasExceptionMessage
        {
            get
            {
                return this.systemMessages.Any(x => x.Severity == SystemMessageSeverityEnum.Exception);
            }
        }

        public IEnumerable<ISystemMessage> ErrorMessages()
        {
            return this.systemMessages.Where(x => x.Severity == SystemMessageSeverityEnum.Error || x.Severity == SystemMessageSeverityEnum.Exception);
        }

        public IEnumerable<ISystemMessage> WarningMessages()
        {
            return this.systemMessages.Where(x => x.Severity == SystemMessageSeverityEnum.Warning);
        }

        public IEnumerable<ISystemMessage> InfoMessages()
        {
            return this.systemMessages.Where(x => x.Severity == SystemMessageSeverityEnum.Info);
        }

        public IEnumerable<ISystemMessage> ExceptionMessages()
        {
            return this.systemMessages.Where(x => x.Severity == SystemMessageSeverityEnum.Exception);
        }

        public IEnumerable<ISystemMessage> DebugMessages()
        {
            return this.systemMessages.Where(x => x.Severity == SystemMessageSeverityEnum.Debug);
        }

        public IEnumerable<ISystemMessage> AllMessages
        {
            get
            {
                return this.systemMessages.ToList();
            }
        }

        public void AddMessage(ISystemMessage messageToAdd)
        {
            this.systemMessages.Add(messageToAdd);
        }

        public void AddMessages(IEnumerable<ISystemMessage> messagesToAdd)
        {
            this.systemMessages.AddRange(messagesToAdd);
        }

        public void Aggregate(ISystemMessageCollection messageCollection)
        {
            this.systemMessages.AddRange(messageCollection.AllMessages);
        }

        public void Clear()
        {
            this.systemMessages.Clear();
        }

        public static ISystemMessageCollection Empty()
        {
            return new SystemMessageCollection();
        }

        public void Dispose()
        {
            this.systemMessages.Clear();
            this.systemMessages = null;
        }

        public SystemMessage[] CopyMessages()
        {
            List<SystemMessage> newMessages = new List<SystemMessage>();
            foreach (var message in systemMessages)
            {
                switch (message.Severity)
                {
                    case SystemMessageSeverityEnum.Error:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Error)); break;
                        }
                    case SystemMessageSeverityEnum.Warning:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Warning)); break;
                        }
                    case SystemMessageSeverityEnum.Info:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Info)); break;
                        }
                    case SystemMessageSeverityEnum.Exception:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Exception)); break;
                        }
                    case SystemMessageSeverityEnum.Debug:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Debug)); break;
                        }
                    default:
                        {
                            throw new ArgumentException("Type Not supported in SystemMessageSeverityEnum");
                        }
                }
            }
            return newMessages.ToArray();
        }
    }
}