using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IBond : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        System.DateTime BondRegistrationDate
        {
            get;
        }
    }
}