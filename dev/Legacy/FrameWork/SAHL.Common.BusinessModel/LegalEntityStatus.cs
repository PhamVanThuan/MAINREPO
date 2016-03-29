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
	/// LegalEntityStatus_DAO is used to store the different statuses that a Legal Entity can be in.
	/// </summary>
	public partial class LegalEntityStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityStatus_DAO>, ILegalEntityStatus
	{
				public LegalEntityStatus(SAHL.Common.BusinessModel.DAO.LegalEntityStatus_DAO LegalEntityStatus) : base(LegalEntityStatus)
		{
			this._DAO = LegalEntityStatus;
		}
		/// <summary>
		/// The description of the Legal Entity Status. e.g. Alive/Deceased/Disabled
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


