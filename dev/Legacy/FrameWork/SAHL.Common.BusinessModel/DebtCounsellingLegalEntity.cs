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
	/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO
	/// </summary>
	public partial class DebtCounsellingLegalEntity : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO>, IDebtCounsellingLegalEntity
	{
				public DebtCounsellingLegalEntity(SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO DebtCounsellingLegalEntity) : base(DebtCounsellingLegalEntity)
		{
			this._DAO = DebtCounsellingLegalEntity;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO.DebtCounsellingLegalEntityKey
		/// </summary>
		public Int32 DebtCounsellingLegalEntityKey 
		{
			get { return _DAO.DebtCounsellingLegalEntityKey; }
			set { _DAO.DebtCounsellingLegalEntityKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO.LegalEntityKey
		/// </summary>
		public Int32 LegalEntityKey 
		{
			get { return _DAO.LegalEntityKey; }
			set { _DAO.LegalEntityKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO.DebtCounselling
		/// </summary>
		public IDebtCounselling DebtCounselling 
		{
			get
			{
				if (null == _DAO.DebtCounselling) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDebtCounselling, DebtCounselling_DAO>(_DAO.DebtCounselling);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DebtCounselling = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DebtCounselling = (DebtCounselling_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


