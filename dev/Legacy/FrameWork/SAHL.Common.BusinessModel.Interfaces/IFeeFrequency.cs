using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO
    /// </summary>
    public partial interface IFeeFrequency : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO.Interval
        /// </summary>
        System.Int32 Interval
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FeeFrequency_DAO.FrequencyUnit
        /// </summary>
        IFrequencyUnit FrequencyUnit
        {
            get;
        }
    }
}