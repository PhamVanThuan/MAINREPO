using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public class EmployeeDeductionModel : ValidatableModel
    {
        public EmployeeDeductionModel(EmployeeDeductionTypeEnum type, double value)
        {
            this.Type = type;
            this.Value = value;
            Validate();
        }

        public EmployeeDeductionTypeEnum Type { get; protected set; }

        [Required]
        public double Value { get; protected set; }
    }
}