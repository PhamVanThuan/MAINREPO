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
	/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO
	/// </summary>
	public partial class MortgageLoanPurposeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO>, IMortgageLoanPurposeGroup
	{
				public MortgageLoanPurposeGroup(SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO MortgageLoanPurposeGroup) : base(MortgageLoanPurposeGroup)
		{
			this._DAO = MortgageLoanPurposeGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO.MortgageLoanPurposes
		/// </summary>
		private DAOEventList<MortgageLoanPurpose_DAO, IMortgageLoanPurpose, MortgageLoanPurpose> _MortgageLoanPurposes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurposeGroup_DAO.MortgageLoanPurposes
		/// </summary>
		public IEventList<IMortgageLoanPurpose> MortgageLoanPurposes
		{
			get
			{
				if (null == _MortgageLoanPurposes) 
				{
					if(null == _DAO.MortgageLoanPurposes)
						_DAO.MortgageLoanPurposes = new List<MortgageLoanPurpose_DAO>();
					_MortgageLoanPurposes = new DAOEventList<MortgageLoanPurpose_DAO, IMortgageLoanPurpose, MortgageLoanPurpose>(_DAO.MortgageLoanPurposes);
					_MortgageLoanPurposes.BeforeAdd += new EventListHandler(OnMortgageLoanPurposes_BeforeAdd);					
					_MortgageLoanPurposes.BeforeRemove += new EventListHandler(OnMortgageLoanPurposes_BeforeRemove);					
					_MortgageLoanPurposes.AfterAdd += new EventListHandler(OnMortgageLoanPurposes_AfterAdd);					
					_MortgageLoanPurposes.AfterRemove += new EventListHandler(OnMortgageLoanPurposes_AfterRemove);					
				}
				return _MortgageLoanPurposes;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_MortgageLoanPurposes = null;
			
		}
	}
}


