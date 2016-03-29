using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    /// <summary>
    /// Contains BuildingBlock methods for the ApplicantsRemove screen
    /// </summary>
    public class ApplicantsRemove : ApplicantsRemoveControls
    {
        /// <summary>
        /// This building block is used to remove an applicant from an application by providing the Legal Entity Name of the applicant to be removed.
        /// </summary>
        /// <param name="LegalEntityName">Legal Entity Name</param>
        /// <param name="b">IE TestBrowser Object</param>
        public void RemoveApplicant(string LegalEntityName)
        {
            base.gridSelectOffer(LegalEntityName).Click();
            base.btnRemove.Click();
        }

        /// <summary>
        /// This building block is used to remove an applicant from an application.
        /// </summary>
        /// <param name="b">IE TestBrowser</param>
        public void RemoveApplicant()
        {
            base.btnRemove.Click();
        }
    }
}