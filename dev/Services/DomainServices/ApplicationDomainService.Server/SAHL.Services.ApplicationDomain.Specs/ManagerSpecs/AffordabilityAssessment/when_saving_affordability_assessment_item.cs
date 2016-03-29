using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_saving_affordability_assessment_item : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static int affordabilityAssessmentKey;
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static int userKey;

        private Establish context = () =>
        {
            affordabilityAssessmentKey = 3938;
            userKey = 99;

            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();

            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);
        };

        private Because of = () =>
        {
            affordabilityAssessmentManager.CreateBlankAffordabilityAssessmentItems(affordabilityAssessmentKey, userKey);
        };

        private It should_tell_the_data_manager_to_save_items_in_the_Income_Category = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Commission_Overtime)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.NetRental)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Investments)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.OtherIncome1)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.OtherIncome2)));
        };

        private It should_tell_the_data_manager_to_save_items_in_the_Income_Deductions_Category = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.PayrollDeductions)));
        };

        private It should_tell_the_data_manager_to_save_items_in_the_NecessaryExpenses_Category = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Accommodationexp_Rental)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Transport)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Food)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Education)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Medical)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Utilities)));
            affordabilityAssessmentDataManager.WasToldTo(
                 x => x.InsertAffordabilityAssessmentItem(
                     Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.ChildSupport)));
        };

        private It should_tell_the_data_manager_to_save_items_in_the_OtherExpenses_Category = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.DomesticSalary)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.InsurancePolicy_ies)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Security)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Telephone_TV)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Other)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.CommittedSavings)));
        };

        private It should_tell_the_data_manager_to_save_items_in_the_PaymentObligations_Category = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.OtherBond_s)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.Vehicle)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.CreditCard_s)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.PersonalLoan_s)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.RetailAccounts)));
            affordabilityAssessmentDataManager.WasToldTo(
                x => x.InsertAffordabilityAssessmentItem(
                    Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.OtherDebtExpenses)));
        };

        private It should_tell_the_data_manager_to_save_items_in_the_SAHLPaymentObligations_Category = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(
               x => x.InsertAffordabilityAssessmentItem(
                   Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.SAHLBond)));
            affordabilityAssessmentDataManager.WasToldTo(
               x => x.InsertAffordabilityAssessmentItem(
                   Arg.Is<AffordabilityAssessmentItemDataModel>(y => y.AffordabilityAssessmentItemTypeKey == (int)AffordabilityAssessmentItemType.HOC)));
        };
    }
}