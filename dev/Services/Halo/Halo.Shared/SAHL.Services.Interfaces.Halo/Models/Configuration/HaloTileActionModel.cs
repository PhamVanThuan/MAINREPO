using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Interfaces.Halo
{
    [DataContract]
    public class HaloTileActionModel
    {
        [DataMember]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "iconName")]
        public string IconName { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "group")]
        public string Group { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "sequence")]
        public string Sequence { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "actionType")]
        public string ActionType { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "tileConfiguration")]
        public IHaloTileConfiguration TileConfiguration { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "wizardTileConfiguration")]
        public IHaloWizardTileConfiguration WizardTileConfiguration { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "rootTileConfigurationName")]
        public string RootTileConfigurationName { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "processName")]
        public string ProcessName { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "workflowName")]
        public string WorkflowName { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "instanceId")]
        public long InstanceId { get; set; }
    }
}
