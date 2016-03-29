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
	/// LegalEntityRelationshipType_DAO is used to store the different relationship types that can exist between Legal Entities.
	/// </summary>
	public partial class LegalEntityRelationshipType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityRelationshipType_DAO>, ILegalEntityRelationshipType
	{
				public LegalEntityRelationshipType(SAHL.Common.BusinessModel.DAO.LegalEntityRelationshipType_DAO LegalEntityRelationshipType) : base(LegalEntityRelationshipType)
		{
			this._DAO = LegalEntityRelationshipType;
		}
		/// <summary>
		/// The description of the relationship. e.g. Spouse/Partner
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


