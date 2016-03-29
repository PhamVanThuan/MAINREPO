using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared.Globals;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.Application.Models;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Services.Capitec.Managers.Application
{
    public class ApplicationManager : IApplicationManager
    {
        private ILookupManager lookupService;
        private IApplicantManager applicantService;
        private IApplicationDataManager applicationDataService;
        private IDecisionTreeServiceClient decisionTreeService;
        private IDecisionTreeResultManager decisionTreeResultService;
        private IITCManager applicantITCService;
        private IServiceCommandRouter serviceCommandRouter;

        public ApplicationManager(ILookupManager lookupService, IApplicantManager applicantService, IApplicationDataManager applicationDataService,
            IDecisionTreeServiceClient decisionTreeService, IDecisionTreeResultManager decisionTreeResultService, IITCManager applicantITCService, IServiceCommandRouter serviceCommandRouter)
        {
            this.lookupService = lookupService;
            this.applicantService = applicantService;
            this.applicationDataService = applicationDataService;
            this.decisionTreeService = decisionTreeService;
            this.decisionTreeResultService = decisionTreeResultService;
            this.applicantITCService = applicantITCService;
            this.serviceCommandRouter = serviceCommandRouter;
        }

        public void AddLoanApplication(int applicationNumber, Guid applicationID, LoanApplication application)
        {
            var applicationPurposeEnumId = application.LoanDetails.ApplicationType;

            if (!DoesApplicationExist(applicationID))
            {
                applicationDataService.AddApplication(applicationID, application.ApplicationDate, applicationNumber, applicationPurposeEnumId, application.UserId, application.CaptureStartTime, application.BranchId);
            }
        }

        public List<ApplicantModel> GetApplicantsForApplication(Guid applicationID)
        {
            return applicationDataService.GetApplicantsForApplication(applicationID);
        }

        public void AddNewPurchaseLoanDetailToApplication(Guid applicationID, ApplicationLoanDetails loanDetails, CalculatedLoanDetailsModel calculatedDetails)
        {
            var applicationLoanDetailID = lookupService.GenerateCombGuid();
            applicationDataService.AddApplicationLoanDetail(applicationID, applicationLoanDetailID, loanDetails.IncomeType, loanDetails.OccupancyType, loanDetails.HouseholdIncome, calculatedDetails.Instalment, calculatedDetails.InterestRate, calculatedDetails.LoanAmount, calculatedDetails.LTV, calculatedDetails.PTI, loanDetails.Fees, calculatedDetails.TermInMonths, loanDetails.CapitaliseFees);
            applicationDataService.AddNewPurchaseApplicationLoanDetail(applicationLoanDetailID, loanDetails.PurchasePrice, loanDetails.Deposit);
        }

        public void AddSwitchLoanDetailToApplication(Guid applicationID, ApplicationLoanDetails loanDetails, CalculatedLoanDetailsModel calculatedDetails)
        {
            var applicationLoanDetailID = lookupService.GenerateCombGuid();
            applicationDataService.AddApplicationLoanDetail(applicationID, applicationLoanDetailID, loanDetails.IncomeType, loanDetails.OccupancyType, loanDetails.HouseholdIncome, calculatedDetails.Instalment, calculatedDetails.InterestRate, calculatedDetails.LoanAmount, calculatedDetails.LTV, calculatedDetails.PTI, loanDetails.Fees, calculatedDetails.TermInMonths, loanDetails.CapitaliseFees);
            applicationDataService.AddSwitchApplicationLoanDetail(applicationLoanDetailID, loanDetails.CashRequired, loanDetails.CurrentBalance, loanDetails.EstimatedMarketValueOfTheHome, loanDetails.InterimInterest);
        }

        public bool PerformITCs(Dictionary<Guid, Interfaces.Capitec.ViewModels.Apply.Applicant> applicants)
        {
            bool itcsPassed = true;
            foreach (var applicant in applicants)
            {
                if (applicant.Value.Declarations.IncomeContributor == Guid.Parse(DeclarationTypeEnumDataModel.YES))
                {
                    try
                    {
                        var performItcCommand = new PerformITCForApplicantCommand(applicant.Key, applicant.Value);
                        serviceCommandRouter.HandleCommand(performItcCommand, new ServiceRequestMetadata());

                        var itcModel = applicantITCService.GetValidITCModelForPerson(applicant.Value.Information.IdentityNumber);
                        if (itcModel != null)
                        {
                            var itcProfile = applicantITCService.GetITCProfile(itcModel.Id);
                            applicant.Value.EmpiricaScore = itcProfile.EmpericaScore ?? -999;
                            applicant.Value.ITCDate = itcModel.ITCDate;
                            applicant.Value.ITCResponse = XDocument.Parse(itcModel.ITCData);
                            var applicantItcPassed = applicantITCService.DoesITCPassQualificationRules(itcProfile, applicant.Key);
                            if (itcsPassed) { itcsPassed = applicantItcPassed; }
                        }
                        else
                        {
                            applicant.Value.EmpiricaScore = -999;
                        }
                    }
                    catch (Exception ex)
                    {
                        applicant.Value.EmpiricaScore = -999;
                    }                    
                }
            }
            return itcsPassed;
        }

        public bool RecalculateSwitchApplication(LoanApplication application, Dictionary<Guid, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> applicants, bool eligibleBorrower, Guid applicationID)
        {
            bool eligibleApplication = false;
            var loanDetails = GetRecalculatedLoanDetailsForApplication(application);
            var calculatedDetails = CalculateLoanDetailForApplication(loanDetails, applicants.Values.ToList(), eligibleBorrower, applicationID);
            AddSwitchLoanDetailToApplication(applicationID, loanDetails, calculatedDetails);
            eligibleApplication = calculatedDetails.EligibleApplication;
            return eligibleApplication;
        }

        public bool RecalculateNewPurchaseApplication(LoanApplication application, Dictionary<Guid, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> applicants, bool eligibleBorrower, Guid applicationID)
        {
            bool eligibleApplication = false;
            var loanDetails = GetRecalculatedLoanDetailsForApplication(application);
            var calculatedDetails = CalculateLoanDetailForApplication(loanDetails, applicants.Values.ToList(), eligibleBorrower, applicationID);
            AddNewPurchaseLoanDetailToApplication(applicationID, loanDetails, calculatedDetails);
            eligibleApplication = calculatedDetails.EligibleApplication;
            return eligibleApplication;
        }

        public CalculatedLoanDetailsModel CalculateLoanDetailForApplication(ApplicationLoanDetails loanDetails, List<SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> applicants, bool eligibleBorrower, Guid applicationID)
        {
            CalculatedLoanDetailsModel result = new CalculatedLoanDetailsModel();

            var applicationType = "";
            if (loanDetails.ApplicationType == Guid.Parse(ApplicationPurposeEnumDataModel.NEW_PURCHASE))
            {
                applicationType = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().NewPurchase;
            }
            else
            {
                applicationType = new Enumerations.SAHomeLoans.MortgageLoanApplicationType().Switch;
            }

            var deposit = loanDetails.Deposit;
            var fees = loanDetails.Fees;
            var interimInterest = loanDetails.InterimInterest;
            var capitaliseFees = loanDetails.CapitaliseFees;
            var householdIncome = loanDetails.HouseholdIncome;
            var householdIncomeType = lookupService.GetDecisionTreeHouseholdIncomeType(loanDetails.IncomeType);
            var occupancyType = lookupService.GetDecisionTreeOccupancyType(loanDetails.OccupancyType);
            var propertyPurchasePrice = loanDetails.PurchasePrice;

            decimal cashAmountRequired = -1;
            decimal currentMortgageLoanBalance = -1;
            decimal estimatedMarketValueofProperty = -1;

            if (applicationType == new Enumerations.SAHomeLoans.MortgageLoanApplicationType().Switch)
            {
                var switchDetails = loanDetails;
                cashAmountRequired = switchDetails.CashRequired;
                currentMortgageLoanBalance = switchDetails.CurrentBalance;
                estimatedMarketValueofProperty = switchDetails.EstimatedMarketValueOfTheHome;
            }

            int eldestApplicantAge = applicantService.CalculateAge(applicants.Min(x => x.Information.DateOfBirth).Value, DateTime.Now);
            int youngestApplicantAge = applicantService.CalculateAge(applicants.Max(x => x.Information.DateOfBirth).Value, DateTime.Now);
            int termInMonths = SAHL.Services.Interfaces.Capitec.Common.CalculatorConstants.CalculatorTermInMonths;

            var incomeContributingApplicants = applicants.Where(x => x.Declarations.IncomeContributor == Guid.Parse(DeclarationTypeEnumDataModel.YES)).ToList();

            int firstIncomeContributorApplicantEmpirica = incomeContributingApplicants[0].EmpiricaScore;
            decimal firstIncomeContributorApplicantIncome = incomeContributingApplicants.FirstOrDefault().EmploymentDetails.CalculateGrossMonthlyIncome();

            int secondIncomeContributorApplicantEmpirica = -999;
            decimal secondIncomeContributorApplicantIncome = -1;
            if (incomeContributingApplicants.Count() > 1)
            {
                secondIncomeContributorApplicantIncome = incomeContributingApplicants[1].EmploymentDetails.CalculateGrossMonthlyIncome();
                secondIncomeContributorApplicantEmpirica = incomeContributingApplicants[1].EmpiricaScore;
            }

            if (firstIncomeContributorApplicantEmpirica == -999 && secondIncomeContributorApplicantEmpirica == -999)
            {
                firstIncomeContributorApplicantIncome = -1;
                secondIncomeContributorApplicantIncome = -1;
            }

            CapitecOriginationCreditPricing_Query treeQuery = new CapitecOriginationCreditPricing_Query(
                applicationType,
                occupancyType,
                householdIncomeType,
                (double)householdIncome,
                (double)propertyPurchasePrice,
                (double)deposit,
                (double)cashAmountRequired,
                (double)currentMortgageLoanBalance,
                (double)estimatedMarketValueofProperty,
                eldestApplicantAge,
                youngestApplicantAge,
                termInMonths,
                firstIncomeContributorApplicantEmpirica,
                (double)firstIncomeContributorApplicantIncome,
                secondIncomeContributorApplicantEmpirica,
                (double)secondIncomeContributorApplicantIncome, eligibleBorrower,
                (double)fees,
                (double)interimInterest,
                capitaliseFees);

            try
            {
                var decisionTreeMessages = decisionTreeService.PerformQuery(treeQuery);

                var decisionTreeResultMessages = SystemMessageCollection.Empty();
                decisionTreeResultMessages.AddMessages(decisionTreeMessages.WarningMessages());
                decisionTreeResultMessages.AddMessages(decisionTreeMessages.InfoMessages());
                decisionTreeResultService.SaveCreditPricingTreeResult(treeQuery, decisionTreeResultMessages, applicationID);

                var treeResult = treeQuery.Result.Results.SingleOrDefault();
                result.InterestRate = (decimal)treeResult.InterestRate;
                result.Instalment = (decimal)treeResult.Instalment;
                result.LoanAmount = (decimal)treeResult.LoanAmount;
                result.LTV = (decimal)treeResult.LoantoValue;
                result.PTI = (decimal)treeResult.PaymenttoIncome;
                result.EligibleApplication = treeResult.EligibleApplication;
                result.TermInMonths = SAHL.Services.Interfaces.Capitec.Common.CalculatorConstants.CalculatorTermInMonths;
            }
            catch (Exception)
            {
                throw new Exception("An error ocurred while running the Capitec Origination Credit Pricing Decision Tree");
            }
            return result;
        }

        public ApplicationLoanDetails GetRecalculatedLoanDetailsForApplication(LoanApplication application)
        {
            var recalculatedLoanDetails = application.LoanDetails;
            Guid declarationTypeEnumID = Guid.Parse(DeclarationTypeEnumDataModel.YES);
            var incomeContributingApplicants = application.Applicants.Where(x => x.Declarations.IncomeContributor == declarationTypeEnumID);
            var totalHouseholdIncome = incomeContributingApplicants.Sum(x => x.EmploymentDetails.CalculateGrossMonthlyIncome());

            var maxContributingIncome = (from i in incomeContributingApplicants
                                         group i by i.EmploymentDetails.EmploymentTypeEnumId into g
                                         select new { EmploymentType = g.Key, MaxIncome = g.Max(x => x.EmploymentDetails.CalculateGrossMonthlyIncome()) })
                                       .OrderByDescending(x => x.MaxIncome).FirstOrDefault();

            var incomeType = maxContributingIncome.EmploymentType;
            recalculatedLoanDetails.IncomeType = incomeType;
            recalculatedLoanDetails.HouseholdIncome = totalHouseholdIncome;
            return recalculatedLoanDetails;
        }

        public int GetApplicationNumberForApplication(Guid guid)
        {
            var application = applicationDataService.GetApplicationByID(guid);
            return application.ApplicationNumber ?? -1;
        }

        public void SetApplicationToDeclined(Guid applicationID)
        {
            var declineStatusID = Guid.Parse(ApplicationStatusEnumDataModel.PORTAL_DECLINED);
            applicationDataService.SetApplicationStatus(applicationID, declineStatusID, DateTime.Now);
        }

        public bool DoesApplicationExist(Guid applicationID)
        {
            if (applicationDataService.DoesApplicationExist(applicationID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateCaptureEndTime(Guid applicationID, DateTime captureEndTime)
        {
            applicationDataService.UpdateCaptureEndTime(applicationID, captureEndTime);
        }
    }
}