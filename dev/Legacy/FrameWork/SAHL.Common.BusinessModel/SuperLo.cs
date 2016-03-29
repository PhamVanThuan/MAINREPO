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
	/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO
	/// </summary>
	public partial class SuperLo : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SuperLo_DAO>, ISuperLo
	{
				public SuperLo(SAHL.Common.BusinessModel.DAO.SuperLo_DAO SuperLo) : base(SuperLo)
		{
			this._DAO = SuperLo;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.ElectionDate
		/// </summary>
		public DateTime ElectionDate 
		{
			get { return _DAO.ElectionDate; }
			set { _DAO.ElectionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.ConvertedDate
		/// </summary>
		public DateTime ConvertedDate 
		{
			get { return _DAO.ConvertedDate; }
			set { _DAO.ConvertedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.NextPaymentDate
		/// </summary>
		public DateTime NextPaymentDate 
		{
			get { return _DAO.NextPaymentDate; }
			set { _DAO.NextPaymentDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr1
		/// </summary>
		public Double PPThresholdYr1 
		{
			get { return _DAO.PPThresholdYr1; }
			set { _DAO.PPThresholdYr1 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr2
		/// </summary>
		public Double PPThresholdYr2 
		{
			get { return _DAO.PPThresholdYr2; }
			set { _DAO.PPThresholdYr2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr3
		/// </summary>
		public Double PPThresholdYr3 
		{
			get { return _DAO.PPThresholdYr3; }
			set { _DAO.PPThresholdYr3 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr4
		/// </summary>
		public Double PPThresholdYr4 
		{
			get { return _DAO.PPThresholdYr4; }
			set { _DAO.PPThresholdYr4 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPThresholdYr5
		/// </summary>
		public Double PPThresholdYr5 
		{
			get { return _DAO.PPThresholdYr5; }
			set { _DAO.PPThresholdYr5 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.MTDLoyaltyBenefit
		/// </summary>
		public Double MTDLoyaltyBenefit 
		{
			get { return _DAO.MTDLoyaltyBenefit; }
			set { _DAO.MTDLoyaltyBenefit = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.PPAllowed
		/// </summary>
		public Double PPAllowed 
		{
			get { return _DAO.PPAllowed; }
			set { _DAO.PPAllowed = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.GeneralStatus
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.Exclusion
		/// </summary>
		public Boolean? Exclusion
		{
			get { return _DAO.Exclusion; }
			set { _DAO.Exclusion = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.ExclusionEndDate
		/// </summary>
		public DateTime? ExclusionEndDate
		{
			get { return _DAO.ExclusionEndDate; }
			set { _DAO.ExclusionEndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.ExclusionReason
		/// </summary>
		public String ExclusionReason 
		{
			get { return _DAO.ExclusionReason; }
			set { _DAO.ExclusionReason = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.OverPaymentAmount
		/// </summary>
		public Double OverPaymentAmount 
		{
			get { return _DAO.OverPaymentAmount; }
			set { _DAO.OverPaymentAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.FinancialServiceAttribute
		/// </summary>
		public IFinancialServiceAttribute FinancialServiceAttribute 
		{
			get
			{
				if (null == _DAO.FinancialServiceAttribute) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialServiceAttribute, FinancialServiceAttribute_DAO>(_DAO.FinancialServiceAttribute);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialServiceAttribute = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialServiceAttribute = (FinancialServiceAttribute_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


