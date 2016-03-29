using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.X2.Framework
{
    [Serializable]
    [JsonObject]
    public class DomainMessageException : Exception
    {
        public DomainMessageException()
            : base() { }

        public DomainMessageException(string message)
            : base(message) { }

        public DomainMessageException(string info, IDomainMessageCollection messageCollection, bool ignoreWarnings)
            : this(info, null, messageCollection, ignoreWarnings) { }

        public DomainMessageException(string message, Exception inner)
            : base(message, inner) { }

        public DomainMessageException(string message, Exception inner, IDomainMessageCollection messageCollection, bool ignoreWarnings)
            : base(message, inner)
        {
            this.IgnoreWarnings = ignoreWarnings;
            this.ErrorMessages = new List<string>();

            if (messageCollection != null)
            {
                foreach (var msg in messageCollection.ErrorMessages)
                {
                    this.ErrorMessages.Add(msg.Message + " - " + msg.Details);
                }
            }
        }

        public bool IgnoreWarnings
        {
            get;
            protected set;
        }

        public List<string> ErrorMessages
        {
            get;
            protected set;
        }
    }

    [Serializable]
    public class X2EngineException : Exception
    {
        public X2EngineException()
            : base("X2EngineException")
        {
        }

        public X2EngineException(string Message)
            : base(Message)
        {
        }
    }
}