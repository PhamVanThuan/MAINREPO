using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Configuration
{
    public interface IEditorPageConfiguration
    {
    }

    public interface IEditorPageConfiguration<T> : IEditorPageConfiguration
        where T : IEditorPageModel
    {
    }
}