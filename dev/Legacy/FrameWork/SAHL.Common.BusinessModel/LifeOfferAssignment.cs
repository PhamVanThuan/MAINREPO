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
	/// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO
	/// </summary>
	public partial class LifeOfferAssignment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO>, ILifeOfferAssignment
	{
				public LifeOfferAssignment(SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO LifeOfferAssignment) : base(LifeOfferAssignment)
		{
			this._DAO = LifeOfferAssignment;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.OfferKey
		/// </summary>
		public Int32 OfferKey 
		{
			get { return _DAO.OfferKey; }
			set { _DAO.OfferKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.LoanAccountKey
		/// </summary>
		public Int32 LoanAccountKey 
		{
			get { return _DAO.LoanAccountKey; }
			set { _DAO.LoanAccountKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.LoanOfferKey
		/// </summary>
		public Int32 LoanOfferKey 
		{
			get { return _DAO.LoanOfferKey; }
			set { _DAO.LoanOfferKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.LoanOfferTypeKey
		/// </summary>
		public Int32 LoanOfferTypeKey 
		{
			get { return _DAO.LoanOfferTypeKey; }
			set { _DAO.LoanOfferTypeKey = value;}
		}
		/// <summary>
		/// The date when the life offer was assigned
		/// </summary>
		public DateTime DateAssigned 
		{
			get { return _DAO.DateAssigned; }
			set { _DAO.DateAssigned = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeOfferAssignment_DAO.ADUserName
		/// </summary>
		public String ADUserName 
		{
			get { return _DAO.ADUserName; }
			set { _DAO.ADUserName = value;}
		}
	}
}


