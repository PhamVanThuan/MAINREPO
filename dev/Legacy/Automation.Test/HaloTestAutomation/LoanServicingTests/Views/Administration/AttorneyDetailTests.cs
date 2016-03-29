using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WatiN.Core;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public sealed class AttorneyDetailTests : TestBase<AttorneyDetail>
    {
        #region PrivateVar

        private Random randomGen = new Random();

        #endregion PrivateVar

        #region TestSetup

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Browser.Dispose();
        }

        #endregion TestSetup

        #region AttorneyDetailTests

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void AddAttorneyTest()
        {
            base.Browser = new TestBrowser(TestUsers.RegistrationsManager);
            var newAttorney = this.GetAttorneyTestData();
            newAttorney.DeedsOffice = Service<IAttorneyService>().GetFirstDeedsOfficeNameWithActiveAttorneys();
            this.NavigateToAttorneyAdd(base.Browser);
            base.View.SelectDeedsOffice(newAttorney.DeedsOffice);
            base.View.PopulateAttorneyDetails(newAttorney);
            base.View.PopulateAttorneyAddress(AddressTypeEnum.Residential, newAttorney.Address);
            base.View.ClickSubmit();
            //Wait for application to finish saving
            BuildingBlocks.Timers.GeneralTimer.Wait(2000);
            AttorneyAssertions.AssertAttorneyRecord(newAttorney);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void UpdateAttorneyTest()
        {
            base.Browser = new TestBrowser(TestUsers.RegistrationsManager);
            string deedsoffice = Service<IAttorneyService>().GetFirstDeedsOfficeNameWithActiveAttorneys();
            string registeredName = base.Service<IAttorneyService>().GetActiveAttorneyNameByDeedsOffice(deedsoffice);
            var existingAttorney = base.Service<IAttorneyService>().GetAttorneyRecord(registeredName);
            registeredName = registeredName.TrimEnd(' ');
            existingAttorney.DeedsOffice = deedsoffice;
            var existingLegalEntityAddress = Service<ILegalEntityAddressService>().GetRandomLegalEntityAddress(legalentityRegisteredName: registeredName);
            existingAttorney.Address = existingLegalEntityAddress.Address;
            this.NavigateToAttorneyUpdate(base.Browser);
            base.View.SelectDeedsOffice(existingAttorney.DeedsOffice);
            base.View.SelectAttorney(registeredName);

            try
            {
                //Change the test data so that it is different
                existingAttorney = this.ChangeAttorneyTestData(existingAttorney);
                base.View.PopulateAttorneyDetails(existingAttorney);

                //Update existing attorney address
                base.View.PopulateAttorneyAddress(AddressTypeEnum.Residential, existingAttorney.Address);
                base.View.ClickSubmit();

                //Wait for application to finish saving
                BuildingBlocks.Timers.GeneralTimer.Wait(2000);

                //Asserts
                LegalEntityAssertions.AssertLegalEntityAddress(existingAttorney.LegalEntity, existingAttorney.Address);
                AttorneyAssertions.AssertAttorneyRecord(existingAttorney);
            }
            catch
            {
                throw;
            }
            finally
            {
                int attorneyKey = existingAttorney.AttorneyKey;
                base.Service<IAttorneyService>().SetRegistrationIndbyAttorneykey(attorneyKey);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void AddAttorneyContactsTest()
        {
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingSupervisor);
            //Get me an existing attorney legalentity
            var testAttorney = base.Service<IAttorneyService>().GetAttorney(true, true);
            string deedsOffice = testAttorney.DeedsOffice;
            string existingLegalName = testAttorney.LegalEntity.RegisteredName;

            //Get me some attorney contact test data
            List<Automation.DataModels.AttorneyContacts> testAttorneyContacts = this.GetAttorneyContactsTestData(5);

            //Start TestBrowser and navigate to rhe attorney contact capture view

            this.NavigateToAttorneyUpdate(base.Browser);
            base.View.SelectDeedsOffice(testAttorney.DeedsOffice);
            base.View.SelectAttorney(testAttorney.LegalEntity.RegisteredName);
            base.View.PopulateAttorneyDetails(testAttorney);
            base.View.ClickContacts();

            //Capture contact with DebtCounselling role type and assert
            testAttorneyContacts[0].ExternalRoleType = ExternalRoleTypeEnum.DebtCounselling;
            base.Browser.Page<AttorneyContact>().PopulateLegalEntityDetail(testAttorneyContacts[0]);
            base.Browser.Page<AttorneyContact>().ClickAddLegalEntity();

            //Capture contact with DeceasedEstates role type
            testAttorneyContacts[1].ExternalRoleType = ExternalRoleTypeEnum.DeceasedEstates;
            base.Browser.Page<AttorneyContact>().PopulateLegalEntityDetail(testAttorneyContacts[1]);
            base.Browser.Page<AttorneyContact>().ClickAddLegalEntity();

            //Capture contact with Foreclosure role type
            testAttorneyContacts[2].ExternalRoleType = ExternalRoleTypeEnum.Foreclosure;
            base.Browser.Page<AttorneyContact>().PopulateLegalEntityDetail(testAttorneyContacts[2]);
            base.Browser.Page<AttorneyContact>().ClickAddLegalEntity();

            //Capture contact with Sequestrations role type
            testAttorneyContacts[3].ExternalRoleType = ExternalRoleTypeEnum.Sequestrations;
            base.Browser.Page<AttorneyContact>().PopulateLegalEntityDetail(testAttorneyContacts[3]);
            base.Browser.Page<AttorneyContact>().ClickAddLegalEntity();

            //Capture contact with WebAccess role type
            testAttorneyContacts[4].ExternalRoleType = ExternalRoleTypeEnum.WebAccess;
            base.Browser.Page<AttorneyContact>().PopulateLegalEntityDetail(testAttorneyContacts[4]);
            base.Browser.Page<AttorneyContact>().ClickAddLegalEntity();
            base.Browser.Page<AttorneyContact>().ClickDone();

            //Wait for application to finish saving
            BuildingBlocks.Timers.GeneralTimer.Wait(2000);

            //Assert
            foreach (Automation.DataModels.AttorneyContacts attorneyContactRecord in testAttorneyContacts)
                AttorneyAssertions.AssertAttorneyContactRecord(attorneyContactRecord);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void AddAttorneyValidationTests()
        {
            base.Browser = new TestBrowser(TestUsers.RegistrationsManager);
            string deedsoffice = Service<IAttorneyService>().GetFirstDeedsOfficeNameWithActiveAttorneys();

            //Navigate to add and select a deedsoffice
            this.NavigateToAttorneyAdd(base.Browser);
            base.View.SelectDeedsOffice(deedsoffice);

            //Clear all fields
            base.View.ClearAllFields();

            //Submit and check that there are validation messages
            base.View.ClickSubmit();
            Assert.That(base.Browser.Page<BasePageAssertions>().ValidationSummaryExists());
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void UpdateAttorneyValidationTests()
        {
            base.Browser = new TestBrowser(TestUsers.RegistrationsManager);
            string deedsoffice = Service<IAttorneyService>().GetFirstDeedsOfficeNameWithActiveAttorneys();
            string registeredName = base.Service<IAttorneyService>().GetActiveAttorneyNameByDeedsOffice(deedsoffice);
            registeredName = registeredName.TrimEnd(' ');
            //Navigate to update and select a deedsoffice and attorney name
            this.NavigateToAttorneyUpdate(base.Browser);
            base.View.SelectDeedsOffice(deedsoffice);
            base.View.SelectAttorney(registeredName);
            //Clear all fields
            base.View.ClearAllFields();
            //Submit and check that there are validation messages
            base.View.ClickSubmit();
            Assert.That(base.Browser.Page<BasePageAssertions>().ValidationSummaryExists());
        }

        #endregion AttorneyDetailTests

        #region TestHelpers

        private void NavigateToAttorneyUpdate(TestBrowser browser)
        {
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().UpdateAttorneyDetails();
        }

        private void NavigateToAttorneyAdd(TestBrowser browser)
        {
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().AddAttorneyDetails();
        }

        private Automation.DataModels.Attorney GetAttorneyTestData()
        {
            int randomNumber = randomGen.Next(0, Int32.MaxValue);
            string buildingNumber = "1";
            string streetNumber = "12";
            string countryDescription = "South Africa";
            string provinceDescription = "Gauteng";
            string suburbDescription = "Sandton";
            string registeredName = String.Format("AttorneyName{0}", randomNumber);
            string streetName = String.Format("StreetName{0}", randomNumber);
            string buildingName = String.Format("BuildingName{0}", randomNumber);

            //Instantiate an Address for the Attorney
            var address = new Automation.DataModels.Address()
                            {
                                AddressFormatKey = AddressFormatEnum.Street,
                                UnitNumber = "1",
                                BuildingNumber = buildingNumber,
                                BuildingName = buildingName,
                                StreetNumber = streetNumber,
                                StreetName = streetName,
                                RRR_SuburbDescription = suburbDescription,
                                RRR_ProvinceDescription = provinceDescription
                            };

            //Instantiate a LegalEntity for the Attorney
            var legalentity = new Automation.DataModels.LegalEntity()
                                {
                                    RegisteredName = registeredName,
                                    EmailAddress = String.Format("AttorneyName{0}@test.com", randomNumber),
                                    WorkPhoneCode = randomNumber.ToString(),
                                    WorkPhoneNumber = randomNumber.ToString()
                                };

            //Instantiate the Attorney at set the address and legalentity
            var testAttorney = new Automation.DataModels.Attorney()
                                {
                                    ContactName = "AttorneyContactTest",
                                    DeedsOffice = "",
                                    IsRegistrationAttorney = true,
                                    LegalEntity = legalentity,
                                    Address = address,
                                    IsWorkflowEnable = true,
                                    Status = GeneralStatusEnum.Active
                                };
            return testAttorney;
        }

        private Automation.DataModels.Attorney ChangeAttorneyTestData(Automation.DataModels.Attorney attorney)
        {
            int randomNumber = randomGen.Next(0, Int32.MaxValue);

            //Change some attorney values and re-capture the details
            attorney.IsWorkflowEnable = true;
            attorney.LegalEntity.RegisteredName = String.Format("{0}New", attorney.LegalEntity.RegisteredName);
            attorney.LegalEntity.TradingName = attorney.LegalEntity.RegisteredName;
            attorney.ContactName = String.Format("{0}New", attorney.ContactName);
            attorney.LegalEntity.WorkPhoneCode = randomNumber.ToString().Remove(3);
            attorney.LegalEntity.WorkPhoneNumber = randomNumber.ToString();
            attorney.LegalEntity.EmailAddress = String.Format("{0}@email.com", randomNumber);
            attorney.Mandate = 4000.00f;
            attorney.IsRegistrationAttorney = false;
            attorney.IsLitigationAttorney = true;

            //Change some address values and re-capture the details
            attorney.Address.StreetName = String.Format("{0} Street", randomNumber);
            attorney.Address.StreetNumber = String.Format("{0}", randomNumber);
            attorney.Address.BuildingName = String.Format("Building {0}", randomNumber);
            attorney.Address.BuildingNumber = String.Format("{0}", randomNumber);
            attorney.Address.RRR_SuburbDescription = "Safari Gardens";
            attorney.Address.RRR_ProvinceDescription = "North West";
            attorney.Address.RRR_CityDescription = "Rustenburg";
            return attorney;
        }

        private List<Automation.DataModels.AttorneyContacts> GetAttorneyContactsTestData(int noAttorneyContacts)
        {
            var contacts = new List<Automation.DataModels.AttorneyContacts>();
            var attorneyContact = new Automation.DataModels.AttorneyContacts();
            if (noAttorneyContacts > 0)
            {
                string emailaddress = "";
                string faxCode = "";
                string faxNumber = "";
                string firstNames = "";
                string surname = "";
                string workPhoneCode = "";
                string workPhoneNumber = "";
                int randomNumber = 0;

                for (int count = 0; count < noAttorneyContacts; count++)
                {
                    do
                    {
                        randomNumber = randomGen.Next(0, 100);
                        emailaddress = String.Format("AttorneyContact{0}@test.com", randomNumber);
                        faxCode = "012";
                        faxNumber = "1234567";
                        firstNames = String.Format("AttorneyContact{0}", randomNumber);
                        surname = String.Format("SurnameTest{0}", randomNumber);
                        workPhoneCode = "012";
                        workPhoneNumber = "1234567";

                        //Loop until we get a legalentity that does not exist.
                        attorneyContact = base.Service<IAttorneyService>().GetAttorneyContactRecord(firstNames, surname);
                    }
                    while (attorneyContact != null);

                    attorneyContact = new Automation.DataModels.AttorneyContacts();
                    var legalentity = new Automation.DataModels.LegalEntity()
                                                {
                                                    EmailAddress = emailaddress,
                                                    FaxCode = faxCode,
                                                    FaxNumber = faxNumber,
                                                    FirstNames = firstNames,
                                                    Surname = surname,
                                                    WorkPhoneCode = workPhoneCode,
                                                    WorkPhoneNumber = workPhoneNumber
                                                };
                    attorneyContact.LegalEntity = legalentity;
                    contacts.Add(attorneyContact);
                }
            }
            return contacts;
        }

        #endregion TestHelpers
    }
}