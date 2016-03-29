using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAHL.Services.Interfaces.Halo
{
    [DataContract]
    public class HaloApplicationModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public IEnumerable<HaloModuleTileModel> Modules { get; set; }
    }
}
