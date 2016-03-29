using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CAP_DAO
    /// </summary>
    public partial interface ICAP : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.TradeKey
        /// </summary>
        System.Int32 TradeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CancellationDate
        /// </summary>
        System.DateTime? CancellationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CancellationReasonKey
        /// </summary>
        System.Int32? CancellationReasonKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CAPBalance
        /// </summary>
        System.Double CAPBalance
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CAPPrePaymentAmount
        /// </summary>
        System.Double CAPPrePaymentAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.MTDCAPPrePaymentAmount
        /// </summary>
        System.Double MTDCAPPrePaymentAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.Invoked
        /// </summary>
        System.Boolean Invoked
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CapOfferDetail
        /// </summary>
        ICapApplicationDetail CapOfferDetail
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CAPPaymentOption
        /// </summary>
        ICapPaymentOption CAPPaymentOption
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.FinancialServiceAttribute
        /// </summary>
        IFinancialServiceAttribute FinancialServiceAttribute
        {
            get;
            set;
        }
    }
}