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
	/// SAHL.Common.BusinessModel.DAO.Language_DAO
	/// </summary>
	public partial class Language : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Language_DAO>, ILanguage
	{
				public Language(SAHL.Common.BusinessModel.DAO.Language_DAO Language) : base(Language)
		{
			this._DAO = Language;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Language_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Language_DAO.Translatable
		/// </summary>
		public Boolean Translatable 
		{
			get { return _DAO.Translatable; }
			set { _DAO.Translatable = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Language_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


