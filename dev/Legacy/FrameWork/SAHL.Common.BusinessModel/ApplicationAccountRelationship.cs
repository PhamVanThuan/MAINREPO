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
	/// 
	/// </summary>
	public partial class ApplicationAccountRelationship : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationAccountRelationship_DAO>, IApplicationAccountRelationship
	{
				public ApplicationAccountRelationship(SAHL.Common.BusinessModel.DAO.ApplicationAccountRelationship_DAO ApplicationAccountRelationship) : base(ApplicationAccountRelationship)
		{
			this._DAO = ApplicationAccountRelationship;
		}
		/// <summary>
		/// 
		/// </summary>
		public IApplication Application 
		{
			get
			{
				if (null == _DAO.Application) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Application = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Application = (Application_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IAccount Account 
		{
			get
			{
				if (null == _DAO.Account) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Account = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Account = (Account_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


