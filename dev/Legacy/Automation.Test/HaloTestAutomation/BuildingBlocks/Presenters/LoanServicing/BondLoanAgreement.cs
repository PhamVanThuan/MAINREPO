using BuildingBlocks.Services.Contracts;
using Common.Extensions;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.LoanServicing
{
    /// <summary>
    /// Contains BuildingBlock methods for the BondLoanAgreement view
    /// </summary>
    public class BondLoanAgreement : BondLoanAgreementControls
    {
        private readonly IAttorneyService attorneyService;

        public BondLoanAgreement()
        {
            attorneyService = ServiceLocator.Instance.GetService<IAttorneyService>();
        }

        /// <summary>
        /// Selects a bond record from the grid when provided with the Bond Reg Number
        /// </summary>
        /// <param name="bondRegNumber">Bond Registration Number</param>
        public void SelectBond(string bondRegNumber)
        {
            base.LoanAgreementGridSelectRow(bondRegNumber).Click();
        }

        /// <summary>
        /// Adds a new loan agreement value to a bond when provided with a new LAA Amount
        /// </summary>
        /// <param name="laaAmount">New Loan Agreement Amount</param>
        public void AddLoanAgreement(string laaAmount)
        {
            base.txtLoanAgreementAmount.Value = laaAmount;
            base.btnSubmit.Click();
        }

        /// <summary>
        /// asserts the contents of the deeds office is displayed correctly.
        /// </summary>
        public void AssertDeedsOffices()
        {
            Logger.LogAction("Asserting the Deeds Office dropdown is populated correctly.");
            var list = (from d in attorneyService.GetDeedsOffices() select d.Description).ToList();
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlDeedsOffice, list);
        }

        /// <summary>
        /// Asserts that the attorney dropdown correctly adds the list of attorneys linked to the selected dropdown.
        /// </summary>
        public void AssertAttorneysBySelectedDeedsOffice()
        {
            Logger.LogAction("Asserting the Attorney dropdown is populated correctly.");
            var list = (from d in attorneyService.GetAttorneys()
                        where d.DeedsOffice == base.ddlDeedsOffice.SelectedOption.Text
                        select d.LegalEntity.RegisteredName
                ).ToList();
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlAttorney, list);
        }

        /// <summary>
        /// Asserts the state of the update bond presenter
        /// </summary>
        public void AssertUpdatePresenterState()
        {
            Logger.LogAction("Asserting the correct update presenter fields exist.");
            Assertions.WatiNAssertions.AssertFieldsExist(this.UpdatePresenterFields);
            Logger.LogAction("Asserting the correct update presenter fields are enabled.");
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(this.UpdatePresenterFields);
            Logger.LogAction("Asserting the correct update presenter fields do not exist.");
            Assertions.WatiNAssertions.AssertFieldsDoNotExist(new List<Element> { base.txtLoanAgreementAmount });
        }

        /// <summary>
        /// List of elements that are enabled and exist on the update bond screen.
        /// </summary>
        private List<Element> UpdatePresenterFields
        {
            get
            {
                var list = new List<Element>
                    {
                        base.ddlDeedsOffice, base.ddlAttorney, base.BondRegistrationAmount, base.BondRegistrationNumber,
                        base.btnCancel, base.btnSubmit, base.BondRegistrationDate
                    };
                return list;
            }
        }

        /// <summary>
        /// List of elements that exist and are enabled on the add bond loan agreement screen.
        /// </summary>
        private List<Element> AddPresenterFields
        {
            get
            {
                var list = new List<Element>
                    {
                        base.txtLoanAgreementDate, base.txtLoanAgreementAmount, base.btnSubmit, base.btnCancel
                    };
                return list;
            }
        }

        /// <summary>
        /// Populate the screen
        /// </summary>
        /// <param name="bond"></param>
        public void Populate(Automation.DataModels.Bond bond)
        {
            if (base.ddlDeedsOffice.SelectedOption.Text != bond.DeedsOffice)
                base.ddlDeedsOffice.Option(bond.DeedsOffice).Select();
            if (base.ddlAttorney.SelectedOption.Text != bond.Attorney)
                base.ddlAttorney.Option(bond.Attorney.Trim()).Select();
            if (base.BondRegistrationNumber.Value != bond.BondRegistrationNumber)
            {
                base.BondRegistrationAmount.Clear();
                base.BondRegistrationAmount.Value = bond.BondRegistrationAmount.ToString();
            }
            if (base.BondRegistrationNumber.Value != bond.BondRegistrationNumber)
            {
                base.BondRegistrationNumber.Clear();
                base.BondRegistrationNumber.Value = bond.BondRegistrationNumber.ToString();
            }
        }

        /// <summary>
        /// click the submit button
        /// </summary>
        public void ClickSubmit()
        {
            base.btnSubmit.Click();
        }

        /// <summary>
        /// Change the value of the deeds office dropdown
        /// </summary>
        public void ChangeDeedsOffice()
        {
            var deedsOffices = (from deeds in attorneyService.GetDeedsOffices()
                                where deeds.Description != base.ddlDeedsOffice.SelectedOption.Text
                                select deeds).FirstOrDefault();
            base.ddlDeedsOffice.Option(deedsOffices.Description).Select();
            base.Document.DomContainer.WaitForComplete();
            base.Document.Page<BondLoanAgreement>().AssertAttorneysBySelectedDeedsOffice();
        }

        /// <summary>
        /// Save the updated bond.
        /// </summary>
        /// <param name="bond"></param>
        public void UpdateBond(Automation.DataModels.Bond bond)
        {
            Populate(bond);
            ClickSubmit();
        }

        /// <summary>
        /// Asserts that the fields are correctly populated with the values of the bond record.
        /// </summary>
        /// <param name="bond"></param>
        public void AssertFieldValues(Automation.DataModels.Bond bond)
        {
            Logger.LogAction("Asserting that the bond record is populated correctly on the screen");
            Assert.That(base.ddlDeedsOffice.SelectedOption.Text.RemoveWhiteSpace() == bond.DeedsOffice.RemoveWhiteSpace(), "Deeds Office not populated correctly.");
            Assert.That(base.ddlAttorney.SelectedOption.Text.RemoveWhiteSpace() == bond.Attorney.RemoveWhiteSpace(), "Attorney not populated correctly.");
            Assert.That(base.BondRegistrationAmount.Text == bond.BondRegistrationAmount.ToString(), "Bond Registration Amount not populated correctly.");
            Assert.That(base.BondRegistrationNumber.Text == bond.BondRegistrationNumber.ToString(), "Bond Registration Number not populated correctly.");
        }

        /// <summary>
        /// Asserts that the add loan agreement presenter has the correct fields enabled and existing when loaded.
        /// </summary>
        public void AssertAddPresenterState()
        {
            Logger.LogAction("Asserting that the Loan Agreement Add screen is loaded correctly.");
            Assertions.WatiNAssertions.AssertFieldsExist(this.AddPresenterFields);
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(this.AddPresenterFields);
        }

        /// <summary>
        ///
        /// </summary>
        public void ClearAddLoanAgreementFields()
        {
            base.txtLoanAgreementAmount.Clear();
            base.txtLoanAgreementDate.Clear();
        }
    }
}