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
	/// ValuationImprovementType_DAO describes the different types of improvements which can be captured against a SAHL Manual
		/// Valuation.
	/// </summary>
	public partial class ValuationImprovementType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationImprovementType_DAO>, IValuationImprovementType
	{
				public ValuationImprovementType(SAHL.Common.BusinessModel.DAO.ValuationImprovementType_DAO ValuationImprovementType) : base(ValuationImprovementType)
		{
			this._DAO = ValuationImprovementType;
		}
		/// <summary>
		/// The Improvement Description.
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


