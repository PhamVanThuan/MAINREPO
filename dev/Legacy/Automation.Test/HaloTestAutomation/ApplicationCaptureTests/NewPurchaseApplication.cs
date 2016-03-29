using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;
using WatiN.Core.Logging;
using NUnit.Framework;
using BuildingBlocks;
using System.Xml;
using System.Configuration;
using System.Reflection;

namespace ApplicationCaptureTests
{
    [TestFixture, RequiresSTA]
    public class NewPurchaseApplication
    {
        private static Browser _browser;
        private string _LegalEntityFullName;
        private string _OfferKey;
        private const string NewPurchase = "New purchase";
        private const string NewPurchaseApplicationDataFileName = "NewPurchaseApplication.xml";

        [TestFixtureSetUp]
        public void InitializeBrowser()
        {
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            Properties.DataFileLoader.LoadFile(NewPurchaseApplicationDataFileName);
            _browser = Navigation.Base.StartBrowser
              (
                Properties.DataFileLoader.FileData.Section[SectionName].Fields["TestUsername", 0],
                Properties.DataFileLoader.FileData.Section[SectionName].Fields["TestPassword", 0]
              );
        }
        [Test]
        public void Step01_CreateVariFix()
        {
            Navigation.Base.gotoLeadCaptureCalculator(_browser);
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            bool IsInterestOnly = false;
            string InterestOnly =  Properties.DataFileLoader.FileData.Section[SectionName].Fields["InterestOnly", 0];
            if (InterestOnly == "true")
                IsInterestOnly = true;

            if (_browser != null)
            {
                BuildingBlocks.Navigation.Base.CloseLoanNodes(_browser);
                BuildingBlocks.Presenters.LoanCalculatorLead_NewPurchase
                    (
                        _browser,
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["Product", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["PurchasePrice", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["CashDeposit", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["EmploymentType", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["Term", 0],
                        IsInterestOnly,
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["PercentageToFix", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["HouseHoldIncome", 0],
                        Presenters.btn.CreateApplication
                    );
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step01_CreateNewVariableLoan()
        {
            Navigation.Base.gotoLeadCaptureCalculator(_browser);
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            if (_browser != null)
            {
                BuildingBlocks.Navigation.Base.CloseLoanNodes(_browser);
                BuildingBlocks.Presenters.LoanCalculatorLead_NewPurchase
                    (
                        _browser,
                        NewPurchase,
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["Product", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["PurchasePrice", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["CashDeposit", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["EmploymentType", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["HouseHoldIncome", 0]
                    );
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step01_CreateOrangeHomeLoan()
        {
            Navigation.Base.gotoLeadCaptureCalculator(_browser);
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            if (_browser != null)
            {
                BuildingBlocks.Navigation.Base.CloseLoanNodes(_browser);
                BuildingBlocks.Presenters.LoanCalculatorLead_NewPurchase
                    (
                        _browser,
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["Product", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["PurchasePrice", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["CashDeposit", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["EmploymentType", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["HouseHoldIncome", 0],
                        Presenters.btn.CreateApplication
                    );
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step02_CaptureLegalEntity()
        {
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            if (_browser != null)
            {
                this._LegalEntityFullName =
                       (Properties.DataFileLoader.FileData.Section[SectionName].Fields["Salutation", 0] as string) + " " +
                       (Properties.DataFileLoader.FileData.Section[SectionName].Fields["FirstNames", 0] as string) + " " +
                       (Properties.DataFileLoader.FileData.Section[SectionName].Fields["Surnames", 0] as string);
                string LegalEntityIDNumber = Properties.DataFileLoader.FileData.Section[SectionName].Fields["IDNumber", 0];
                if (BuildingBlocks.Views.App_LegalEntityDetailsAdd.LegalEntityExist(LegalEntityIDNumber))
                {
                    BuildingBlocks.Views.App_LegalEntityDetailsAdd.AddExistingLegalEntity
                        (
                            _browser,
                            LegalEntityIDNumber,
                            out _LegalEntityFullName
                        );
                }
                else
                {
                    bool IsIncomeContributor = false;
                    bool IsTeleMarketing = false;
                    bool IsMarketing = false;
                    bool IsCustomerList = false;
                    bool IsEmail = false;
                    bool IsSms = false;

                    string IncomeContributor = Properties.DataFileLoader.FileData.Section[SectionName].Fields["IncomeContributor", 0];
                    string TeleMarketing = Properties.DataFileLoader.FileData.Section[SectionName].Fields["TeleMarketing", 0];
                    string Marketing = Properties.DataFileLoader.FileData.Section[SectionName].Fields["Marketing", 0];
                    string CustomerList = Properties.DataFileLoader.FileData.Section[SectionName].Fields["CustomerList", 0];
                    string Email = Properties.DataFileLoader.FileData.Section[SectionName].Fields["Email", 0];
                    string sms = Properties.DataFileLoader.FileData.Section[SectionName].Fields["sms", 0];

                    if (IncomeContributor == "true")
                            IsIncomeContributor = true;
                    if (TeleMarketing == "true")
                            IsTeleMarketing = true;
                    if (Marketing == "true")
                            IsMarketing = true;
                    if (CustomerList == "true")
                            IsCustomerList = true;
                    if (Email == "true")
                            IsEmail = true;
                    if (sms == "true")
                            IsSms = true;


                    BuildingBlocks.Views.App_LegalEntityDetailsAdd.AddLegalEntity
                      (
                            _browser,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Role", 0],
                            IsIncomeContributor,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["IDNumber", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Salutation", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Initials", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["FirstNames", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Surnames", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["PreferredName", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Gender", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["MaritalStatus", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["PopulationGroup", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Education", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["CitizenshipType", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["DOB", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["PassportNumber", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["TaxNumber", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["HomeLanguage", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["DocumentLanguage", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Status", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["HomePhoneCode", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["HomePhoneNo", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["WorkPhoneCode", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["WorkPhoneNo", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["FaxCode", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["FaxNumber", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["CellphoneNo", 0] as string,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["EmailAddress", 0] as string,
                            IsTeleMarketing,
                            IsMarketing,
                            IsCustomerList,
                            IsEmail,
                            IsSms
                      );
                }
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step03_CaptureApplicationProperty()
        {
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            if (_browser != null)
            {
                BuildingBlocks.Navigation.PropertiesNode.Properties(_browser);
                BuildingBlocks.Navigation.PropertiesNode.PropertySummary(_browser, Navigation.NodeType.Add);
                BuildingBlocks.Views.App_PropertyCapture.CaptureProperty
                    (
                        _browser,
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["UnitNumber", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["BuildingNo", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["BuildingName", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["StreetNo", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["StreetName", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["Province", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["Suburb", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["PropDesc1", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["PropDesc2", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["PropDesc3", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["PropertyType", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["TitleType", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["OccupancyType", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["InspectionContact1", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["InspectionTel1", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["InspectionContact2", 0],
                        Properties.DataFileLoader.FileData.Section[SectionName].Fields["InspectionTel2", 0]
                    );
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step04_CaptureDeclarations()
        {
            if (_browser != null)
            {
                Navigation.LegalEntityNode.FindAndSelectLegalEntityByIndex(_browser, 0);
                BuildingBlocks.Navigation.LegalEntityNode.ApplicationDeclarations(_browser, Navigation.NodeType.Update);
                BuildingBlocks.Views.ApplicationDeclarations.ApplicationDeclarationsUpdate2(_browser);
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step05_CaptureEmployment()
        {
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            if (_browser != null)
            {
                //Navigate to LE employment view!
                BuildingBlocks.Navigation.LegalEntityNode.EmploymentDetails(_browser, Navigation.NodeType.Add);
                //Check if Legalentity has Employment. 
                if (!BuildingBlocks.Views.LegalEntityEmploymentDetails.HasEmploymentDetails(_browser, this._LegalEntityFullName))
                {
                    //add employment details.  if NOT then add one
                    BuildingBlocks.Views.LegalEntityEmploymentDetails.AddEmploymentDetails
                        (
                             _browser,
                             Properties.DataFileLoader.FileData.Section[SectionName].Fields["EmployerName", 0],
                             Properties.DataFileLoader.FileData.Section[SectionName].Fields["EmploymentType", 0],
                             Properties.DataFileLoader.FileData.Section[SectionName].Fields["RemunerationType", 0],
                             Properties.DataFileLoader.FileData.Section[SectionName].Fields["StartDate", 0],
                             Properties.DataFileLoader.FileData.Section[SectionName].Fields["MonthlyIncomeRands", 0]
                        );
                }
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step06_CaptureLegalEntityAddress()
        {
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            if (_browser != null)
            {
                //Navigate to LE address view!
                BuildingBlocks.Navigation.LegalEntityNode.LegalEntityAddress(_browser, Navigation.NodeType.Add);
                //Check if Legalentity has address. if NOT then add one
                if (!BuildingBlocks.Views.LegalEntityAddressDetails.HasAddress(_browser, this._LegalEntityFullName))
                {
                    //add address details
                    BuildingBlocks.Views.LegalEntityAddressDetails.AddResidentialAddress
                        (
                             _browser,
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["StreetNo", 0],
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["StreetName", 0],
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Province", 0],
                            Properties.DataFileLoader.FileData.Section[SectionName].Fields["Suburb", 0]
                        );
                }
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step07_CaptureMailingAddress()
        {
            string SectionName = MethodInfo.GetCurrentMethod().Name;
            bool IsOnlineStatement = false;
            string OnlineStatement = Properties.DataFileLoader.FileData.Section[SectionName].Fields["OnlineStatement", 0];
            if (OnlineStatement == "true")
                IsOnlineStatement = true;
            if (_browser != null)
            {
                //Select the Loan Details Node
                BuildingBlocks.Navigation.LoanDetailsNode.LoanDetails(_browser);
                //Navigate to Loan Mailing Address and Update 
                BuildingBlocks.Navigation.LoanDetailsNode.ApplicationMailingAddress(_browser, Navigation.NodeType.Update);
                BuildingBlocks.Views.ApplicationMailingAddressUpdate.UpdateApplicationMailingAddress
                    (
                         _browser,
                         Properties.DataFileLoader.FileData.Section[SectionName].Fields["CorrespondenceMedium", 0] as string,
                         Properties.DataFileLoader.FileData.Section[SectionName].Fields["CorrespondenceLanguage", 0] as string,
                         IsOnlineStatement,
                         Properties.DataFileLoader.FileData.Section[SectionName].Fields["OnlineStatementFormat", 0] as string
                    );
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
        [Test]
        public void Step08_CommitApplication()
        {
            if (_browser != null)
            {
                //Application done and can be commited.
                BuildingBlocks.Navigation.ApplicationLoanNode.FirstLoanInFloBOMenu(_browser);
                BuildingBlocks.Navigation.ApplicationLoanNode.SubmitApplication(_browser);
                BuildingBlocks.Views.WorkflowYesNo.Confirm(_browser, true);
                BuildingBlocks.Views.WorkflowYesNo.ContinueWithWarnings(_browser);
            }
            else
            {
                throw new ArgumentException("Need to Start Browser first");
            }
        }
    }
}
