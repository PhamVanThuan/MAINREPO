using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;

namespace SAHL.X2Engine2.Factories
{
    public class SafeMessage : ISystemMessage, IDomainMessage
    {
        public string Message
        { get; set; }

        public string Details
        { get; set; }

        public SystemMessageSeverityEnum Severity
        { get; set; }

        public SafeMessage(IDomainMessage message)
        {
            this.Message = message.Message;
            this.Details = message.Details;
            this.Severity = (message.MessageType == DomainMessageType.Error) ? SystemMessageSeverityEnum.Error : SystemMessageSeverityEnum.Warning;
        }

        public SafeMessage(ISystemMessage message)
        {
            this.Message = message.Message;
            this.Severity = message.Severity;
        }

        public override bool Equals(object obj)
        {
            if (obj is SafeMessage)
            {
                if (this.Severity == ((SafeMessage)obj).Severity)
                {
                    if (string.Compare(this.Message, ((SafeMessage)obj).Message) == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        string IDomainMessage.Message
        {
            get
            { return Message; }
        }

        DomainMessageType IDomainMessage.MessageType
        {
            get
            {
                if (this.Severity == SystemMessageSeverityEnum.Error)
                    return DomainMessageType.Error;
                else
                    return DomainMessageType.Warning;
            }
        }
    }

    public class MessageCollection : MarshalByRefObject, ISystemMessageCollection, ISponsor, IDomainMessageCollection
    {
        // use on and lynq across it to get what you need.
        private List<SafeMessage> all = new List<SafeMessage>();

        private IList<IDomainMessage> GetIDomainMessageList(List<SafeMessage> messages)
        {
            List<IDomainMessage> result = new List<IDomainMessage>();
            foreach (var message in messages)
            {
                result.Add(new SAHL.Common.DomainMessages.Error(message.Message, message.Details));
            }
            return result;
        }

        private IList<ISystemMessage> GetISystemMessageList(List<SafeMessage> messages)
        {
            List<ISystemMessage> result = new List<ISystemMessage>();
            foreach (var message in messages)
            {
                result.Add(new SystemMessage(message.Message, message.Severity));
            }
            return result;
        }

        public MessageCollection()
        {
        }

        ~MessageCollection()
        {
            this.Dispose();
        }

        #region MarshalByRef and ISponsor

        public ILease Lease
        {
            get
            {
                return (ILease)RemotingServices.GetLifetimeService(this);
            }
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = base.InitializeLifetimeService() as ILease;

            //Set lease properties
            lease.InitialLeaseTime = TimeSpan.FromSeconds(60);
            lease.RenewOnCallTime = TimeSpan.FromSeconds(30);
            lease.SponsorshipTimeout = TimeSpan.FromSeconds(60);

            lease.Register(this);
            return lease;
        }

        public void Dispose()
        {
            ILease lease = this.Lease;
            if (lease != null)
            {
                lease.Renew(TimeSpan.Zero);
                lease.Unregister(this);
            }
            this.all.Clear();
            this.all = null;
        }

        public TimeSpan Renewal(ILease lease)
        {
            return TimeSpan.FromSeconds(10);
        }

        #endregion MarshalByRef and ISponsor

        #region ISystemMessageCollection

        private SystemMessageSeverityEnum CastSeverity(DomainMessageType severity)
        {
            switch (severity)
            {
                case DomainMessageType.Error:
                    return SystemMessageSeverityEnum.Error;

                case DomainMessageType.Warning:
                    return SystemMessageSeverityEnum.Warning;

                case DomainMessageType.Info:
                    return SystemMessageSeverityEnum.Info;

                default:
                    throw new ArgumentException("Type not in enum");
            }
        }

        public void AddMessage(ISystemMessage messageToAdd)
        {
            all.Add(new SafeMessage(messageToAdd));
        }

        public void AddMessages(IEnumerable<ISystemMessage> messagesToAdd)
        {
            foreach (var message in messagesToAdd)
            {
                AddMessage(message);
            }
        }

        public void Aggregate(ISystemMessageCollection messageCollection)
        {
            AddMessages(messageCollection.AllMessages);
        }

        public IEnumerable<ISystemMessage> AllMessages
        {
            get
            {
                List<ISystemMessage> messages = new List<ISystemMessage>();
                foreach (var message in all)
                {
                    messages.Add(new SystemMessage(message.Message, message.Severity));
                }
                return messages;
            }
        }

        public void Clear()
        {
            all.Clear();
        }

        public IEnumerable<ISystemMessage> ErrorMessages()
        {
            var messages = all.Where(x => x.Severity == SystemMessageSeverityEnum.Error).ToList();
            List<ISystemMessage> result = new List<ISystemMessage>();
            foreach (var message in messages)
            {
                result.Add(new SystemMessage(message.Message, message.Severity));
            }
            return result;
        }

        public bool HasErrors
        {
            get
            {
                return all.Where(x => x.Severity == SystemMessageSeverityEnum.Error).ToList().Count > 0 ? true : false;
            }
        }

        public bool HasExceptions
        {
            get
            {
                return this.all.Any(x => x.Severity == SystemMessageSeverityEnum.Exception);
            }
        }

        public bool HasWarnings
        {
            get
            { return all.Where(x => x.Severity == SystemMessageSeverityEnum.Warning).ToList().Count > 0 ? true : false; }
        }

        public IEnumerable<ISystemMessage> WarningMessages()
        {
            var messages = all.Where(x => x.Severity == SystemMessageSeverityEnum.Warning).ToList();
            List<ISystemMessage> result = new List<ISystemMessage>();
            foreach (var message in messages)
            {
                result.Add(new SystemMessage(message.Message, message.Severity));
            }
            return result;
        }

        IEnumerable<ISystemMessage> ISystemMessageCollection.InfoMessages()
        {
            var messages = all.Where(x => x.Severity == SystemMessageSeverityEnum.Info).ToList();
            List<ISystemMessage> result = new List<ISystemMessage>();
            foreach (var message in messages)
            {
                result.Add(new SystemMessage(message.Message, message.Severity));
            }
            return result;
        }

        #endregion ISystemMessageCollection

        ReadOnlyCollection<IDomainMessage> IDomainMessageCollection.ErrorMessages
        {
            get
            {
                var messages = all.Where(x => x.Severity == SystemMessageSeverityEnum.Error).ToList();
                List<IDomainMessage> result = new List<IDomainMessage>();
                foreach (var message in messages)
                {
                    result.Add(new Error(message.Message, message.Details));
                }
                return new ReadOnlyCollection<IDomainMessage>(result);
            }
        }

        public bool HasErrorMessages
        {
            get
            { return all.Where(x => x.Severity == SystemMessageSeverityEnum.Error).ToList().Count > 0 ? true : false; }
        }

        public bool HasInfoMessages
        {
            get
            { return all.Where(x => x.Severity == SystemMessageSeverityEnum.Info).ToList().Count > 0 ? true : false; }
        }

        public bool HasWarningMessages
        {
            get
            { return all.Where(x => x.Severity == SystemMessageSeverityEnum.Warning).ToList().Count > 0 ? true : false; }
        }

      
        public ReadOnlyCollection<IDomainMessage> InfoMessages
        {
            get
            {
                var messages = all.Where(x => x.Severity == SystemMessageSeverityEnum.Info).ToList();
                List<IDomainMessage> result = new List<IDomainMessage>();
                foreach (var message in messages)
                {
                    result.Add(new Information(message.Message, message.Details));
                }
                return new ReadOnlyCollection<IDomainMessage>(result);
            }
        }

        ReadOnlyCollection<IDomainMessage> IDomainMessageCollection.WarningMessages
        {
            get
            {
                var messages = all.Where(x => x.Severity == SystemMessageSeverityEnum.Warning).ToList();
                List<IDomainMessage> result = new List<IDomainMessage>();
                foreach (var message in messages)
                {
                    result.Add(new Warning(message.Message, message.Details));
                }
                return new ReadOnlyCollection<IDomainMessage>(result);
            }
        }

        public int IndexOf(IDomainMessage item)
        {
            var result = all.Where(x => x.Message == item.Message && x.Details == item.Details && x.Severity == CastSeverity(item.MessageType)).FirstOrDefault();
            return all.IndexOf(result);
        }

        public void Insert(int index, IDomainMessage item)
        {
            all.Insert(index, new SafeMessage(item));
        }

        public void RemoveAt(int index)
        {
            all.RemoveAt(index);
        }

        public IDomainMessage this[int index]
        {
            get
            {
                return GetDomainMessageFromSafe(all[index]);
            }

            set
            {
                SafeMessage message = new SafeMessage(this[index]);
                all[index] = message;
            }
        }

        public void Add(IDomainMessage item)
        {
            all.Add(new SafeMessage(item));
        }

        public bool Contains(IDomainMessage item)
        {
            return all.Where(x => x.Message == item.Message && x.Details == item.Details && x.Severity == CastSeverity(item.MessageType)).Count() > 0 ? true : false;
        }

        public void CopyTo(IDomainMessage[] array, int arrayIndex)
        {
            GetDomainMessagesFromSafe().CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return all.Count();
            }
        }

        public bool IsReadOnly
        {
            get
            { return false; }
        }

        public bool Remove(IDomainMessage item)
        {
            var result = all.Where(x => x.Message == item.Message && x.Details == item.Details && x.Severity == CastSeverity(item.MessageType)).FirstOrDefault();
            if (result != null)
            {
                all.Remove(result);
            }
            return true;
        }

        public IEnumerator<IDomainMessage> GetEnumerator()
        {
            return GetDomainMessagesFromSafe().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetDomainMessagesFromSafe().GetEnumerator();
        }

        private DomainMessage GetDomainMessageFromSafe(SafeMessage safeMessage)
        {
            switch (safeMessage.Severity)
            {
                case SystemMessageSeverityEnum.Error:
                    {
                        return new Error(safeMessage.Message, safeMessage.Details);
                    }
                case SystemMessageSeverityEnum.Warning:
                    {
                        return new Warning(safeMessage.Message, safeMessage.Details);
                    }
                case SystemMessageSeverityEnum.Info:
                    {
                        return new Information(safeMessage.Message, safeMessage.Details);
                    }
                default:
                    {
                        throw new ArgumentException("Type Not supported in SystemMessageSeverityEnum");
                    }
            }
        }

        private List<IDomainMessage> GetDomainMessagesFromSafe()
        {
            List<IDomainMessage> messages = new List<IDomainMessage>();
            foreach (var safeMessage in all)
            {
                messages.Add(GetDomainMessageFromSafe(safeMessage));
            }
            return messages;
        }

        public SystemMessage[] CopyMessages()
        {
            List<SystemMessage> newMessages = new List<SystemMessage>();
            foreach (var message in all)
            {
                switch (message.Severity)
                {
                    case SystemMessageSeverityEnum.Error:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Error));
                            break;
                        }
                    case SystemMessageSeverityEnum.Warning:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Warning));
                            break;
                        }
                    case SystemMessageSeverityEnum.Info:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Info));
                            break;
                        }
                    case SystemMessageSeverityEnum.Debug:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Debug));
                            break;
                        }
                    case SystemMessageSeverityEnum.Exception:
                        {
                            newMessages.Add(new SystemMessage(message.Message, SystemMessageSeverityEnum.Exception));
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException("Type Not supported in SystemMessageSeverityEnum");
                        }
                }
            }
            return newMessages.ToArray();
        }

        public IEnumerable<ISystemMessage> DebugMessages()
        {
            return new List<ISystemMessage>();
        }

        public IEnumerable<ISystemMessage> ExceptionMessages()
        {
            return new List<ISystemMessage>();
        }
    }
}