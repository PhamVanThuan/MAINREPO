using System.Collections.Generic;
using System.Runtime.Serialization;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Halo
{
    [DataContract]
    public class HaloModuleTileModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public bool IsTileBased { get; set; }

        [DataMember]
        public string NonTilePageState { get; set; }

        public bool AllRoots { get; set; }
        public string ModuleParameters { get; set; }
        public HaloRoleModel RoleModel { get; set; }

        [DataMember]
        public IEnumerable<HaloTileConfigurationModel> RootTileConfigurations { get; set; }

        [DataMember]
        public IEnumerable<HaloTileConfigurationModel> ChildTileConfigurations { get; set; }
    }
}
