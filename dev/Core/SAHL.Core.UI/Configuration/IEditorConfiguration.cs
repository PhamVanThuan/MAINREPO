using SAHL.Core.UI.Elements.Editors;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Configuration
{
    public interface IEditorConfiguration : IBusinessElementConfiguration<EditorElement>
    {
    }

    public interface IEditorConfiguration<T> : IEditorConfiguration where T : IEditor
    {
    }
}