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
	/// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO
	/// </summary>
	public partial class TransactionTypeBalanceEffect : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO>, ITransactionTypeBalanceEffect
	{
				public TransactionTypeBalanceEffect(SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO TransactionTypeBalanceEffect) : base(TransactionTypeBalanceEffect)
		{
			this._DAO = TransactionTypeBalanceEffect;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.TransactionType
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.BalanceType
		/// </summary>
		public IBalanceType BalanceType 
		{
			get
			{
				if (null == _DAO.BalanceType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IBalanceType, BalanceType_DAO>(_DAO.BalanceType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.BalanceType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.BalanceType = (BalanceType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.TransactionEffect
		/// </summary>
		public ITransactionEffect TransactionEffect 
		{
			get
			{
				if (null == _DAO.TransactionEffect) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITransactionEffect, TransactionEffect_DAO>(_DAO.TransactionEffect);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TransactionEffect = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TransactionEffect = (TransactionEffect_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.SPVTransactionEffect
		/// </summary>
		public ITransactionEffect SPVTransactionEffect 
		{
			get
			{
				if (null == _DAO.SPVTransactionEffect) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITransactionEffect, TransactionEffect_DAO>(_DAO.SPVTransactionEffect);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SPVTransactionEffect = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SPVTransactionEffect = (TransactionEffect_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.ParentTransactionTypeBalanceEffectKey
		/// </summary>
		public ITransactionTypeBalanceEffect ParentTransactionTypeBalanceEffectKey 
		{
			get
			{
				if (null == _DAO.ParentTransactionTypeBalanceEffectKey) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITransactionTypeBalanceEffect, TransactionTypeBalanceEffect_DAO>(_DAO.ParentTransactionTypeBalanceEffectKey);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ParentTransactionTypeBalanceEffectKey = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ParentTransactionTypeBalanceEffectKey = (TransactionTypeBalanceEffect_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


