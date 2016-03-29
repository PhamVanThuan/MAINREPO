using SAHL.Core.Services;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class SubmitEditorPageForUserCommand : ServiceCommand, IServiceCommandWithReturnedData<IEnumerable<IUIValidationResult>>, IHaloServiceCommand
    {
        public SubmitEditorPageForUserCommand(string userName, EditorBusinessContext editorBusinessContext, IEditorPageModel pageModelToValidate)
        {
            this.UserName = userName;
            this.EditorBusinessContext = editorBusinessContext;
            this.PageModelToValidate = pageModelToValidate;
        }

        public IEnumerable<IUIValidationResult> Result { get; set; }

        public string UserName { get; set; }

        public EditorBusinessContext EditorBusinessContext { get; set; }

        public IEditorPageModel PageModelToValidate { get; set; }
    }
}