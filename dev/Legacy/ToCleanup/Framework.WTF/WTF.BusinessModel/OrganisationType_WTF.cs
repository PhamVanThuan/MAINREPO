
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;

using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.OrganisationType_DAO
	/// </summary>
    public partial class OrganisationType_WTF : BusinessModelBase<OrganisationType_WTF_DAO>, IOrganisationType
	{
        public OrganisationType_WTF(OrganisationType_WTF_DAO OrganisationType_WTF) : base(OrganisationType_WTF)
		{
            this._DAO = OrganisationType_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}



