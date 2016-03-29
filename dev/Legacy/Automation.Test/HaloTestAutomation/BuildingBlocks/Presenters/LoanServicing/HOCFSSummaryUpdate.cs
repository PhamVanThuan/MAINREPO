using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing
{
    /// <summary>
    /// The screen used from Loan Servicing to update the HOC details.
    /// </summary>
    public class HOCFSSummaryUpdate : HOCFSSummaryUpdateControls
    {
        /// <summary>
        /// Ensures that the view loads correctly for SAHL HOC clients.
        /// </summary>
        public void AssertViewLoadedSAHLHOC()
        {
            Assertions.WatiNAssertions.AssertFieldsAreDisabled(new List<Element> { base.Ceded, base.HOCPolicyNumber, base.HOCSumInsured });
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(
                new List<Element> { base.HOCStatus, base.HOCInsurer, base.SubsidenceDescription, base.ConstructionDescription });
            AssertDropdownListsLoaded();
        }

        /// <summary>
        /// Ensures the view loads correctly for other insurers.
        /// </summary>
        public void AssertViewLoadedOtherInsurers()
        {
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(new List<Element> { base.Ceded, base.HOCPolicyNumber, base.HOCSumInsured,
                 base.HOCStatus, base.HOCInsurer, base.SubsidenceDescription, base.ConstructionDescription });
            AssertDropdownListsLoaded();
        }

        /// <summary>
        /// Click the update button
        /// </summary>
        public void ClickUpdate()
        {
            base.btnUpdate.Click();
        }

        /// <summary>
        /// Checks that the dropdown lists on the screen are correctly populated with the expected values.
        /// </summary>
        public void AssertDropdownListsLoaded()
        {
            //construction
            Assertions.WatiNAssertions.AssertSelectListContents(base.ConstructionDescription,
                new List<string> { HOCConstruction.BrickandTile, HOCConstruction.Wooden }, true);
            //subsidence
            Assertions.WatiNAssertions.AssertSelectListContents(base.SubsidenceDescription,
                new List<string> { HOCSubsidence.Extended, HOCSubsidence.Limited, HOCSubsidence.NotRequired }, true);
        }

        /// <summary>
        /// sets the HOC insurer dropdown selected value to the one provided.
        /// </summary>
        /// <param name="HOCInsurer">new Insurer to select</param>
        public void SetHOCInsurer(string HOCInsurer)
        {
            base.HOCInsurer.SelectByValue(HOCInsurer);
        }

        /// <summary>
        /// sets the policy number field
        /// </summary>
        /// <param name="policyNumber">new Policy Number to set</param>
        public void SetPolicyNumber(string policyNumber)
        {
            base.HOCPolicyNumber.Clear();
            base.HOCPolicyNumber.Value = policyNumber;
        }

        /// <summary>
        /// Sets a new HOC sum insured value
        /// </summary>
        /// <param name="totalSumInsured">new total sum insured to set</param>
        public void UpdateSumInsured(double totalSumInsured)
        {
            base.HOCSumInsured.Clear();
            base.HOCSumInsured.Value = (totalSumInsured).ToString();
            base.btnUpdate.Click();
        }

        /// <summary>
        /// This will change the ceded value, setting it to be the opposite of what is currently selected.
        /// </summary>
        public bool UpdateCededValue()
        {
            //get the current state
            bool cededState = base.Ceded.Checked;
            //chane it
            base.Ceded.Checked = !cededState;
            base.btnUpdate.Click();
            return !cededState;
        }

        /// <summary>
        /// Sets the value of the HOC status dropdown
        /// </summary>
        /// <param name="HOCStatus">New HOC Status</param>
        public void SetHOCStatus(HOCStatusEnum HOCStatus)
        {
            base.HOCStatus.SelectByValue(((int)HOCStatus).ToString());
        }

        /// <summary>
        /// Asserts that the current selected value of the HOC Insurer dropdown matches the expected value
        /// </summary>
        /// <param name="HOCInsurer">Expected HOC Insurer</param>
        public void AssertHOCInsurer(HOCInsurerEnum HOCInsurer)
        {
            var currentValue = base.HOCInsurer.SelectedOption.Value;
            Assert.That(int.Parse(currentValue) == (int)HOCInsurer);
        }
    }
}