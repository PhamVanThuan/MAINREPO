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
	/// SAHL.Common.BusinessModel.DAO.DocumentTypeGroupConfiguration_DAO
	/// </summary>
	public partial class DocumentTypeGroupConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DocumentTypeGroupConfiguration_DAO>, IDocumentTypeGroupConfiguration
	{
				public DocumentTypeGroupConfiguration(SAHL.Common.BusinessModel.DAO.DocumentTypeGroupConfiguration_DAO DocumentTypeGroupConfiguration) : base(DocumentTypeGroupConfiguration)
		{
			this._DAO = DocumentTypeGroupConfiguration;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeGroupConfiguration_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeGroupConfiguration_DAO.DocumentType
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
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeGroupConfiguration_DAO.DocumentGroup
		/// </summary>
		public IDocumentGroup DocumentGroup 
		{
			get
			{
				if (null == _DAO.DocumentGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDocumentGroup, DocumentGroup_DAO>(_DAO.DocumentGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DocumentGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DocumentGroup = (DocumentGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentTypeGroupConfiguration_DAO.OriginationSourceProduct
		/// </summary>
		public IOriginationSourceProduct OriginationSourceProduct 
		{
			get
			{
				if (null == _DAO.OriginationSourceProduct) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOriginationSourceProduct, OriginationSourceProduct_DAO>(_DAO.OriginationSourceProduct);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OriginationSourceProduct = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OriginationSourceProduct = (OriginationSourceProduct_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


