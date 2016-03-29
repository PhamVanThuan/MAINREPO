using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.ApplicationState.Managers
{
    public class EditorConfigurationManager : IEditorConfigurationManager
    {
        private IIocContainer iocContainer;

        public EditorConfigurationManager(IIocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        public IEditorConfiguration GetEditorConfigurationForTile(Type tileConfigurationType)
        {
            Type openGenericParentedEditorConfigType = typeof(IParentedEditorConfiguration<>);
            Type genericParentedEditorConfigType = openGenericParentedEditorConfigType.MakeGenericType(tileConfigurationType);

            IEditorConfiguration editorConfiguration = this.iocContainer.GetInstance(genericParentedEditorConfigType) as IEditorConfiguration;

            return editorConfiguration;
        }

        public IEditor GetEditorFromEditorType(Type editorType)
        {
            return this.iocContainer.GetConcreteInstance(editorType) as IEditor;
        }

        public IEditorPageModelSelector GetEditorPageSelectorForEditorConfiguration(Type editorConfigurationType)
        {
            Type openGenericEditorPageSelectorConfiguration = typeof(IEditorPageSelectorConfiguration<>);
            Type genericEditorPageSelectorConfiguration = openGenericEditorPageSelectorConfiguration.MakeGenericType(editorConfigurationType);
            IEditorPageSelectorConfiguration selectorConfiguration = this.iocContainer.GetInstance(genericEditorPageSelectorConfiguration) as IEditorPageSelectorConfiguration;

            var selector = this.iocContainer.GetConcreteInstance(selectorConfiguration.PageSelectorType);
            return selector as IEditorPageModelSelector;
        }

        public IEditorPageModel CreateEditorPageModelFromType(Type editorPageModelType, BusinessContext businessContext)
        {
            IEditorPageModel editorPage = this.iocContainer.GetConcreteInstance(editorPageModelType) as IEditorPageModel;

            editorPage.Initialise(businessContext);

            return editorPage;
        }

        public IEditorPageModelValidator GetEditorPageModelValidatorFromType(Type editorPageModelType)
        {
            Type openGenericEditorPageModelValidator = typeof(IEditorPageModelValidator<>);
            Type genericEditorPageModelValidator = openGenericEditorPageModelValidator.MakeGenericType(editorPageModelType);
            return this.iocContainer.GetInstance(genericEditorPageModelValidator) as IEditorPageModelValidator;
        }
    }
}