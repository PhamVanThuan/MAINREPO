using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class SalariedEmploymentModel : EmploymentModel
    {
        [JsonProperty]
        private IEnumerable<EmployeeDeductionModel> employeeDeductions;

        public SalariedEmploymentModel(double basicIncome, int salaryPaymentDay, EmployerModel employer, DateTime? startDate,
            SalariedRemunerationType salariedRemunerationType,
            EmploymentStatus employmentStatus, IEnumerable<EmployeeDeductionModel> employeeDeductions)
            : base(
                employer,
                startDate,
                EmploymentType.Salaried,
                employmentStatus,
                (RemunerationType)salariedRemunerationType,
                basicIncome,
                salaryPaymentDay)
        {
            this.SalariedRemunerationType = salariedRemunerationType;
            this.EmployeeDeductions = employeeDeductions;
        }

        [DataMember]
        public SalariedRemunerationType SalariedRemunerationType { get; set; }

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
