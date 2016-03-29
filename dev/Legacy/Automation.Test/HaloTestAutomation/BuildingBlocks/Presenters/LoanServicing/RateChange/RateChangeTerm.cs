using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.RateChange
{
    public class RateChangeTerm : RateChangeTermControls
    {
        private readonly IWatiNService watinService;

        public RateChangeTerm()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Provides the new remaining term value for an account
        /// </summary>
        /// <param name="NewTerm">New Term Value</param>
        public void NewTermValue(string NewTerm)
        {
            //new remaining term value
            base.NewRemainingTerm.Value = NewTerm;
        }

        /// <summary>
        /// Provides the required comment for processing the term change
        /// </summary>
        /// <param name="Comment">Reason for Term Change</param>
        public void AddComment(string Comment)
        {
            //add a comment
            base.Comments.Value = Comment;
        }

        /// <summary>
        /// Calculates the amended values for the specified new remaining term
        /// </summary>
        public void CalculateTermChange()
        {
            //click the Calculate button
            base.CalculateButton.Click();
        }

        /// <summary>
        /// Processes the Term Change
        /// </summary>
        public void ProcessTermChange()
        {
            //click on the Process Term Change button
            base.ProcessTermChangeButton.Click();
        }

        /// <summary>
        /// Processes the Term Change and handles the subsequent dialogue box popup
        /// </summary>
        /// <param name="handlePopUp">Handles the dialogue box popup</param>
        public void ProcessTermChange(bool handlePopUp, bool createCase)
        {
            if (handlePopUp && createCase)
            {
                watinService.HandleConfirmationPopup(base.ProcessTermChangeButton);
                base.Document.Page<BasePage>().DomainWarningClickYesHandlingPopUp();
            }
            else if (handlePopUp && !createCase)
            {
                watinService.HandleConfirmationPopup(base.ProcessTermChangeButton);
            }
        }

        /// <summary>
        /// Checks that the Process Term Change button is disabled.
        /// </summary>
        public void AssertProcessTermChangeDisabled()
        {
            Assert.That(base.ProcessTermChangeButton.Enabled == false);
        }
    }
}