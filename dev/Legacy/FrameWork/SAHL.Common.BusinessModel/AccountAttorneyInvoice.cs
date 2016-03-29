using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO
    /// </summary>
    public partial class AccountAttorneyInvoice : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO>, IAccountAttorneyInvoice
    {
        public AccountAttorneyInvoice(SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO AccountAttorneyInvoice)
            : base(AccountAttorneyInvoice)
        {
            this._DAO = AccountAttorneyInvoice;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.AccountKey
        /// </summary>
        public Int32 AccountKey
        {
            get { return _DAO.AccountKey; }
            set { _DAO.AccountKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.AttorneyKey
        /// </summary>
        public Int32 AttorneyKey
        {
            get { return _DAO.AttorneyKey; }
            set { _DAO.AttorneyKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.InvoiceNumber
        /// </summary>
        public String InvoiceNumber
        {
            get { return _DAO.InvoiceNumber; }
            set { _DAO.InvoiceNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.InvoiceDate
        /// </summary>
        public DateTime InvoiceDate
        {
            get { return _DAO.InvoiceDate; }
            set { _DAO.InvoiceDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.Amount
        /// </summary>
        public Decimal Amount
        {
            get { return _DAO.Amount; }
            set { _DAO.Amount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.VatAmount
        /// </summary>
        public Decimal VatAmount
        {
            get { return _DAO.VatAmount; }
            set { _DAO.VatAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.TotalAmount
        /// </summary>
        public Decimal TotalAmount
        {
            get { return _DAO.TotalAmount; }
            set { _DAO.TotalAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.Comment
        /// </summary>
        public String Comment
        {
            get { return _DAO.Comment; }
            set { _DAO.Comment = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO.ChangeDate
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }
    }
}