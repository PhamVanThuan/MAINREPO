
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;

using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO
	/// </summary>
    public partial class StageDefinitionStageDefinitionGroup_WTF : BusinessModelBase<StageDefinitionStageDefinitionGroup_WTF_DAO>, IStageDefinitionStageDefinitionGroup_WTF
	{
        public StageDefinitionStageDefinitionGroup_WTF(StageDefinitionStageDefinitionGroup_WTF_DAO StageDefinitionStageDefinitionGroup_WTF) : base(StageDefinitionStageDefinitionGroup_WTF)
		{
            this._DAO = StageDefinitionStageDefinitionGroup_WTF;
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
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinitionComposites
		/// </summary>
        private DAOEventList<StageDefinitionComposite_WTF_DAO, IStageDefinitionComposite_WTF, StageDefinitionComposite_WTF> _StageDefinitionComposites;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinitionComposites
		/// </summary>
        public IEventList<IStageDefinitionComposite_WTF> StageDefinitionComposites
		{
			get
			{
				if (null == _StageDefinitionComposites) 
				{
					if(null == _DAO.StageDefinitionComposites)
                        _DAO.StageDefinitionComposites = new List<StageDefinitionComposite_WTF_DAO>();
                    _StageDefinitionComposites = new DAOEventList<StageDefinitionComposite_WTF_DAO, IStageDefinitionComposite_WTF, StageDefinitionComposite_WTF>(_DAO.StageDefinitionComposites);
					_StageDefinitionComposites.BeforeAdd += new EventListHandler(OnStageDefinitionComposites_BeforeAdd);					
					_StageDefinitionComposites.BeforeRemove += new EventListHandler(OnStageDefinitionComposites_BeforeRemove);					
					_StageDefinitionComposites.AfterAdd += new EventListHandler(OnStageDefinitionComposites_AfterAdd);					
					_StageDefinitionComposites.AfterRemove += new EventListHandler(OnStageDefinitionComposites_AfterRemove);					
				}
				return _StageDefinitionComposites;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageTransitions
		/// </summary>
        private DAOEventList<StageTransition_WTF_DAO, IStageTransition_WTF, StageTransition_WTF> _StageTransitions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageTransitions
		/// </summary>
        public IEventList<IStageTransition_WTF> StageTransitions
		{
			get
			{
				if (null == _StageTransitions) 
				{
					if(null == _DAO.StageTransitions)
                        _DAO.StageTransitions = new List<StageTransition_WTF_DAO>();
                    _StageTransitions = new DAOEventList<StageTransition_WTF_DAO, IStageTransition_WTF, StageTransition_WTF>(_DAO.StageTransitions);
					_StageTransitions.BeforeAdd += new EventListHandler(OnStageTransitions_BeforeAdd);					
					_StageTransitions.BeforeRemove += new EventListHandler(OnStageTransitions_BeforeRemove);					
					_StageTransitions.AfterAdd += new EventListHandler(OnStageTransitions_AfterAdd);					
					_StageTransitions.AfterRemove += new EventListHandler(OnStageTransitions_AfterRemove);					
				}
				return _StageTransitions;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageTransitionComposites
		/// </summary>
        private DAOEventList<StageTransitionComposite_WTF_DAO, IStageTransitionComposite_WTF, StageTransitionComposite_WTF> _StageTransitionComposites;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageTransitionComposites
		/// </summary>
        public IEventList<IStageTransitionComposite_WTF> StageTransitionComposites
		{
			get
			{
				if (null == _StageTransitionComposites) 
				{
					if(null == _DAO.StageTransitionComposites)
                        _DAO.StageTransitionComposites = new List<StageTransitionComposite_WTF_DAO>();
                    _StageTransitionComposites = new DAOEventList<StageTransitionComposite_WTF_DAO, IStageTransitionComposite_WTF, StageTransitionComposite_WTF>(_DAO.StageTransitionComposites);
					_StageTransitionComposites.BeforeAdd += new EventListHandler(OnStageTransitionComposites_BeforeAdd);					
					_StageTransitionComposites.BeforeRemove += new EventListHandler(OnStageTransitionComposites_BeforeRemove);					
					_StageTransitionComposites.AfterAdd += new EventListHandler(OnStageTransitionComposites_AfterAdd);					
					_StageTransitionComposites.AfterRemove += new EventListHandler(OnStageTransitionComposites_AfterRemove);					
				}
				return _StageTransitionComposites;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinition
		/// </summary>
        public IStageDefinition_WTF StageDefinition 
		{
			get
			{
				if (null == _DAO.StageDefinition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IStageDefinition_WTF, StageDefinition_WTF_DAO>(_DAO.StageDefinition);
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
                    _DAO.StageDefinition = (StageDefinition_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinitionGroup
		/// </summary>
        public IStageDefinitionGroup_WTF StageDefinitionGroup 
		{
			get
			{
				if (null == _DAO.StageDefinitionGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IStageDefinitionGroup_WTF, StageDefinitionGroup_WTF_DAO>(_DAO.StageDefinitionGroup);
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
                    _DAO.StageDefinitionGroup = (StageDefinitionGroup_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



