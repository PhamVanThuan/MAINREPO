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
	/// ValuationCombinedThatch_DAO stores the Combined Total Thatch Value of the SAHL Manual Valuation where the roof type
		/// is Thatch.
	/// </summary>
	public partial class ValuationCombinedThatch : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationCombinedThatch_DAO>, IValuationCombinedThatch
	{
				public ValuationCombinedThatch(SAHL.Common.BusinessModel.DAO.ValuationCombinedThatch_DAO ValuationCombinedThatch) : base(ValuationCombinedThatch)
		{
			this._DAO = ValuationCombinedThatch;
		}
		/// <summary>
		/// The Combined Total Thatch Value
		/// </summary>
		public Double Value 
		{
			get { return _DAO.Value; }
			set { _DAO.Value = value;}
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
		/// The Combined Total Thatch Value is related to a single Valuation.
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
	}
}


