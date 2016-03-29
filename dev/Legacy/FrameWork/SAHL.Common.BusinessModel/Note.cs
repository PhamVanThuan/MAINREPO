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
	/// SAHL.Common.BusinessModel.DAO.Note_DAO
	/// </summary>
	public partial class Note : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Note_DAO>, INote
	{
				public Note(SAHL.Common.BusinessModel.DAO.Note_DAO Note) : base(Note)
		{
			this._DAO = Note;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.GenericKeyType
		/// </summary>
		public IGenericKeyType GenericKeyType 
		{
			get
			{
				if (null == _DAO.GenericKeyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GenericKeyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.NoteDetails
		/// </summary>
		private DAOEventList<NoteDetail_DAO, INoteDetail, NoteDetail> _NoteDetails;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Note_DAO.NoteDetails
		/// </summary>
		public IEventList<INoteDetail> NoteDetails
		{
			get
			{
				if (null == _NoteDetails) 
				{
					if(null == _DAO.NoteDetails)
						_DAO.NoteDetails = new List<NoteDetail_DAO>();
					_NoteDetails = new DAOEventList<NoteDetail_DAO, INoteDetail, NoteDetail>(_DAO.NoteDetails);
					_NoteDetails.BeforeAdd += new EventListHandler(OnNoteDetails_BeforeAdd);					
					_NoteDetails.BeforeRemove += new EventListHandler(OnNoteDetails_BeforeRemove);					
					_NoteDetails.AfterAdd += new EventListHandler(OnNoteDetails_AfterAdd);					
					_NoteDetails.AfterRemove += new EventListHandler(OnNoteDetails_AfterRemove);					
				}
				return _NoteDetails;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_NoteDetails = null;
			
		}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Note_DAO.DiaryDate
        /// </summary>
        public DateTime? DiaryDate
        {
            get { return _DAO.DiaryDate; }
            set { _DAO.DiaryDate = value; }
        }
    }
}


