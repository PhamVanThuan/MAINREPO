using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_updating_an_affordability_assessment_items : WithFakes
    {
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static AffordabilityAssessmentModel affordabilityAssessment;
        private static AffordabilityAssessmentDataModel affordabilityAssessmentDataModel;

        private static AffordabilityAssessmentDetailModel affordabilityAssessmentDetail;
        private static AffordabilityAssessmentIncomeDetailModel income;
        private static AffordabilityAssessmentIncomeDeductionsDetailModel incomeDeductions;
        private static AffordabilityAssessmentNecessaryExpensesDetailModel necessaryExpenses;
        private static AffordabilityAssessmentPaymentObligationDetailModel paymentObligations;
        private static AffordabilityAssessmentSAHLPaymentObligationDetailModel _SAHLPaymentObligations;
        private static AffordabilityAssessmentOtherExpensesDetailModel otherExpenses;

        private Establish context = () =>
        {
            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();
            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);

            affordabilityAssessmentDataModel = new AffordabilityAssessmentDataModel(0, 0, 0, 0, 0, 0, DateTime.Now, 0, 0, 0, null, null, null);
            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentByKey(Param.IsAny<int>())).Return(affordabilityAssessmentDataModel);
            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentContributors(Param.IsAny<int>())).Return(new List<int>() { 2, 3, 4 });

            income = new AffordabilityAssessmentIncomeDetailModel();
            incomeDeductions = new AffordabilityAssessmentIncomeDeductionsDetailModel();
            necessaryExpenses = new AffordabilityAssessmentNecessaryExpensesDetailModel();
            paymentObligations = new AffordabilityAssessmentPaymentObligationDetailModel();
            _SAHLPaymentObligations = new AffordabilityAssessmentSAHLPaymentObligationDetailModel();
            otherExpenses = new AffordabilityAssessmentOtherExpensesDetailModel();
            affordabilityAssessmentDetail = new AffordabilityAssessmentDetailModel(income, incomeDeductions, necessaryExpenses, paymentObligations, _SAHLPaymentObligations, otherExpenses, "0%", 0, 0);
            affordabilityAssessment = new AffordabilityAssessmentModel(0, 0, AffordabilityAssessmentStatus.Unconfirmed, DateTime.Now, 0, 0, 0, new List<int>(), affordabilityAssessmentDetail, null);
        };

        private Because of = () =>
        {
            affordabilityAssessmentManager.UpdateAffordabilityAssessmentAndAffordabilityAssessmentItems(affordabilityAssessment, 0);
        };

        private It should_update_the_assessment = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.UpdateAffordabilityAssessment(Param.IsAny<AffordabilityAssessmentDataModel>()));
        };

        private It should_update_items = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.UpdateAffordabilityAssessmentItem(Param.IsAny<AffordabilityAssessmentItemDataModel>()));
        };
    }
}