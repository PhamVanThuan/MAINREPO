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
	/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO
	/// </summary>
	public partial class SPVMandate : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SPVMandate_DAO>, ISPVMandate
	{
				public SPVMandate(SAHL.Common.BusinessModel.DAO.SPVMandate_DAO SPVMandate) : base(SPVMandate)
		{
			this._DAO = SPVMandate;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.MaxLTV
		/// </summary>
		public Double? MaxLTV
		{
			get { return _DAO.MaxLTV; }
			set { _DAO.MaxLTV = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.MaxPTI
		/// </summary>
		public Double? MaxPTI
		{
			get { return _DAO.MaxPTI; }
			set { _DAO.MaxPTI = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.MaxLoanAmount
		/// </summary>
		public Double? MaxLoanAmount
		{
			get { return _DAO.MaxLoanAmount; }
			set { _DAO.MaxLoanAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.ExceedBondPercent
		/// </summary>
		public Double? ExceedBondPercent
		{
			get { return _DAO.ExceedBondPercent; }
			set { _DAO.ExceedBondPercent = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.ExceedLoanAgreementPercent
		/// </summary>
		public Double? ExceedLoanAgreementPercent
		{
			get { return _DAO.ExceedLoanAgreementPercent; }
			set { _DAO.ExceedLoanAgreementPercent = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.ExceedBondAmount
		/// </summary>
		public Double? ExceedBondAmount
		{
			get { return _DAO.ExceedBondAmount; }
			set { _DAO.ExceedBondAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.SPVMaxTerm
		/// </summary>
		public Int32? SPVMaxTerm
		{
			get { return _DAO.SPVMaxTerm; }
			set { _DAO.SPVMaxTerm = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPVMandate_DAO.SPV
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
	}
}


