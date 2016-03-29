using System;

namespace SAHL.Core.UI.Configuration
{
    public interface IEditorPageSelectorConfiguration
    {
        Type PageSelectorType { get; }
    }

    public interface IEditorPageSelectorConfiguration<T> : IEditorPageSelectorConfiguration
        where T : IEditorConfiguration
    {
    }
}