using BuildingBlocks;
using System.Linq;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Xml.Linq;
using Automation.DataModels;
using SAHL.Core.BusinessModel.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using BuildingBlocks.Timers;
using WatiN.Core.Logging;
namespace ApplicationCaptureTests.Workflow.Capitec
{
    public abstract class CapitecBase : TestBase<BasePage>
    {
        private IApplicationService applicationService;
        private ICapitecApplicationService capitecApplicationService;
        private ILegalEntityService legalEntityService;
        private IAddressService addressService;
        private IAssignmentService assignmentService;
        private IEmploymentService employmentService;
        private ILegalEntityAddressService legalEntityAddressService;
        private IADUserService aduserService;
        public enum IncludeITC { IncludeITC = 0, ExcludeITC = 1 }
        private static Random random = new Random();

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            var auditService = Service<IAuditService>();
            auditService.EnableLegalEntityUpdateTrigger();
            auditService.EnableLegalEntityInsertTrigger();
            base.Browser = new TestBrowser(TestUsers.TELCUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            this.legalEntityAddressService = Service<ILegalEntityAddressService>();
            this.capitecApplicationService = Service<ICapitecApplicationService>();
            this.applicationService = Service<IApplicationService>();
            this.legalEntityService = Service<ILegalEntityService>();
            this.addressService = Service<IAddressService>();
            this.employmentService = Service<IEmploymentService>();
            this.aduserService = Service<IADUserService>();
            this.assignmentService = Service<IAssignmentService>();
            this.Assertable = new CapitecApplicationAssertion();
        }

        protected void CreateApplication(
                MortgageLoanPurposeEnum loanPurpose,
                bool includeITC,
                SalutationTypeEnum salutation = SalutationTypeEnum.Mr,
                string homePhoneNumber = "0316549871",
                string workPhoneNumber = "0315242320",
                string cellPhoneNumber = "0795242320",
                string emailAddress = "capitecApp@gmail.com",
                bool isMainContact = false,
                string unitNumber = "1",
                string buildingNumber = "2",
                string buildingName = "building name", //Gets a random unique id appended
                string streetNumber = "2",
                string streetName = "street name",
                string suburb = "Durban North",
                string province = "KwaZulu-Natal",
                string city = "Durban",
                string postalCode = "4051",
                int suburbKey = 6204,
                EmploymentTypeEnum employmentType = EmploymentTypeEnum.Salaried,
                decimal grossIncome = 36000M,
                string idNumber = "7801015014248",
                DateTime? dateOfBirth = null,
                bool checkDirtyLegalEntityNotUpdated = false,
                bool isDirtyLegalEntity = false,
                string firstNames = "Capitec",
                string surname = "Application",
                bool incomeContributor = true,
                bool marriedCOP = false,
                OfferStatusEnum offerStatus = OfferStatusEnum.Open,
                string reasonType = "",
                string reasonDescription = "",
                bool isITCLinkedToLegalEntity = false)
        {
            if (dateOfBirth == null)
                dateOfBirth = IDNumbers.GetDateFromIdNumber(idNumber);
            streetName = String.Format("{0}{1}", streetName, this.GetUniqueString());
            this.Assertable = new CapitecApplicationAssertion();
            this.Assertable.ExpectedLoanPurpose = loanPurpose;
            this.Assertable.ExpectedTerm = 240;
            this.Assertable.ExpectedEmploymentTypeKey = employmentType;
            this.Assertable.ExpectedHouseholdIncome = (int)grossIncome;
            this.Assertable.IsMainContact = isMainContact;
            this.Assertable.ExpectedOfferStatusKey = offerStatus;
            this.Assertable.HasITC = includeITC;
            this.Assertable.HasApplicationReasons = false;
            this.Assertable.ExpectedAddressUserID = @"SAHL\WebLeads";
            this.Assertable.ExpectedConsultantDetails = new ConsultantDetails(TestUsers.TELCUser, "Telecentre");
            this.Assertable.ExpectedStreetNumber = streetNumber;
            this.Assertable.ExpectedStreetName = streetName;
            this.Assertable.ExpectedProvince = province;
            this.Assertable.ExpectedSuburb = suburb;
            this.Assertable.ExpectedIdNumber = idNumber;
            this.Assertable.ExpectedAddressChangeDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(5));
            this.Assertable.ExpectedTeleConsultant = this.assignmentService.GetNextRoundRobinUser(OfferRoleTypeEnum.BranchConsultantD, RoundRobinPointerEnum.CapitecConsultant);
            this.Assertable.ExpectedApplicationKey = this.capitecApplicationService.GetUnusedOfferKey();
            this.Assertable.IsITCLinkedToLegalEntity = isITCLinkedToLegalEntity;

