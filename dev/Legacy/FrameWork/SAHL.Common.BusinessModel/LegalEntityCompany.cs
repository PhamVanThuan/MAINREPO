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
	/// LegalEntityCompany_DAO is derived from the LegalEntity_DAO class and is used to instantiate a Legal Entity of type Company.
	/// </summary>
	public partial class LegalEntityCompany : LegalEntity, ILegalEntityCompany
	{
		protected new SAHL.Common.BusinessModel.DAO.LegalEntityCompany_DAO _DAO;
		public LegalEntityCompany(SAHL.Common.BusinessModel.DAO.LegalEntityCompany_DAO LegalEntityCompany) : base(LegalEntityCompany)
		{
			this._DAO = LegalEntityCompany;
		}
		/// <summary>
		/// The name under which the Company trades, this is not always the same as the Registered Name.
		/// </summary>
		public String TradingName 
		{
			get { return _DAO.TradingName; }
			set { _DAO.TradingName = value;}
		}
	}
}


