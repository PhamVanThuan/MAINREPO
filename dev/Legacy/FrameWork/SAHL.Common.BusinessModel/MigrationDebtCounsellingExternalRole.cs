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
	/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO
	/// </summary>
	public partial class MigrationDebtCounsellingExternalRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO>, IMigrationDebtCounsellingExternalRole
	{
				public MigrationDebtCounsellingExternalRole(SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO MigrationDebtCounsellingExternalRole) : base(MigrationDebtCounsellingExternalRole)
		{
			this._DAO = MigrationDebtCounsellingExternalRole;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO.LegalEntityKey
		/// </summary>
		public Int32 LegalEntityKey 
		{
			get { return _DAO.LegalEntityKey; }
			set { _DAO.LegalEntityKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO.ExternalRoleTypeKey
		/// </summary>
		public Int32 ExternalRoleTypeKey 
		{
			get { return _DAO.ExternalRoleTypeKey; }
			set { _DAO.ExternalRoleTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO.DebtCounselling
		/// </summary>
		public IMigrationDebtCounselling DebtCounselling 
		{
			get
			{
				if (null == _DAO.DebtCounselling) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMigrationDebtCounselling, MigrationDebtCounselling_DAO>(_DAO.DebtCounselling);
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
					_DAO.DebtCounselling = (MigrationDebtCounselling_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


