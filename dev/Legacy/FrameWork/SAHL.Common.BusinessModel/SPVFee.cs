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
	/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO
	/// </summary>
	public partial class SPVFee : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SPVFee_DAO>, ISPVFee
	{
				public SPVFee(SAHL.Common.BusinessModel.DAO.SPVFee_DAO SPVFee) : base(SPVFee)
		{
			this._DAO = SPVFee;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.SPV
		/// </summary>
		public ISPV SPV 
		{
			get
			{
				if (null == _DAO.SPV) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISPV, SPV_DAO>(_DAO.SPV);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SPV = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SPV = (SPV_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.SPVFeeType
		/// </summary>
		public ISPVFeeType SPVFeeType 
		{
			get
			{
				if (null == _DAO.SPVFeeType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISPVFeeType, SPVFeeType_DAO>(_DAO.SPVFeeType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SPVFeeType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SPVFeeType = (SPVFeeType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.Value
		/// </summary>
		public Double Value 
		{
			get { return _DAO.Value; }
			set { _DAO.Value = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.MaxFeeAmount
		/// </summary>
		public Double? MaxFeeAmount
		{
			get { return _DAO.MaxFeeAmount; }
			set { _DAO.MaxFeeAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.MinFeeAmount
		/// </summary>
		public Double? MinFeeAmount
		{
			get { return _DAO.MinFeeAmount; }
			set { _DAO.MinFeeAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.RoundingYield
		/// </summary>
		public Double? RoundingYield
		{
			get { return _DAO.RoundingYield; }
			set { _DAO.RoundingYield = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVFee_DAO.AdditionalYield
		/// </summary>
		public Double? AdditionalYield
		{
			get { return _DAO.AdditionalYield; }
			set { _DAO.AdditionalYield = value;}
		}
	}
}


