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
	/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO
	/// </summary>
	public partial class StageTransitionComposite : BusinessModelBase<SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO>, IStageTransitionComposite
	{
				public StageTransitionComposite(SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO StageTransitionComposite) : base(StageTransitionComposite)
		{
			this._DAO = StageTransitionComposite;
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
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageTransitionReasonKey
		/// </summary>
		public Int32 StageTransitionReasonKey 
		{
			get { return _DAO.StageTransitionReasonKey; }
			set { _DAO.StageTransitionReasonKey = value;}
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
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageDefinitionStageDefinitionGroup
		/// </summary>
		public IStageDefinitionStageDefinitionGroup StageDefinitionStageDefinitionGroup 
		{
			get
			{
				if (null == _DAO.StageDefinitionStageDefinitionGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStageDefinitionStageDefinitionGroup, StageDefinitionStageDefinitionGroup_DAO>(_DAO.StageDefinitionStageDefinitionGroup);
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
					_DAO.StageDefinitionStageDefinitionGroup = (StageDefinitionStageDefinitionGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.StageTransition
		/// </summary>
		public IStageTransition StageTransition 
		{
			get
			{
				if (null == _DAO.StageTransition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStageTransition, StageTransition_DAO>(_DAO.StageTransition);
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
					_DAO.StageTransition = (StageTransition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransitionComposite_DAO.ADUser
		/// </summary>
		public IADUser ADUser 
		{
			get
			{
				if (null == _DAO.ADUser) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
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
					_DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


