using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_asked_to_get_affordability_assessment : WithFakes
    {
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static AffordabilityAssessmentModel result;
        private static List<AffordabilityAssessmentItemDataModel> items;

        private static int aaKey = 1;

        private Establish context = () =>
        {
            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();
            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);

            var summary = new AffordabilityAssessmentSummaryModel(aaKey, 1, "client detail - bob smith", 1, "Unconfirmed", "Bob", DateTime.Now, 1, 1, 2, "0.5", 0.04, 2500, null,1);

            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentSummary(summary.Key)).Return(summary);

            var basicGrossSalary_Drawings = new AffordabilityAssessmentItemDataModel(1, 
                                                                                    summary.Key, 
                                                                                    (int)AffordabilityAssessmentItemType.BasicGrossSalary_Drawings, 
                                                                                    DateTime.Now, 
                                                                                    1, 
                                                                                    1111, 
                                                                                    2222, 
                                                                                    null, 
                                                                                    "test basicGrossSalary_Drawings");
            var transport = new AffordabilityAssessmentItemDataModel(2, summary.Key, (int)AffordabilityAssessmentItemType.Transport, DateTime.Now, 1, 3333, 4444, null, "test transport");
            var security = new AffordabilityAssessmentItemDataModel(3, summary.Key, (int)AffordabilityAssessmentItemType.Security, DateTime.Now, 1, 5555, 6666, null, "test security");
            var vehicle = new AffordabilityAssessmentItemDataModel(4, summary.Key, (int)AffordabilityAssessmentItemType.Vehicle, DateTime.Now, 1, 7777, 8888, 12345, "test vehicle");
            var sahlBond = new AffordabilityAssessmentItemDataModel(5, summary.Key, (int)AffordabilityAssessmentItemType.SAHLBond, DateTime.Now, 1, 9999, 0000, null, "test vehicle");
            items = new List<AffordabilityAssessmentItemDataModel>() { basicGrossSalary_Drawings, transport, security, vehicle, sahlBond };

            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentItems(summary.Key)).Return(items);

            var contributors = new List<int>();
            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentContributors(summary.Key)).Return(contributors);
        };

        private Because of = () =>
        {
            result = affordabilityAssessmentManager.GetAffordabilityAssessment(aaKey);
        };

        private It should_map_income = () =>
        {
            var detail = items.Where(x => x.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.BasicGrossSalary_Drawings).FirstOrDefault();
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ClientValue = detail.ClientValue;
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.CreditValue = detail.CreditValue;
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ItemNotes = detail.ItemNotes;
        };

        private It should_map_necessary_expenses = () =>
        {
            var detail = items.Where(x => x.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Transport).FirstOrDefault();
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ClientValue = detail.ClientValue;
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.CreditValue = detail.CreditValue;
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ItemNotes = detail.ItemNotes;
        };

        private It should_map_other_expenses = () =>
        {
            var detail = items.Where(x => x.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Security).FirstOrDefault();
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ClientValue = detail.ClientValue;
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.CreditValue = detail.CreditValue;
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ItemNotes = detail.ItemNotes;
        };

        private It should_map_sahl_payment_obligations = () =>
        {
            var detail = items.Where(x => x.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.SAHLBond).FirstOrDefault();
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ClientValue = detail.ClientValue;
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.CreditValue = detail.CreditValue;
            result.AffordabilityAssessmentDetail.Income.BasicGrossSalary_Drawings.ItemNotes = detail.ItemNotes;
        };

        private It should_map_payment_obligations = () =>
        {
            var detail = items.Where(x => x.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Vehicle).FirstOrDefault();
            result.AffordabilityAssessmentDetail.PaymentObligations.Vehicle.ClientValue = detail.ClientValue;
            result.AffordabilityAssessmentDetail.PaymentObligations.Vehicle.CreditValue = detail.CreditValue;
            result.AffordabilityAssessmentDetail.PaymentObligations.Vehicle.ConsolidationValue = detail.DebtToConsolidateValue;
            result.AffordabilityAssessmentDetail.PaymentObligations.Vehicle.ItemNotes = detail.ItemNotes;
        };
    }
}