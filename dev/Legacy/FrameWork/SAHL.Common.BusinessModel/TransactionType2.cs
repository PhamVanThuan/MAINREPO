using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord.Queries;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class TransactionType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TransactionType_DAO>, ITransactionType
	{
        public int ScreenBatch
        {
            get 
            { 
                string HQL = "from TransactionTypeUI_DAO tt where tt.TransactionType.Key = ?";
                SimpleQuery<TransactionTypeUI_DAO> q = new SimpleQuery<TransactionTypeUI_DAO>(HQL, this.Key);
                TransactionTypeUI_DAO[] transactionTypeUI = q.Execute();

                if (transactionTypeUI.Length == 0)
                    return 0;
                else
                    return transactionTypeUI[0].ScreenBatch;
            }
        }

        public string HTMLColour
        {
            get
            {
                string HQL = "from TransactionTypeUI_DAO tt where tt.TransactionType.Key = ?";
                SimpleQuery<TransactionTypeUI_DAO> q = new SimpleQuery<TransactionTypeUI_DAO>(HQL, this.Key);
                TransactionTypeUI_DAO[] transactionTypeUI = q.Execute();

                if (transactionTypeUI.Length == 0)
                    return null;
                else
                    return transactionTypeUI[0].HTMLColour;
            }
        }

	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnTransactionTypeDataAccess_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"><see cref="ICancelDomainArgs"/></param>
		/// <param name="Item"></param>
		protected void OnTransactionTypeDataAccess_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnTransactionTypeDataAccess_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnTransactionTypeDataAccess_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnTransactionGroups_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnTransactionGroups_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnTransactionGroups_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnTransactionGroups_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
    }
}


