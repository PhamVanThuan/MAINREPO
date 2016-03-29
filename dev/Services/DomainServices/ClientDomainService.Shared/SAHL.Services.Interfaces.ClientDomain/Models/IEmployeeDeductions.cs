using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public interface IEmployeeDeductions
    {
        IEnumerable<EmployeeDeductionModel> EmployeeDeductions { get; }
    }
}