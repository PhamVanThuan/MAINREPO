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
	/// ValuationCottage_DAO describes the Extent, Roof Type and rate/sq m of a Cottage for a SAHL Manual Valuation.
	/// </summary>
	public partial class ValuationCottage : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationCottage_DAO>, IValuationCottage
	{
				public ValuationCottage(SAHL.Common.BusinessModel.DAO.ValuationCottage_DAO ValuationCottage) : base(ValuationCottage)
		{
			this._DAO = ValuationCottage;
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
		/// The size, in sq m, of the cottage.
		/// </summary>
		public Double? Extent
		{
			get { return _DAO.Extent; }
			set { _DAO.Extent = value;}
		}
		/// <summary>
		/// The replacement value per sq m for the Cottage.
		/// </summary>
		public Double? Rate
		{
			get { return _DAO.Rate; }
			set { _DAO.Rate = value;}
		}
		/// <summary>
		/// The Cottage is related to a single Valuation.
		/// </summary>
		public IValuation Valuation 
		{
			get
			{
				if (null == _DAO.Valuation) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuation, Valuation_DAO>(_DAO.Valuation);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Valuation = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Valuation = (Valuation_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The foreign key reference to the ValuationRoofType table.
		/// </summary>
		public IValuationRoofType ValuationRoofType 
		{
			get
			{
				if (null == _DAO.ValuationRoofType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuationRoofType, ValuationRoofType_DAO>(_DAO.ValuationRoofType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValuationRoofType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValuationRoofType = (ValuationRoofType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


