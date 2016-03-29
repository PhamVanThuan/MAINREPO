using SAHL.Core.UI.ApplicationState.Managers;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Editors;
using SAHL.Core.UI.Models;
using SAHL.Core.UI.UserState.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Core.UI.UserState.Managers
{
    public class EditorManager : IEditorManager
    {
        private readonly IEditorConfigurationManager editorConfigurationManager;

        public EditorManager(IEditorConfigurationManager editorConfigurationManager)
        {
            this.editorConfigurationManager = editorConfigurationManager;
        }

        public EditorElement GetEditor(TileBusinessContext tileBusinessContext)
        {
            IEditorConfiguration editorConfiguration = this.editorConfigurationManager.GetEditorConfigurationForTile(tileBusinessContext.TileConfigurationType);
            return editorConfiguration.CreateElement(tileBusinessContext);
        }

        public EditorState CreateEditorState(EditorBusinessContext editorBusinessContext)
        {
            // create the editor
            IEditor editor = this.editorConfigurationManager.GetEditorFromEditorType(editorBusinessContext.EditorType);

            // initialise the editor
            editor.Initialise(editorBusinessContext);

            // get the editor page selector
            IEditorPageModelSelector selector = this.editorConfigurationManager.GetEditorPageSelectorForEditorConfiguration(editorBusinessContext.EditorModelConfigurationType);

            // initialise the page selector
            ((dynamic)selector).Initialise((dynamic)editor);

            // create the editor state
            return new EditorState(editorBusinessContext, editor, selector);
        }

        public void SetFirstPageModelOnEditorState(EditorState editorState)
        {
            if (editorState.CurrentPage != null)
            {
                return;
            }
            var firstPageType = editorState.PageModelSelector.GetFirstPage();
            Type pageType = firstPageType.EditorPageModelType;

            var page = this.editorConfigurationManager.CreateEditorPageModelFromType(pageType, editorState.EditorBusinessContext);

            // initialise the page
            page.Initialise(editorState.EditorBusinessContext);

            editorState.CurrentPage = new UIEditorPageModel(page, firstPageType.IsFirstPage, firstPageType.IsLastPage);
        }

        public void SetNextPageModelOnEditorState(EditorState editorState)
        {
            if (editorState.CurrentPage == null || editorState.CurrentPage.IsLastPage)
            {
                return;
            }
            var nextPageType = editorState.PageModelSelector.GetNextPage(editorState.CurrentPage.EditorPageModel);
            Type pageType = nextPageType.EditorPageModelType;

            var page = this.editorConfigurationManager.CreateEditorPageModelFromType(pageType, editorState.EditorBusinessContext);

            // initialise the page
            page.Initialise(editorState.EditorBusinessContext);

            editorState.CurrentPage = new UIEditorPageModel(page, nextPageType.IsFirstPage, nextPageType.IsLastPage);
        }

        public void SetPreviousPageModelOnEditorState(EditorState editorState)
        {
            if (editorState.CurrentPage == null || editorState.CurrentPage.IsFirstPage)
            {
                return;
            }
            var oldCurrent = editorState.CurrentPage.EditorPageModel;
            var prevPageType = editorState.PageModelSelector.GetPreviousPage(editorState.CurrentPage.EditorPageModel);

            editorState.CurrentPage = new UIEditorPageModel(editorState.SubmittedPageModels[editorState.SubmittedPageModels.Count - 1], prevPageType.IsFirstPage, prevPageType.IsLastPage);

            // check if the oldCurrent has already been submitted as a validated model, if so remove it
            editorState.SubmittedPageModels.Remove(oldCurrent);
        }

        public IEnumerable<IUIValidationResult> SubmitPageModelOnEditorState(EditorState editorState, IEditorPageModel editorPageModelToValidate)
        {
            List<IUIValidationResult> validationResults = new List<IUIValidationResult>();
            // validate first
            // get the validator if any for this pagemodel
            IEditorPageModelValidator validator = this.editorConfigurationManager.GetEditorPageModelValidatorFromType(editorPageModelToValidate.GetType());
            if (validator != null)
            {
                // do the validation
                validationResults.AddRange(validator.Validate(editorPageModelToValidate));
            }

            if (validationResults.Count != 0)
            {
                return validationResults;
            }
            // if it has no validation errors then add the page to the submitted models
            editorState.SubmittedPageModels.Add(editorPageModelToValidate);

            // if this is not the last page we can move on
            if (!editorState.CurrentPage.IsLastPage)
            {
                this.SetNextPageModelOnEditorState(editorState);
            }
            else
            {
                // if this is the last page we should submit the editor
                validationResults.AddRange(editorState.Editor.SubmitEditor(editorState.SubmittedPageModels));
            }
            return validationResults;
        }
    }
}