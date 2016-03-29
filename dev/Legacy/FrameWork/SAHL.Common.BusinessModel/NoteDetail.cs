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
	/// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO
	/// </summary>
	public partial class NoteDetail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.NoteDetail_DAO>, INoteDetail
	{
				public NoteDetail(SAHL.Common.BusinessModel.DAO.NoteDetail_DAO NoteDetail) : base(NoteDetail)
		{
			this._DAO = NoteDetail;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.Tag
		/// </summary>
		public String Tag 
		{
			get { return _DAO.Tag; }
			set { _DAO.Tag = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.WorkflowState
		/// </summary>
		public String WorkflowState 
		{
			get { return _DAO.WorkflowState; }
			set { _DAO.WorkflowState = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.InsertedDate
		/// </summary>
		public DateTime InsertedDate 
		{
			get { return _DAO.InsertedDate; }
			set { _DAO.InsertedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.NoteText
		/// </summary>
		public String NoteText 
		{
			get { return _DAO.NoteText; }
			set { _DAO.NoteText = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.LegalEntity
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.NoteDetail_DAO.Note
		/// </summary>
		public INote Note 
		{
			get
			{
				if (null == _DAO.Note) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<INote, Note_DAO>(_DAO.Note);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Note = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Note = (Note_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


