using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO
    /// </summary>
    public partial interface IAuditFinancialTranPosted : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.LoanNumber
        /// </summary>
        System.Int32 LoanNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionTypeNumber
        /// </summary>
        System.Int16 TransactionTypeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionEffectiveDate
        /// </summary>
        System.DateTime TransactionEffectiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionInsertDate
        /// </summary>
        System.DateTime TransactionInsertDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionAmount
        /// </summary>
        System.Double TransactionAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.TransactionReference
        /// </summary>
        System.String TransactionReference
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AuditFinancialTranPosted_DAO.Key
        /// </summary>
        System.Decimal Key
        {
            get;
            set;
        }
    }
}