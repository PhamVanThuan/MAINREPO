using System.Runtime.Serialization;

namespace SAHL.Services.Interfaces.Halo
{
    [DataContract]
    public class HaloModuleModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public bool IsTileBased { get; set; }

        [DataMember]
        public string NonTilePageState { get; set; }
    }
}
