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
	/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO
	/// </summary>
	public partial class ImportLegalEntity : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO>, IImportLegalEntity
	{
				public ImportLegalEntity(SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO ImportLegalEntity) : base(ImportLegalEntity)
		{
			this._DAO = ImportLegalEntity;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.MaritalStatusKey
		/// </summary>
		public String MaritalStatusKey 
		{
			get { return _DAO.MaritalStatusKey; }
			set { _DAO.MaritalStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.GenderKey
		/// </summary>
		public String GenderKey 
		{
			get { return _DAO.GenderKey; }
			set { _DAO.GenderKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.CitizenTypeKey
		/// </summary>
		public String CitizenTypeKey 
		{
			get { return _DAO.CitizenTypeKey; }
			set { _DAO.CitizenTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.SalutationKey
		/// </summary>
		public String SalutationKey 
		{
			get { return _DAO.SalutationKey; }
			set { _DAO.SalutationKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.FirstNames
		/// </summary>
		public String FirstNames 
		{
			get { return _DAO.FirstNames; }
			set { _DAO.FirstNames = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.Initials
		/// </summary>
		public String Initials 
		{
			get { return _DAO.Initials; }
			set { _DAO.Initials = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.Surname
		/// </summary>
		public String Surname 
		{
			get { return _DAO.Surname; }
			set { _DAO.Surname = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.PreferredName
		/// </summary>
		public String PreferredName 
		{
			get { return _DAO.PreferredName; }
			set { _DAO.PreferredName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.IDNumber
		/// </summary>
		public String IDNumber 
		{
			get { return _DAO.IDNumber; }
			set { _DAO.IDNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.PassportNumber
		/// </summary>
		public String PassportNumber 
		{
			get { return _DAO.PassportNumber; }
			set { _DAO.PassportNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.TaxNumber
		/// </summary>
		public String TaxNumber 
		{
			get { return _DAO.TaxNumber; }
			set { _DAO.TaxNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.HomePhoneCode
		/// </summary>
		public String HomePhoneCode 
		{
			get { return _DAO.HomePhoneCode; }
			set { _DAO.HomePhoneCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.HomePhoneNumber
		/// </summary>
		public String HomePhoneNumber 
		{
			get { return _DAO.HomePhoneNumber; }
			set { _DAO.HomePhoneNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.WorkPhoneCode
		/// </summary>
		public String WorkPhoneCode 
		{
			get { return _DAO.WorkPhoneCode; }
			set { _DAO.WorkPhoneCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.WorkPhoneNumber
		/// </summary>
		public String WorkPhoneNumber 
		{
			get { return _DAO.WorkPhoneNumber; }
			set { _DAO.WorkPhoneNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.CellPhoneNumber
		/// </summary>
		public String CellPhoneNumber 
		{
			get { return _DAO.CellPhoneNumber; }
			set { _DAO.CellPhoneNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.EmailAddress
		/// </summary>
		public String EmailAddress 
		{
			get { return _DAO.EmailAddress; }
			set { _DAO.EmailAddress = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.FaxCode
		/// </summary>
		public String FaxCode 
		{
			get { return _DAO.FaxCode; }
			set { _DAO.FaxCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.FaxNumber
		/// </summary>
		public String FaxNumber 
		{
			get { return _DAO.FaxNumber; }
			set { _DAO.FaxNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.ImportID
		/// </summary>
		public Int32 ImportID 
		{
			get { return _DAO.ImportID; }
			set { _DAO.ImportID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.ImportApplication
		/// </summary>
		public IImportApplication ImportApplication 
		{
			get
			{
				if (null == _DAO.ImportApplication) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IImportApplication, ImportApplication_DAO>(_DAO.ImportApplication);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ImportApplication = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ImportApplication = (ImportApplication_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


