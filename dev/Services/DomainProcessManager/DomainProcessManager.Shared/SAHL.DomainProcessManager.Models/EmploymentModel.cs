using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class EmploymentModel : IDataModel
    {
        public EmploymentModel(EmployerModel employer, DateTime? startDate, EmploymentType employmentType, EmploymentStatus employmentStatus,
                               RemunerationType remunerationType, double basicIncome, int salaryPaymentDay)
        {
            this.Employer = employer;
            this.StartDate = startDate;
            this.EmploymentType = employmentType;
            this.EmploymentStatus = employmentStatus;
            this.RemunerationType = remunerationType;
            this.SalaryPaymentDay = salaryPaymentDay;
            this.BasicIncome = basicIncome;
        }

        [DataMember]
        public EmployerModel Employer { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public EmploymentType EmploymentType { get; set; }

        [DataMember]
        public EmploymentStatus EmploymentStatus { get; set; }

        [DataMember]
        public RemunerationType RemunerationType { get; set; }

        [DataMember]
        public double BasicIncome { get; set; }

        [DataMember]
        public int SalaryPaymentDay { get; set; }
    }
}