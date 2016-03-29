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
	/// LegalEntityCloseCorporation_DAO is derived from LegalEntity_DAO and is used to instantiate a Legal Entity of type Close
		/// Corporation.
	/// </summary>
	public partial class LegalEntityCloseCorporation : LegalEntity, ILegalEntityCloseCorporation
	{
		protected new SAHL.Common.BusinessModel.DAO.LegalEntityCloseCorporation_DAO _DAO;
		public LegalEntityCloseCorporation(SAHL.Common.BusinessModel.DAO.LegalEntityCloseCorporation_DAO LegalEntityCloseCorporation) : base(LegalEntityCloseCorporation)
		{
			this._DAO = LegalEntityCloseCorporation;
		}
		/// <summary>
		/// The name under which the Close Corporation trades, this is not always the same as the Registered Name.
		/// </summary>
		public String TradingName 
		{
			get { return _DAO.TradingName; }
			set { _DAO.TradingName = value;}
		}
	}
}


