using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Managers.Employment
{
    public interface IEmploymentDataManager
    {
        void AddApplicantEmployment(Guid applicantID, Guid employmentTypeEnumId, decimal basicMonthlyIncome, decimal threeMonthAverageCommission, decimal housingAllowance);
    }
}
