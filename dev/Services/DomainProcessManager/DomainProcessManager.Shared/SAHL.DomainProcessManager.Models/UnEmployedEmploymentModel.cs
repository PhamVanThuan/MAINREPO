using System;
using System.Runtime.Serialization;

using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class UnemployedEmploymentModel : EmploymentModel
    {
        public UnemployedEmploymentModel(double basicIncome, int salaryPaymentDay, EmployerModel employer, DateTime? startDate,
                                              Core.BusinessModel.Enums.UnemployedRemunerationType remunerationType, EmploymentStatus employmentStatus)
            : base(employer, startDate, EmploymentType.Unemployed, employmentStatus, (RemunerationType)remunerationType, basicIncome, salaryPaymentDay)
        {
            this.UnemployedRemunerationType = remunerationType;
        }

        [DataMember]
        public UnemployedRemunerationType UnemployedRemunerationType { get; set; }
    }
}
