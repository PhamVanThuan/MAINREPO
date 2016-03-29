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
	/// SAHL.Common.BusinessModel.DAO.Lookup_DAO
	/// </summary>
	public partial class Lookup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Lookup_DAO>, ILookup
	{
		public Lookup(SAHL.Common.BusinessModel.DAO.Lookup_DAO Lookup) : base(Lookup)
		{
			this._DAO = Lookup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Lookup_DAO.STORid
		/// </summary>
		public Decimal STORid 
		{
			get { return _DAO.STORid; }
			set { _DAO.STORid = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Lookup_DAO.Field
		/// </summary>
		public String Field 
		{
			get { return _DAO.Field; }
			set { _DAO.Field = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Lookup_DAO.Text
		/// </summary>
		public String Text 
		{
			get { return _DAO.Text; }
			set { _DAO.Text = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Lookup_DAO.Key
		/// </summary>
		public Decimal Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


