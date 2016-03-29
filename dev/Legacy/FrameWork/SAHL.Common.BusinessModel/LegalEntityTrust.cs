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
	/// LegalEntityTrust_DAO is derived from the LegalEntity_DAO class. It is used to instantiate a Legal Entity of type "Trust".
	/// </summary>
	public partial class LegalEntityTrust : LegalEntity, ILegalEntityTrust
	{
		protected new SAHL.Common.BusinessModel.DAO.LegalEntityTrust_DAO _DAO;
		public LegalEntityTrust(SAHL.Common.BusinessModel.DAO.LegalEntityTrust_DAO LegalEntityTrust) : base(LegalEntityTrust)
		{
			this._DAO = LegalEntityTrust;
		}
		/// <summary>
		/// The Trading Name of the Trust.
		/// </summary>
		public String TradingName 
		{
			get { return _DAO.TradingName; }
			set { _DAO.TradingName = value;}
		}
	}
}


