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
	/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO
	/// </summary>
	public partial class LegalEntityCleanUp : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO>, ILegalEntityCleanUp
	{
				public LegalEntityCleanUp(SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO LegalEntityCleanUp) : base(LegalEntityCleanUp)
		{
			this._DAO = LegalEntityCleanUp;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO.Surname
		/// </summary>
		public String Surname 
		{
			get { return _DAO.Surname; }
			set { _DAO.Surname = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO.Firstnames
		/// </summary>
		public String Firstnames 
		{
			get { return _DAO.Firstnames; }
			set { _DAO.Firstnames = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO.IDNumber
		/// </summary>
		public String IDNumber 
		{
			get { return _DAO.IDNumber; }
			set { _DAO.IDNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO.Accounts
		/// </summary>
		public String Accounts 
		{
			get { return _DAO.Accounts; }
			set { _DAO.Accounts = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO.LegalEntity
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityCleanUp_DAO.LegalEntityExceptionReason
		/// </summary>
		public ILegalEntityExceptionReason LegalEntityExceptionReason 
		{
			get
			{
				if (null == _DAO.LegalEntityExceptionReason) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntityExceptionReason, LegalEntityExceptionReason_DAO>(_DAO.LegalEntityExceptionReason);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntityExceptionReason = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntityExceptionReason = (LegalEntityExceptionReason_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


