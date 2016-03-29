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
	/// Ties StageDefinition_DAO objects with StageDefinitionGroup_DAO objects.
	/// </summary>
	public partial class StageDefinitionStageDefinitionGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO>, IStageDefinitionStageDefinitionGroup
	{
				public StageDefinitionStageDefinitionGroup(SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO StageDefinitionStageDefinitionGroup) : base(StageDefinitionStageDefinitionGroup)
		{
			this._DAO = StageDefinitionStageDefinitionGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinitionGroup
		/// </summary>
		public IStageDefinitionGroup StageDefinitionGroup 
		{
			get
			{
				if (null == _DAO.StageDefinitionGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStageDefinitionGroup, StageDefinitionGroup_DAO>(_DAO.StageDefinitionGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.StageDefinitionGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.StageDefinitionGroup = (StageDefinitionGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinition
		/// </summary>
		public IStageDefinition StageDefinition 
		{
			get
			{
				if (null == _DAO.StageDefinition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStageDefinition, StageDefinition_DAO>(_DAO.StageDefinition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.StageDefinition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.StageDefinition = (StageDefinition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.CompositeChildStageDefinitionStageDefinitionGroups
		/// </summary>
		private DAOEventList<StageDefinitionStageDefinitionGroup_DAO, IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup> _CompositeChildStageDefinitionStageDefinitionGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.CompositeChildStageDefinitionStageDefinitionGroups
		/// </summary>
		public IEventList<IStageDefinitionStageDefinitionGroup> CompositeChildStageDefinitionStageDefinitionGroups
		{
			get
			{
				if (null == _CompositeChildStageDefinitionStageDefinitionGroups) 
				{
					if(null == _DAO.CompositeChildStageDefinitionStageDefinitionGroups)
						_DAO.CompositeChildStageDefinitionStageDefinitionGroups = new List<StageDefinitionStageDefinitionGroup_DAO>();
					_CompositeChildStageDefinitionStageDefinitionGroups = new DAOEventList<StageDefinitionStageDefinitionGroup_DAO, IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup>(_DAO.CompositeChildStageDefinitionStageDefinitionGroups);
					_CompositeChildStageDefinitionStageDefinitionGroups.BeforeAdd += new EventListHandler(OnCompositeChildStageDefinitionStageDefinitionGroups_BeforeAdd);					
					_CompositeChildStageDefinitionStageDefinitionGroups.BeforeRemove += new EventListHandler(OnCompositeChildStageDefinitionStageDefinitionGroups_BeforeRemove);					
					_CompositeChildStageDefinitionStageDefinitionGroups.AfterAdd += new EventListHandler(OnCompositeChildStageDefinitionStageDefinitionGroups_AfterAdd);					
					_CompositeChildStageDefinitionStageDefinitionGroups.AfterRemove += new EventListHandler(OnCompositeChildStageDefinitionStageDefinitionGroups_AfterRemove);					
				}
				return _CompositeChildStageDefinitionStageDefinitionGroups;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.CompositeParentStageDefinitionStageDefinitionGroups
		/// </summary>
		private DAOEventList<StageDefinitionStageDefinitionGroup_DAO, IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup> _CompositeParentStageDefinitionStageDefinitionGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.CompositeParentStageDefinitionStageDefinitionGroups
		/// </summary>
		public IEventList<IStageDefinitionStageDefinitionGroup> CompositeParentStageDefinitionStageDefinitionGroups
		{
			get
			{
				if (null == _CompositeParentStageDefinitionStageDefinitionGroups) 
				{
					if(null == _DAO.CompositeParentStageDefinitionStageDefinitionGroups)
						_DAO.CompositeParentStageDefinitionStageDefinitionGroups = new List<StageDefinitionStageDefinitionGroup_DAO>();
					_CompositeParentStageDefinitionStageDefinitionGroups = new DAOEventList<StageDefinitionStageDefinitionGroup_DAO, IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup>(_DAO.CompositeParentStageDefinitionStageDefinitionGroups);
					_CompositeParentStageDefinitionStageDefinitionGroups.BeforeAdd += new EventListHandler(OnCompositeParentStageDefinitionStageDefinitionGroups_BeforeAdd);					
					_CompositeParentStageDefinitionStageDefinitionGroups.BeforeRemove += new EventListHandler(OnCompositeParentStageDefinitionStageDefinitionGroups_BeforeRemove);					
					_CompositeParentStageDefinitionStageDefinitionGroups.AfterAdd += new EventListHandler(OnCompositeParentStageDefinitionStageDefinitionGroups_AfterAdd);					
					_CompositeParentStageDefinitionStageDefinitionGroups.AfterRemove += new EventListHandler(OnCompositeParentStageDefinitionStageDefinitionGroups_AfterRemove);					
				}
				return _CompositeParentStageDefinitionStageDefinitionGroups;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_CompositeChildStageDefinitionStageDefinitionGroups = null;
			_CompositeParentStageDefinitionStageDefinitionGroups = null;
			
		}
	}
}


