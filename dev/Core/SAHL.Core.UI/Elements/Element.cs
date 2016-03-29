using Newtonsoft.Json;
using SAHL.Core.BusinessModel;

namespace SAHL.Core.UI.Elements
{
    public class Element : IElement
    {
        public Element(string id)
        {
            this.id = id;
        }

        [JsonProperty]
        private string id;

        public string Id
        {
            get
            {
                return id;
            }
        }

        public BusinessContext BusinessContext { get; protected set; }
    }
}