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
	/// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO
	/// </summary>
	public partial class GenericSetDefinition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO>, IGenericSetDefinition
	{
				public GenericSetDefinition(SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO GenericSetDefinition) : base(GenericSetDefinition)
		{
			this._DAO = GenericSetDefinition;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.Explanation
		/// </summary>
		public String Explanation 
		{
			get { return _DAO.Explanation; }
			set { _DAO.Explanation = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.GenericSets
		/// </summary>
		private DAOEventList<GenericSet_DAO, IGenericSet, GenericSet> _GenericSets;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.GenericSets
		/// </summary>
		public IEventList<IGenericSet> GenericSets
		{
			get
			{
				if (null == _GenericSets) 
				{
					if(null == _DAO.GenericSets)
						_DAO.GenericSets = new List<GenericSet_DAO>();
					_GenericSets = new DAOEventList<GenericSet_DAO, IGenericSet, GenericSet>(_DAO.GenericSets);
					_GenericSets.BeforeAdd += new EventListHandler(OnGenericSets_BeforeAdd);					
					_GenericSets.BeforeRemove += new EventListHandler(OnGenericSets_BeforeRemove);					
					_GenericSets.AfterAdd += new EventListHandler(OnGenericSets_AfterAdd);					
					_GenericSets.AfterRemove += new EventListHandler(OnGenericSets_AfterRemove);					
				}
				return _GenericSets;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSetDefinition_DAO.GenericSetType
		/// </summary>
		public IGenericSetType GenericSetType 
		{
			get
			{
				if (null == _DAO.GenericSetType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericSetType, GenericSetType_DAO>(_DAO.GenericSetType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GenericSetType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GenericSetType = (GenericSetType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_GenericSets = null;
			
		}
	}
}


