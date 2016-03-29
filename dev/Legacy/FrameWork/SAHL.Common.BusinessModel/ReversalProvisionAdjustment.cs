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
	/// SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO
	/// </summary>
	public partial class ReversalProvisionAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO>, IReversalProvisionAdjustment
	{
				public ReversalProvisionAdjustment(SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO ReversalProvisionAdjustment) : base(ReversalProvisionAdjustment)
		{
			this._DAO = ReversalProvisionAdjustment;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO.ReversalPercentage
		/// </summary>
		public Double ReversalPercentage 
		{
			get { return _DAO.ReversalPercentage; }
			set { _DAO.ReversalPercentage = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReversalProvisionAdjustment_DAO.TransactionType
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


