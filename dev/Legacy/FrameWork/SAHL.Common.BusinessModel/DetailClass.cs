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
	/// SAHL.Common.BusinessModel.DAO.DetailClass_DAO
	/// </summary>
	public partial class DetailClass : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DetailClass_DAO>, IDetailClass
	{
				public DetailClass(SAHL.Common.BusinessModel.DAO.DetailClass_DAO DetailClass) : base(DetailClass)
		{
			this._DAO = DetailClass;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailClass_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailClass_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailClass_DAO.DetailTypes
		/// </summary>
		private DAOEventList<DetailType_DAO, IDetailType, DetailType> _DetailTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DetailClass_DAO.DetailTypes
		/// </summary>
		public IEventList<IDetailType> DetailTypes
		{
			get
			{
				if (null == _DetailTypes) 
				{
					if(null == _DAO.DetailTypes)
						_DAO.DetailTypes = new List<DetailType_DAO>();
					_DetailTypes = new DAOEventList<DetailType_DAO, IDetailType, DetailType>(_DAO.DetailTypes);
					_DetailTypes.BeforeAdd += new EventListHandler(OnDetailTypes_BeforeAdd);					
					_DetailTypes.BeforeRemove += new EventListHandler(OnDetailTypes_BeforeRemove);					
					_DetailTypes.AfterAdd += new EventListHandler(OnDetailTypes_AfterAdd);					
					_DetailTypes.AfterRemove += new EventListHandler(OnDetailTypes_AfterRemove);					
				}
				return _DetailTypes;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_DetailTypes = null;
			
		}
	}
}


