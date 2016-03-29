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
	/// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO
	/// </summary>
	public partial class DocumentSetConfig : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO>, IDocumentSetConfig
	{
				public DocumentSetConfig(SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO DocumentSetConfig) : base(DocumentSetConfig)
		{
			this._DAO = DocumentSetConfig;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO.DocumentSet
		/// </summary>
		public IDocumentSet DocumentSet 
		{
			get
			{
				if (null == _DAO.DocumentSet) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDocumentSet, DocumentSet_DAO>(_DAO.DocumentSet);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DocumentSet = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DocumentSet = (DocumentSet_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO.DocumentType
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
		/// SAHL.Common.BusinessModel.DAO.DocumentSetConfig_DAO.RuleItem
		/// </summary>
		public IRuleItem RuleItem 
		{
			get
			{
				if (null == _DAO.RuleItem) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRuleItem, RuleItem_DAO>(_DAO.RuleItem);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RuleItem = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RuleItem = (RuleItem_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


