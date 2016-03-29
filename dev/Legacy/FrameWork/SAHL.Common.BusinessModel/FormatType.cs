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
	/// SAHL.Common.BusinessModel.DAO.FormatType_DAO
	/// </summary>
	public partial class FormatType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FormatType_DAO>, IFormatType
	{
				public FormatType(SAHL.Common.BusinessModel.DAO.FormatType_DAO FormatType) : base(FormatType)
		{
			this._DAO = FormatType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FormatType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FormatType_DAO.Format
		/// </summary>
		public String Format 
		{
			get { return _DAO.Format; }
			set { _DAO.Format = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FormatType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


