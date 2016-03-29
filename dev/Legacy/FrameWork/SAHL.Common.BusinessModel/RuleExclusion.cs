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
	/// SAHL.Common.BusinessModel.DAO.RuleExclusion_DAO
	/// </summary>
	public partial class RuleExclusion : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RuleExclusion_DAO>, IRuleExclusion
	{
				public RuleExclusion(SAHL.Common.BusinessModel.DAO.RuleExclusion_DAO RuleExclusion) : base(RuleExclusion)
		{
			this._DAO = RuleExclusion;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleExclusion_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleExclusion_DAO.RuleExclusionSet
		/// </summary>
		public IRuleExclusionSet RuleExclusionSet 
		{
			get
			{
				if (null == _DAO.RuleExclusionSet) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRuleExclusionSet, RuleExclusionSet_DAO>(_DAO.RuleExclusionSet);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RuleExclusionSet = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RuleExclusionSet = (RuleExclusionSet_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The key of the  object.  This is declared as an int for 
		/// performance reasons - the RuleItem is not usually required for exclusions as all 
		/// we're interested in is the key.  If you require the item, load it.
		/// </summary>
		public Int32 RuleItemKey 
		{
			get { return _DAO.RuleItemKey; }
			set { _DAO.RuleItemKey = value;}
		}
	}
}


