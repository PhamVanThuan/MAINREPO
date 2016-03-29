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
	/// ValuationClassification_DAO is the Property Classification captured during a SAHL Manual Valuation.
	/// </summary>
	public partial class ValuationClassification : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationClassification_DAO>, IValuationClassification
	{
				public ValuationClassification(SAHL.Common.BusinessModel.DAO.ValuationClassification_DAO ValuationClassification) : base(ValuationClassification)
		{
			this._DAO = ValuationClassification;
		}
		/// <summary>
		/// The Description of the ValuationClassification_DAO. e.g. Budget Standard.
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


