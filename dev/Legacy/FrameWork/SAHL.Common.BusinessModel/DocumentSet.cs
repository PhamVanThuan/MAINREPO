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
	/// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO
	/// </summary>
	public partial class DocumentSet : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DocumentSet_DAO>, IDocumentSet
	{
				public DocumentSet(SAHL.Common.BusinessModel.DAO.DocumentSet_DAO DocumentSet) : base(DocumentSet)
		{
			this._DAO = DocumentSet;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.DocumentSetConfigs
		/// </summary>
		private DAOEventList<DocumentSetConfig_DAO, IDocumentSetConfig, DocumentSetConfig> _DocumentSetConfigs;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.DocumentSetConfigs
		/// </summary>
		public IEventList<IDocumentSetConfig> DocumentSetConfigs
		{
			get
			{
				if (null == _DocumentSetConfigs) 
				{
					if(null == _DAO.DocumentSetConfigs)
						_DAO.DocumentSetConfigs = new List<DocumentSetConfig_DAO>();
					_DocumentSetConfigs = new DAOEventList<DocumentSetConfig_DAO, IDocumentSetConfig, DocumentSetConfig>(_DAO.DocumentSetConfigs);
					_DocumentSetConfigs.BeforeAdd += new EventListHandler(OnDocumentSetConfigs_BeforeAdd);					
					_DocumentSetConfigs.BeforeRemove += new EventListHandler(OnDocumentSetConfigs_BeforeRemove);					
					_DocumentSetConfigs.AfterAdd += new EventListHandler(OnDocumentSetConfigs_AfterAdd);					
					_DocumentSetConfigs.AfterRemove += new EventListHandler(OnDocumentSetConfigs_AfterRemove);					
				}
				return _DocumentSetConfigs;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.ApplicationType
		/// </summary>
		public IApplicationType ApplicationType 
		{
			get
			{
				if (null == _DAO.ApplicationType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationType, ApplicationType_DAO>(_DAO.ApplicationType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationType = (ApplicationType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DocumentSet_DAO.OriginationSourceProduct
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
		public override void Refresh()
		{
			base.Refresh();
			_DocumentSetConfigs = null;
			
		}
	}
}


