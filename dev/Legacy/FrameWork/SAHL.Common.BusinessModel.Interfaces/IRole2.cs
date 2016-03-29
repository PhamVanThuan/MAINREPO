using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IRole : IEntityValidation
    {
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IRoleType RoleType
        {
            get;
            set;
        }

        /// <summary>
        /// A property that shows if an active Debt Counselling case exists
        /// for this role (Account and LegalEntity);
        /// </summary>
        bool UnderDebtCounselling(bool activeOnly);
    }
}