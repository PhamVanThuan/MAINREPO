using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO
    /// </summary>
    public partial interface IFinancialServiceAttribute : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.FinancialServiceAttributeType
        /// </summary>
        IFinancialServiceAttributeType FinancialServiceAttributeType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.FinancialAdjustments
        /// </summary>
        IEventList<IFinancialAdjustment> FinancialAdjustments
        {
            get;
        }
    }
}