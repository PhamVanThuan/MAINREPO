using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using SAHL.Shared.DataAccess;
using System.Collections;

namespace SAHL.Shared.Messages
{
    [Serializable]
    public abstract class MessageBase : ModelBase, IMessage
    {
        protected MessageBase()
        {

        }

        public MessageBase(string application, string user = "", Dictionary<string, object> parameters = null)
            : this(application, "", user : user, parameters : parameters, machineName : null)
        {
        }

        public MessageBase(string application, string source, string user = "", Dictionary<string, object> parameters = null, string machineName = null)
        {
            this.Application = application;
            this.Source = source;
            this.MachineName = String.IsNullOrEmpty(machineName) ? Environment.MachineName : machineName;
            this.Parameters = new Dictionary<string, string>();
            if (null != parameters)
            {
                foreach (var kvp in parameters)
                {
                    try
                    {
                        Type bussinessModelType = null;
                        object paramValue = null;

                        if (kvp.Key == "MethodParametersFromAspect")
                        {
                            List<object> filteredParams = new List<object>();

                            foreach (KeyValuePair<string, object> param in (IDictionary<string, object>)kvp.Value)
                            {
                                bussinessModelType = param.Value.GetType().GetInterface("SAHL.Common.Interfaces.IBusinessModelObject");
                                if (bussinessModelType == null)
                                {
                                    filteredParams.Add(param);
                                }
                            }
                        }
                        else
                        {
                            bussinessModelType = kvp.Value.GetType().GetInterface("SAHL.Common.Interfaces.IBusinessModelObject");
                            if (bussinessModelType == null)
                            {
                                paramValue = kvp.Value;
                            }
                        }

                        if (paramValue != null)
                        {
                            this.Parameters.Add(kvp.Key, JsonConvert.SerializeObject(paramValue, Formatting.Indented));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

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
            MessageDate = DateTime.Now;
        }

        public virtual string Source { get; protected set; }

        public virtual string User { get; protected set; }

        public virtual DateTime MessageDate { get; protected set; }

        public virtual string MachineName { get; protected set; }

        public virtual string Application { get; protected set; }

        public virtual IDictionary<string, string> Parameters { get; protected set; }

        public virtual int Id { get; set; }
    }
}