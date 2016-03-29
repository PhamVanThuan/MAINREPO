
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
	/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO
	/// </summary>
    public partial class StageDefinitionGroup_WTF : BusinessModelBase<StageDefinitionGroup_WTF_DAO>, IStageDefinitionGroup_WTF
	{
        public StageDefinitionGroup_WTF(StageDefinitionGroup_WTF_DAO StageDefinitionGroup_WTF)  : base(StageDefinitionGroup_WTF)
		{
            this._DAO = StageDefinitionGroup_WTF;
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
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.GenericKeyTypeKey
		/// </summary>
		public Int32 GenericKeyTypeKey 
		{
			get { return _DAO.GenericKeyTypeKey; }
			set { _DAO.GenericKeyTypeKey = value;}
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
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionGroups
		/// </summary>
        private DAOEventList<StageDefinitionGroup_WTF_DAO, IStageDefinitionGroup_WTF, StageDefinitionGroup_WTF> _StageDefinitionGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionGroups
		/// </summary>
        public IEventList<IStageDefinitionGroup_WTF> StageDefinitionGroups
		{
			get
			{
				if (null == _StageDefinitionGroups) 
				{
					if(null == _DAO.StageDefinitionGroups)
                        _DAO.StageDefinitionGroups = new List<StageDefinitionGroup_WTF_DAO>();
                    _StageDefinitionGroups = new DAOEventList<StageDefinitionGroup_WTF_DAO, IStageDefinitionGroup_WTF, StageDefinitionGroup_WTF>(_DAO.StageDefinitionGroups);
					_StageDefinitionGroups.BeforeAdd += new EventListHandler(OnStageDefinitionGroups_BeforeAdd);					
					_StageDefinitionGroups.BeforeRemove += new EventListHandler(OnStageDefinitionGroups_BeforeRemove);					
					_StageDefinitionGroups.AfterAdd += new EventListHandler(OnStageDefinitionGroups_AfterAdd);					
					_StageDefinitionGroups.AfterRemove += new EventListHandler(OnStageDefinitionGroups_AfterRemove);					
				}
				return _StageDefinitionGroups;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionStageDefinitionGroups
		/// </summary>
        private DAOEventList<StageDefinitionStageDefinitionGroup_WTF_DAO, IStageDefinitionStageDefinitionGroup_WTF, StageDefinitionStageDefinitionGroup_WTF> _StageDefinitionStageDefinitionGroups;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionStageDefinitionGroups
		/// </summary>
        public IEventList<IStageDefinitionStageDefinitionGroup_WTF> StageDefinitionStageDefinitionGroups
		{
			get
			{
				if (null == _StageDefinitionStageDefinitionGroups) 
				{
					if(null == _DAO.StageDefinitionStageDefinitionGroups)
                        _DAO.StageDefinitionStageDefinitionGroups = new List<StageDefinitionStageDefinitionGroup_WTF_DAO>();
                    _StageDefinitionStageDefinitionGroups = new DAOEventList<StageDefinitionStageDefinitionGroup_WTF_DAO, IStageDefinitionStageDefinitionGroup_WTF, StageDefinitionStageDefinitionGroup_WTF>(_DAO.StageDefinitionStageDefinitionGroups);
					_StageDefinitionStageDefinitionGroups.BeforeAdd += new EventListHandler(OnStageDefinitionStageDefinitionGroups_BeforeAdd);					
					_StageDefinitionStageDefinitionGroups.BeforeRemove += new EventListHandler(OnStageDefinitionStageDefinitionGroups_BeforeRemove);					
					_StageDefinitionStageDefinitionGroups.AfterAdd += new EventListHandler(OnStageDefinitionStageDefinitionGroups_AfterAdd);					
					_StageDefinitionStageDefinitionGroups.AfterRemove += new EventListHandler(OnStageDefinitionStageDefinitionGroups_AfterRemove);					
				}
				return _StageDefinitionStageDefinitionGroups;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.GeneralStatus
		/// </summary>
        public IGeneralStatus_WTF GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus_WTF, GeneralStatus_WTF_DAO>(_DAO.GeneralStatus);
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
                    _DAO.GeneralStatus = (GeneralStatus_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionGroup
		/// </summary>
        public IStageDefinitionGroup_WTF ParentStageDefinitionGroup 
		{
			get
			{
                if (null == _DAO.ParentStageDefinitionGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IStageDefinitionGroup_WTF, StageDefinitionGroup_WTF_DAO>(_DAO.ParentStageDefinitionGroup);
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
                    _DAO.ParentStageDefinitionGroup = (StageDefinitionGroup_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



