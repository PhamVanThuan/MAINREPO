using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicantEmploymentDataModel :  IDataModel
    {
        public ApplicantEmploymentDataModel(Guid applicantId, Guid employmentTypeEnumId, decimal? basicIncome, decimal? threeMonthAverageCommission, decimal? housingAllowance)
        {
            this.ApplicantId = applicantId;
            this.EmploymentTypeEnumId = employmentTypeEnumId;
            this.BasicIncome = basicIncome;
            this.ThreeMonthAverageCommission = threeMonthAverageCommission;
            this.HousingAllowance = housingAllowance;
		
        }

        public ApplicantEmploymentDataModel(Guid id, Guid applicantId, Guid employmentTypeEnumId, decimal? basicIncome, decimal? threeMonthAverageCommission, decimal? housingAllowance)
        {
            this.Id = id;
            this.ApplicantId = applicantId;
            this.EmploymentTypeEnumId = employmentTypeEnumId;
            this.BasicIncome = basicIncome;
            this.ThreeMonthAverageCommission = threeMonthAverageCommission;
            this.HousingAllowance = housingAllowance;
		
        }		

        public Guid Id { get; set; }

        public Guid ApplicantId { get; set; }

        public Guid EmploymentTypeEnumId { get; set; }

        public decimal? BasicIncome { get; set; }

        public decimal? ThreeMonthAverageCommission { get; set; }

        public decimal? HousingAllowance { get; set; }
    }
}