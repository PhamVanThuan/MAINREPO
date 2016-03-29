using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using SAHL.Core.Data;

namespace SAHL.Services.Interfaces.Halo.Models.Configuration
{
    [DataContract]
    public class HaloTileHeaderModel
    {
        [DataMember]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "icons")]
        public IEnumerable<HaloTileHeaderIconModel> Icons { get; set; }

        public IDataModel Data { get; set; }
    }
}
