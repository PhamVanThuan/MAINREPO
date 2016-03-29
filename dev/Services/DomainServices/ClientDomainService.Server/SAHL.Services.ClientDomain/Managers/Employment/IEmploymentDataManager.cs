using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ClientDomain.Managers
{
    public interface IEmploymentDataManager
    {
        int SaveSalaryDeductionEmployment(int clientKey, SalaryDeductionEmploymentModel model);

        int SaveSalariedEmployment(int clientKey, SalariedEmploymentModel salariedEmployment);

        int SaveUnemployedEmployment(int clientKey, UnemployedEmploymentModel unemployedEmployement);

        IEnumerable<EmployerDataModel> FindExistingEmployer(EmployerModel employerModel);

        int SaveEmployer(EmployerModel employerModel);
    }
}