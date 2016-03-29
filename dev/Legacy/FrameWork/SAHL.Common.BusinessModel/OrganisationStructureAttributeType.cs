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
	/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO
	/// </summary>
	public partial class OrganisationStructureAttributeType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO>, IOrganisationStructureAttributeType
	{
				public OrganisationStructureAttributeType(SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO OrganisationStructureAttributeType) : base(OrganisationStructureAttributeType)
		{
			this._DAO = OrganisationStructureAttributeType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.Length
		/// </summary>
		public Int32 Length 
		{
			get { return _DAO.Length; }
			set { _DAO.Length = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.OrganisationStructureAttributes
		/// </summary>
		private DAOEventList<OrganisationStructureAttribute_DAO, IOrganisationStructureAttribute, OrganisationStructureAttribute> _OrganisationStructureAttributes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.OrganisationStructureAttributes
		/// </summary>
		public IEventList<IOrganisationStructureAttribute> OrganisationStructureAttributes
		{
			get
			{
				if (null == _OrganisationStructureAttributes) 
				{
					if(null == _DAO.OrganisationStructureAttributes)
						_DAO.OrganisationStructureAttributes = new List<OrganisationStructureAttribute_DAO>();
					_OrganisationStructureAttributes = new DAOEventList<OrganisationStructureAttribute_DAO, IOrganisationStructureAttribute, OrganisationStructureAttribute>(_DAO.OrganisationStructureAttributes);
					_OrganisationStructureAttributes.BeforeAdd += new EventListHandler(OnOrganisationStructureAttributes_BeforeAdd);					
					_OrganisationStructureAttributes.BeforeRemove += new EventListHandler(OnOrganisationStructureAttributes_BeforeRemove);					
					_OrganisationStructureAttributes.AfterAdd += new EventListHandler(OnOrganisationStructureAttributes_AfterAdd);					
					_OrganisationStructureAttributes.AfterRemove += new EventListHandler(OnOrganisationStructureAttributes_AfterRemove);					
				}
				return _OrganisationStructureAttributes;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.DataType
		/// </summary>
		public IDataType DataType 
		{
			get
			{
				if (null == _DAO.DataType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDataType, DataType_DAO>(_DAO.DataType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DataType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DataType = (DataType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_OrganisationStructureAttributes = null;
			
		}
	}
}


