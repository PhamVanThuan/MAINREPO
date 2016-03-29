namespace SAHL.Core.UI.Configuration
{
    public interface IEditorPageOrderedConfiguration : IEditorPageConfiguration
    {
        int Sequence { get; }
    }
}