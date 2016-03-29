using ObjectMaps.FloBO;

namespace BuildingBlocks.Navigation.FLOBO
{
    public class ApplicationOverviewNode : ApplicationOverviewNodeControls
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="offerTypeDescription"></param>
        public void Select(int offerKey, string offerTypeDescription)
        {
            base.ApplicationOverview(offerKey, offerTypeDescription).Click();
        }

        /// <summary>
        /// </summary>
        public void LoanDetailsNode()
        {
            base.LoanDetails.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void PropertySummaryNode()
        {
            base.PropertySummary.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void ValuationsNode()
        {
            base.Valuations.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void ApplicationMemoNode()
        {
            base.ApplicationMemo.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void HomeOwnersCoverNode()
        {
            base.HomeOwnersCover.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void LoanConditionsNode()
        {
            base.LoanConditions.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void ApplicantSummaryNode()
        {
            base.ApplicantSummary.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void ITCSummaryNode()
        {
            base.ITCSummary.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void EmploymentNode()
        {
            base.Employment.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void AffordabilityNode()
        {
            base.Affordability.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void AssetsAndLiabilitiesNode()
        {
            base.AssetsAndLiabilities.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void GroupExposureNode()
        {
            base.GroupExposure.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void ApplicationWarningsNode()
        {
            base.ApplicationWarnings.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void LoanAttributesNode()
        {
            base.LoanAttributes.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void RevisionsNode()
        {
            base.Revisions.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void DeclineReasonsNode()
        {
            base.DeclineReasons.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void ApplicationReasonsNode()
        {
            base.ApplicationReasons.Click();
        }
    }
}