            var reasons = new List<string>();
            if (reasonType != String.Empty && reasonDescription != String.Empty)
            {
                this.Assertable.HasApplicationReasons = true;
                this.Assertable.ExpectedApplicationReasonType = reasonType;
                this.Assertable.ExpectedApplicationReasonDescription = reasonDescription;
                reasons.Add(reasonDescription);
            }

            var app = default(CapitecApplication);
            var itc = default(ApplicantITC);
            if (includeITC)
                itc = this.GetITCFromTemplate();


            var address = new ApplicantResidentialAddress(unitNumber, buildingNumber, buildingName, streetNumber, streetName, suburb, province, city, postalCode, suburbKey);

            var employmentDetails = default(EmploymentDetails);
            var renumerationType = default(RemunerationTypeEnum);
            switch (this.Assertable.ExpectedEmploymentTypeKey)
            {
                case EmploymentTypeEnum.Salaried:
                    renumerationType = RemunerationTypeEnum.Salaried;
                    employmentDetails = new SalariedDetails(this.Assertable.ExpectedHouseholdIncome);
                    break;
                case EmploymentTypeEnum.SelfEmployed:
                    renumerationType = RemunerationTypeEnum.Drawings;
                    employmentDetails = new SelfEmployedDetails(this.Assertable.ExpectedHouseholdIncome);
                    break;
            }

            var employment = new ApplicantEmploymentDetails((int)this.Assertable.ExpectedEmploymentTypeKey, employmentDetails);
            var applicationInformation = new ApplicantInformation(idNumber, firstNames, surname, (int)salutation, homePhoneNumber, workPhoneNumber, cellPhoneNumber, emailAddress, dateOfBirth, String.Empty, isMainContact);
            var appDeclarations = new ApplicantDeclarations(incomeContributor, true, true, marriedCOP);
            var applicant = new Applicant(applicationInformation, address, employment, appDeclarations, itc);
            var applicants = new Applicant[] { applicant };
            var testUsers = this.aduserService.GetADUserKeyByADUserName(TestUsers.TeleTestUsers);
            var offerRoles = this.applicationService.GetActiveOfferRolesByLegalEntityKeys(testUsers.Select(x => x.LegalEntityKey).ToArray(), OfferRoleTypeGroupEnum.Operator);

            switch (loanPurpose)
            {
                case MortgageLoanPurposeEnum.Switchloan:
                    app = new SwitchLoanApplication(this.Assertable.ExpectedApplicationKey, (int)offerStatus, DateTime.Now,
                            this.GetSwitchLoanLoanDetails(),
                            applicants, (int)employmentType,
                            this.Assertable.ExpectedConsultantDetails,
                            reasons);
                    break;
                case MortgageLoanPurposeEnum.Newpurchase:
                    app = new NewPurchaseApplication(this.Assertable.ExpectedApplicationKey, (int)offerStatus, DateTime.Now,
                            this.GetNewPurchaseLoanDetails(),
                            applicants,
                            (int)employmentType,
                            this.Assertable.ExpectedConsultantDetails,
                            reasons);
                    break;
            }

            //Sum the employments if existing legal entity and add to householdincome before creating the new application.
            this.Assertable.ExpectedLegalEntityKey = this.legalEntityService.GetLegalEntityKeyByIdNumber(this.Assertable.ExpectedIdNumber);
            if (this.Assertable.ExpectedLegalEntityKey > 0)
            {
                this.Assertable.ExpectedHouseholdIncome += this.employmentService
                                        .GetEmployments(this.Assertable.ExpectedLegalEntityKey, EmploymentStatusEnum.Current)
                                                    .Select(x => (int)x.BasicIncome).Sum();
            }

            //-------------------------------------------CREATE APPLICATION------------------------------------------------------
            Logger.LogDebug("ApplicationKey: {0}", this.Assertable.ExpectedApplicationKey);
            this.capitecApplicationService.CreateCapitecApplication(app);
            //-------------------------------------------------------------------------------------------------------------------
          

