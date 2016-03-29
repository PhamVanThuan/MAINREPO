using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IEmploymentSubsidised : IEntityValidation, IEmployment
    {
        /// <summary>
        /// Get/sets the subsidy information application to a subsidised employment
        /// </summary>
        ISubsidy Subsidy
        {
            get;
            set;
        }
    }
}