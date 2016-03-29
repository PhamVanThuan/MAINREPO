using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Editors;
using SAHL.Core.UI.Models;
using SAHL.Core.UI.UserState.Models;
using System.Collections.Generic;

namespace SAHL.Core.UI.UserState.Managers
{
    public interface IEditorManager
    {
        EditorElement GetEditor(TileBusinessContext editorBusinessContext);

        EditorState CreateEditorState(EditorBusinessContext editorBusinessContext);

        void SetFirstPageModelOnEditorState(EditorState editorState);

        void SetNextPageModelOnEditorState(EditorState editorState);

        void SetPreviousPageModelOnEditorState(EditorState editorState);

        IEnumerable<IUIValidationResult> SubmitPageModelOnEditorState(EditorState editorState, IEditorPageModel editorPageModelToValidate);
    }
}