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
	/// SAHL.Common.BusinessModel.DAO.DocumentGroup_DAO
	/// </summary>
	public partial class DocumentGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DocumentGroup_DAO>, IDocumentGroup
	{
				public DocumentGroup(SAHL.Common.BusinessModel.DAO.DocumentGroup_DAO DocumentGroup) : base(DocumentGroup)
		{
			this._DAO = DocumentGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


