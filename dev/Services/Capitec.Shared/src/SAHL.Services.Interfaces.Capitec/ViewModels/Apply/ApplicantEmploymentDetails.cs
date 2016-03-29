using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.ViewModels.Apply
{
    public class ApplicantEmploymentDetails
    {
        public ApplicantEmploymentDetails(Guid employmentTypeEnumId, SalariedDetails salariedDetails, SalariedWithCommissionDetails salariedWithCommissionDetails, SalariedWithHousingAllowanceDetails salariedWithHousingAllowanceDetails, SelfEmployedDetails selfEmployedDetails, UnEmployedDetails unEmployedDetails)
        {
            this.EmploymentTypeEnumId = employmentTypeEnumId;
            this.SalariedDetails = salariedDetails;
            this.SalariedWithCommissionDetails = salariedWithCommissionDetails;
            this.SalariedWithHousingAllowanceDetails = salariedWithHousingAllowanceDetails;
            this.SelfEmployedDetails = selfEmployedDetails;
            this.UnEmployedDetails = unEmployedDetails;
        }

        [Required(ErrorMessage = "An employment type is required")]
        public Guid EmploymentTypeEnumId { get; set; }

        public SalariedDetails SalariedDetails { get; set; }

        public SalariedWithCommissionDetails SalariedWithCommissionDetails { get; set; }

        public SalariedWithHousingAllowanceDetails SalariedWithHousingAllowanceDetails { get; set; }

        public SelfEmployedDetails SelfEmployedDetails { get; set; }

        public UnEmployedDetails UnEmployedDetails { get; set; }


        public decimal BasicMonthlyIncome()
        {
            if (this.SalariedDetails != null)
            {
                return this.SalariedDetails.GrossMonthlyIncome;
            }
            else if (this.SalariedWithCommissionDetails != null)
            {
                return this.SalariedWithCommissionDetails.GrossMonthlyIncome;
            }
            else if (this.SalariedWithHousingAllowanceDetails != null)
            {
                return this.SalariedWithHousingAllowanceDetails.GrossMonthlyIncome;
            }
            else if (this.SelfEmployedDetails != null)
            {
                return this.SelfEmployedDetails.GrossMonthlyIncome;
            }
            else if (this.UnEmployedDetails != null)
            {
                return this.UnEmployedDetails.GrossMonthlyIncome;
            }

            return 0;
        }

        public decimal ThreeMonthAverageCommission()
        {
            return (this.SalariedWithCommissionDetails != null)? this.SalariedWithCommissionDetails.ThreeMonthAverageCommission : 0;
        }


        public decimal HousingAllowance()
        {
            return (this.SalariedWithHousingAllowanceDetails != null)? this.SalariedWithHousingAllowanceDetails.HousingAllowance : 0;
        }

        public decimal CalculateGrossMonthlyIncome()
        {
            return BasicMonthlyIncome() + ThreeMonthAverageCommission() + HousingAllowance();
        }
    }
} 
