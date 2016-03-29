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
	/// SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO
	/// </summary>
	public partial class TransactionTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO>, ITransactionTypeGroup
	{
				public TransactionTypeGroup(SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO TransactionTypeGroup) : base(TransactionTypeGroup)
		{
			this._DAO = TransactionTypeGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO.TransactionGroup
		/// </summary>
		public ITransactionGroup TransactionGroup 
		{
			get
			{
				if (null == _DAO.TransactionGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITransactionGroup, TransactionGroup_DAO>(_DAO.TransactionGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TransactionGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TransactionGroup = (TransactionGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO.TransactionType
		/// </summary>
		public ITransactionType TransactionType 
		{
			get
			{
				if (null == _DAO.TransactionType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITransactionType, TransactionType_DAO>(_DAO.TransactionType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TransactionType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TransactionType = (TransactionType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


