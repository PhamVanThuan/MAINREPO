using SAHL.Core.UI.Context;
using SAHL.Core.UI.Models;
using System.Collections.Generic;

namespace SAHL.Core.UI.UserState.Models
{
    public class EditorState
    {
        public EditorState(EditorBusinessContext editorBusinessContext, IEditor editor, IEditorPageModelSelector pageModelSelector)
        {
            this.EditorBusinessContext = editorBusinessContext;
            this.Editor = editor;
            this.PageModelSelector = pageModelSelector;

            if (SubmittedPageModels == null)
            {
                SubmittedPageModels = new List<IEditorPageModel>();
            }
        }

        public IEditor Editor { get; protected set; }

        public IEditorPageModelSelector PageModelSelector { get; protected set; }

        public EditorBusinessContext EditorBusinessContext { get; protected set; }

        public List<IEditorPageModel> SubmittedPageModels { get; protected set; }

        public IUIEditorPageModel CurrentPage { get; set; }
    }
}