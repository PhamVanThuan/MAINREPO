using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DisbursementType_DAO
    /// </summary>
    public partial interface IDisbursementType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisbursementType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisbursementType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}