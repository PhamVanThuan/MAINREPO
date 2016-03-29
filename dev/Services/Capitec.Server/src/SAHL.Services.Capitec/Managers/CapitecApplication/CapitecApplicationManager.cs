using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Interfaces.Capitec.Common;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Capitec.Managers.CapitecApplication
{
    public class CapitecApplicationManager : ICapitecApplicationManager
    {
        private ILookupManager lookupService;
        private ICapitecApplicationDataManager dataManager;

        public CapitecApplicationManager(ILookupManager lookupService, ICapitecApplicationDataManager dataManager)
        {
            this.lookupService = lookupService;
            this.dataManager = dataManager;
        }

        public NewPurchaseApplication CreateCapitecApplicationFromNewPurchaseApplication(int applicationNumber, Enumerations.ApplicationStatusEnums applicationStatusEnum
            , SAHL.Services.Interfaces.Capitec.ViewModels.Apply.NewPurchaseApplication capitecNewPurchaseApplication, ISystemMessageCollection messages, Guid applicationId)
        {
            IList<SAHL.Services.Capitec.Models.Shared.Applicant> applicants = SetupCapitecApplicants(capitecNewPurchaseApplication.Applicants);

            IList<string> sharedMessages = new List<string>();
            foreach (var message in messages.WarningMessages())
            {
                sharedMessages.Add(message.Message);
            }

            SAHL.Services.Capitec.Models.Shared.NewPurchaseLoanDetails newPurchaseLoanDetails =
                new SAHL.Services.Capitec.Models.Shared.NewPurchaseLoanDetails(
                    this.lookupService.GetSahlKeyByCapitecGuid(capitecNewPurchaseApplication.NewPurchaseLoanDetails.IncomeType),
                    capitecNewPurchaseApplication.NewPurchaseLoanDetails.HouseholdIncome,
                    capitecNewPurchaseApplication.NewPurchaseLoanDetails.PurchasePrice,
                    capitecNewPurchaseApplication.NewPurchaseLoanDetails.Deposit,
                    capitecNewPurchaseApplication.NewPurchaseLoanDetails.CapitaliseFees,
                    capitecNewPurchaseApplication.NewPurchaseLoanDetails.TermInMonths);

            var userBranchMapping = dataManager.GetCapitecUserBranchMappingForApplication(applicationId);
            var consultantDetails = new ConsultantDetails(userBranchMapping.UserName, userBranchMapping.BranchName);

            SAHL.Services.Capitec.Models.Shared.NewPurchaseApplication newPurchaseApplication =
                new SAHL.Services.Capitec.Models.Shared.NewPurchaseApplication(
                    applicationNumber,
                    GetSAHLApplicationStatus(applicationStatusEnum),
                    capitecNewPurchaseApplication.ApplicationDate,
                    newPurchaseLoanDetails,
                    applicants,
                    this.lookupService.GetSahlKeyByCapitecGuid(capitecNewPurchaseApplication.NewPurchaseLoanDetails.IncomeType),
                    consultantDetails,
                    sharedMessages);

            return newPurchaseApplication;
        }

        private int GetSAHLApplicationStatus(Enumerations.ApplicationStatusEnums applicationStatusEnum)
        {
            if (applicationStatusEnum == Enumerations.ApplicationStatusEnums.PortalDecline)
                return 5;

            return (int)applicationStatusEnum;
        }

        public SwitchLoanApplication CreateCapitecApplicationFromSwitchLoanApplication(int applicationNumber, Enumerations.ApplicationStatusEnums applicationStatusEnum, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.SwitchLoanApplication capitecSwitchLoanApplication, ISystemMessageCollection messages, Guid applicationId)
        {
            IList<SAHL.Services.Capitec.Models.Shared.Applicant> applicants = SetupCapitecApplicants(capitecSwitchLoanApplication.Applicants);

            IList<string> sharedMessages = new List<string>();
            foreach (var message in messages.WarningMessages())
            {
                sharedMessages.Add(message.Message);
            }

            SAHL.Services.Capitec.Models.Shared.SwitchLoanDetails switchLoanDetails =
                new SAHL.Services.Capitec.Models.Shared.SwitchLoanDetails(
                    this.lookupService.GetSahlKeyByCapitecGuid(capitecSwitchLoanApplication.SwitchLoanDetails.IncomeType),
                    capitecSwitchLoanApplication.SwitchLoanDetails.HouseholdIncome,
                    capitecSwitchLoanApplication.SwitchLoanDetails.EstimatedMarketValueOfTheHome,
                    capitecSwitchLoanApplication.SwitchLoanDetails.CashRequired,
                    capitecSwitchLoanApplication.SwitchLoanDetails.CurrentBalance,
                    capitecSwitchLoanApplication.SwitchLoanDetails.InterimInterest,
                    capitecSwitchLoanApplication.SwitchLoanDetails.CapitaliseFees,
                    capitecSwitchLoanApplication.SwitchLoanDetails.TermInMonths);

            var userBranchMapping = dataManager.GetCapitecUserBranchMappingForApplication(applicationId);
            var consultantDetails = new ConsultantDetails(userBranchMapping.UserName, userBranchMapping.BranchName);

            SAHL.Services.Capitec.Models.Shared.SwitchLoanApplication switchLoanApplication =
            new SAHL.Services.Capitec.Models.Shared.SwitchLoanApplication(
                applicationNumber,
                GetSAHLApplicationStatus(applicationStatusEnum),
                capitecSwitchLoanApplication.ApplicationDate,
                switchLoanDetails,
                applicants,
                this.lookupService.GetSahlKeyByCapitecGuid(capitecSwitchLoanApplication.SwitchLoanDetails.IncomeType),
                consultantDetails,
                    sharedMessages);

            return switchLoanApplication;
        }

        private IList<SAHL.Services.Capitec.Models.Shared.Applicant> SetupCapitecApplicants(IEnumerable<SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> modelApplicants)
        {
            EmploymentDetails employmentDetails = null;
            IList<SAHL.Services.Capitec.Models.Shared.Applicant> applicants = new List<SAHL.Services.Capitec.Models.Shared.Applicant>();

            foreach (SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant applicant in modelApplicants)
            {
                employmentDetails = null;
                if (applicant.EmploymentDetails.SalariedDetails != null)
                    employmentDetails =
                        new SAHL.Services.Capitec.Models.Shared.SalariedDetails(
                        applicant.EmploymentDetails.SalariedDetails.GrossMonthlyIncome);
                else if (applicant.EmploymentDetails.SelfEmployedDetails != null)
                    employmentDetails =
                        new SAHL.Services.Capitec.Models.Shared.SelfEmployedDetails(
                        applicant.EmploymentDetails.SelfEmployedDetails.GrossMonthlyIncome);
                else if (applicant.EmploymentDetails.SalariedWithCommissionDetails != null)
                    employmentDetails =
                        new SAHL.Services.Capitec.Models.Shared.SalariedWithCommissionDetails(
                        applicant.EmploymentDetails.SalariedWithCommissionDetails.GrossMonthlyIncome,
                        applicant.EmploymentDetails.SalariedWithCommissionDetails.ThreeMonthAverageCommission);
                else if (applicant.EmploymentDetails.SalariedWithHousingAllowanceDetails != null)
                    employmentDetails =
                        new SAHL.Services.Capitec.Models.Shared.SalariedWithHousingAllowanceDetails(
                        applicant.EmploymentDetails.SalariedWithHousingAllowanceDetails.GrossMonthlyIncome,
                        applicant.EmploymentDetails.SalariedWithHousingAllowanceDetails.HousingAllowance);

                SAHL.Services.Capitec.Models.Shared.ApplicantEmploymentDetails applicantEmploymentDetails =
                    new SAHL.Services.Capitec.Models.Shared.ApplicantEmploymentDetails(
                        this.lookupService.GetSahlKeyByCapitecGuid(applicant.EmploymentDetails.EmploymentTypeEnumId),
                        employmentDetails);

                SAHL.Services.Capitec.Models.Shared.ApplicantResidentialAddress applicantResidentialAddress =
                    new SAHL.Services.Capitec.Models.Shared.ApplicantResidentialAddress(
                        applicant.ResidentialAddress.UnitNumber,
                        applicant.ResidentialAddress.BuildingNumber,
                        applicant.ResidentialAddress.BuildingName,
                        applicant.ResidentialAddress.StreetNumber,
                        applicant.ResidentialAddress.StreetName,
                        applicant.ResidentialAddress.Suburb,
                        applicant.ResidentialAddress.Province,
                        applicant.ResidentialAddress.City,
                        applicant.ResidentialAddress.PostalCode,
                        this.lookupService.GetSahlKeyByCapitecGuid(applicant.ResidentialAddress.SuburbId));

                SAHL.Services.Capitec.Models.Shared.ApplicantDeclarations applicantDeclarations =
                    new SAHL.Services.Capitec.Models.Shared.ApplicantDeclarations(
                        this.lookupService.GetDeclarationResultByCapitecGuid(applicant.Declarations.IncomeContributor),
                        this.lookupService.GetDeclarationResultByCapitecGuid(applicant.Declarations.AllowCreditBureauCheck),
                        this.lookupService.GetDeclarationResultByCapitecGuid(applicant.Declarations.HasCapitecTransactionAccount),
                        this.lookupService.GetDeclarationResultByCapitecGuid(applicant.Declarations.MarriedInCommunityOfProperty));

                SAHL.Services.Capitec.Models.Shared.ApplicantInformation applicantInformation =
                    new SAHL.Services.Capitec.Models.Shared.ApplicantInformation(
                        applicant.Information.IdentityNumber,
                        applicant.Information.FirstName,
                        applicant.Information.Surname,
                        this.lookupService.GetSahlKeyByCapitecGuid(applicant.Information.SalutationEnumId),
                        applicant.Information.HomePhoneNumber,
                        applicant.Information.WorkPhoneNumber,
                        applicant.Information.CellPhoneNumber,
                        applicant.Information.EmailAddress,
                        applicant.Information.DateOfBirth,
                        applicant.Information.Title,
                        applicant.Information.MainContact);

                var itcResponse = applicant.ITCResponse;
                ApplicantITC applicantITC = null;
                if (itcResponse != null)
                {
                    var request = applicant.ITCRequest == null ? string.Empty : applicant.ITCRequest.Root.ToString();
                    var response = applicant.ITCResponse == null ? string.Empty : itcResponse.Root.ToString();
                    applicantITC = new ApplicantITC(applicant.ITCDate, request, response);
                }

                SAHL.Services.Capitec.Models.Shared.Applicant capitecApplicant =
                    new SAHL.Services.Capitec.Models.Shared.Applicant(
                        applicantInformation,
                        applicantResidentialAddress,
                        applicantEmploymentDetails,
                        applicantDeclarations,
                        applicantITC);

                applicants.Add(capitecApplicant);
            }
            return applicants;
        }
    }
}