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
	/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO
	/// </summary>
	public partial class StageDefinitionGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO>, IStageDefinitionGroup
	{
				public StageDefinitionGroup(SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO StageDefinitionGroup) : base(StageDefinitionGroup)
		{
			this._DAO = StageDefinitionGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.GeneralStatus
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionStageDefinitionGroups
		/// </summary>
		private DAOEventList<StageDefinitionStageDefinitionGroup_DAO, IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup> _StageDefinitionStageDefinitionGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionStageDefinitionGroups
		/// </summary>
		public IEventList<IStageDefinitionStageDefinitionGroup> StageDefinitionStageDefinitionGroups
		{
			get
			{
				if (null == _StageDefinitionStageDefinitionGroups) 
				{
					if(null == _DAO.StageDefinitionStageDefinitionGroups)
						_DAO.StageDefinitionStageDefinitionGroups = new List<StageDefinitionStageDefinitionGroup_DAO>();
					_StageDefinitionStageDefinitionGroups = new DAOEventList<StageDefinitionStageDefinitionGroup_DAO, IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup>(_DAO.StageDefinitionStageDefinitionGroups);
					_StageDefinitionStageDefinitionGroups.BeforeAdd += new EventListHandler(OnStageDefinitionStageDefinitionGroups_BeforeAdd);					
					_StageDefinitionStageDefinitionGroups.BeforeRemove += new EventListHandler(OnStageDefinitionStageDefinitionGroups_BeforeRemove);					
					_StageDefinitionStageDefinitionGroups.AfterAdd += new EventListHandler(OnStageDefinitionStageDefinitionGroups_AfterAdd);					
					_StageDefinitionStageDefinitionGroups.AfterRemove += new EventListHandler(OnStageDefinitionStageDefinitionGroups_AfterRemove);					
				}
				return _StageDefinitionStageDefinitionGroups;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.GenericKeyType
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
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.ParentStageDefinitionGroup
		/// </summary>
		public IStageDefinitionGroup ParentStageDefinitionGroup 
		{
			get
			{
				if (null == _DAO.ParentStageDefinitionGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStageDefinitionGroup, StageDefinitionGroup_DAO>(_DAO.ParentStageDefinitionGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ParentStageDefinitionGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ParentStageDefinitionGroup = (StageDefinitionGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.ChildStageDefinitionGroups
		/// </summary>
		private DAOEventList<StageDefinitionGroup_DAO, IStageDefinitionGroup, StageDefinitionGroup> _ChildStageDefinitionGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.ChildStageDefinitionGroups
		/// </summary>
		public IEventList<IStageDefinitionGroup> ChildStageDefinitionGroups
		{
			get
			{
				if (null == _ChildStageDefinitionGroups) 
				{
					if(null == _DAO.ChildStageDefinitionGroups)
						_DAO.ChildStageDefinitionGroups = new List<StageDefinitionGroup_DAO>();
					_ChildStageDefinitionGroups = new DAOEventList<StageDefinitionGroup_DAO, IStageDefinitionGroup, StageDefinitionGroup>(_DAO.ChildStageDefinitionGroups);
					_ChildStageDefinitionGroups.BeforeAdd += new EventListHandler(OnChildStageDefinitionGroups_BeforeAdd);					
					_ChildStageDefinitionGroups.BeforeRemove += new EventListHandler(OnChildStageDefinitionGroups_BeforeRemove);					
					_ChildStageDefinitionGroups.AfterAdd += new EventListHandler(OnChildStageDefinitionGroups_AfterAdd);					
					_ChildStageDefinitionGroups.AfterRemove += new EventListHandler(OnChildStageDefinitionGroups_AfterRemove);					
				}
				return _ChildStageDefinitionGroups;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_StageDefinitionStageDefinitionGroups = null;
			_ChildStageDefinitionGroups = null;
			
		}
	}
}


