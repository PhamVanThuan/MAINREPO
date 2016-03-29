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
	/// SAHL.Common.BusinessModel.DAO.PaymentAdjustment_DAO
	/// </summary>
	public partial class PaymentAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PaymentAdjustment_DAO>, IPaymentAdjustment
	{
				public PaymentAdjustment(SAHL.Common.BusinessModel.DAO.PaymentAdjustment_DAO PaymentAdjustment) : base(PaymentAdjustment)
		{
			this._DAO = PaymentAdjustment;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PaymentAdjustment_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PaymentAdjustment_DAO.Amount
		/// </summary>
		public Double Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
	}
}


