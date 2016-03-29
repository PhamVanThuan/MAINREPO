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
	/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO
	/// </summary>
	public partial class RecoveriesProposal : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO>, IRecoveriesProposal
	{
				public RecoveriesProposal(SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO RecoveriesProposal) : base(RecoveriesProposal)
		{
			this._DAO = RecoveriesProposal;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.Account
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.ShortfallAmount
		/// </summary>
		public Double ShortfallAmount 
		{
			get { return _DAO.ShortfallAmount; }
			set { _DAO.ShortfallAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.RepaymentAmount
		/// </summary>
		public Double RepaymentAmount 
		{
			get { return _DAO.RepaymentAmount; }
			set { _DAO.RepaymentAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.StartDate
		/// </summary>
		public DateTime StartDate 
		{
			get { return _DAO.StartDate; }
			set { _DAO.StartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.AcknowledgementOfDebt
		/// </summary>
		public Boolean? AcknowledgementOfDebt
		{
			get { return _DAO.AcknowledgementOfDebt; }
			set { _DAO.AcknowledgementOfDebt = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.ADUser
		/// </summary>
		public IADUser ADUser 
		{
			get
			{
				if (null == _DAO.ADUser) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ADUser = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.CreateDate
		/// </summary>
		public DateTime CreateDate 
		{
			get { return _DAO.CreateDate; }
			set { _DAO.CreateDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RecoveriesProposal_DAO.GeneralStatus
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
	}
}


