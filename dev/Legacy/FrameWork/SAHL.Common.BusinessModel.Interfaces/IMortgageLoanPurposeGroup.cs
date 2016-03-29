using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO
    /// </summary>
    public partial interface IMortgageLoanPurposeGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO.MortgageLoanPurposes
        /// </summary>
        IEventList<IMortgageLoanPurpose> MortgageLoanPurposes
        {
            get;
        }
    }
}