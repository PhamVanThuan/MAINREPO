using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO
    /// </summary>
    public partial class AuditFinancialTranPosted : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO>, IAuditFinancialTranPosted
    {
        public AuditFinancialTranPosted(SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO AuditFinancialTranPosted)
            : base(AuditFinancialTranPosted)
        {
            this._DAO = AuditFinancialTranPosted;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.LoanNumber
        /// </summary>
        public Int32 LoanNumber
        {
            get { return _DAO.LoanNumber; }
            set { _DAO.LoanNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionTypeNumber
        /// </summary>
        public Int16 TransactionTypeNumber
        {
            get { return _DAO.TransactionTypeNumber; }
            set { _DAO.TransactionTypeNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionEffectiveDate
        /// </summary>
        public DateTime TransactionEffectiveDate
        {
            get { return _DAO.TransactionEffectiveDate; }
            set { _DAO.TransactionEffectiveDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionInsertDate
        /// </summary>
        public DateTime TransactionInsertDate
        {
            get { return _DAO.TransactionInsertDate; }
            set { _DAO.TransactionInsertDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionAmount
        /// </summary>
        public Double TransactionAmount
        {
            get { return _DAO.TransactionAmount; }
            set { _DAO.TransactionAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionReference
        /// </summary>
        public String TransactionReference
        {
            get { return _DAO.TransactionReference; }
            set { _DAO.TransactionReference = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.Key
        /// </summary>
        public Decimal Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}