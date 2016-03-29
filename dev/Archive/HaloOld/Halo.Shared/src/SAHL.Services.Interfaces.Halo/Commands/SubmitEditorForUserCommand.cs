using SAHL.Core.Services;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.Halo.Commands
{
    public class SubmitEditorForUserCommand : ServiceCommand, IServiceCommandWithReturnedData<IEnumerable<IUIValidationResult>>, IHaloServiceCommand
    {
        public SubmitEditorForUserCommand(string userName, EditorBusinessContext editorBusinessContext, IEditorPageModel finalPageModel)
        {
            this.UserName = userName;
            this.EditorBusinessContext = editorBusinessContext;
            this.FinalPageModel = finalPageModel;
        }

        public IEnumerable<IUIValidationResult> Result { get; set; }

        public string UserName { get; set; }

        public EditorBusinessContext EditorBusinessContext { get; set; }

        public IEditorPageModel FinalPageModel { get; set; }
    }
}