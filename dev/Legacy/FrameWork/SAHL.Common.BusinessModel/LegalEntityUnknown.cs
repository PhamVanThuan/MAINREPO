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
	/// LegalEntityUnknown_DAO is derived from the LegalEntity_DAO class. It is used to instantiate a Legal Entity of type "Unknown".
	/// </summary>
	public partial class LegalEntityUnknown : LegalEntity, ILegalEntityUnknown
	{
		protected new SAHL.Common.BusinessModel.DAO.LegalEntityUnknown_DAO _DAO;
		public LegalEntityUnknown(SAHL.Common.BusinessModel.DAO.LegalEntityUnknown_DAO LegalEntityUnknown) : base(LegalEntityUnknown)
		{
			this._DAO = LegalEntityUnknown;
		}
	}
}


