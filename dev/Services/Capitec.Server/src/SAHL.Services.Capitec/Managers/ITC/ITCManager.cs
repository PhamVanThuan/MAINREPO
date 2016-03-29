using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Logging;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using SAHL.Services.Interfaces.ITC;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.Interfaces.ITC.Queries;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Managers.ITC
{
    public class ITCManager : IITCManager
    {
        private ILookupManager lookupService;
        private IApplicantDataManager applicantDataService;
        private IDecisionTreeServiceClient decisionTreeService;
        private IDecisionTreeResultManager decisionTreeResultService;
        private ILogger logger;
        private ILoggerSource loggerSource;
        private IItcServiceClient itcServiceClient;
        private IITCDataManager applicantItcDataService;

        private const int daysItcIsValid = 31;

        public ITCManager(ILookupManager lookupService, IApplicantDataManager applicantDataService, IITCDataManager applicantItcDataService, IDecisionTreeServiceClient decisionTreeService,
            IDecisionTreeResultManager decisionTreeResultService, ILogger logger, ILoggerSource loggerSource, IItcServiceClient itcServiceClient)
        {
            this.lookupService = lookupService;
            this.applicantDataService = applicantDataService;
            this.decisionTreeService = decisionTreeService;
            this.decisionTreeResultService = decisionTreeResultService;
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.itcServiceClient = itcServiceClient;
            this.applicantItcDataService = applicantItcDataService;
        }

        public bool DoesITCPassQualificationRules(ItcProfile itc, Guid applicantID)
        {
            var judgmentsInLast3Years = itc.Judgments.Where(x => x.JudgmentDate >= DateTime.Now.AddYears(-3));
            var defaultsInLast2Years = itc.Defaults.Where(x => x.InformationDate >= DateTime.Now.AddYears(-2));
            var notices = itc.Notices;
            var paymentProfile = itc.PaymentProfiles;

            bool creditBureauMatch = itc.CreditBureauMatch;
            int applicantEmpirica = itc.EmpericaScore ?? -999;

            int numberofJudgmentswithinLast3Years = judgmentsInLast3Years.Count();
            decimal nonSettledAggregatedJudgmentValuewithinLast3Years = judgmentsInLast3Years.Sum(x => x.Amount);
            decimal aggregatedJudgmentValuewithinLast3Years = judgmentsInLast3Years.Sum(x => x.Amount);

            int numberofUnsettledDefaultswithinLast2Years = defaultsInLast2Years.Count();

            bool administrationOrderNotice = notices.Any(x => x.NoticeTypeCode == "A");
            bool sequestrationNotice = notices.Any(x => x.NoticeTypeCode == "SEQ");

            bool paidOutonDeceasedClaim = paymentProfile.Any(x => x.PaymentHistories.Any(y => y.StatusCode == "K"));
            bool consumerAbsconded = paymentProfile.Any(x => x.PaymentHistories.Any(y => y.StatusCode == "A"));
            bool consumerDeceasedNotification = paymentProfile.Any(x => x.PaymentHistories.Any(y => y.StatusCode == "Z"));
            bool creditCardRevoked = paymentProfile.Any(x => x.PaymentHistories.Any(y => y.StatusCode == "I"));

            bool debtReviewNotice = false;
            bool debtCounsellingNotice = false;
            if (itc.DebtCounselling != null)
            {
                debtReviewNotice = itc.DebtCounselling.Code == "DOR";
                debtCounsellingNotice = true;
            }
            var creditCheckQuery = new CapitecClientCreditBureauAssessment_Query(
                applicantEmpirica,
                numberofJudgmentswithinLast3Years,
                (double)aggregatedJudgmentValuewithinLast3Years,
                (double)nonSettledAggregatedJudgmentValuewithinLast3Years,
                numberofUnsettledDefaultswithinLast2Years,
                sequestrationNotice,
                administrationOrderNotice,
                debtCounsellingNotice,
                debtReviewNotice,
                consumerDeceasedNotification,
                creditCardRevoked,
                consumerAbsconded,
                paidOutonDeceasedClaim,
                creditBureauMatch);

            var decisionTreeMessages = decisionTreeService.PerformQuery(creditCheckQuery);
            decisionTreeResultService.SaveCreditAssessmentTreeResult(creditCheckQuery, decisionTreeMessages, applicantID);

            return creditCheckQuery.Result.Results.SingleOrDefault().EligibleBorrower;
        }

        public ITCDataModel GetValidITCModelForPerson(string identityNumber)
        {
            var itcs = applicantItcDataService.GetItcModelsForPerson(identityNumber);
            var validItcDate = DateTime.Now.AddDays(-1 * daysItcIsValid);
            var validItc = itcs.OrderByDescending(x => x.ITCDate).FirstOrDefault(y => y.ITCDate >= validItcDate);
            return validItc;
        }

        public ApplicantITCRequestDetailsModel CreateApplicantITCRequest(Interfaces.Capitec.ViewModels.Apply.Applicant applicant)
        {
            var addressLine1 = String.Concat(applicant.ResidentialAddress.BuildingNumber, ' ', applicant.ResidentialAddress.BuildingName);
            var addressLine2 = String.Concat(applicant.ResidentialAddress.StreetNumber, ' ', applicant.ResidentialAddress.StreetName);

            ApplicantITCRequestDetailsModel applicantITCRequestDetails = new ApplicantITCRequestDetailsModel(applicant.Information.FirstName,
                applicant.Information.Surname, applicant.Information.DateOfBirth, applicant.Information.IdentityNumber, applicant.Information.Title,
                applicant.Information.HomePhoneNumber, applicant.Information.WorkPhoneNumber, applicant.Information.CellPhoneNumber,
                applicant.Information.EmailAddress, addressLine1, addressLine2, applicant.ResidentialAddress.Suburb, applicant.ResidentialAddress.City,
                applicant.ResidentialAddress.PostalCode);

            return applicantITCRequestDetails;
        }

        public ItcProfile GetITCProfile(Guid itcID)
        {
            var getItcProfileQuery = new GetCapitecITCProfileQuery(itcID);
            var messages = itcServiceClient.PerformQuery(getItcProfileQuery);

            if (getItcProfileQuery.Result != null && getItcProfileQuery.Result.Results.Count() > 0)
            {
                return getItcProfileQuery.Result.Results.First().ITCProfile;
            }
            return null;
        }

        public ITCDataModel GetITC(Guid itcID)
        {
            return applicantItcDataService.GetItcById(itcID);
        }

        public void LinkItcToPerson(Guid personID, Guid itcID, DateTime itcDate)
        {
            var personITC = applicantItcDataService.GetPersonITC(personID);
            if (personITC == null)
            {
                applicantItcDataService.SavePersonItc(personID, itcID, itcDate);
            }
            else if(personITC.CurrentITCId != itcID)
            {
                applicantItcDataService.UpdatePersonItc(personID, itcID, itcDate);
            }
        }
    }
}