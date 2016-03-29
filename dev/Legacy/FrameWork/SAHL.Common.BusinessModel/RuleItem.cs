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
	/// SAHL.Common.BusinessModel.DAO.RuleItem_DAO
	/// </summary>
	public partial class RuleItem : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RuleItem_DAO>, IRuleItem
	{
				public RuleItem(SAHL.Common.BusinessModel.DAO.RuleItem_DAO RuleItem) : base(RuleItem)
		{
			this._DAO = RuleItem;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.Name
		/// </summary>
		public String Name 
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.GeneralStatusReasonDescription
		/// </summary>
		public String GeneralStatusReasonDescription 
		{
			get { return _DAO.GeneralStatusReasonDescription; }
			set { _DAO.GeneralStatusReasonDescription = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.AssemblyName
		/// </summary>
		public String AssemblyName 
		{
			get { return _DAO.AssemblyName; }
			set { _DAO.AssemblyName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.TypeName
		/// </summary>
		public String TypeName 
		{
			get { return _DAO.TypeName; }
			set { _DAO.TypeName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// A list of parameters that apply to the rule.
		/// </summary>
		private DAOEventList<RuleParameter_DAO, IRuleParameter, RuleParameter> _RuleParameters;
		/// <summary>
		/// A list of parameters that apply to the rule.
		/// </summary>
		public IEventList<IRuleParameter> RuleParameters
		{
			get
			{
				if (null == _RuleParameters) 
				{
					if(null == _DAO.RuleParameters)
						_DAO.RuleParameters = new List<RuleParameter_DAO>();
					_RuleParameters = new DAOEventList<RuleParameter_DAO, IRuleParameter, RuleParameter>(_DAO.RuleParameters);
					_RuleParameters.BeforeAdd += new EventListHandler(OnRuleParameters_BeforeAdd);					
					_RuleParameters.BeforeRemove += new EventListHandler(OnRuleParameters_BeforeRemove);					
					_RuleParameters.AfterAdd += new EventListHandler(OnRuleParameters_AfterAdd);					
					_RuleParameters.AfterRemove += new EventListHandler(OnRuleParameters_AfterRemove);					
				}
				return _RuleParameters;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RuleItem_DAO.EnforceRule
		/// </summary>
		public Boolean EnforceRule 
		{
			get { return _DAO.EnforceRule; }
			set { _DAO.EnforceRule = value;}
		}
		/// <summary>
		/// Foreign Key reference to the GeneralStatus table. Rules that are marked as Inactive should not be executed by 
		/// the domain.
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_RuleParameters = null;
			
		}
	}
}


