using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IEmployment : IEntityValidation
    {
        /// <summary>
        /// Gets/sets the confirmed basic income amount.
        /// </summary>
        double? ConfirmedBasicIncome { get; set; }

        /// <summary>
        /// Gets the employment type (this will correspond with the discriminated type of the object).
        /// </summary>
        IEmploymentType EmploymentType { get; }

        /// <summary>
        /// Gets extended employment details.  This will return null unless the remuneration type is Salaried
        /// or BasicAndCommission.
        /// </summary>
        IExtendedEmployment ExtendedEmployment { get; }

        /// <summary>
        /// Gets/sets the basic income. 
        /// </summary>
        double? BasicIncome { get; set; }

        /// <summary>
        /// Gets the remuneration type.
        /// </summary>
        IRemunerationType RemunerationType { get; set; }

        /// <summary>
        /// Gets whether the employment has been confirmed or not.
        /// </summary>
        bool IsConfirmed { get; }

        /// <summary>
        /// Used to determine if the employment object requires extended employment information.
        /// </summary>
        bool RequiresExtended { get; }

        IEmploymentStatus EmploymentStatus { get; set; }

        /// <summary>
        /// Used to determine if the confirmed income values have changed since the employment record was loaded.
        /// </summary>
        bool ConfirmedIncomeChanged { get; }

        bool ContactPersonChanged { get; }

        bool ContactPhoneNumberChanged { get; }

        bool ContactPhoneCodeChanged { get; }

        bool DepartmentChanged { get; }

        /// <summary>
        /// Returns the enumeration types supported by the employment object.
        /// </summary>
        IReadOnlyEventList<RemunerationTypes> SupportedRemunerationTypes { get; }

        /// <summary>
        /// Gets the total unconfirmed monthly income for the employment object (this will take extended employment
        /// information into account).
        /// </summary>
        double MonthlyIncome { get; }

        /// <summary>
        /// Gets the total confirmed income for the employment object (this will take extended employment
        /// information into account).
        /// </summary>
        double ConfirmedIncome { get; }

        /// <summary>
        ///
        /// </summary>
        IEmploymentConfirmationSource EmploymentConfirmationSource
        {
            get;
            set;
        }
    }
}