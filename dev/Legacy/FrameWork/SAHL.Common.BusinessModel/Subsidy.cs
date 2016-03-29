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
	/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO
	/// </summary>
	public partial class Subsidy : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Subsidy_DAO>, ISubsidy
	{
				public Subsidy(SAHL.Common.BusinessModel.DAO.Subsidy_DAO Subsidy) : base(Subsidy)
		{
			this._DAO = Subsidy;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.SalaryNumber
		/// </summary>
		public String SalaryNumber 
		{
			get { return _DAO.SalaryNumber; }
			set { _DAO.SalaryNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Paypoint
		/// </summary>
		public String Paypoint 
		{
			get { return _DAO.Paypoint; }
			set { _DAO.Paypoint = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Notch
		/// </summary>
		public String Notch 
		{
			get { return _DAO.Notch; }
			set { _DAO.Notch = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Rank
		/// </summary>
		public String Rank 
		{
			get { return _DAO.Rank; }
			set { _DAO.Rank = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.StopOrderAmount
		/// </summary>
		public Double StopOrderAmount 
		{
			get { return _DAO.StopOrderAmount; }
			set { _DAO.StopOrderAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.Employment
		/// </summary>
		public IEmploymentSubsidised Employment 
		{
			get
			{
				if (null == _DAO.Employment) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IEmploymentSubsidised, EmploymentSubsidised_DAO>(_DAO.Employment);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Employment = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Employment = (EmploymentSubsidised_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.LegalEntity
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.SubsidyProvider
		/// </summary>
		public ISubsidyProvider SubsidyProvider 
		{
			get
			{
				if (null == _DAO.SubsidyProvider) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISubsidyProvider, SubsidyProvider_DAO>(_DAO.SubsidyProvider);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SubsidyProvider = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SubsidyProvider = (SubsidyProvider_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Subsidy_DAO.GEPFMember
        /// </summary>
        public bool GEPFMember
        {
            get { return _DAO.GEPFMember; }
            set { _DAO.GEPFMember = value; }
        }
	}
}


