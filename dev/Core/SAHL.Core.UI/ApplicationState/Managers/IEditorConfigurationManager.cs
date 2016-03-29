using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.ApplicationState.Managers
{
    public interface IEditorConfigurationManager
    {
        IEditorConfiguration GetEditorConfigurationForTile(Type tileConfigurationType);

        IEditor GetEditorFromEditorType(Type editorType);

        IEditorPageModelSelector GetEditorPageSelectorForEditorConfiguration(Type editorConfigurationType);

        IEditorPageModel CreateEditorPageModelFromType(Type editorPageModelType, BusinessContext businessContext);

        IEditorPageModelValidator GetEditorPageModelValidatorFromType(Type editorPageModelType);
    }
}