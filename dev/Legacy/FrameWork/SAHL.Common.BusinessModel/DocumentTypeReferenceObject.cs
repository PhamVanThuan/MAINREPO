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
	/// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO
	/// </summary>
	public partial class DocumentTypeReferenceObject : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO>, IDocumentTypeReferenceObject
	{
				public DocumentTypeReferenceObject(SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO DocumentTypeReferenceObject) : base(DocumentTypeReferenceObject)
		{
			this._DAO = DocumentTypeReferenceObject;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.ApplicationDocumentReferences
		/// </summary>
		private DAOEventList<ApplicationDocumentReference_DAO, IApplicationDocumentReference, ApplicationDocumentReference> _ApplicationDocumentReferences;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.ApplicationDocumentReferences
		/// </summary>
		public IEventList<IApplicationDocumentReference> ApplicationDocumentReferences
		{
			get
			{
				if (null == _ApplicationDocumentReferences) 
				{
					if(null == _DAO.ApplicationDocumentReferences)
						_DAO.ApplicationDocumentReferences = new List<ApplicationDocumentReference_DAO>();
					_ApplicationDocumentReferences = new DAOEventList<ApplicationDocumentReference_DAO, IApplicationDocumentReference, ApplicationDocumentReference>(_DAO.ApplicationDocumentReferences);
					_ApplicationDocumentReferences.BeforeAdd += new EventListHandler(OnApplicationDocumentReferences_BeforeAdd);					
					_ApplicationDocumentReferences.BeforeRemove += new EventListHandler(OnApplicationDocumentReferences_BeforeRemove);					
					_ApplicationDocumentReferences.AfterAdd += new EventListHandler(OnApplicationDocumentReferences_AfterAdd);					
					_ApplicationDocumentReferences.AfterRemove += new EventListHandler(OnApplicationDocumentReferences_AfterRemove);					
				}
				return _ApplicationDocumentReferences;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.DocumentType
		/// </summary>
		public IDocumentType DocumentType 
		{
			get
			{
				if (null == _DAO.DocumentType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDocumentType, DocumentType_DAO>(_DAO.DocumentType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DocumentType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DocumentType = (DocumentType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeReferenceObject_DAO.GenericKeyType
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
			_ApplicationDocumentReferences = null;
			
		}
	}
}


