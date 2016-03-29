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
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO
	/// </summary>
	public partial class TransactionEffect : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO>, ITransactionEffect
	{
				public TransactionEffect(SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO TransactionEffect) : base(TransactionEffect)
		{
			this._DAO = TransactionEffect;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO.Coefficient
		/// </summary>
		public Double Coefficient 
		{
			get { return _DAO.Coefficient; }
			set { _DAO.Coefficient = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO.BalanceEffect
		/// </summary>
		public Double BalanceEffect 
		{
			get { return _DAO.BalanceEffect; }
			set { _DAO.BalanceEffect = value;}
		}
	}
}


