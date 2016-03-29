using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using System.Threading;
using WatiN.Core;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class DebtCounsellingCreateCase : DebtCounsellingCreateCaseControls
    {
        private readonly IDebtCounsellingService debtCounsellingService;

        public DebtCounsellingCreateCase()
        {
            debtCounsellingService = ServiceLocator.Instance.GetService<IDebtCounsellingService>();
        }

        public void PopulateView(DateTime _17pt1Date)
        {
            if (base._17Pt1Date.Value != _17pt1Date.ToString(Formats.DateFormat))
            {
                this.Remove17pt1Date();
                base._17Pt1Date.Value = _17pt1Date.ToString(Formats.DateFormat);
            }
        }

        /// <summary>
        /// adds the reference nunmber
        /// </summary>
        public void AddReference(string testReference)
        {
            if (!string.IsNullOrEmpty(testReference))
            {
                base.TestReference.Value = testReference;
            }
        }

        /// <summary>
        /// adds the reference nunmber
        /// </summary>
        public void UpdateReference(string testReference)
        {
            if (!string.IsNullOrEmpty(testReference))
            {
                base.TestReference.Value = testReference;
            }
            base.CreateCase.Click();
        }

        /// <summary>
        /// removes the date
        /// </summary>
        public void Remove17pt1Date()
        {
            base._17Pt1Date.Clear();
        }

        /// <summary>
        /// This method will Search for the number(s) and add each legal entity found to the people of importance grid.
        /// </summary>
        /// <param name="IdPassportNumbers">ID/Passport number(s)</param>
        public void SearchAndAddPeopleofImportance(string[] IdPassportNumbers)
        {
            foreach (string IdPassportNumber in IdPassportNumbers)
            {
                base.IDPassportNumber.TypeText(IdPassportNumber);
                base.Document.DomContainer.WaitForComplete();
                foreach (TableRow legalEntity_row in base.PeopleofImportance)
                {
                    legalEntity_row.Click();
                }
            }
        }

        /// <summary>
        /// Searches for a person on the create case screen and adds them to grid below
        /// </summary>
        /// <param name="IdPassportNumber">ID/Passport Number to search for</param>
        public void SearchAndAddPeopleofImportance(string IdPassportNumber)
        {
            base.IDPassportNumber.TypeText(IdPassportNumber);
            //BrowserExtensions.WaitForAjaxRequest(b);
            Thread.Sleep(5000);
            foreach (TableRow legalEntity_row in base.LegalEntitySearchResults)
            {
                legalEntity_row.Click();
            }
        }

        /// <summary>
        /// When provided with a test identifier this will retrieve the legal entities for whom we want to create debt counselling cases
        /// from the configuration tables in the test schema. It will then loop through the checkboxes and check the configuration to determine
        /// if should be checked or unchecked.
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <param name="b"></param>
        public void SelectLegalEntitiesFromTree(string testIdentifier, TestBrowser b)
        {
            DeselectAllCheckBoxes();
            //we need to get the test data
            QueryResults r = debtCounsellingService.GetLegalEntitiesForDebtCounsellingCaseCreate(testIdentifier);

            bool isUnderDebtCounselling = false;
            foreach (QueryResultsRow row in r.RowList)
            {
                isUnderDebtCounselling = row.Column("UnderDebtCounselling").GetValueAs<bool>();
                SelectLegalEntitiesFromTree
                    (
                         row.Column("LegalEntityKey").GetValueAs<int>(),
                         row.Column("AccountKey").GetValueAs<int>(),
                         isUnderDebtCounselling
                    );
            }
            r.Dispose();
        }

        public void ClickCreateCase()
        {
            base.CreateCase.Click();
        }

        public void DeselectAllCheckBoxes()
        {
            Div d = base.Accounts;
            if (d.CheckBoxes.Count > 0)
            {
                foreach (var checkbox in d.CheckBoxes)
                {
                    if (checkbox.Enabled)
                        checkbox.Checked = false;
                }
            }
        }

        /// <summary>
        /// This method will create a List from the AccountsOfImportance ObjectMap.
        /// Dictionary Key = legalentitykey, Value = accountkey
        /// </summary>
        /// <returns></returns>
        public List<string> GetAccountsOfImportance()
        {
            List<string> accountsOfImportance
                = new List<string>();
            foreach (var checkbox in base.Accounts.CheckBoxes)
            {
                //account
                string attValue = checkbox.GetAttributeValue("value");
                //Can only be an account with a legal entity key
                if (attValue.Length == 7)
                    accountsOfImportance.Add(attValue);
            }
            return accountsOfImportance;
        }

        /// <summary>
        /// deselects all the checkboxes in the Accounts DIV
        /// </summary>
        public void SelectAllCheckBoxes()
        {
            Div d = base.Accounts;
            if (d.CheckBoxes.Count > 0)
            {
                foreach (var checkbox in d.CheckBoxes)
                {
                    if (checkbox.Enabled)
                        checkbox.Checked = true;
                }
            }
        }

        /// <summary>
        /// This method will check the checkboxes on the this createcase view.
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="accountKey"></param>
        /// <param name="isUnderDebtCounselling"></param>
        public void SelectLegalEntitiesFromTree(int legalEntityKey, int accountKey, bool isUnderDebtCounselling)
        {
            Div d = base.Accounts;
            string checkbox_value =
                string.Format("{0}/{1}-{2}", accountKey, accountKey, legalEntityKey);

            if (d.CheckBoxes.Count > 0)
            {
                foreach (var checkbox in d.CheckBoxes)
                {
                    if (checkbox.GetAttributeValue("value").ToString() == checkbox_value)
                    {
                        if (!isUnderDebtCounselling)
                        {
                            if (checkbox.Enabled)
                            {
                                checkbox.Checked = false;
                                break;
                            }
                        }
                        else if (isUnderDebtCounselling)
                        {
                            if (checkbox.Enabled)
                            {
                                checkbox.Checked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This assertion will check that checkboxes that we are expecting to be disabled are correctly disabled when the search tree builds up
        /// the list of results. This should only be used on test cases in the debt counselling configuration tables where the UnderDebtCounselling value
        /// is set to ZERO in order to indicate that this checkbox should be disabled for selection.
        /// </summary>
        /// <param name="testIdentifier"></param>
        public void AssertCheckBoxesDisabled(string testIdentifier)
        {
            Div d = base.Accounts;
            //we need to get the test data
            QueryResults r = debtCounsellingService.GetLegalEntitiesForDebtCounsellingCaseCreate(testIdentifier);

            foreach (QueryResultsRow row in r.RowList)
            {
                var accountKey = row.Column("AccountKey").Value;
                var legalEntityKey = row.Column("LegalEntityKey").Value;
                string checkboxValue =
                    string.Format("{0}/{1}-{2}", accountKey, accountKey, legalEntityKey);
                bool checkboxExists = false;

                if (d.CheckBoxes.Count > 0)
                {
                    foreach (var checkbox in d.CheckBoxes)
                    {
                        if (checkbox.GetAttributeValue("value") == checkboxValue)
                        {
                            checkboxExists = true;
                            if (row.Column("UnderDebtCounselling").Value == "False")
                            {
                                Assert.False(checkbox.Enabled, "A checkbox we are expecting to be disabled is enabled. AccountKey: {0}, LegalEntityKey {1}", accountKey, legalEntityKey);
                            }
                        }
                    }
                }
                Assert.True(checkboxExists, "A checkbox with value: {0} was not found. AccountKey: {1}, LegalEntityKey: {2}", checkboxValue, accountKey, legalEntityKey);
            }
        }
    }
}