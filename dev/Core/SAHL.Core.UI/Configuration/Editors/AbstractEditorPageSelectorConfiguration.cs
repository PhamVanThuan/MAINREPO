using System;

namespace SAHL.Core.UI.Configuration.Editors
{
    public abstract class AbstractEditorPageSelectorConfiguration<T> : IEditorPageSelectorConfiguration<T>
        where T : IEditorConfiguration
    {
        public abstract Type PageSelectorType { get; }
    }
}