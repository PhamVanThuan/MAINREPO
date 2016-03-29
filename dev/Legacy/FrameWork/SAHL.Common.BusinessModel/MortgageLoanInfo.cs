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
	/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO
	/// </summary>
	public partial class MortgageLoanInfo : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO>, IMortgageLoanInfo
	{
				public MortgageLoanInfo(SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO MortgageLoanInfo) : base(MortgageLoanInfo)
		{
			this._DAO = MortgageLoanInfo;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.ElectionDate
		/// </summary>
		public DateTime? ElectionDate
		{
			get { return _DAO.ElectionDate; }
			set { _DAO.ElectionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.ConvertedDate
		/// </summary>
		public DateTime? ConvertedDate
		{
			get { return _DAO.ConvertedDate; }
			set { _DAO.ConvertedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.AccumulatedLoyaltyBenefit
		/// </summary>
		public Double? AccumulatedLoyaltyBenefit
		{
			get { return _DAO.AccumulatedLoyaltyBenefit; }
			set { _DAO.AccumulatedLoyaltyBenefit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.NextPaymentDate
		/// </summary>
		public DateTime? NextPaymentDate
		{
			get { return _DAO.NextPaymentDate; }
			set { _DAO.NextPaymentDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.DiscountRate
		/// </summary>
		public Double? DiscountRate
		{
			get { return _DAO.DiscountRate; }
			set { _DAO.DiscountRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr1
		/// </summary>
		public Double? PPThresholdYr1
		{
			get { return _DAO.PPThresholdYr1; }
			set { _DAO.PPThresholdYr1 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr2
		/// </summary>
		public Double? PPThresholdYr2
		{
			get { return _DAO.PPThresholdYr2; }
			set { _DAO.PPThresholdYr2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr3
		/// </summary>
		public Double? PPThresholdYr3
		{
			get { return _DAO.PPThresholdYr3; }
			set { _DAO.PPThresholdYr3 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr4
		/// </summary>
		public Double? PPThresholdYr4
		{
			get { return _DAO.PPThresholdYr4; }
			set { _DAO.PPThresholdYr4 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr5
		/// </summary>
		public Double? PPThresholdYr5
		{
			get { return _DAO.PPThresholdYr5; }
			set { _DAO.PPThresholdYr5 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.MTDLoyaltyBenefit
		/// </summary>
		public Double MTDLoyaltyBenefit 
		{
			get { return _DAO.MTDLoyaltyBenefit; }
			set { _DAO.MTDLoyaltyBenefit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPAllowed
		/// </summary>
		public Double? PPAllowed
		{
			get { return _DAO.PPAllowed; }
			set { _DAO.PPAllowed = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.GeneralStatusKey
		/// </summary>
		public Int32 GeneralStatusKey 
		{
			get { return _DAO.GeneralStatusKey; }
			set { _DAO.GeneralStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.MortgageLoan
		/// </summary>
		public IMortgageLoan MortgageLoan 
		{
			get
			{
				if (null == _DAO.MortgageLoan) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMortgageLoan, MortgageLoan_DAO>(_DAO.MortgageLoan);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MortgageLoan = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MortgageLoan = (MortgageLoan_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.Exclusion
		/// </summary>
		public String Exclusion 
		{
			get { return _DAO.Exclusion; }
			set { _DAO.Exclusion = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.ExclusionEndDate
		/// </summary>
		public DateTime? ExclusionEndDate
		{
			get { return _DAO.ExclusionEndDate; }
			set { _DAO.ExclusionEndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.ExclusionReason
		/// </summary>
		public String ExclusionReason 
		{
			get { return _DAO.ExclusionReason; }
			set { _DAO.ExclusionReason = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.OverPaymentAmount
		/// </summary>
		public Double? OverPaymentAmount
		{
			get { return _DAO.OverPaymentAmount; }
			set { _DAO.OverPaymentAmount = value;}
		}
	}
}


