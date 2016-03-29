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
	/// SAHL.Common.BusinessModel.DAO.GenericSetType_DAO
	/// </summary>
	public partial class GenericSetType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.GenericSetType_DAO>, IGenericSetType
	{
				public GenericSetType(SAHL.Common.BusinessModel.DAO.GenericSetType_DAO GenericSetType) : base(GenericSetType)
		{
			this._DAO = GenericSetType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetType_DAO.GenericKeyType
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
		/// SAHL.Common.BusinessModel.DAO.GenericSetType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetType_DAO.GenericSetDefinitions
		/// </summary>
		private DAOEventList<GenericSetDefinition_DAO, IGenericSetDefinition, GenericSetDefinition> _GenericSetDefinitions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetType_DAO.GenericSetDefinitions
		/// </summary>
		public IEventList<IGenericSetDefinition> GenericSetDefinitions
		{
			get
			{
				if (null == _GenericSetDefinitions) 
				{
					if(null == _DAO.GenericSetDefinitions)
						_DAO.GenericSetDefinitions = new List<GenericSetDefinition_DAO>();
					_GenericSetDefinitions = new DAOEventList<GenericSetDefinition_DAO, IGenericSetDefinition, GenericSetDefinition>(_DAO.GenericSetDefinitions);
					_GenericSetDefinitions.BeforeAdd += new EventListHandler(OnGenericSetDefinitions_BeforeAdd);					
					_GenericSetDefinitions.BeforeRemove += new EventListHandler(OnGenericSetDefinitions_BeforeRemove);					
					_GenericSetDefinitions.AfterAdd += new EventListHandler(OnGenericSetDefinitions_AfterAdd);					
					_GenericSetDefinitions.AfterRemove += new EventListHandler(OnGenericSetDefinitions_AfterRemove);					
				}
				return _GenericSetDefinitions;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_GenericSetDefinitions = null;
			
		}
	}
}


