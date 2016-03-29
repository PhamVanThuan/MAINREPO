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
	/// SAHL.Common.BusinessModel.DAO.HOCInsurer_DAO
	/// </summary>
	public partial class HOCInsurer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HOCInsurer_DAO>, IHOCInsurer
	{
				public HOCInsurer(SAHL.Common.BusinessModel.DAO.HOCInsurer_DAO HOCInsurer) : base(HOCInsurer)
		{
			this._DAO = HOCInsurer;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCInsurer_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCInsurer_DAO.HOCInsurerStatus
		/// </summary>
		public Int16? HOCInsurerStatus
		{
			get { return _DAO.HOCInsurerStatus; }
			set { _DAO.HOCInsurerStatus = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCInsurer_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCInsurer_DAO.HOCRates
		/// </summary>
		private DAOEventList<HOCRates_DAO, IHOCRates, HOCRates> _HOCRates;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCInsurer_DAO.HOCRates
		/// </summary>
		public IEventList<IHOCRates> HOCRates
		{
			get
			{
				if (null == _HOCRates) 
				{
					if(null == _DAO.HOCRates)
						_DAO.HOCRates = new List<HOCRates_DAO>();
					_HOCRates = new DAOEventList<HOCRates_DAO, IHOCRates, HOCRates>(_DAO.HOCRates);
					_HOCRates.BeforeAdd += new EventListHandler(OnHOCRates_BeforeAdd);					
					_HOCRates.BeforeRemove += new EventListHandler(OnHOCRates_BeforeRemove);					
					_HOCRates.AfterAdd += new EventListHandler(OnHOCRates_AfterAdd);					
					_HOCRates.AfterRemove += new EventListHandler(OnHOCRates_AfterRemove);					
				}
				return _HOCRates;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_HOCRates = null;
			
		}
	}
}