            //If application is declined by capitec we don't do consultant stuff.
            if (offerStatus != OfferStatusEnum.Declined)
            {
                this.Assertable.ExpectedConsultantLegalEntityKey = (from ofr in this.applicationService.GetActiveOfferRolesByOfferKey(
                                                                        app.ReservedApplicationKey, OfferRoleTypeGroupEnum.Operator)
                                                                    select ofr.LegalEntityKey).FirstOrDefault();
                if (this.Assertable.ExpectedConsultantLegalEntityKey > 0)
                {
                    var results = this.aduserService.GetAdUserNameByLegalEntityKey(this.Assertable.ExpectedConsultantLegalEntityKey);
                    if (results.HasResults)
                        this.Assertable.ExpectedAssignedConsultant = results.FirstOrDefault().Column("adusername").Value;
                }
            }
            this.Assertable.ExpectedLegalEntityKey = this.legalEntityService.GetLegalEntityKeyByIdNumber(this.Assertable.ExpectedIdNumber);
            if (this.Assertable.ExpectedLegalEntityKey > 0)
                this.Assertable.ExpectedLegalEntityAddressKey = this.legalEntityAddressService.GetLegalEntityAddresses(this.Assertable.ExpectedLegalEntityKey).Max(x => x.LegalEntityAddressKey);

            var maritalStatus = MaritalStatusEnum.Unknown;
            if (marriedCOP)
                maritalStatus = MaritalStatusEnum.MarriedCommunityOfProperty;

            //Employment
            this.Assertable.ExpectedEmploymentKey 
                        = this.employmentService.GetEmployments(this.Assertable.ExpectedLegalEntityKey, EmploymentStatusEnum.Current)
                                                .Where(x => x.ChangeDate > DateTime.Now.Subtract(TimeSpan.FromMinutes(10)))
                                                .Select(x => x.EmploymentKey)
                                                .Max();

            this.Assertable.ExpectedLegalEntity = new LegalEntity
            {
                FirstNames = firstNames,
                Surname = surname,
                DateOfBirth = dateOfBirth,
                CitizenTypeKey = CitizenTypeEnum.None,
                CellPhoneNumber = cellPhoneNumber,
                IntroductionDate = DateTime.Now,
                SalutationKey = salutation,
                IdNumber = idNumber,
                EmailAddress = emailAddress,
                LegalEntityStatusKey = LegalEntityStatusEnum.Alive,
                IncomeContributor = incomeContributor,
                MaritalStatusKey = maritalStatus,
                DocumentLanguageKey = LanguageEnum.English,
                DocumentLanguageDescription = "English",
                EducationKey = EducationEnum.Unknown,
                UserID = @"SAHL\WebLeads",
                ChangeDate = DateTime.Now,
                LegalEntityKey = this.Assertable.ExpectedLegalEntityKey,
                LegalEntityTypeKey = LegalEntityTypeEnum.NaturalPerson,
                LegalEntityExceptionStatusKey = LegalEntityExceptionStatusEnum.None
            };

            if (!String.IsNullOrEmpty(homePhoneNumber))
            {
                this.Assertable.ExpectedLegalEntity.HomePhoneCode = homePhoneNumber.Replace(" ", "").Substring(0, 3);
                this.Assertable.ExpectedLegalEntity.HomePhoneNumber = homePhoneNumber.Replace(" ", "").Substring(3, 7);
            }
            if (!String.IsNullOrEmpty(workPhoneNumber))
            {
                this.Assertable.ExpectedLegalEntity.WorkPhoneCode = workPhoneNumber.Replace(" ", "").Substring(0, 3);
                this.Assertable.ExpectedLegalEntity.WorkPhoneNumber = workPhoneNumber.Replace(" ", "").Substring(3, 7);
            }
            this.Assertable.ExpectedAddressKey = this.addressService.GetExistingResidentialAddress(
                                                                                streetNumber, streetName, province, suburb);
            if (isDirtyLegalEntity)
            {
                this.Assertable.CheckDirtyLegalEntityNotUpdated = checkDirtyLegalEntityNotUpdated;
                this.Assertable.DirtyLegalEntityDetails = new DirtyLegalEntityDetails();
                this.Assertable.DirtyLegalEntityDetails.LegalEntityKey = this.Assertable.ExpectedLegalEntityKey;
                this.Assertable.DirtyLegalEntityDetails.IDNumber = idNumber;
                this.Assertable.DirtyLegalEntityDetails.DateOfBirth = dateOfBirth;
                this.Assertable.DirtyLegalEntityDetails.Surname = surname;
                this.Assertable.DirtyLegalEntityDetails.FirstNames = firstNames;
                this.Assertable.DirtyLegalEntityDetails.Salutation = salutation;
            }
            this.Assertable.ExpectedAuditLegalEntity = new AuditLegalEntity
            {
                FirstNames = firstNames,
                Surname = surname,
                HomePhoneCode = this.Assertable.ExpectedLegalEntity.HomePhoneCode,
                HomePhoneNumber = this.Assertable.ExpectedLegalEntity.HomePhoneNumber,
                WorkPhoneCode = this.Assertable.ExpectedLegalEntity.WorkPhoneCode,
                WorkPhoneNumber = this.Assertable.ExpectedLegalEntity.WorkPhoneNumber,
                EmailAddress = this.Assertable.ExpectedLegalEntity.EmailAddress
            };
        }

