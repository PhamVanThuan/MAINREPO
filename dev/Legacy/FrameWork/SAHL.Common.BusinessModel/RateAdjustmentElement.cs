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
	/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO
	/// </summary>
	public partial class RateAdjustmentElement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO>, IRateAdjustmentElement
	{
				public RateAdjustmentElement(SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO RateAdjustmentElement) : base(RateAdjustmentElement)
		{
			this._DAO = RateAdjustmentElement;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.ElementMinValue
		/// </summary>
		public Double? ElementMinValue
		{
			get { return _DAO.ElementMinValue; }
			set { _DAO.ElementMinValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.ElementMaxValue
		/// </summary>
		public Double? ElementMaxValue
		{
			get { return _DAO.ElementMaxValue; }
			set { _DAO.ElementMaxValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.ElementText
		/// </summary>
		public String ElementText 
		{
			get { return _DAO.ElementText; }
			set { _DAO.ElementText = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.RateAdjustmentValue
		/// </summary>
		public Double RateAdjustmentValue 
		{
			get { return _DAO.RateAdjustmentValue; }
			set { _DAO.RateAdjustmentValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.EffectiveDate
		/// </summary>
		public DateTime EffectiveDate 
		{
			get { return _DAO.EffectiveDate; }
			set { _DAO.EffectiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.GenericKeyType
		/// </summary>
		public IGenericKeyType GenericKeyType 
		{
			get
			{
				if (null == _DAO.GenericKeyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GenericKeyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.RateAdjustmentElementType
		/// </summary>
		public IRateAdjustmentElementType RateAdjustmentElementType 
		{
			get
			{
				if (null == _DAO.RateAdjustmentElementType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRateAdjustmentElementType, RateAdjustmentElementType_DAO>(_DAO.RateAdjustmentElementType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RateAdjustmentElementType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RateAdjustmentElementType = (RateAdjustmentElementType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.RateAdjustmentGroup
		/// </summary>
		public IRateAdjustmentGroup RateAdjustmentGroup 
		{
			get
			{
				if (null == _DAO.RateAdjustmentGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRateAdjustmentGroup, RateAdjustmentGroup_DAO>(_DAO.RateAdjustmentGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RateAdjustmentGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RateAdjustmentGroup = (RateAdjustmentGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.FinancialAdjustmentTypeSource
		/// </summary>
		public IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource 
		{
			get
			{
				if (null == _DAO.FinancialAdjustmentTypeSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustmentTypeSource, FinancialAdjustmentTypeSource_DAO>(_DAO.FinancialAdjustmentTypeSource);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustmentTypeSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustmentTypeSource = (FinancialAdjustmentTypeSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RateAdjustmentElement_DAO.RuleItem
		/// </summary>
		public IRuleItem RuleItem 
		{
			get
			{
				if (null == _DAO.RuleItem) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRuleItem, RuleItem_DAO>(_DAO.RuleItem);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RuleItem = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RuleItem = (RuleItem_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


