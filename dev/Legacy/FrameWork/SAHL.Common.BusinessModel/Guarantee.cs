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
	/// SAHL.Common.BusinessModel.DAO.Guarantee_DAO
	/// </summary>
	public partial class Guarantee : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Guarantee_DAO>, IGuarantee
	{
				public Guarantee(SAHL.Common.BusinessModel.DAO.Guarantee_DAO Guarantee) : base(Guarantee)
		{
			this._DAO = Guarantee;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.LimitedAmount
		/// </summary>
		public Double LimitedAmount 
		{
			get { return _DAO.LimitedAmount; }
			set { _DAO.LimitedAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.IssueDate
		/// </summary>
		public DateTime IssueDate 
		{
			get { return _DAO.IssueDate; }
			set { _DAO.IssueDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.StatusNumber
		/// </summary>
		public Byte StatusNumber 
		{
			get { return _DAO.StatusNumber; }
			set { _DAO.StatusNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.CancelledDate
		/// </summary>
		public DateTime? CancelledDate
		{
			get { return _DAO.CancelledDate; }
			set { _DAO.CancelledDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Guarantee_DAO.Account
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


