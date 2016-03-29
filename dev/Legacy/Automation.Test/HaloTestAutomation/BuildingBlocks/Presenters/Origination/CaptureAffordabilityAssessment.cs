using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Presenters.Origination
{
    public class CaptureAffordabilityAssessment : CaptureAffordabilityAssessmentControls
    {
        public void ClickSaveButton()
        {
            base.Save.Click();
        }

        public void ClickDeleteAssessmentButton()
        {
            base.DeleteAssessment.Click();
        }

        public void ClickAddContributorButton()
        {
            base.AddContributor.Click();
        }

        public void SelectIncomeAndNonIncomeContributorsByLegalEntityKey(int legalEntityKey)
        {
            base.chkIncomeAndNonIncomeContributors(legalEntityKey.ToString()).Click();
        }

        public void AddHouseholdDependants(int householdDependants)
        {
            base.txtHouseholdDependants.Value = householdDependants.ToString(); 
        }

        public void AddNonApplicantIncomeContributor(int legalEntityKey)
        {
            base.ddlNonApplicantIncomeContributors.SelectByValue(legalEntityKey.ToString());
        }
    }
}