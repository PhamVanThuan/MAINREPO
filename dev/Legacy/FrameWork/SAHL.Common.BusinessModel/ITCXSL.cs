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
	/// SAHL.Common.BusinessModel.DAO.ITCXSL_DAO
	/// </summary>
	public partial class ITCXSL : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ITCXSL_DAO>, IITCXSL
	{
				public ITCXSL(SAHL.Common.BusinessModel.DAO.ITCXSL_DAO ITCXSL) : base(ITCXSL)
		{
			this._DAO = ITCXSL;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCXSL_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCXSL_DAO.EffectiveDate
		/// </summary>
		public DateTime EffectiveDate 
		{
			get { return _DAO.EffectiveDate; }
			set { _DAO.EffectiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITCXSL_DAO.StyleSheet
		/// </summary>
		public String StyleSheet 
		{
			get { return _DAO.StyleSheet; }
			set { _DAO.StyleSheet = value;}
		}
	}
}


