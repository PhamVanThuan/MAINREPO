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
	/// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO
	/// </summary>
	public partial class DifferentialProvisionAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO>, IDifferentialProvisionAdjustment
	{
				public DifferentialProvisionAdjustment(SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO DifferentialProvisionAdjustment) : base(DifferentialProvisionAdjustment)
		{
			this._DAO = DifferentialProvisionAdjustment;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO.DifferentialAdjustment
		/// </summary>
		public Double DifferentialAdjustment 
		{
			get { return _DAO.DifferentialAdjustment; }
			set { _DAO.DifferentialAdjustment = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO.BalanceType
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
		/// SAHL.Common.BusinessModel.DAO.DifferentialProvisionAdjustment_DAO.TransactionType
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


