using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Capitec.Specs.ApplicantITCServiceSpecs
{
    public class when_asked_to_check_the_itc_judgements : with_applicant_itc_service
    {
        private static ItcProfile itcProfile;
        private static Guid applicantID;

        private static bool AdministrationOrderNotice = false;
        private static decimal AggregatedJudgmentValuewithinLast3Years = 24788;
        private static int ApplicantEmpirica = 608;
        private static bool ConsumerAbsconded = false;
        private static bool ConsumerDeceasedNotification = false;
        private static bool CreditBureauMatch = true;
        private static bool CreditCardRevoked = false;
        private static bool DebtCounsellingNotice = false;
        private static bool DebtReviewNotice = false;
        private static decimal NonSettledAggregatedJudgmentValuewithinLast3Years = 24788;
        private static int NumberofJudgmentswithinLast3Years = 2;
        private static bool PaidOutonDeceasedClaim = false;
        private static bool SequestrationNotice = false;

        private Establish context = () =>
        {
            applicantID = new Guid();

            int? empericaScore = ApplicantEmpirica;
            IEnumerable<ItcJudgement> judgments = new List<ItcJudgement>() { new ItcJudgement(DateTime.Today.AddYears(-2).AddDays(1), 20000), new ItcJudgement(DateTime.Today.AddYears(-2).AddDays(1), 4788) };
            IEnumerable<ItcDefault> defaults = new List<ItcDefault>() { };
            IEnumerable<ItcPaymentProfile> paymentProfiles = new List<ItcPaymentProfile>() { };
            IEnumerable<ItcNotice> notices = new List<ItcNotice>() { };
            ItcDebtCounselling debtCounselling = null;
            bool creditBureauMatch = true;

            itcProfile = new ItcProfile(empericaScore, judgments, defaults, paymentProfiles, notices, debtCounselling, creditBureauMatch);

            var serviceQueryResult = new ServiceQueryResult<CapitecClientCreditBureauAssessment_QueryResult>(new CapitecClientCreditBureauAssessment_QueryResult[]
                {
                    new CapitecClientCreditBureauAssessment_QueryResult()
                    {
                      EligibleBorrower = true
                    }
                });

            decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecClientCreditBureauAssessment_Query>()))
                .Callback<CapitecClientCreditBureauAssessment_Query>(y => { y.Result = serviceQueryResult; });
        };

        private Because of = () =>
        {
            applicantITCService.DoesITCPassQualificationRules(itcProfile, applicantID);
        };

        private It should_return_a_valid_Administration_Order_Notice = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.AdministrationOrderNotice == AdministrationOrderNotice)));
        };

        private It should_return_a_valid_Aggregated_Judgment_Within_Last_3_Years = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.AggregatedJudgmentValuewithinLast3Years == (double)AggregatedJudgmentValuewithinLast3Years)));
        };

        private It should_return_a_valid_Empirca_score = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.ApplicantEmpirica == ApplicantEmpirica)));
        };

        private It should_return_a_valid_Consumer_Absconded = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.ConsumerAbsconded == ConsumerAbsconded)));
        };

        private It should_return_a_valid_Consumer_Deceased_Notification = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.ConsumerDeceasedNotification == ConsumerDeceasedNotification)));
        };

        private It should_return_a_valid_Credit_Bureau_Match = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.CreditBureauMatch == CreditBureauMatch)));
        };

        private It should_return_a_valid_Credit_Card_Revoked = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.CreditCardRevoked == CreditCardRevoked)));
        };

        private It should_return_a_valid_Debt_Counselling_Notice = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.DebtCounsellingNotice == DebtCounsellingNotice)));
        };

        private It should_return_a_valid_Debt_Review_Notice = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.DebtReviewNotice == DebtReviewNotice)));
        };

        private It should_return_a_valid_NonSettled_Aggregated_Judgment_Within_Last_3_Years = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.NonSettledAggregatedJudgmentValuewithinLast3Years == (double)NonSettledAggregatedJudgmentValuewithinLast3Years)));
        };

        private It should_return_a_valid_Number_of_Judgments_Within_Last_3_Years = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.NumberofJudgmentswithinLast3Years == NumberofJudgmentswithinLast3Years)));
        };

        private It should_return_a_valid_Paid_Out_on_Deceased_Claim = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.PaidOutonDeceasedClaim == PaidOutonDeceasedClaim)));
        };

        private It should_return_a_valid_Sequestration_Notice = () =>
        {
            decisionTreeService.WasToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param<CapitecClientCreditBureauAssessment_Query>
              .Matches(y => y.SequestrationNotice == SequestrationNotice)));
        };
    }
}