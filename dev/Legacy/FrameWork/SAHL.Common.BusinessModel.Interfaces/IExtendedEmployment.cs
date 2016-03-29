namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IExtendedEmployment
    {
        double? Allowances { get; set; }

        double? Overtime { get; set; }

        double? Commission { get; set; }

        double? ConfirmedAllowances { get; set; }

        double? ConfirmedCommission { get; set; }

        double? ConfirmedMedicalAid { get; set; }

        double? ConfirmedOvertime { get; set; }

        double? ConfirmedPAYE { get; set; }

        double? ConfirmedPensionProvident { get; set; }

        double? ConfirmedPerformance { get; set; }

        double? ConfirmedShift { get; set; }

        double? ConfirmedUIF { get; set; }

        double? MedicalAid { get; set; }

        double? PAYE { get; set; }

        double? PensionProvident { get; set; }

        double? Performance { get; set; }

        double? Shift { get; set; }

        double? UIF { get; set; }

        /// <summary>
        /// Gets the total monthly income of the individual.  This is a sum of Commission, Overtime,
        /// Shift, Performance and Allowances.
        /// </summary>
        double VariableIncome { get; }

        /// <summary>
        /// Gets the total confirmed income of the individual.  This is the sum of the confirmed
        /// values used for <see cref="VariableIncome"/>.
        /// </summary>
        double ConfirmedVariableIncome { get; }

        /// <summary>
        /// Gets the total deductions for the individual.  This is the sum of PAYE, UIF, Pension/Provident/RA
        /// and Medical Aid.
        /// </summary>
        double Deductions { get; }

        /// <summary>
        /// Gets the total confirmed deductions for the individual.  This is the sum of the confirmed
        /// values used for <see cref="Deductions"/>.
        /// </summary>
        double ConfirmedDeductions { get; }

        /// <summary>
        /// Gets the monthly net income for the individual.  This is the monthly income minus total deductions.
        /// </summary>
        double NetIncome { get; }

        /// <summary>
        /// Gets the confirmed gross income of the individual.  This is the confirmed income
        /// minus total confirmed deductions.
        /// </summary>
        double ConfirmedNetIncome { get; }

        /// <summary>
        /// 
        /// </summary>
        int? SalaryPayDay { get; }
    }
}