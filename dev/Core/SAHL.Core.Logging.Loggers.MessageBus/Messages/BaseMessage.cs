using SAHL.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;

namespace SAHL.Core.Logging.Messages
{
    [Serializable]
    public abstract class BaseMessage : IBaseMessage
    {
        public BaseMessage(string source, string user = "", IDictionary<string, object> parameters = null)
            : this(source, ApplicationConfigurationProvider.Instance.ApplicationName, user, parameters)
        {
        }

        public BaseMessage(string source, string application, string user = "", IDictionary<string, object> parameters = null)
        {
            this.Application = application;
            this.MachineName = Environment.MachineName;
            this.Parameters = parameters;
            this.Source = source;
            this.MessageDate = DateTime.Now;

            if (string.IsNullOrEmpty(user))
            {
                IPrincipal principal = Thread.CurrentPrincipal;

                if (principal != null)
                {
                    if (principal.Identity != null)
                    {
                        User = principal.Identity.Name;
                    }
                }
            }
        }

        public Guid Id { get; protected set; }

        public DateTime MessageDate { get; protected set; }

        public string MachineName { get; protected set; }

        public string Application { get; protected set; }

        public string User { get; protected set; }

        public string Source { get; protected set; }

        public IDictionary<string, object> Parameters { get; protected set; }
    }
}