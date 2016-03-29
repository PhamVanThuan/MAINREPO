using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO
    /// </summary>
    public partial interface IAccountAttorneyInvoice : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.AccountKey
        /// </summary>
        System.Int32 AccountKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.AttorneyKey
        /// </summary>
        System.Int32 AttorneyKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.InvoiceNumber
        /// </summary>
        System.String InvoiceNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.InvoiceDate
        /// </summary>
        System.DateTime InvoiceDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.Amount
        /// </summary>
        System.Decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.VatAmount
        /// </summary>
        System.Decimal VatAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.TotalAmount
        /// </summary>
        System.Decimal TotalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.Comment
        /// </summary>
        System.String Comment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }
    }
}