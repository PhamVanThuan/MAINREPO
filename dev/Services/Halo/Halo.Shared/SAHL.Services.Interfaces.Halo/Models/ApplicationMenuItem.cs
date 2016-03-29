using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Interfaces.Halo
{
    [DataContract]
    public class ApplicationMenuItem
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public string ModuleName { get; set; }

        [DataMember]
        public bool IsTileBased { get; set; }

        [DataMember]
        public string NonTilePageState { get; set; }
    }
}
