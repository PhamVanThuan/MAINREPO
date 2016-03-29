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
	/// Groups rule exclusions into a set so that rules can be excluded during validation.  A set 
		/// will contain multiple RuleExclusions.
	/// </summary>
	public partial class RuleExclusionSet : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RuleExclusionSet_DAO>, IRuleExclusionSet
	{
				public RuleExclusionSet(SAHL.Common.BusinessModel.DAO.RuleExclusionSet_DAO RuleExclusionSet) : base(RuleExclusionSet)
		{
			this._DAO = RuleExclusionSet;
		}
		/// <summary>
		/// The description of the RuleExclusionSet.  This is a unique name.
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// Describes what the exclusion set is used for.
		/// </summary>
		public String Comment 
		{
			get { return _DAO.Comment; }
			set { _DAO.Comment = value;}
		}
		/// <summary>
		/// Primary key.
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// Gets a list of RuleExclusion objects that fall under the set.
		/// </summary>
		private DAOEventList<RuleExclusion_DAO, IRuleExclusion, RuleExclusion> _RuleExclusions;
		/// <summary>
		/// Gets a list of RuleExclusion objects that fall under the set.
		/// </summary>
		public IEventList<IRuleExclusion> RuleExclusions
		{
			get
			{
				if (null == _RuleExclusions) 
				{
					if(null == _DAO.RuleExclusions)
						_DAO.RuleExclusions = new List<RuleExclusion_DAO>();
					_RuleExclusions = new DAOEventList<RuleExclusion_DAO, IRuleExclusion, RuleExclusion>(_DAO.RuleExclusions);
					_RuleExclusions.BeforeAdd += new EventListHandler(OnRuleExclusions_BeforeAdd);					
					_RuleExclusions.BeforeRemove += new EventListHandler(OnRuleExclusions_BeforeRemove);					
					_RuleExclusions.AfterAdd += new EventListHandler(OnRuleExclusions_AfterAdd);					
					_RuleExclusions.AfterRemove += new EventListHandler(OnRuleExclusions_AfterRemove);					
				}
				return _RuleExclusions;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_RuleExclusions = null;
			
		}
	}
}


