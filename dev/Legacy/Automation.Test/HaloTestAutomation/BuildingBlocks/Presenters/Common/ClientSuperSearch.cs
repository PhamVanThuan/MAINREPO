using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;

using System;
using System.Linq;
using WatiN.Core;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    public class ClientSuperSearch : ClientSuperSearchControls
    {
        private ILegalEntityService legalEntityService;

        public ClientSuperSearch()
        {
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        }

        /// <summary>
        /// Loads the first main applicant off the client super search results into the CBO menu.
        /// </summary>
        /// <param name="accountKey">Mortgage Loan Account</param>
        /// <returns>LegalEntityKey</returns>
        public int SearchByAccountKeyForFirstMainApplicant(int accountKey)
        {
            base.BasicSearchLink.Click();
            base.txtAccountKey.Value = (accountKey.ToString());
            base.btnSearch.Click();
            //using the account key we need to find a main applicant to select
            //need to find the first main applicant
            string legalEntityKey = (from r in legalEntityService.GetLegalEntityNamesAndRoleByAccountKey(accountKey)
                                     where r.Column("Role").Value == RoleType.MainApplicant
                                     select r.Column("LegalEntityKey").Value).FirstOrDefault();
            //select the legal entity

            base.LegalEntitySelectbyHref(legalEntityKey).Click();
            return int.Parse(legalEntityKey);
        }

        /// <summary>
        /// Loads the first legalentity off the client super search results into the CBO menu.
        /// </summary>
        /// <param name="accountKey">Mortgage Loan Account</param>
        /// <returns>LegalEntityKey</returns>
        public int SearchByAccountKeyForFirstLegalEntity(int accountKey)
        {
            base.BasicSearchLink.Click();
            base.txtAccountKey.Value = (accountKey.ToString());
            base.btnSearch.Click();
            //using the account key we need to find a main applicant to select
            //need to find the first main applicant
            string legalEntityKey = (from r in legalEntityService.GetLegalEntityNamesAndRoleByAccountKey(accountKey)
                                     select r.Column("LegalEntityKey").Value).FirstOrDefault();
            //select the legal entity

            base.LegalEntitySelectbyHref(legalEntityKey).Click();
            return int.Parse(legalEntityKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        public void SearchByAccountKeyForSuretor(int accountKey)
        {
            base.txtAccountKey.Value = (accountKey.ToString());
            base.btnSearch.Click();
            //using the account key we need to find a main applicant to select
            //need to find the first main applicant
            string legalEntityToSelect = (from r in legalEntityService.GetLegalEntityNamesAndRoleByAccountKey(accountKey)
                                          where r.Column("Role").Value == RoleType.Suretor
                                          select r.Column("Name").Value).FirstOrDefault();
            //select the legal entity
            base.LegalEntitySelect(String.Format("Select {0}", legalEntityToSelect)).Click();
        }

        /// <summary>
        /// This will complete the fields on the view, provided the parameters.
        /// </summary>
        /// <param name="legalEntityFirstNames"></param>
        /// <param name="legalentitySurname">leave blank if not required.</param>
        /// <param name="idNumber">leave blank if not required.</param>
        /// <param name="salaryNo">leave blank if not required.</param>
        /// <param name="accountkey">leave blank if not required.</param>
        public void PopulateSearch
            (
                string legalEntityFirstNames,
                string legalentitySurname,
                string idNumber,
                string salaryNo,
                string accountkey
            )
        {
            if (!string.IsNullOrEmpty(legalEntityFirstNames))
                base.txtFirstName.Value = legalEntityFirstNames;
            if (!string.IsNullOrEmpty(legalentitySurname))
                base.txtSurname.Value = legalentitySurname;
            if (!string.IsNullOrEmpty(idNumber))
                base.txtID.Value = idNumber;
            if (!string.IsNullOrEmpty(salaryNo))
                base.txtSalaryNumber.Value = salaryNo;
            if (!string.IsNullOrEmpty(accountkey))
                base.txtAccountKey.Value = accountkey;
        }

        /// <summary>
        /// This will pupulate the firstnames, surnames and idnumber fields
        /// </summary>
        /// <param name="legalEntity"></param>
        public void PopulateSearch(Automation.DataModels.LegalEntity legalEntity)
        {
            if (legalEntity.LegalEntityTypeKey == Common.Enums.LegalEntityTypeEnum.NaturalPerson)
            {
                base.BasicSearchLink.Click();
                if (!string.IsNullOrEmpty(legalEntity.FirstNames))
                    base.txtFirstName.Value = legalEntity.FirstNames;
                if (!string.IsNullOrEmpty(legalEntity.Surname))
                    base.txtSurname.Value = legalEntity.Surname;
                if (!string.IsNullOrEmpty(legalEntity.IdNumber))
                    base.txtID.Value = legalEntity.IdNumber;
            }
            else
            {
                base.AdvancedSearchLink.Click();
                base.txtSearch.Value = legalEntity.RegistrationNumber;
                base.LegalEntityType.Option("Company").Select();
                base.AccountType.Option("All").Select();
            }
        }

        /// <summary>
        /// Click on the Search button.
        /// </summary>
        public void PerformSearch()
        {
            base.btnSearch.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Table GetResultsGrid()
        {
            return base.SearchResultsTable;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityFirstNames"></param>
        /// <param name="legalentitySurname"></param>
        /// <param name="idNumber"></param>
        /// <param name="salaryNo"></param>
        /// <param name="accountkey"></param>
        public void Clear(bool legalEntityFirstNames, bool legalentitySurname, bool idNumber, bool salaryNo, bool accountkey)
        {
            if (legalEntityFirstNames)
                base.txtFirstName.Clear();
            if (legalentitySurname)
                base.txtSurname.Clear();
            if (idNumber)
                base.txtID.Clear();
            if (salaryNo)
                base.txtSalaryNumber.Clear();
            if (accountkey)
                base.txtAccountKey.Clear();
        }

        /// <summary>
        /// Selects a legal entity from the client super search when provided with their ID number
        /// </summary>
        /// <param name="idNumber">Legal Entity ID Number</param>
        public void SelectByIDNumber(string idNumber)
        {
            GetLinkByIdNumber(idNumber).Click();
        }

        private Link GetLinkByIdNumber(string idNumber)
        {
            string legalEntityLegalName = legalEntityService.GetLegalEntityLegalNameByIDNumber(idNumber);
            //select the legal entity
            return base.LegalEntitySelect(String.Format("Select {0}", legalEntityLegalName));
        }

        /// <summary>
        /// Selects a legal entity from the client super search when provided with a registration number.
        /// </summary>
        /// <param name="registrationNumber">Legal Entity Company registration number</param>
        public void SelectByLegalName(string registrationNumber)
        {
            string legalEntityLegalName = legalEntityService.GetLegalEntityLegalNameByRegistrationNumber(registrationNumber);
            //select the legal entity
            base.LegalEntitySelect(String.Format("Select {0}", legalEntityLegalName)).Click();
        }

        /// <summary>
        /// Performs an advanced search
        /// </summary>
        /// <param name="searchText">Search Text</param>
        /// <param name="legalEntityType">Legal Entity Type to select</param>
        /// <param name="accountType">Account Type to select</param>
        public void PerformAdvancedSearch(string searchText, string legalEntityType, string accountType, string idnumber = null)
        {
            base.AdvancedSearchLink.Click();
            base.txtSearch.Value = searchText;
            base.LegalEntityType.Option(legalEntityType).Select();
            base.AccountType.Option(accountType).Select();
            base.btnAdvancedSearch.Click();

            if (idnumber != null)
            {
                for (int i = 0; i < 50; i++)
                {
                    base.btnAdvancedSearch.Click();
                    if (GetLinkByIdNumber(idnumber).Exists)
                        break;
                }
                GetLinkByIdNumber(idnumber).Click();
            }
        }

        /// <summary>
        /// Selects a legal entity from the client super search when provided with their legalentitykey
        /// </summary>
        /// <param name="legalentitykey">legalentitykey</param>
        public void SelectByLegalEntityKey(int legalentitykey)
        {
            base.LegalEntitySelectbyHref(legalentitykey.ToString()).Click();
        }

        public void AddNewLegalEntity()
        {
            base.btnNewLegalEntity.Click();
        }

        public void ClickNewAssuredLife()
        {
            base.NewAssuredLife.Click();
        }

        public void AssertViewDisplayed(string viewname)
        {
            Assert.AreEqual(viewname, base.ViewName.Text, "Current view is not being displayed. Current View Displayed: {0}", base.ViewName.Text);
        }
    }
}