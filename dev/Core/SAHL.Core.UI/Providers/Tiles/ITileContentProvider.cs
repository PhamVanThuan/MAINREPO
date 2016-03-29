using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Providers.Tiles
{
    public interface ITileContentProvider
    {
        dynamic GetContent(BusinessKey businessKey);
    }

    public interface ITileContentProvider<T> : ITileContentProvider where T : ITileModel
    {
    }
}