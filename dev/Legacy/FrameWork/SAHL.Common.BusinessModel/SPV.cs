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
	/// SAHL.Common.BusinessModel.DAO.SPV_DAO
	/// </summary>
	public partial class SPV : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SPV_DAO>, ISPV
	{
				public SPV(SAHL.Common.BusinessModel.DAO.SPV_DAO SPV) : base(SPV)
		{
			this._DAO = SPV;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.ParentSPV
		/// </summary>
		public ISPV ParentSPV 
		{
			get
			{
				if (null == _DAO.ParentSPV) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISPV, SPV_DAO>(_DAO.ParentSPV);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ParentSPV = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ParentSPV = (SPV_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVCompany
		/// </summary>
		public ISPVCompany SPVCompany 
		{
			get
			{
				if (null == _DAO.SPVCompany) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISPVCompany, SPVCompany_DAO>(_DAO.SPVCompany);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SPVCompany = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SPVCompany = (SPVCompany_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.ReportDescription
		/// </summary>
		public String ReportDescription 
		{
			get { return _DAO.ReportDescription; }
			set { _DAO.ReportDescription = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.ResetConfigurationKey
		/// </summary>
		public Int32? ResetConfigurationKey
		{
			get { return _DAO.ResetConfigurationKey; }
			set { _DAO.ResetConfigurationKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.AnniversaryDay
		/// </summary>
		public Int16 AnniversaryDay 
		{
			get { return _DAO.AnniversaryDay; }
			set { _DAO.AnniversaryDay = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.FundingWarehouse
		/// </summary>
		public Int32? FundingWarehouse
		{
			get { return _DAO.FundingWarehouse; }
			set { _DAO.FundingWarehouse = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.CreditProviderNumber
		/// </summary>
		public String CreditProviderNumber 
		{
			get { return _DAO.CreditProviderNumber; }
			set { _DAO.CreditProviderNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.RegistrationNumber
		/// </summary>
		public String RegistrationNumber 
		{
			get { return _DAO.RegistrationNumber; }
			set { _DAO.RegistrationNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.BankAccount
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
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.ChildSPVs
		/// </summary>
		private DAOEventList<SPV_DAO, ISPV, SPV> _ChildSPVs;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.ChildSPVs
		/// </summary>
		public IEventList<ISPV> ChildSPVs
		{
			get
			{
				if (null == _ChildSPVs) 
				{
					if(null == _DAO.ChildSPVs)
						_DAO.ChildSPVs = new List<SPV_DAO>();
					_ChildSPVs = new DAOEventList<SPV_DAO, ISPV, SPV>(_DAO.ChildSPVs);
					_ChildSPVs.BeforeAdd += new EventListHandler(OnChildSPVs_BeforeAdd);					
					_ChildSPVs.BeforeRemove += new EventListHandler(OnChildSPVs_BeforeRemove);					
					_ChildSPVs.AfterAdd += new EventListHandler(OnChildSPVs_AfterAdd);					
					_ChildSPVs.AfterRemove += new EventListHandler(OnChildSPVs_AfterRemove);					
				}
				return _ChildSPVs;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVMandates
		/// </summary>
		private DAOEventList<SPVMandate_DAO, ISPVMandate, SPVMandate> _SPVMandates;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVMandates
		/// </summary>
		public IEventList<ISPVMandate> SPVMandates
		{
			get
			{
				if (null == _SPVMandates) 
				{
					if(null == _DAO.SPVMandates)
						_DAO.SPVMandates = new List<SPVMandate_DAO>();
					_SPVMandates = new DAOEventList<SPVMandate_DAO, ISPVMandate, SPVMandate>(_DAO.SPVMandates);
					_SPVMandates.BeforeAdd += new EventListHandler(OnSPVMandates_BeforeAdd);					
					_SPVMandates.BeforeRemove += new EventListHandler(OnSPVMandates_BeforeRemove);					
					_SPVMandates.AfterAdd += new EventListHandler(OnSPVMandates_AfterAdd);					
					_SPVMandates.AfterRemove += new EventListHandler(OnSPVMandates_AfterRemove);					
				}
				return _SPVMandates;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVFees
		/// </summary>
		private DAOEventList<SPVFee_DAO, ISPVFee, SPVFee> _SPVFees;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVFees
		/// </summary>
		public IEventList<ISPVFee> SPVFees
		{
			get
			{
				if (null == _SPVFees) 
				{
					if(null == _DAO.SPVFees)
						_DAO.SPVFees = new List<SPVFee_DAO>();
					_SPVFees = new DAOEventList<SPVFee_DAO, ISPVFee, SPVFee>(_DAO.SPVFees);
					_SPVFees.BeforeAdd += new EventListHandler(OnSPVFees_BeforeAdd);					
					_SPVFees.BeforeRemove += new EventListHandler(OnSPVFees_BeforeRemove);					
					_SPVFees.AfterAdd += new EventListHandler(OnSPVFees_AfterAdd);					
					_SPVFees.AfterRemove += new EventListHandler(OnSPVFees_AfterRemove);					
				}
				return _SPVFees;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVAttributes
		/// </summary>
		private DAOEventList<SPVAttribute_DAO, ISPVAttribute, SPVAttribute> _SPVAttributes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVAttributes
		/// </summary>
		public IEventList<ISPVAttribute> SPVAttributes
		{
			get
			{
				if (null == _SPVAttributes) 
				{
					if(null == _DAO.SPVAttributes)
						_DAO.SPVAttributes = new List<SPVAttribute_DAO>();
					_SPVAttributes = new DAOEventList<SPVAttribute_DAO, ISPVAttribute, SPVAttribute>(_DAO.SPVAttributes);
					_SPVAttributes.BeforeAdd += new EventListHandler(OnSPVAttributes_BeforeAdd);					
					_SPVAttributes.BeforeRemove += new EventListHandler(OnSPVAttributes_BeforeRemove);					
					_SPVAttributes.AfterAdd += new EventListHandler(OnSPVAttributes_AfterAdd);					
					_SPVAttributes.AfterRemove += new EventListHandler(OnSPVAttributes_AfterRemove);					
				}
				return _SPVAttributes;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVBalances
		/// </summary>
		private DAOEventList<SPVBalance_DAO, ISPVBalance, SPVBalance> _SPVBalances;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVBalances
		/// </summary>
		public IEventList<ISPVBalance> SPVBalances
		{
			get
			{
				if (null == _SPVBalances) 
				{
					if(null == _DAO.SPVBalances)
						_DAO.SPVBalances = new List<SPVBalance_DAO>();
					_SPVBalances = new DAOEventList<SPVBalance_DAO, ISPVBalance, SPVBalance>(_DAO.SPVBalances);
					_SPVBalances.BeforeAdd += new EventListHandler(OnSPVBalances_BeforeAdd);					
					_SPVBalances.BeforeRemove += new EventListHandler(OnSPVBalances_BeforeRemove);					
					_SPVBalances.AfterAdd += new EventListHandler(OnSPVBalances_AfterAdd);					
					_SPVBalances.AfterRemove += new EventListHandler(OnSPVBalances_AfterRemove);					
				}
				return _SPVBalances;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ChildSPVs = null;
			_SPVMandates = null;
			_SPVFees = null;
			_SPVAttributes = null;
			_SPVBalances = null;
			
		}
	}
}


