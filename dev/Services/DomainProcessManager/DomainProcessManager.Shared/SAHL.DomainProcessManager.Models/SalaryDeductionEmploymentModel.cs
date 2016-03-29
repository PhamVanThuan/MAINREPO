using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class SalaryDeductionEmploymentModel : EmploymentModel
    {
        [JsonProperty]
        private IEnumerable<EmployeeDeductionModel> employeeDeductions;

        public SalaryDeductionEmploymentModel(double basicIncome, double housingAllowance, int salaryPaymentDay, EmployerModel employer,
            DateTime? startDate,
            SalaryDeductionRemunerationType salaryDeductionRemunerationType, EmploymentStatus employmentStatus,
            IEnumerable<EmployeeDeductionModel> employeeDeductions)
            : base(employer,
                startDate,
                EmploymentType.SalariedwithDeduction,
                employmentStatus,
                (RemunerationType)salaryDeductionRemunerationType,
                basicIncome,
                salaryPaymentDay)
        {
            this.HousingAllowance = housingAllowance;
            this.SalaryDeductionRemunerationType = salaryDeductionRemunerationType;
            this.EmployeeDeductions = employeeDeductions;
        }

        [DataMember]
        public double HousingAllowance { get; set; }

        [DataMember]
        public SalaryDeductionRemunerationType SalaryDeductionRemunerationType { get; set; }

        [DataMember]
        public IEnumerable<EmployeeDeductionModel> EmployeeDeductions
        {
            get { return employeeDeductions; }
            set
            {
                if (value != null)
                {
                    employeeDeductions = new List<EmployeeDeductionModel>(value);
                }
            }
        }
    }
}
