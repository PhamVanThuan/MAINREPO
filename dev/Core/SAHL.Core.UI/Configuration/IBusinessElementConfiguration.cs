using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Elements;

namespace SAHL.Core.UI.Configuration
{
    public interface IBusinessElementConfiguration
    {
    }

    public interface IBusinessElementConfiguration<T> : IElementConfiguration where T : IElement
    {
        T CreateElement(BusinessContext businessContext);
    }
}