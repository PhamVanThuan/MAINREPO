using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Interfaces.Halo.Models.Configuration
{
    [DataContract]
    public class HaloTileHeaderIconModel
    {
        [DataMember]
        [JsonProperty(PropertyName = "alignment")]
        public HaloTileIconAlignment Alignment { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "iconName")]
        public string IconName { get; set; }
    }
}