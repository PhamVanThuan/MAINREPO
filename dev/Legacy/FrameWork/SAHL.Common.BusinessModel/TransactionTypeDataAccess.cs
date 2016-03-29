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
	/// SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO
	/// </summary>
	public partial class TransactionTypeDataAccess : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO>, ITransactionTypeDataAccess
	{
				public TransactionTypeDataAccess(SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO TransactionTypeDataAccess) : base(TransactionTypeDataAccess)
		{
			this._DAO = TransactionTypeDataAccess;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO.ADCredentials
		/// </summary>
		public String ADCredentials 
		{
			get { return _DAO.ADCredentials; }
			set { _DAO.ADCredentials = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO.TransactionTypeKey
		/// </summary>
		public Int32 TransactionTypeKey 
		{
			get { return _DAO.TransactionTypeKey; }
			set { _DAO.TransactionTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeDataAccess_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