        protected void AssertApplication()
        {
            OfferAssertions.AssertApplicationInformation(this.Assertable.ExpectedTerm, this.Assertable.ExpectedApplicationKey, "Term");
            OfferAssertions.AssertApplicationInformation(this.Assertable.ExpectedLAA, this.Assertable.ExpectedApplicationKey, "LoanAgreementAmount");
            OfferAssertions.AssertApplicationInformation(this.Assertable.ExpectedHouseholdIncome, this.Assertable.ExpectedApplicationKey, "HouseholdIncome");
            OfferAssertions.AssertApplicationInformation((int)this.Assertable.ExpectedEmploymentTypeKey, this.Assertable.ExpectedApplicationKey, "EmploymentTypeKey");
            OfferAssertions.AssertLatestOfferInformationProduct(this.Assertable.ExpectedApplicationKey, Products.NewVariableLoan);
            OfferAssertions.AssertOfferRoleExists(this.Assertable.ExpectedLegalEntityKey, this.Assertable.ExpectedApplicationKey, GeneralStatusEnum.Active, true);
            OfferAssertions.AssertOfferRoleAttributeExists(this.Assertable.ExpectedApplicationKey, this.Assertable.ExpectedLegalEntityKey, OfferRoleAttributeTypeEnum.IncomeContributor, true);
            if (this.Assertable.IsMainContact)
                OfferAssertions.AssertOfferRoleAttributeExists(this.Assertable.ExpectedApplicationKey, Assertable.ExpectedLegalEntityKey, OfferRoleAttributeTypeEnum.MainContact, true);
            OfferAssertions.AssertOfferAttributeExists(Assertable.ExpectedApplicationKey, OfferAttributeTypeEnum.Capitec, true);
            AddressAssertions.AssertResidentialAddressRecordExists(this.Assertable.ExpectedStreetNumber, this.Assertable.ExpectedStreetName, this.Assertable.ExpectedProvince, this.Assertable.ExpectedSuburb);
            AddressAssertions.AssertAddressChangeDateExists(this.Assertable.ExpectedAddressKey, this.Assertable.ExpectedAddressChangeDate);
            AddressAssertions.AssertAddressUserID(this.Assertable.ExpectedAddressKey, this.Assertable.ExpectedAddressUserID);

            if (this.Assertable.DirtyLegalEntityDetails != null)
            {
                if (this.Assertable.CheckDirtyLegalEntityNotUpdated)
                {
                    LegalEntityAssertions.AssertDirtyLegalEntityNotUpdated(this.Assertable.DirtyLegalEntityDetails);
                }
                else
                {
                    LegalEntityAssertions.AssertDirtyLegalEntityUpdated(this.Assertable.DirtyLegalEntityDetails);
                }
            }
            else
            {
                LegalEntityAssertions.AssertAllLegalEntityDetails(this.Assertable.ExpectedLegalEntity);
            }

            if (this.Assertable.HasITC)
                OfferAssertions.AssertITCRecordExists(this.Assertable.ExpectedApplicationKey);
            if (this.Assertable.IsITCLinkedToLegalEntity)
                LegalEntityAssertions.AssertITCRecordExists(this.Assertable.ExpectedLegalEntityKey);
            X2Assertions.AssertX2RequestProcessed(this.Assertable.ExpectedApplicationKey);
            if (this.Assertable.ExpectedOfferStatusKey != OfferStatusEnum.Declined)
            {
                OfferAssertions.AssertOfferRoleExists(this.Assertable.ExpectedConsultantLegalEntityKey, this.Assertable.ExpectedApplicationKey, GeneralStatusEnum.Active, true);
                OfferAssertions.AssertOfferRoleCreatedAndAssignedToADUser(this.Assertable.ExpectedApplicationKey, this.Assertable.ExpectedTeleConsultant, OfferRoleTypeEnum.BranchConsultantD, (int)UserOrganisationStructureEnum.TeleConsultant);
                X2Assertions.AssertCurrentAppCapX2State(this.Assertable.ExpectedApplicationKey, WorkflowStates.ApplicationCaptureWF.CapitecApplications);
                X2Assertions.AssertUserRoleSecurity(Workflows.ApplicationCapture, WorkflowActivities.ApplicationCapture.ContactwithClient, WorkflowStates.ApplicationCaptureWF.CapitecApplications, X2SecurityGroups.BranchConsultant);
                X2Assertions.AssertWorklistOwner(this.Assertable.ExpectedApplicationKey, WorkflowStates.ApplicationCaptureWF.CapitecApplications, Workflows.ApplicationCapture, this.Assertable.ExpectedAssignedConsultant);
                StageTransitionAssertions.AssertStageTransitionCreated(this.Assertable.ExpectedApplicationKey, StageDefinitionStageDefinitionGroupEnum.CreateCapitecApplication);
            }
            else
            {
                X2Assertions.AssertCurrentAppCapX2State(this.Assertable.ExpectedApplicationKey, WorkflowStates.ApplicationCaptureWF.CapitecBranchDeclined);
                StageTransitionAssertions.AssertStageTransitionCreated(this.Assertable.ExpectedApplicationKey, StageDefinitionStageDefinitionGroupEnum.DeclineCapitecApplication);
            }
            if (this.Assertable.HasApplicationReasons)
            {
                ReasonAssertions.AssertReason(this.Assertable.ExpectedApplicationReasonDescription,
                                                this.Assertable.ExpectedApplicationReasonType,
                                                this.Assertable.ExpectedApplicationKey,
                                                GenericKeyTypeEnum.OfferInformation_OfferInformationKey,
                                                reasonExists: true);
            }

            if (!this.Assertable.CheckDirtyLegalEntityNotUpdated)
                AuditAssertions.AssertLegalEntity(this.Assertable.ExpectedLegalEntityKey, this.Assertable.ExpectedAuditLegalEntity);
        }
        
