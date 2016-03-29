using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElementType_DAO
    /// </summary>
    public partial interface IRateAdjustmentElementType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElementType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElementType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElementType_DAO.StatementName
        /// </summary>
        System.String StatementName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RateAdjustmentElementType_DAO.RateAdjustmentElements
        /// </summary>
        IEventList<IRateAdjustmentElement> RateAdjustmentElements
        {
            get;
        }
    }
}