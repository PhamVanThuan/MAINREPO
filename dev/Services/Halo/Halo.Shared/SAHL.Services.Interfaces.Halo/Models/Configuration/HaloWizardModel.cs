using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace SAHL.Services.Interfaces.Halo.Models.Configuration
{
    [DataContract]
    public class HaloWizardModel
    {
        [DataMember]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "wizardType")]
        public string WizardType { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "wizardPages")]
        public IEnumerable<HaloWizardPageModel> WizardPages { get; set; }
    }
}
