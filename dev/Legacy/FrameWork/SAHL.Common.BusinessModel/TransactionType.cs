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
	/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO
	/// </summary>
	public partial class TransactionType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TransactionType_DAO>, ITransactionType
	{
				public TransactionType(SAHL.Common.BusinessModel.DAO.TransactionType_DAO TransactionType) : base(TransactionType)
		{
			this._DAO = TransactionType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.SPVDescription
		/// </summary>
		public String SPVDescription 
		{
			get { return _DAO.SPVDescription; }
			set { _DAO.SPVDescription = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.GLAccount
		/// </summary>
		public String GLAccount 
		{
			get { return _DAO.GLAccount; }
			set { _DAO.GLAccount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.GLAccountContra
		/// </summary>
		public String GLAccountContra 
		{
			get { return _DAO.GLAccountContra; }
			set { _DAO.GLAccountContra = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.GeneralStatus
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.ReversalTransactionType
		/// </summary>
		public ITransactionType ReversalTransactionType 
		{
			get
			{
				if (null == _DAO.ReversalTransactionType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITransactionType, TransactionType_DAO>(_DAO.ReversalTransactionType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReversalTransactionType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReversalTransactionType = (TransactionType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.TransactionGroups
		/// </summary>
		private DAOEventList<TransactionGroup_DAO, ITransactionGroup, TransactionGroup> _TransactionGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionType_DAO.TransactionGroups
		/// </summary>
		public IEventList<ITransactionGroup> TransactionGroups
		{
			get
			{
				if (null == _TransactionGroups) 
				{
					if(null == _DAO.TransactionGroups)
						_DAO.TransactionGroups = new List<TransactionGroup_DAO>();
					_TransactionGroups = new DAOEventList<TransactionGroup_DAO, ITransactionGroup, TransactionGroup>(_DAO.TransactionGroups);
				}
				return _TransactionGroups;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_TransactionGroups = null;
			
		}
	}
}


