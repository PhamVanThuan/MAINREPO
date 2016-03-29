using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.Services.Interfaces.Halo.Models.Configuration;

namespace SAHL.Services.Interfaces.Halo
{
    [DataContract]
    public class HaloTileConfigurationModel
    {
        [DataMember]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "tileType")]
        public string TileType { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "sequence")]
        public int Sequence { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "isTileBased")]
        public bool IsTileBased { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "nonTilePageState")]
        public string NonTilePageState { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "startRow")]
        public int StartRow { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "startColumn")]
        public int StartColumn { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "noOfRows")]
        public int NoOfRows { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "noOfColumns")]
        public int NoOfColumns { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "businessContext")]
        public BusinessContext BusinessContext { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "role")]
        public HaloRoleModel Role { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "tileHeader")]
        public HaloTileHeaderModel TileHeader { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "hasDrilldown")]
        public bool HasDrilldown { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "dataModelType")]
        public string DataModelType { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "tileData")]
        public IDataModel TileData { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "tileDataItems")]
        public IEnumerable<IDataModel> TileDataItems { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "tileSubKeys")]
        public IEnumerable<BusinessContext> TileSubKeys { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "tileActions")]
        public IEnumerable<HaloTileActionModel> TileActions { get; set; }
    }
}
