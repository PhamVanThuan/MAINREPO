using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ACBBranch_DAO
    /// </summary>
    public partial interface IACBBranch : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The primary key from the ACBBank table to which the branch belongs.
        /// </summary>
        IACBBank ACBBank
        {
            get;
            set;
        }

        /// <summary>
        /// The description of the branch. e.g. Durban North
        /// </summary>
        System.String ACBBranchDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether this branch record is active or not.
        /// </summary>
        System.Char ActiveIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// The distinct branch code which is allocated to each branch of a bank.
        /// </summary>
        System.String Key
        {
            get;
            set;
        }
    }
}