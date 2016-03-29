
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
	/// SAHL.Common.BusinessModel.DAO.ADUser_DAO
	/// </summary>
    public partial class ADUser_WTF : BusinessModelBase<ADUser_WTF_DAO>, IADUser_WTF
	{
        public ADUser_WTF(ADUser_WTF_DAO ADUser_WTF) : base(ADUser_WTF)
		{
            this._DAO = ADUser_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.ADUserName
		/// </summary>
		public String ADUserName 
		{
			get { return _DAO.ADUserName; }
			set { _DAO.ADUserName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.Password
		/// </summary>
		public String Password 
		{
			get { return _DAO.Password; }
			set { _DAO.Password = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.PasswordQuestion
		/// </summary>
		public String PasswordQuestion 
		{
			get { return _DAO.PasswordQuestion; }
			set { _DAO.PasswordQuestion = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.PasswordAnswer
		/// </summary>
		public String PasswordAnswer 
		{
			get { return _DAO.PasswordAnswer; }
			set { _DAO.PasswordAnswer = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserOrganisationStructures
		/// </summary>
        private DAOEventList<UserOrganisationStructure_WTF_DAO, IUserOrganisationStructure_WTF, UserOrganisationStructure_WTF> _UserOrganisationStructures;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserOrganisationStructures
		/// </summary>
        public IEventList<IUserOrganisationStructure_WTF> UserOrganisationStructures
		{
			get
			{
				if (null == _UserOrganisationStructures) 
				{
					if(null == _DAO.UserOrganisationStructures)
                        _DAO.UserOrganisationStructures = new List<UserOrganisationStructure_WTF_DAO>();
                    _UserOrganisationStructures = new DAOEventList<UserOrganisationStructure_WTF_DAO, IUserOrganisationStructure_WTF, UserOrganisationStructure_WTF>(_DAO.UserOrganisationStructures);
					_UserOrganisationStructures.BeforeAdd += new EventListHandler(OnUserOrganisationStructures_BeforeAdd);					
					_UserOrganisationStructures.BeforeRemove += new EventListHandler(OnUserOrganisationStructures_BeforeRemove);					
					_UserOrganisationStructures.AfterAdd += new EventListHandler(OnUserOrganisationStructures_AfterAdd);					
					_UserOrganisationStructures.AfterRemove += new EventListHandler(OnUserOrganisationStructures_AfterRemove);					
				}
				return _UserOrganisationStructures;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.StageTransitions
		/// </summary>
        private DAOEventList<StageTransition_WTF_DAO, IStageTransition_WTF, StageTransition_WTF> _StageTransitions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.StageTransitions
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
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.StageTransitionComposites
		/// </summary>
        private DAOEventList<StageTransitionComposite_WTF_DAO, IStageTransitionComposite_WTF, StageTransitionComposite_WTF> _StageTransitionComposites;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.StageTransitionComposites
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
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.LegalEntity
		/// </summary>
        public ILegalEntity_WTF LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity_WTF, LegalEntity_WTF_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
                    _DAO.LegalEntity = (LegalEntity_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



