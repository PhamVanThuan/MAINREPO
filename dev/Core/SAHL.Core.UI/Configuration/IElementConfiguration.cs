using SAHL.Core.UI.Elements;

namespace SAHL.Core.UI.Configuration
{
    public interface IElementConfiguration
    {
    }

    public interface IElementConfiguration<T> : IElementConfiguration where T : IElement
    {
        T CreateElement();
    }
}