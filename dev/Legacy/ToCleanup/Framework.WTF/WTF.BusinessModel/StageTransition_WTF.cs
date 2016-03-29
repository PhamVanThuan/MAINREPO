
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
	/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO
	/// </summary>
    public partial class StageTransition_WTF : BusinessModelBase<StageTransition_WTF_DAO>, IStageTransition_WTF
	{
        public StageTransition_WTF(StageTransition_WTF_DAO StageTransition_WTF) : base(StageTransition_WTF)
		{
            this._DAO = StageTransition_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.TransitionDate
		/// </summary>
		public DateTime TransitionDate 
		{
			get { return _DAO.TransitionDate; }
			set { _DAO.TransitionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.Comments
		/// </summary>
		public String Comments 
		{
			get { return _DAO.Comments; }
			set { _DAO.Comments = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.EndTransitionDate
		/// </summary>
		public DateTime? EndTransitionDate 
		{
			get { return _DAO.EndTransitionDate; }
			set { _DAO.EndTransitionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.StageTransitionComposites
		/// </summary>
        private DAOEventList<StageTransitionComposite_WTF_DAO, IStageTransitionComposite_WTF, StageTransitionComposite_WTF> _StageTransitionComposites;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.StageTransitionComposites
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
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.ADUser
		/// </summary>
        public IADUser_WTF ADUser 
		{
			get
			{
				if (null == _DAO.ADUser) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IADUser_WTF, ADUser_WTF_DAO>(_DAO.ADUser);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ADUser = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
                    _DAO.ADUser = (ADUser_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.StageDefinitionStageDefinitionGroup
		/// </summary>
        public IStageDefinitionStageDefinitionGroup_WTF StageDefinitionStageDefinitionGroup 
		{
			get
			{
				if (null == _DAO.StageDefinitionStageDefinitionGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IStageDefinitionStageDefinitionGroup_WTF, StageDefinitionStageDefinitionGroup_WTF_DAO>(_DAO.StageDefinitionStageDefinitionGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.StageDefinitionStageDefinitionGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
                    _DAO.StageDefinitionStageDefinitionGroup = (StageDefinitionStageDefinitionGroup_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



