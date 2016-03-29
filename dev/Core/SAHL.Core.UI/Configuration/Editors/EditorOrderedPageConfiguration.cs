using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Configuration.Editors
{
    public class EditorOrderedPageConfiguration<T> : EditorPageConfiguration<T>, IEditorPageOrderedConfiguration
        where T : IEditorPageModel
    {
        public EditorOrderedPageConfiguration(int sequence)
        {
            this.Sequence = sequence;
        }

        public int Sequence { get; protected set; }
    }
}