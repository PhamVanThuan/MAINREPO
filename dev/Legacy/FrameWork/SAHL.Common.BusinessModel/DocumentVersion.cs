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
	/// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO
	/// </summary>
	public partial class DocumentVersion : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO>, IDocumentVersion
	{
				public DocumentVersion(SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO DocumentVersion) : base(DocumentVersion)
		{
			this._DAO = DocumentVersion;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.Version
		/// </summary>
		public String Version 
		{
			get { return _DAO.Version; }
			set { _DAO.Version = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.EffectiveDate
		/// </summary>
		public DateTime EffectiveDate 
		{
			get { return _DAO.EffectiveDate; }
			set { _DAO.EffectiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.ActiveIndicator
		/// </summary>
		public Boolean ActiveIndicator 
		{
			get { return _DAO.ActiveIndicator; }
			set { _DAO.ActiveIndicator = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentVersion_DAO.DocumentType
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
	}
}


