using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Interfaces.Halo.Models.Configuration
{
    [DataContract]
    public class HaloWizardPageModel
    {
        [DataMember]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "wizardPageType")]
        public string WizardPageType { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "sequence")]
        public int Sequence { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "contentMessage")]
        public string ContentMessage { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "pageState")]
        public string PageState { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "contentModel")]
        public IHaloTileModel ContentModel { get; set; }
    }
}
