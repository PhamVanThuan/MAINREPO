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
	/// 
	/// </summary>
	public partial class ImportProperty : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportProperty_DAO>, IImportProperty
	{
				public ImportProperty(SAHL.Common.BusinessModel.DAO.ImportProperty_DAO ImportProperty) : base(ImportProperty)
		{
			this._DAO = ImportProperty;
		}
		/// <summary>
		/// 
		/// </summary>
		public String PropertyTypeKey 
		{
			get { return _DAO.PropertyTypeKey; }
			set { _DAO.PropertyTypeKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String TitleTypeKey 
		{
			get { return _DAO.TitleTypeKey; }
			set { _DAO.TitleTypeKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String AreaClassificationKey 
		{
			get { return _DAO.AreaClassificationKey; }
			set { _DAO.AreaClassificationKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String OccupancyTypeKey 
		{
			get { return _DAO.OccupancyTypeKey; }
			set { _DAO.OccupancyTypeKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String PropertyDescription1 
		{
			get { return _DAO.PropertyDescription1; }
			set { _DAO.PropertyDescription1 = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String PropertyDescription2 
		{
			get { return _DAO.PropertyDescription2; }
			set { _DAO.PropertyDescription2 = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String PropertyDescription3 
		{
			get { return _DAO.PropertyDescription3; }
			set { _DAO.PropertyDescription3 = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double DeedsOfficeValue 
		{
			get { return _DAO.DeedsOfficeValue; }
			set { _DAO.DeedsOfficeValue = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CurrentBondDate
		{
			get { return _DAO.CurrentBondDate; }
			set { _DAO.CurrentBondDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ErfNumber 
		{
			get { return _DAO.ErfNumber; }
			set { _DAO.ErfNumber = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ErfPortionNumber 
		{
			get { return _DAO.ErfPortionNumber; }
			set { _DAO.ErfPortionNumber = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String SectionalSchemeName 
		{
			get { return _DAO.SectionalSchemeName; }
			set { _DAO.SectionalSchemeName = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String SectionalUnitNumber 
		{
			get { return _DAO.SectionalUnitNumber; }
			set { _DAO.SectionalUnitNumber = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String DeedsPropertyTypeKey 
		{
			get { return _DAO.DeedsPropertyTypeKey; }
			set { _DAO.DeedsPropertyTypeKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ErfSuburbDescription 
		{
			get { return _DAO.ErfSuburbDescription; }
			set { _DAO.ErfSuburbDescription = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String ErfMetroDescription 
		{
			get { return _DAO.ErfMetroDescription; }
			set { _DAO.ErfMetroDescription = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String TitleDeedNumber 
		{
			get { return _DAO.TitleDeedNumber; }
			set { _DAO.TitleDeedNumber = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// 
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


