using SAHL.Core.UI.Context;
using SAHL.Core.UI.Data.Models;
using SAHL.Core.UI.Elements.Areas;
using SAHL.Core.UI.Elements.Editors;
using SAHL.Core.UI.Models;
using SAHL.Core.UI.UserState.Models;
using System.Collections.Generic;
using System.Security.Principal;

namespace SAHL.Core.UI.UserState.Managers
{
    public interface IUserStateManager
    {
        MenuElementArea GetOrCreateMenuForUser(IPrincipal user);

        IUserDetails GetUserDetailsForUser(IPrincipal user);

        IEnumerable<FeatureAccess> GetFeatureAccessForUser(IPrincipal user);

        TileElementArea GetUsersTilesForContext(IPrincipal user, TileBusinessContext tileBusinessContext);

        TileElementArea DrillDownAndGetUsersTilesForContext(IPrincipal user, TileBusinessContext tileBusinessContext);

        EditorElement ShowEditorForUser(IPrincipal user, TileBusinessContext editorBusinessContext);

        IUIEditorPageModel GetEditorPageContentForUser(IPrincipal user, EditorBusinessContext editorBusinessContext);

        IUIEditorPageModel GetPreviousEditorPageContentForUser(IPrincipal user, EditorBusinessContext editorBusinessContext);

        IEnumerable<IUIValidationResult> SubmitEditorPageForUser(IPrincipal user, EditorBusinessContext editorBusinessContext, IEditorPageModel pageModelToValidate);

        IEnumerable<IUIValidationResult> SubmitEditorForUser(IPrincipal user, EditorBusinessContext editorBusinessContext, IEditorPageModel finalPageModel);
    }
}