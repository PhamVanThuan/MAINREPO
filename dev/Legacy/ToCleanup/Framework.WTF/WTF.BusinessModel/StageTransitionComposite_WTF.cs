
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
	/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO
	/// </summary>
    public partial class StageTransitionComposite_WTF : BusinessModelBase<StageTransitionComposite_WTF_DAO>, IStageTransitionComposite_WTF
	{
        public StageTransitionComposite_WTF(StageTransitionComposite_WTF_DAO StageTransitionComposite_WTF) : base(StageTransitionComposite_WTF)
		{
            this._DAO = StageTransitionComposite_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.TransitionDate
		/// </summary>
		public DateTime TransitionDate 
		{
			get { return _DAO.TransitionDate; }
			set { _DAO.TransitionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.Comments
		/// </summary>
		public String Comments 
		{
			get { return _DAO.Comments; }
			set { _DAO.Comments = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageTransitionReasonKey
		/// </summary>
		public Int32 StageTransitionReasonKey 
		{
			get { return _DAO.StageTransitionReasonKey; }
			set { _DAO.StageTransitionReasonKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.ADUser
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
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageDefinitionStageDefinitionGroup
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageTransition
		/// </summary>
        public IStageTransition_WTF StageTransition 
		{
			get
			{
				if (null == _DAO.StageTransition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IStageTransition_WTF, StageTransition_WTF_DAO>(_DAO.StageTransition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.StageTransition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
                    _DAO.StageTransition = (StageTransition_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



