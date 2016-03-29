using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Core.UI.Elements
{
    public class ElementBusinessContext : BusinessContext
    {
        public ElementBusinessContext(string context, GenericKeyType keyType, long businessKeyValue, string elementId)
            : base(context, keyType, businessKeyValue)
        {
            this.ElementId = elementId;
        }

        public ElementBusinessContext(string context, BusinessKey businessKey, string elementId)
            : this(context, businessKey.KeyType, businessKey.Key, elementId)
        {
        }

        public string ElementId { get; protected set; }
    }
}