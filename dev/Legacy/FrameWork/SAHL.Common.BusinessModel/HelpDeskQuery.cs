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
	/// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO
	/// </summary>
	public partial class HelpDeskQuery : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO>, IHelpDeskQuery
	{
				public HelpDeskQuery(SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO HelpDeskQuery) : base(HelpDeskQuery)
		{
			this._DAO = HelpDeskQuery;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.InsertDate
		/// </summary>
		public DateTime InsertDate 
		{
			get { return _DAO.InsertDate; }
			set { _DAO.InsertDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.Memo
		/// </summary>
		public IMemo Memo 
		{
			get
			{
				if (null == _DAO.Memo) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMemo, Memo_DAO>(_DAO.Memo);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Memo = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Memo = (Memo_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.ResolvedDate
		/// </summary>
		public DateTime? ResolvedDate
		{
			get { return _DAO.ResolvedDate; }
			set { _DAO.ResolvedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO.HelpDeskCategory
		/// </summary>
		public IHelpDeskCategory HelpDeskCategory 
		{
			get
			{
				if (null == _DAO.HelpDeskCategory) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHelpDeskCategory, HelpDeskCategory_DAO>(_DAO.HelpDeskCategory);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HelpDeskCategory = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HelpDeskCategory = (HelpDeskCategory_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


