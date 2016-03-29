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
    public class when_populating_expenses_for_applicant_affordability : WithCoreFakes
    {
        private static AffordabilityModelManager modelManager;
        private static List<ExpenditureItem> comcorpExpenses;
        private static List<ExpenditureItem> expenseItems;
        private static List<ApplicantAffordabilityModel> expenses;
        private static ValidationUtils _validationUtils;

        private Establish context = () =>
        {
            _validationUtils = new ValidationUtils();
            modelManager = new AffordabilityModelManager(_validationUtils);
            expenseItems = IntegrationServiceTestHelper.PopulateExpenditureItems();
            comcorpExpenses = (from i in expenseItems where !i.ExpenditureDesc.Contains("Other") select i)
                .ToList();
        };

        private Because of = () =>
        {
            expenses = modelManager.PopulateExpenses(expenseItems)
                .Where(x => x.AffordabilityType != AffordabilityType.Other && x.AffordabilityType != AffordabilityType.Otherdebtrepayment
                && x.AffordabilityType != AffordabilityType.OtherInstalments).ToList();
        };

        private It should_map_all_of_expenses = () =>
        {
            foreach (var exp in comcorpExpenses)
            {
                string sanitizedExpense = exp.ExpenditureDesc.Trim().Replace(" ", string.Empty);
                foreach (var character in exp.ExpenditureDesc)
                {
                    if (!char.IsLetterOrDigit(character))
                    {
                        sanitizedExpense = sanitizedExpense.Replace(character.ToString(), "_");
                    }
                }
                expenses.Where(x => x.AffordabilityType == (AffordabilityType)Enum.Parse(typeof(AffordabilityType), sanitizedExpense, true) && x.Amount > 0)
                    .First().ShouldNotBeNull();
            }
        };
    }
}