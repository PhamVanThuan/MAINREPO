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
	/// Valuation_DAO stores the information on a valuation for a property. It is discriminated using the ValuationDataProviderDataService
		/// which allows currently:
		/// 
		/// SAHL Client Estimate
		/// 
		/// SAHL Manual Valuation
		/// 
		/// Lightstone Automated Valuation
		/// 
		/// AdCheck Desktop Valuation
		/// 
		/// AdCheck Physical Valuation
	/// </summary>
	public abstract partial class Valuation : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Valuation_DAO>, IValuation
	{
				public Valuation(SAHL.Common.BusinessModel.DAO.Valuation_DAO Valuation) : base(Valuation)
		{
			this._DAO = Valuation;
		}
		/// <summary>
		/// The date on which the Valuation is completed.
		/// </summary>
		public DateTime ValuationDate 
		{
			get { return _DAO.ValuationDate; }
			set { _DAO.ValuationDate = value;}
		}
		/// <summary>
		/// The Total Valuation Amount.
		/// </summary>
		public Double? ValuationAmount
		{
			get { return _DAO.ValuationAmount; }
			set { _DAO.ValuationAmount = value;}
		}
		/// <summary>
		/// The replacement HOC Total Valuation Amount
		/// </summary>
		public Double? ValuationHOCValue
		{
			get { return _DAO.ValuationHOCValue; }
			set { _DAO.ValuationHOCValue = value;}
		}
		/// <summary>
		/// The Municipal Valuation as provided by manual municipal enquiry.
		/// </summary>
		public Double? ValuationMunicipal
		{
			get { return _DAO.ValuationMunicipal; }
			set { _DAO.ValuationMunicipal = value;}
		}
		/// <summary>
		/// The Valuation User who captured the Valuation.
		/// </summary>
		public String ValuationUserID 
		{
			get { return _DAO.ValuationUserID; }
			set { _DAO.ValuationUserID = value;}
		}
		/// <summary>
		/// The HOC Thatch Roof Type Total value.
		/// </summary>
		public Double? HOCThatchAmount
		{
			get { return _DAO.HOCThatchAmount; }
			set { _DAO.HOCThatchAmount = value;}
		}
		/// <summary>
		/// The HOC Conventional Roof Type Total value.
		/// </summary>
		public Double? HOCConventionalAmount
		{
			get { return _DAO.HOCConventionalAmount; }
			set { _DAO.HOCConventionalAmount = value;}
		}
		/// <summary>
		/// The HOC Shingle Roof Type Total value.
		/// </summary>
		public Double? HOCShingleAmount
		{
			get { return _DAO.HOCShingleAmount; }
			set { _DAO.HOCShingleAmount = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The foreign reference to the Property table. Each Valuation belongs to a single Property.
		/// </summary>
		public IProperty Property 
		{
			get
			{
				if (null == _DAO.Property) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProperty, Property_DAO>(_DAO.Property);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Property = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Property = (Property_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The foreign reference to the Valuator table. Each Valuation is carried out by a single Valuator. The Valuator details
		/// are stored as Legal Entity details.
		/// </summary>
		public IValuator Valuator 
		{
			get
			{
				if (null == _DAO.Valuator) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuator, Valuator_DAO>(_DAO.Valuator);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Valuator = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Valuator = (Valuator_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public IValuationClassification ValuationClassification 
		{
			get
			{
				if (null == _DAO.ValuationClassification) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuationClassification, ValuationClassification_DAO>(_DAO.ValuationClassification);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValuationClassification = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValuationClassification = (ValuationClassification_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The HOC escalation percentage applied to the HOC Valuation portions of this Valuation.
		/// </summary>
		public Double? ValuationEscalationPercentage
		{
			get { return _DAO.ValuationEscalationPercentage; }
			set { _DAO.ValuationEscalationPercentage = value;}
		}
		/// <summary>
		/// The foreign key reference to the ValuationStatus table. Each Valuation is assigned a status of pending or complete. The
		/// status is updated via the X2 workflow.
		/// </summary>
		public IValuationStatus ValuationStatus 
		{
			get
			{
				if (null == _DAO.ValuationStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuationStatus, ValuationStatus_DAO>(_DAO.ValuationStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValuationStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValuationStatus = (ValuationStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// This is the XML data detailing the valuation. Its structure is specific to each of the discriminations and in the case
		/// of Lightstone and AdCheck are the resulting XML documents received via the respective web services for a completed Valuation.
		/// </summary>
		public String Data 
		{
			get { return _DAO.Data; }
			set { _DAO.Data = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Valuation_DAO.ValuationDataProviderDataService
		/// </summary>
		public IValuationDataProviderDataService ValuationDataProviderDataService 
		{
			get
			{
				if (null == _DAO.ValuationDataProviderDataService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuationDataProviderDataService, ValuationDataProviderDataService_DAO>(_DAO.ValuationDataProviderDataService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValuationDataProviderDataService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValuationDataProviderDataService = (ValuationDataProviderDataService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// This is a Valuation flag which is set by business rules (can be overridden manually). A property can have many completed 
		/// valuations against it, from many sources, but only one active Valuation which is what this flag indicates.
		/// </summary>
		public Boolean IsActive 
		{
			get { return _DAO.IsActive; }
			set { _DAO.IsActive = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Valuation_DAO.HOCRoof
		/// </summary>
		public IHOCRoof HOCRoof 
		{
			get
			{
				if (null == _DAO.HOCRoof) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCRoof, HOCRoof_DAO>(_DAO.HOCRoof);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCRoof = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCRoof = (HOCRoof_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


