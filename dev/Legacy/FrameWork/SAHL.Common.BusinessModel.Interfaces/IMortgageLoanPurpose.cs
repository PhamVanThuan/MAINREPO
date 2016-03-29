using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO
    /// </summary>
    public partial interface IMortgageLoanPurpose : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO.MortgageLoanPurposeGroup
        /// </summary>
        IMortgageLoanPurposeGroup MortgageLoanPurposeGroup
        {
            get;
            set;
        }
    }
}