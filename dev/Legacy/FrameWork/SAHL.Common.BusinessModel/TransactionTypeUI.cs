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
	/// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO
	/// </summary>
	public partial class TransactionTypeUI : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO>, ITransactionTypeUI
	{
				public TransactionTypeUI(SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO TransactionTypeUI) : base(TransactionTypeUI)
		{
			this._DAO = TransactionTypeUI;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO.TransactionType
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
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO.ScreenBatch
		/// </summary>
		public Int32 ScreenBatch 
		{
			get { return _DAO.ScreenBatch; }
			set { _DAO.ScreenBatch = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.TransactionTypeUI_DAO.HTMLColour
		/// </summary>
		public String HTMLColour 
		{
			get { return _DAO.HTMLColour; }
			set { _DAO.HTMLColour = value;}
		}
	}
}


