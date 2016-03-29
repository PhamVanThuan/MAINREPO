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
	/// ValuationImprovement_DAO describes improvements, the extent (where applicable) and the replacement rate/value
		/// of the improvements captured for a SAHL Manual Valuation.
	/// </summary>
	public partial class ValuationImprovement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationImprovement_DAO>, IValuationImprovement
	{
				public ValuationImprovement(SAHL.Common.BusinessModel.DAO.ValuationImprovement_DAO ValuationImprovement) : base(ValuationImprovement)
		{
			this._DAO = ValuationImprovement;
		}
		/// <summary>
		/// The date on which the Improvement was added.
		/// </summary>
		public DateTime? ImprovementDate
		{
			get { return _DAO.ImprovementDate; }
			set { _DAO.ImprovementDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ValuationImprovement_DAO.ImprovementValue
		/// </summary>
		public Double ImprovementValue 
		{
			get { return _DAO.ImprovementValue; }
			set { _DAO.ImprovementValue = value;}
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
		/// An Improvement may only be related to a single Valuation.
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
		/// The foreign key reference to the ValuationImprovementType table. Each Improvement requires a Type.
		/// </summary>
		public IValuationImprovementType ValuationImprovementType 
		{
			get
			{
				if (null == _DAO.ValuationImprovementType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IValuationImprovementType, ValuationImprovementType_DAO>(_DAO.ValuationImprovementType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ValuationImprovementType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ValuationImprovementType = (ValuationImprovementType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


