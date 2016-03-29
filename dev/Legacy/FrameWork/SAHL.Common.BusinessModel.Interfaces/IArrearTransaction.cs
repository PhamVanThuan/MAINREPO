using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO
    /// </summary>
    public partial interface IArrearTransaction : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.InsertDate
        /// </summary>
        System.DateTime InsertDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.EffectiveDate
        /// </summary>
        System.DateTime EffectiveDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.CorrectionDate
        /// </summary>
        DateTime? CorrectionDate
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Balance
        /// </summary>
        System.Double Balance
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Reference
        /// </summary>
        System.String Reference
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.Userid
        /// </summary>
        System.String Userid
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.IsRolledBack
        /// </summary>
        System.Boolean IsRolledBack
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ArrearTransaction_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
        }
    }
}