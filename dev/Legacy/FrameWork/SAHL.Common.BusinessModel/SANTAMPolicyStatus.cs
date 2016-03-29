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
	/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyStatus_DAO
	/// </summary>
	public partial class SANTAMPolicyStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SANTAMPolicyStatus_DAO>, ISANTAMPolicyStatus
	{
				public SANTAMPolicyStatus(SAHL.Common.BusinessModel.DAO.SANTAMPolicyStatus_DAO SANTAMPolicyStatus) : base(SANTAMPolicyStatus)
		{
			this._DAO = SANTAMPolicyStatus;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyStatus_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyStatus_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyStatus_DAO.SANTAMPolicyTrackings
		/// </summary>
		private DAOEventList<SANTAMPolicyTracking_DAO, ISANTAMPolicyTracking, SANTAMPolicyTracking> _SANTAMPolicyTrackings;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SANTAMPolicyStatus_DAO.SANTAMPolicyTrackings
		/// </summary>
		public IEventList<ISANTAMPolicyTracking> SANTAMPolicyTrackings
		{
			get
			{
				if (null == _SANTAMPolicyTrackings) 
				{
					if(null == _DAO.SANTAMPolicyTrackings)
						_DAO.SANTAMPolicyTrackings = new List<SANTAMPolicyTracking_DAO>();
					_SANTAMPolicyTrackings = new DAOEventList<SANTAMPolicyTracking_DAO, ISANTAMPolicyTracking, SANTAMPolicyTracking>(_DAO.SANTAMPolicyTrackings);
					_SANTAMPolicyTrackings.BeforeAdd += new EventListHandler(OnSANTAMPolicyTrackings_BeforeAdd);					
					_SANTAMPolicyTrackings.BeforeRemove += new EventListHandler(OnSANTAMPolicyTrackings_BeforeRemove);					
					_SANTAMPolicyTrackings.AfterAdd += new EventListHandler(OnSANTAMPolicyTrackings_AfterAdd);					
					_SANTAMPolicyTrackings.AfterRemove += new EventListHandler(OnSANTAMPolicyTrackings_AfterRemove);					
				}
				return _SANTAMPolicyTrackings;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_SANTAMPolicyTrackings = null;
			
		}
	}
}


