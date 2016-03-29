using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Reset_DAO
    /// </summary>
    public partial interface IReset : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Reset_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Reset_DAO.ResetDate
        /// </summary>
        System.DateTime ResetDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Reset_DAO.RunDate
        /// </summary>
        System.DateTime RunDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Reset_DAO.JIBARRate
        /// </summary>
        System.Double JIBARRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Reset_DAO.JIBARDiscountRate
        /// </summary>
        System.Double JIBARDiscountRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Reset_DAO.ResetConfiguration
        /// </summary>
        IResetConfiguration ResetConfiguration
        {
            get;
            set;
        }
    }
}