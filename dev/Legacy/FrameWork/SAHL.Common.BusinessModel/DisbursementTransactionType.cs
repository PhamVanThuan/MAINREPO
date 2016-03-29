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
	/// SAHL.Common.BusinessModel.DAO.DisbursementTransactionType_DAO
	/// </summary>
	public partial class DisbursementTransactionType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DisbursementTransactionType_DAO>, IDisbursementTransactionType
	{
				public DisbursementTransactionType(SAHL.Common.BusinessModel.DAO.DisbursementTransactionType_DAO DisbursementTransactionType) : base(DisbursementTransactionType)
		{
			this._DAO = DisbursementTransactionType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DisbursementTransactionType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DisbursementTransactionType_DAO.TransactionTypeNumber
		/// </summary>
		public Int32? TransactionTypeNumber
		{
			get { return _DAO.TransactionTypeNumber; }
			set { _DAO.TransactionTypeNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DisbursementTransactionType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


