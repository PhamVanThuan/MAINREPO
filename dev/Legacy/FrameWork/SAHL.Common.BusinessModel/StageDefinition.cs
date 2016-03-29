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
	/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO
	/// </summary>
	public partial class StageDefinition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.StageDefinition_DAO>, IStageDefinition
	{
				public StageDefinition(SAHL.Common.BusinessModel.DAO.StageDefinition_DAO StageDefinition) : base(StageDefinition)
		{
			this._DAO = StageDefinition;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.HasCompositeLogic
		/// </summary>
		public Boolean HasCompositeLogic 
		{
			get { return _DAO.HasCompositeLogic; }
			set { _DAO.HasCompositeLogic = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Name
		/// </summary>
		public String Name 
		{
			get { return _DAO.Name; }
			set { _DAO.Name = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.StageDefinitionStageDefinitionGroups
		/// </summary>
		private DAOEventList<StageDefinitionStageDefinitionGroup_DAO, IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup> _StageDefinitionStageDefinitionGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.StageDefinitionStageDefinitionGroups
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
		public override void Refresh()
		{
			base.Refresh();
			_StageDefinitionStageDefinitionGroups = null;
			
		}
	}
}


