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
	/// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO
	/// </summary>
	public partial class HeaderIconType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO>, IHeaderIconType
	{
				public HeaderIconType(SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO HeaderIconType) : base(HeaderIconType)
		{
			this._DAO = HeaderIconType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.Icon
		/// </summary>
		public String Icon 
		{
			get { return _DAO.Icon; }
			set { _DAO.Icon = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.HeaderIconDetails
		/// </summary>
		private DAOEventList<HeaderIconDetails_DAO, IHeaderIconDetails, HeaderIconDetails> _HeaderIconDetails;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HeaderIconType_DAO.HeaderIconDetails
		/// </summary>
		public IEventList<IHeaderIconDetails> HeaderIconDetails
		{
			get
			{
				if (null == _HeaderIconDetails) 
				{
					if(null == _DAO.HeaderIconDetails)
						_DAO.HeaderIconDetails = new List<HeaderIconDetails_DAO>();
					_HeaderIconDetails = new DAOEventList<HeaderIconDetails_DAO, IHeaderIconDetails, HeaderIconDetails>(_DAO.HeaderIconDetails);
					_HeaderIconDetails.BeforeAdd += new EventListHandler(OnHeaderIconDetails_BeforeAdd);					
					_HeaderIconDetails.BeforeRemove += new EventListHandler(OnHeaderIconDetails_BeforeRemove);					
					_HeaderIconDetails.AfterAdd += new EventListHandler(OnHeaderIconDetails_AfterAdd);					
					_HeaderIconDetails.AfterRemove += new EventListHandler(OnHeaderIconDetails_AfterRemove);					
				}
				return _HeaderIconDetails;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_HeaderIconDetails = null;
			
		}
	}
}


