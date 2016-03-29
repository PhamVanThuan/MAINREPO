using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public interface IAffordabilityModelManager
    {
        List<ApplicantAffordabilityModel> PopulateExpenses(List<ExpenditureItem> comcorpExpenditureItems);

        List<ApplicantAffordabilityModel> PopulateIncomes(List<IncomeItem> comcorpIncomeItems);
    }
}