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
	/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO
	/// </summary>
	public partial class FinancialServiceBankAccount : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO>, IFinancialServiceBankAccount
	{
				public FinancialServiceBankAccount(SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO FinancialServiceBankAccount) : base(FinancialServiceBankAccount)
		{
			this._DAO = FinancialServiceBankAccount;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.Percentage
		/// </summary>
		public Double Percentage 
		{
			get { return _DAO.Percentage; }
			set { _DAO.Percentage = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.BankAccount
		/// </summary>
		public IBankAccount BankAccount 
		{
			get
			{
				if (null == _DAO.BankAccount) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IBankAccount, BankAccount_DAO>(_DAO.BankAccount);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.BankAccount = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.BankAccount = (BankAccount_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.FinancialService
		/// </summary>
		public IFinancialService FinancialService 
		{
			get
			{
				if (null == _DAO.FinancialService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(_DAO.FinancialService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialService = (FinancialService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.FinancialServicePaymentType
		/// </summary>
		public IFinancialServicePaymentType FinancialServicePaymentType 
		{
			get
			{
				if (null == _DAO.FinancialServicePaymentType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialServicePaymentType, FinancialServicePaymentType_DAO>(_DAO.FinancialServicePaymentType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialServicePaymentType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialServicePaymentType = (FinancialServicePaymentType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.PaymentSplitType
		/// </summary>
		public IPaymentSplitType PaymentSplitType 
		{
			get
			{
				if (null == _DAO.PaymentSplitType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IPaymentSplitType, PaymentSplitType_DAO>(_DAO.PaymentSplitType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.PaymentSplitType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.PaymentSplitType = (PaymentSplitType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialServiceBankAccount_DAO.IsNaedoCompliant
        /// </summary>
        public bool IsNaedoCompliant
        {
            get { return _DAO.IsNaedoCompliant; }
            set { _DAO.IsNaedoCompliant = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? ProviderKey
        {
            get { return _DAO.ProviderKey; }
            set { _DAO.ProviderKey = value; }
        }
        
	}
}


