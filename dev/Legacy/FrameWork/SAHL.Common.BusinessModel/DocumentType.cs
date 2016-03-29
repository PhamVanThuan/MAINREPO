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
	/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO
	/// </summary>
	public partial class DocumentType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DocumentType_DAO>, IDocumentType
	{
				public DocumentType(SAHL.Common.BusinessModel.DAO.DocumentType_DAO DocumentType) : base(DocumentType)
		{
			this._DAO = DocumentType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.LegalEntity
		/// </summary>
		public Boolean? LegalEntity
		{
			get { return _DAO.LegalEntity; }
			set { _DAO.LegalEntity = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.DocumentTypes
		/// </summary>
		private DAOEventList<DocumentType_DAO, IDocumentType, DocumentType> _DocumentTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.DocumentTypes
		/// </summary>
		public IEventList<IDocumentType> DocumentTypes
		{
			get
			{
				if (null == _DocumentTypes) 
				{
					if(null == _DAO.DocumentTypes)
						_DAO.DocumentTypes = new List<DocumentType_DAO>();
					_DocumentTypes = new DAOEventList<DocumentType_DAO, IDocumentType, DocumentType>(_DAO.DocumentTypes);
					_DocumentTypes.BeforeAdd += new EventListHandler(OnDocumentTypes_BeforeAdd);					
					_DocumentTypes.BeforeRemove += new EventListHandler(OnDocumentTypes_BeforeRemove);					
					_DocumentTypes.AfterAdd += new EventListHandler(OnDocumentTypes_AfterAdd);					
					_DocumentTypes.AfterRemove += new EventListHandler(OnDocumentTypes_AfterRemove);					
				}
				return _DocumentTypes;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.DocumentVersions
		/// </summary>
		private DAOEventList<DocumentVersion_DAO, IDocumentVersion, DocumentVersion> _DocumentVersions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.DocumentVersions
		/// </summary>
		public IEventList<IDocumentVersion> DocumentVersions
		{
			get
			{
				if (null == _DocumentVersions) 
				{
					if(null == _DAO.DocumentVersions)
						_DAO.DocumentVersions = new List<DocumentVersion_DAO>();
					_DocumentVersions = new DAOEventList<DocumentVersion_DAO, IDocumentVersion, DocumentVersion>(_DAO.DocumentVersions);
					_DocumentVersions.BeforeAdd += new EventListHandler(OnDocumentVersions_BeforeAdd);					
					_DocumentVersions.BeforeRemove += new EventListHandler(OnDocumentVersions_BeforeRemove);					
					_DocumentVersions.AfterAdd += new EventListHandler(OnDocumentVersions_AfterAdd);					
					_DocumentVersions.AfterRemove += new EventListHandler(OnDocumentVersions_AfterRemove);					
				}
				return _DocumentVersions;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentType_DAO.GenericKeyType
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
		public override void Refresh()
		{
			base.Refresh();
			_DocumentTypes = null;
			_DocumentVersions = null;
			
		}
	}
}


