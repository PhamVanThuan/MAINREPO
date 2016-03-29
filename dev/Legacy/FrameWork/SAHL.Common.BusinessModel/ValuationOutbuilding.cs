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
	/// ValuationOutbuilding_DAO describes the extent, rate and roof type of the Outbuilding associated to a SAHL Manual Valuation.
	/// </summary>
	public partial class ValuationOutbuilding : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationOutbuilding_DAO>, IValuationOutbuilding
	{
				public ValuationOutbuilding(SAHL.Common.BusinessModel.DAO.ValuationOutbuilding_DAO ValuationOutbuilding) : base(ValuationOutbuilding)
		{
			this._DAO = ValuationOutbuilding;
		}
		/// <summary>
		/// The size, in sq m, of the Outbuilding.
		/// </summary>
		public Double? Extent
		{
			get { return _DAO.Extent; }
			set { _DAO.Extent = value;}
		}
		/// <summary>
		/// The replacement value per sq m for the Outbuilding.
		/// </summary>
		public Double? Rate
		{
			get { return _DAO.Rate; }
			set { _DAO.Rate = value;}
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
		/// Foreign key reference to the Valuation table. Each Outbuilding can only belong to a single valuation.
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
		/// Foreign key reference to the ValuationRoofType table. Each Outbuilding can only belong to one ValuationRoofType.
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