        private string GetUniqueString()
        {
            return random.Next(Int32.MinValue, Int32.MaxValue).ToString();
        }
        private ApplicantITC GetITCFromTemplate()
        {
            var responseDoc = XDocument.Load(@"Templates\ITCResponse.xml");
            var requestDoc = XDocument.Load(@"Templates\ITCRequest.xml");
            var requestStr = requestDoc.ToString();
            var responseStr = responseDoc.ToString();
            return new ApplicantITC(DateTime.Now, requestStr, responseStr);
        }
        private SwitchLoanDetails GetSwitchLoanLoanDetails()
        {
            var loanDetails = new SwitchLoanDetails((int)this.Assertable.ExpectedEmploymentTypeKey, 
                                    this.Assertable.ExpectedHouseholdIncome, 920000, 120000, 240000, 0M, true, 
                                    this.Assertable.ExpectedTerm);
            this.Assertable.ExpectedLAA = 378798;//There are initiation fee changes that is not in production yet
            return loanDetails;
        }
        private NewPurchaseLoanDetails GetNewPurchaseLoanDetails()
        {
            var loanDetails = new NewPurchaseLoanDetails((int)this.Assertable.ExpectedEmploymentTypeKey, 
                                    this.Assertable.ExpectedHouseholdIncome, 900000, 130000, false, 
                                    this.Assertable.ExpectedTerm);
            this.Assertable.ExpectedLAA = (900000 - 130000);
            return loanDetails;
        }
        protected CapitecApplicationAssertion Assertable { get; private set; }
    }
}
