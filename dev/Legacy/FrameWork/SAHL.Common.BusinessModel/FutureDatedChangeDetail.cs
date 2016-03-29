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
	/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO
	/// </summary>
	public partial class FutureDatedChangeDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO>, IFutureDatedChangeDetail
	{
				public FutureDatedChangeDetail(SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO FutureDatedChangeDetail) : base(FutureDatedChangeDetail)
		{
			this._DAO = FutureDatedChangeDetail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.ReferenceKey
		/// </summary>
		public Int32 ReferenceKey 
		{
			get { return _DAO.ReferenceKey; }
			set { _DAO.ReferenceKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.Action
		/// </summary>
		public Char Action 
		{
			get { return _DAO.Action; }
			set { _DAO.Action = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.TableName
		/// </summary>
		public String TableName 
		{
			get { return _DAO.TableName; }
			set { _DAO.TableName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.ColumnName
		/// </summary>
		public String ColumnName 
		{
			get { return _DAO.ColumnName; }
			set { _DAO.ColumnName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.Value
		/// </summary>
		public String Value 
		{
			get { return _DAO.Value; }
			set { _DAO.Value = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FutureDatedChangeDetail_DAO.FutureDatedChange
		/// </summary>
		public IFutureDatedChange FutureDatedChange 
		{
			get
			{
				if (null == _DAO.FutureDatedChange) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFutureDatedChange, FutureDatedChange_DAO>(_DAO.FutureDatedChange);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FutureDatedChange = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FutureDatedChange = (FutureDatedChange_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


