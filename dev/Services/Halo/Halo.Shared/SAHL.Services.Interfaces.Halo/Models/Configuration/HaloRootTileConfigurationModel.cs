using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using SAHL.Core.BusinessModel;

namespace SAHL.Services.Interfaces.Halo.Models
{
    [DataContract]
    public class HaloRootTileConfigurationModel
    {
        [DataMember]
        [JsonProperty(PropertyName = "rootTileConfigurations")]
        public IEnumerable<HaloTileConfigurationModel> RootTileConfigurations { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "childTileConfigurations")]
        public IEnumerable<HaloTileConfigurationModel> ChildTileConfigurations { get; set; }

        // Properties required for Mapping parameters
        public BusinessContext BusinessContext { get; set; }
        public HaloRoleModel Role { get; set; }
    }
}
