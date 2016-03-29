using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements
{
    public interface IUrlElement
    {
        string Url { get; }

        UrlAction UrlAction { get; }
    }
}