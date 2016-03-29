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
	/// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO
	/// </summary>
	public partial class LifeCommissionTargets : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO>, ILifeCommissionTargets
	{
				public LifeCommissionTargets(SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO LifeCommissionTargets) : base(LifeCommissionTargets)
		{
			this._DAO = LifeCommissionTargets;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.Consultant
		/// </summary>
		public String Consultant 
		{
			get { return _DAO.Consultant; }
			set { _DAO.Consultant = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.EffectiveYear
		/// </summary>
		public Int32? EffectiveYear
		{
			get { return _DAO.EffectiveYear; }
			set { _DAO.EffectiveYear = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.EffectiveMonth
		/// </summary>
		public Int32? EffectiveMonth
		{
			get { return _DAO.EffectiveMonth; }
			set { _DAO.EffectiveMonth = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.TargetPolicies
		/// </summary>
		public Int32? TargetPolicies
		{
			get { return _DAO.TargetPolicies; }
			set { _DAO.TargetPolicies = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.MinPoliciesToQualify
		/// </summary>
		public Int32? MinPoliciesToQualify
		{
			get { return _DAO.MinPoliciesToQualify; }
			set { _DAO.MinPoliciesToQualify = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionTargets_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


