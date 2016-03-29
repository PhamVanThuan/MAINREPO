using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ADUser_DAO
    /// </summary>
    public partial class ADUser : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ADUser_DAO>, IADUser
    {
        public ADUser(SAHL.Common.BusinessModel.DAO.ADUser_DAO ADUser)
            : base(ADUser)
        {
            this._DAO = ADUser;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.LegalEntity
        /// </summary>
        public ILegalEntityNaturalPerson LegalEntity
        {
            get
            {
                if (null == _DAO.LegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntityNaturalPerson, LegalEntityNaturalPerson_DAO>(_DAO.LegalEntity);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LegalEntity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LegalEntity = (LegalEntityNaturalPerson_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.ADUserName
        /// </summary>
        public String ADUserName
        {
            get { return _DAO.ADUserName; }
            set { _DAO.ADUserName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.GeneralStatusKey
        /// </summary>
        public IGeneralStatus GeneralStatusKey
        {
            get
            {
                if (null == _DAO.GeneralStatusKey) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatusKey);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GeneralStatusKey = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatusKey = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.Password
        /// </summary>
        public String Password
        {
            get { return _DAO.Password; }
            set { _DAO.Password = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.PasswordQuestion
        /// </summary>
        public String PasswordQuestion
        {
            get { return _DAO.PasswordQuestion; }
            set { _DAO.PasswordQuestion = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.PasswordAnswer
        /// </summary>
        public String PasswordAnswer
        {
            get { return _DAO.PasswordAnswer; }
            set { _DAO.PasswordAnswer = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserProfileSettings
        /// </summary>
        private DAOEventList<UserProfileSetting_DAO, IUserProfileSetting, UserProfileSetting> _UserProfileSettings;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserProfileSettings
        /// </summary>
        public IEventList<IUserProfileSetting> UserProfileSettings
        {
            get
            {
                if (null == _UserProfileSettings)
                {
                    if (null == _DAO.UserProfileSettings)
                        _DAO.UserProfileSettings = new List<UserProfileSetting_DAO>();
                    _UserProfileSettings = new DAOEventList<UserProfileSetting_DAO, IUserProfileSetting, UserProfileSetting>(_DAO.UserProfileSettings);
                    _UserProfileSettings.BeforeAdd += new EventListHandler(OnUserProfileSettings_BeforeAdd);
                    _UserProfileSettings.BeforeRemove += new EventListHandler(OnUserProfileSettings_BeforeRemove);
                    _UserProfileSettings.AfterAdd += new EventListHandler(OnUserProfileSettings_AfterAdd);
                    _UserProfileSettings.AfterRemove += new EventListHandler(OnUserProfileSettings_AfterRemove);
                }
                return _UserProfileSettings;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserOrganisationStructure
        /// </summary>
        private DAOEventList<UserOrganisationStructure_DAO, IUserOrganisationStructure, UserOrganisationStructure> _UserOrganisationStructure;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserOrganisationStructure
        /// </summary>
        public IEventList<IUserOrganisationStructure> UserOrganisationStructure
        {
            get
            {
                if (null == _UserOrganisationStructure)
                {
                    if (null == _DAO.UserOrganisationStructure)
                        _DAO.UserOrganisationStructure = new List<UserOrganisationStructure_DAO>();
                    _UserOrganisationStructure = new DAOEventList<UserOrganisationStructure_DAO, IUserOrganisationStructure, UserOrganisationStructure>(_DAO.UserOrganisationStructure);
                    _UserOrganisationStructure.BeforeAdd += new EventListHandler(OnUserOrganisationStructure_BeforeAdd);
                    _UserOrganisationStructure.BeforeRemove += new EventListHandler(OnUserOrganisationStructure_BeforeRemove);
                    _UserOrganisationStructure.AfterAdd += new EventListHandler(OnUserOrganisationStructure_AfterAdd);
                    _UserOrganisationStructure.AfterRemove += new EventListHandler(OnUserOrganisationStructure_AfterRemove);
                }
                return _UserOrganisationStructure;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _UserProfileSettings = null;
            _UserOrganisationStructure = null;
        }
    }
}