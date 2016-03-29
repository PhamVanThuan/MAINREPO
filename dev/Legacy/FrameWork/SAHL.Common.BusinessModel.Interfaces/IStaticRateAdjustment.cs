using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.StaticRateAdjustment_DAO
    /// </summary>
    public partial interface IStaticRateAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InterestRateAdjustment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InterestRateAdjustment_DAO.Adjustment
        /// </summary>
        System.Double Rate
        {
            get;
        }
    }
}