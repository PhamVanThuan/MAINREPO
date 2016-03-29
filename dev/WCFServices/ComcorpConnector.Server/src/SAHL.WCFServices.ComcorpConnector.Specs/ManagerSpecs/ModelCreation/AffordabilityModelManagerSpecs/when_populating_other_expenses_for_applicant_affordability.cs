using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AffordabilityModelManagerSpecs
{
    public class when_populating_other_expenses_for_applicant_affordability : WithCoreFakes
    {
        private static AffordabilityModelManager modelManager;
        private static List<ExpenditureItem> comcorpOtherExpenses;
        private static List<ExpenditureItem> expenseItems;
        private static List<ApplicantAffordabilityModel> otherExpenses;
        private static ValidationUtils _validationUtils;

        private Establish context = () =>
        {
            _validationUtils = new ValidationUtils();
            modelManager = new AffordabilityModelManager(_validationUtils);
            expenseItems = IntegrationServiceTestHelper.PopulateExpenditureItems();
            comcorpOtherExpenses = (from i in expenseItems where i.ExpenditureDesc.Contains("Other") select i)
                .ToList();
        };

        private Because of = () =>
        {
            otherExpenses = modelManager.PopulateExpenses(expenseItems)
                .Where(x => x.AffordabilityType == AffordabilityType.Other || x.AffordabilityType == AffordabilityType.Otherdebtrepayment
                || x.AffordabilityType == AffordabilityType.OtherInstalments).ToList();
        };

        private It should_map_both_of_the_other_incomes = () =>
        {
            otherExpenses.Count.ShouldEqual(3);
        };

        private It should_map_the_other_expense_amount = () =>
        {
            otherExpenses.Where(x => x.AffordabilityType == AffordabilityType.Other).First().Amount
                .ShouldEqual(comcorpOtherExpenses.Where(x => x.ExpenditureDesc.Equals("Other")).First().ExpenditureAmount);
        };

        private It should_map_the_other_debt_repayment_amount = () =>
        {
            otherExpenses.Where(x => x.AffordabilityType == AffordabilityType.Otherdebtrepayment).First().Amount
                .ShouldEqual(comcorpOtherExpenses.Where(x => x.ExpenditureDesc.Equals("Other Debt Repayment")).First().ExpenditureAmount);
        };

        private It should_map_the_other_instalments_amount = () =>
        {
            otherExpenses.Where(x => x.AffordabilityType == AffordabilityType.OtherInstalments).First().Amount
                .ShouldEqual(comcorpOtherExpenses.Where(x => x.ExpenditureDesc.Equals("Other Instalments")).First().ExpenditureAmount);
        };

        private It should_map_the_other_debt_repayment_description = () =>
        {
            otherExpenses.Where(x => x.AffordabilityType == AffordabilityType.Otherdebtrepayment).First().Description
                .ShouldEqual(comcorpOtherExpenses.Where(x => x.ExpenditureDesc.Equals("Other Debt Repayment")).First().CapturedDescription);
        };

        private It should_map_the_other_expense_description = () =>
        {
            otherExpenses.Where(x => x.AffordabilityType == AffordabilityType.Other).First().Description
                .ShouldEqual(comcorpOtherExpenses.Where(x => x.ExpenditureDesc.Equals("Other")).First().CapturedDescription);
        };

        private It should_map_the_other_instalments_description = () =>
        {
            otherExpenses.Where(x => x.AffordabilityType == AffordabilityType.OtherInstalments).First().Description
                .ShouldEqual(comcorpOtherExpenses.Where(x => x.ExpenditureDesc.Equals("Other Instalments")).First().CapturedDescription);
        };
    }
}