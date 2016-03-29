using SAHL.Common.BusinessModel.Interfaces;
using SAHL.V3.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Repositories
{
    public interface IDecisionTreeRepository : IV3Service
    {
        QualifyApplicationFor30YearLoanTermResult QualifyApplication(int applicationKey);

        decimal DetermineNCRGuidelineMinMonthlyFixedExpenses(decimal grossMonthlyIncome);
    }
}
