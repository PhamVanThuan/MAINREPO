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
	/// SAHL.Common.BusinessModel.DAO.DeedsOffice_DAO
	/// </summary>
	public partial class DeedsOffice : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DeedsOffice_DAO>, IDeedsOffice
	{
				public DeedsOffice(SAHL.Common.BusinessModel.DAO.DeedsOffice_DAO DeedsOffice) : base(DeedsOffice)
		{
			this._DAO = DeedsOffice;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DeedsOffice_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DeedsOffice_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DeedsOffice_DAO.Attorneys
		/// </summary>
		private DAOEventList<Attorney_DAO, IAttorney, Attorney> _Attorneys;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DeedsOffice_DAO.Attorneys
		/// </summary>
		public IEventList<IAttorney> Attorneys
		{
			get
			{
				if (null == _Attorneys) 
				{
					if(null == _DAO.Attorneys)
						_DAO.Attorneys = new List<Attorney_DAO>();
					_Attorneys = new DAOEventList<Attorney_DAO, IAttorney, Attorney>(_DAO.Attorneys);
					_Attorneys.BeforeAdd += new EventListHandler(OnAttorneys_BeforeAdd);					
					_Attorneys.BeforeRemove += new EventListHandler(OnAttorneys_BeforeRemove);					
					_Attorneys.AfterAdd += new EventListHandler(OnAttorneys_AfterAdd);					
					_Attorneys.AfterRemove += new EventListHandler(OnAttorneys_AfterRemove);					
				}
				return _Attorneys;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_Attorneys = null;
			
		}
	}
}


