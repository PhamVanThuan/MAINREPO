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
	/// SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO
	/// </summary>
	public partial class DebtCounsellorDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO>, IDebtCounsellorDetail
	{
				public DebtCounsellorDetail(SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO DebtCounsellorDetail) : base(DebtCounsellorDetail)
		{
			this._DAO = DebtCounsellorDetail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO.NCRDCRegistrationNumber
		/// </summary>
		public String NCRDCRegistrationNumber 
		{
			get { return _DAO.NCRDCRegistrationNumber; }
			set { _DAO.NCRDCRegistrationNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO.LegalEntity
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


