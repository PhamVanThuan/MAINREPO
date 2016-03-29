using SAHL.Core.BusinessModel;

namespace SAHL.Core.UI.Elements
{
    public interface IElement
    {
        string Id { get; }

        BusinessContext BusinessContext { get; }
    }
}