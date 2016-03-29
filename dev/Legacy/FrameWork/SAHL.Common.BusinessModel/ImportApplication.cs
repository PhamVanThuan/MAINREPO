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
	/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO
	/// </summary>
	public partial class ImportApplication : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportApplication_DAO>, IImportApplication
	{
				public ImportApplication(SAHL.Common.BusinessModel.DAO.ImportApplication_DAO ImportApplication) : base(ImportApplication)
		{
			this._DAO = ImportApplication;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ApplicationAmount
		/// </summary>
		public Double ApplicationAmount 
		{
			get { return _DAO.ApplicationAmount; }
			set { _DAO.ApplicationAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ApplicationStartDate
		/// </summary>
		public DateTime? ApplicationStartDate
		{
			get { return _DAO.ApplicationStartDate; }
			set { _DAO.ApplicationStartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ApplicationEndDate
		/// </summary>
		public DateTime? ApplicationEndDate
		{
			get { return _DAO.ApplicationEndDate; }
			set { _DAO.ApplicationEndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.MortgageLoanPurposeKey
		/// </summary>
		public String MortgageLoanPurposeKey 
		{
			get { return _DAO.MortgageLoanPurposeKey; }
			set { _DAO.MortgageLoanPurposeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ApplicantTypeKey
		/// </summary>
		public String ApplicantTypeKey 
		{
			get { return _DAO.ApplicantTypeKey; }
			set { _DAO.ApplicantTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.NumberApplicants
		/// </summary>
		public Int32 NumberApplicants 
		{
			get { return _DAO.NumberApplicants; }
			set { _DAO.NumberApplicants = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.HomePurchaseDate
		/// </summary>
		public DateTime? HomePurchaseDate
		{
			get { return _DAO.HomePurchaseDate; }
			set { _DAO.HomePurchaseDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.BondRegistrationDate
		/// </summary>
		public DateTime? BondRegistrationDate
		{
			get { return _DAO.BondRegistrationDate; }
			set { _DAO.BondRegistrationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.CurrentBondValue
		/// </summary>
		public Double CurrentBondValue 
		{
			get { return _DAO.CurrentBondValue; }
			set { _DAO.CurrentBondValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.DeedsOfficeDate
		/// </summary>
		public DateTime? DeedsOfficeDate
		{
			get { return _DAO.DeedsOfficeDate; }
			set { _DAO.DeedsOfficeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.BondFinancialInstitution
		/// </summary>
		public String BondFinancialInstitution 
		{
			get { return _DAO.BondFinancialInstitution; }
			set { _DAO.BondFinancialInstitution = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ExistingLoan
		/// </summary>
		public Double ExistingLoan 
		{
			get { return _DAO.ExistingLoan; }
			set { _DAO.ExistingLoan = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.PurchasePrice
		/// </summary>
		public Double PurchasePrice 
		{
			get { return _DAO.PurchasePrice; }
			set { _DAO.PurchasePrice = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.Reference
		/// </summary>
		public String Reference 
		{
			get { return _DAO.Reference; }
			set { _DAO.Reference = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ErrorMsg
		/// </summary>
		public String ErrorMsg 
		{
			get { return _DAO.ErrorMsg; }
			set { _DAO.ErrorMsg = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportID
		/// </summary>
		public Int32 ImportID 
		{
			get { return _DAO.ImportID; }
			set { _DAO.ImportID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportLegalEntities
		/// </summary>
		private DAOEventList<ImportLegalEntity_DAO, IImportLegalEntity, ImportLegalEntity> _ImportLegalEntities;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportLegalEntities
		/// </summary>
		public IEventList<IImportLegalEntity> ImportLegalEntities
		{
			get
			{
				if (null == _ImportLegalEntities) 
				{
					if(null == _DAO.ImportLegalEntities)
						_DAO.ImportLegalEntities = new List<ImportLegalEntity_DAO>();
					_ImportLegalEntities = new DAOEventList<ImportLegalEntity_DAO, IImportLegalEntity, ImportLegalEntity>(_DAO.ImportLegalEntities);
					_ImportLegalEntities.BeforeAdd += new EventListHandler(OnImportLegalEntities_BeforeAdd);					
					_ImportLegalEntities.BeforeRemove += new EventListHandler(OnImportLegalEntities_BeforeRemove);					
					_ImportLegalEntities.AfterAdd += new EventListHandler(OnImportLegalEntities_AfterAdd);					
					_ImportLegalEntities.AfterRemove += new EventListHandler(OnImportLegalEntities_AfterRemove);					
				}
				return _ImportLegalEntities;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportStatus
		/// </summary>
		public IImportStatus ImportStatus 
		{
			get
			{
				if (null == _DAO.ImportStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IImportStatus, ImportStatus_DAO>(_DAO.ImportStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ImportStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ImportStatus = (ImportStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportFile
		/// </summary>
		public IImportFile ImportFile 
		{
			get
			{
				if (null == _DAO.ImportFile) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IImportFile, ImportFile_DAO>(_DAO.ImportFile);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ImportFile = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ImportFile = (ImportFile_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ImportLegalEntities = null;
			
		}
	}
}


