using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class AffordabilityModelManager : IAffordabilityModelManager
    {
        private IValidationUtils validationUtils;

        public AffordabilityModelManager(IValidationUtils validationUtils)
        {
            this.validationUtils = validationUtils;
        }

        public List<ApplicantAffordabilityModel> PopulateIncomes(List<IncomeItem> comcorpIncomeItems)
        {
            var incomes = new List<ApplicantAffordabilityModel>();
            foreach (var incomeItem in comcorpIncomeItems)
            {
                AffordabilityType affordabilityType = validationUtils.ParseEnum<AffordabilityType>(incomeItem.IncomeDesc);

                if (incomeItem.IncomeAmount > 0)
                {
                    string description = incomeItem.CapturedDescription;

                    bool affordabilityTypeRequiresDescription = validationUtils.CheckIfAffordabilityRequiresDescription(affordabilityType);

                    if (String.IsNullOrWhiteSpace(description))
                    {
                        if (affordabilityTypeRequiresDescription)
                        {
                            description = "Unknown";
                        }
                        else
                        {
                            description = null;
                        }
                    }

                    ApplicantAffordabilityModel affordabilityModel = new ApplicantAffordabilityModel(affordabilityType, description, incomeItem.IncomeAmount);

                    incomes.Add(affordabilityModel);
                }
            }
            return incomes;
        }

        public List<ApplicantAffordabilityModel> PopulateExpenses(List<ExpenditureItem> comcorpExpenditureItems)
        {
            var expenses = new List<ApplicantAffordabilityModel>();

            foreach (var expenseItem in comcorpExpenditureItems)
            {
                AffordabilityType affordabilityType = validationUtils.ParseEnum<AffordabilityType>(expenseItem.ExpenditureDesc);

                if (expenseItem.ExpenditureAmount > 0)
                {
                    string description = expenseItem.CapturedDescription;

                    bool affordabilityTypeRequiresDescription = validationUtils.CheckIfAffordabilityRequiresDescription(affordabilityType);

                    if (String.IsNullOrWhiteSpace(description))
                    {
                        if (affordabilityTypeRequiresDescription)
                        {
                            description = "Unknown";
                        }
                        else
                        {
                            description = null;
                        }
                    }

                    ApplicantAffordabilityModel affordabilityModel = new ApplicantAffordabilityModel(affordabilityType, description, expenseItem.ExpenditureAmount);

                    expenses.Add(affordabilityModel);
                }
            }
            return expenses;
        }
    }
}