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
	/// SAHL.Common.BusinessModel.DAO.Property_DAO
	/// </summary>
	public partial class Property : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Property_DAO>, IProperty
	{
				public Property(SAHL.Common.BusinessModel.DAO.Property_DAO Property) : base(Property)
		{
			this._DAO = Property;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDescription1
		/// </summary>
		public String PropertyDescription1 
		{
			get { return _DAO.PropertyDescription1; }
			set { _DAO.PropertyDescription1 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDescription2
		/// </summary>
		public String PropertyDescription2 
		{
			get { return _DAO.PropertyDescription2; }
			set { _DAO.PropertyDescription2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDescription3
		/// </summary>
		public String PropertyDescription3 
		{
			get { return _DAO.PropertyDescription3; }
			set { _DAO.PropertyDescription3 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.DeedsOfficeValue
		/// </summary>
		public Double? DeedsOfficeValue
		{
			get { return _DAO.DeedsOfficeValue; }
			set { _DAO.DeedsOfficeValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.CurrentBondDate
		/// </summary>
		public DateTime? CurrentBondDate
		{
			get { return _DAO.CurrentBondDate; }
			set { _DAO.CurrentBondDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.ErfNumber
		/// </summary>
		public String ErfNumber 
		{
			get { return _DAO.ErfNumber; }
			set { _DAO.ErfNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.ErfPortionNumber
		/// </summary>
		public String ErfPortionNumber 
		{
			get { return _DAO.ErfPortionNumber; }
			set { _DAO.ErfPortionNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.SectionalSchemeName
		/// </summary>
		public String SectionalSchemeName 
		{
			get { return _DAO.SectionalSchemeName; }
			set { _DAO.SectionalSchemeName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.SectionalUnitNumber
		/// </summary>
		public String SectionalUnitNumber 
		{
			get { return _DAO.SectionalUnitNumber; }
			set { _DAO.SectionalUnitNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.ErfSuburbDescription
		/// </summary>
		public String ErfSuburbDescription 
		{
			get { return _DAO.ErfSuburbDescription; }
			set { _DAO.ErfSuburbDescription = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.ErfMetroDescription
		/// </summary>
		public String ErfMetroDescription 
		{
			get { return _DAO.ErfMetroDescription; }
			set { _DAO.ErfMetroDescription = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.MortgageLoanProperties
		/// </summary>
		private DAOEventList<FinancialService_DAO, IFinancialService, FinancialService> _MortgageLoanProperties;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.MortgageLoanProperties
		/// </summary>
		public IEventList<IFinancialService> MortgageLoanProperties
		{
			get
			{
				if (null == _MortgageLoanProperties) 
				{
					if(null == _DAO.MortgageLoanProperties)
						_DAO.MortgageLoanProperties = new List<FinancialService_DAO>();
					_MortgageLoanProperties = new DAOEventList<FinancialService_DAO, IFinancialService, FinancialService>(_DAO.MortgageLoanProperties);
					_MortgageLoanProperties.BeforeAdd += new EventListHandler(OnMortgageLoanProperties_BeforeAdd);					
					_MortgageLoanProperties.BeforeRemove += new EventListHandler(OnMortgageLoanProperties_BeforeRemove);					
					_MortgageLoanProperties.AfterAdd += new EventListHandler(OnMortgageLoanProperties_AfterAdd);					
					_MortgageLoanProperties.AfterRemove += new EventListHandler(OnMortgageLoanProperties_AfterRemove);					
				}
				return _MortgageLoanProperties;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyTitleDeeds
		/// </summary>
		private DAOEventList<PropertyTitleDeed_DAO, IPropertyTitleDeed, PropertyTitleDeed> _PropertyTitleDeeds;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyTitleDeeds
		/// </summary>
		public IEventList<IPropertyTitleDeed> PropertyTitleDeeds
		{
			get
			{
				if (null == _PropertyTitleDeeds) 
				{
					if(null == _DAO.PropertyTitleDeeds)
						_DAO.PropertyTitleDeeds = new List<PropertyTitleDeed_DAO>();
					_PropertyTitleDeeds = new DAOEventList<PropertyTitleDeed_DAO, IPropertyTitleDeed, PropertyTitleDeed>(_DAO.PropertyTitleDeeds);
					_PropertyTitleDeeds.BeforeAdd += new EventListHandler(OnPropertyTitleDeeds_BeforeAdd);					
					_PropertyTitleDeeds.BeforeRemove += new EventListHandler(OnPropertyTitleDeeds_BeforeRemove);					
					_PropertyTitleDeeds.AfterAdd += new EventListHandler(OnPropertyTitleDeeds_AfterAdd);					
					_PropertyTitleDeeds.AfterRemove += new EventListHandler(OnPropertyTitleDeeds_AfterRemove);					
				}
				return _PropertyTitleDeeds;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.Valuations
		/// </summary>
		private DAOEventList<Valuation_DAO, IValuation, Valuation> _Valuations;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.Valuations
		/// </summary>
		public IEventList<IValuation> Valuations
		{
			get
			{
				if (null == _Valuations) 
				{
					if(null == _DAO.Valuations)
						_DAO.Valuations = new List<Valuation_DAO>();
					_Valuations = new DAOEventList<Valuation_DAO, IValuation, Valuation>(_DAO.Valuations);
					_Valuations.BeforeAdd += new EventListHandler(OnValuations_BeforeAdd);					
					_Valuations.BeforeRemove += new EventListHandler(OnValuations_BeforeRemove);					
					_Valuations.AfterAdd += new EventListHandler(OnValuations_AfterAdd);					
					_Valuations.AfterRemove += new EventListHandler(OnValuations_AfterRemove);					
				}
				return _Valuations;
			}
		}
		/// <summary>
		/// The address of the Property
		/// </summary>
		public IAddress Address 
		{
			get
			{
				if (null == _DAO.Address) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAddress, Address_DAO>(_DAO.Address);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Address = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Address = (Address_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The Area Classification of property
		/// </summary>
		public IAreaClassification AreaClassification 
		{
			get
			{
				if (null == _DAO.AreaClassification) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAreaClassification, AreaClassification_DAO>(_DAO.AreaClassification);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.AreaClassification = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.AreaClassification = (AreaClassification_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.DeedsPropertyType
		/// </summary>
		public IDeedsPropertyType DeedsPropertyType 
		{
			get
			{
				if (null == _DAO.DeedsPropertyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDeedsPropertyType, DeedsPropertyType_DAO>(_DAO.DeedsPropertyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DeedsPropertyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DeedsPropertyType = (DeedsPropertyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
        
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.OccupancyType
		/// </summary>
		public IOccupancyType OccupancyType 
		{
			get
			{
				if (null == _DAO.OccupancyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOccupancyType, OccupancyType_DAO>(_DAO.OccupancyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OccupancyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OccupancyType = (OccupancyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyType
		/// </summary>
		public IPropertyType PropertyType 
		{
			get
			{
				if (null == _DAO.PropertyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IPropertyType, PropertyType_DAO>(_DAO.PropertyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.PropertyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.PropertyType = (PropertyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.TitleType
		/// </summary>
		public ITitleType TitleType 
		{
			get
			{
				if (null == _DAO.TitleType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITitleType, TitleType_DAO>(_DAO.TitleType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TitleType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TitleType = (TitleType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.DataProvider
		/// </summary>
		public IDataProvider DataProvider 
		{
			get
			{
				if (null == _DAO.DataProvider) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDataProvider, DataProvider_DAO>(_DAO.DataProvider);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DataProvider = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DataProvider = (DataProvider_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDatas
		/// </summary>
		private DAOEventList<PropertyData_DAO, IPropertyData, PropertyData> _PropertyDatas;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Property_DAO.PropertyDatas
		/// </summary>
		public IEventList<IPropertyData> PropertyDatas
		{
			get
			{
				if (null == _PropertyDatas) 
				{
					if(null == _DAO.PropertyDatas)
						_DAO.PropertyDatas = new List<PropertyData_DAO>();
					_PropertyDatas = new DAOEventList<PropertyData_DAO, IPropertyData, PropertyData>(_DAO.PropertyDatas);
					_PropertyDatas.BeforeAdd += new EventListHandler(OnPropertyDatas_BeforeAdd);					
					_PropertyDatas.BeforeRemove += new EventListHandler(OnPropertyDatas_BeforeRemove);					
					_PropertyDatas.AfterAdd += new EventListHandler(OnPropertyDatas_AfterAdd);					
					_PropertyDatas.AfterRemove += new EventListHandler(OnPropertyDatas_AfterRemove);					
				}
				return _PropertyDatas;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_MortgageLoanProperties = null;
			_PropertyTitleDeeds = null;
			_Valuations = null;
			_PropertyDatas = null;
			
		}
	}
}


