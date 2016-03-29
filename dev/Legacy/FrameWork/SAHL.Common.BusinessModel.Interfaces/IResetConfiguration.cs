using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO
    /// </summary>
    public partial interface IResetConfiguration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.IntervalType
        /// </summary>
        System.String IntervalType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.IntervalDuration
        /// </summary>
        System.Int32 IntervalDuration
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.ResetDate
        /// </summary>
        System.DateTime ResetDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.ActionDate
        /// </summary>
        System.DateTime ActionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.BusinessDayIndicator
        /// </summary>
        System.Char BusinessDayIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.FinancialServiceTypes
        /// </summary>
        IEventList<IFinancialServiceType> FinancialServiceTypes
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ResetConfiguration_DAO.OriginationSourceProductConfigurations
        /// </summary>
        IEventList<IOriginationSourceProductConfiguration> OriginationSourceProductConfigurations
        {
            get;
        }
    }
}