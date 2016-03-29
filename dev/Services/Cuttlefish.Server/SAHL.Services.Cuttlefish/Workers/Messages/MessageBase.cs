using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SAHL.Shared.Messages
{
    public abstract class MessageBase
    {
        [JsonProperty]
        public virtual string Source { get; set; }

        [JsonProperty]
        public virtual string User { get; set; }

        [JsonProperty]
        public virtual DateTime MessageDate { get; set; }

        [JsonProperty]
        public virtual string MachineName { get; set; }

        [JsonProperty]
        public virtual string Application { get; set; }

        [JsonProperty]
        public virtual Dictionary<string, string> Parameters { get; set; }

        [JsonProperty]
        public virtual int Id { get; set; }
    }
}