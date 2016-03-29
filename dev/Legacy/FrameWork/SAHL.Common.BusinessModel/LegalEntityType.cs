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
	/// LegalEntityType_DAO is used in order to store the different Legal Entity types that exist. The LegalEntityType forms
		/// the basis for the Legal Entity discrimination.
	/// </summary>
	public partial class LegalEntityType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityType_DAO>, ILegalEntityType
	{
				public LegalEntityType(SAHL.Common.BusinessModel.DAO.LegalEntityType_DAO LegalEntityType) : base(LegalEntityType)
		{
			this._DAO = LegalEntityType;
		}
		/// <summary>
		/// The Legal Entity Type Description. e.g. Natural Person/Trust
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


