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
    /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO
    /// </summary>
    public partial class ClientQuestionnaire : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO>, IClientQuestionnaire
    {
        public ClientQuestionnaire(SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO ClientQuestionnaire)
            : base(ClientQuestionnaire)
        {
            this._DAO = ClientQuestionnaire;
            OnConstruction();
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.BusinessEventQuestionnaire
        /// </summary>
        public IBusinessEventQuestionnaire BusinessEventQuestionnaire
        {
            get
            {
                if (null == _DAO.BusinessEventQuestionnaire) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBusinessEventQuestionnaire, BusinessEventQuestionnaire_DAO>(_DAO.BusinessEventQuestionnaire);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BusinessEventQuestionnaire = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BusinessEventQuestionnaire = (BusinessEventQuestionnaire_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.DatePresented
        /// </summary>
        public DateTime DatePresented
        {
            get { return _DAO.DatePresented; }
            set { _DAO.DatePresented = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.ADUser
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
                if (value == null)
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

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.GenericKeyType
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
                if (value == null)
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
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.DateReceived
        /// </summary>
        public DateTime? DateReceived
        {
            get { return _DAO.DateReceived; }
            set { _DAO.DateReceived = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.LegalEntity
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                if (null == _DAO.LegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
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
                    _DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.ClientAnswers
        /// </summary>
        private DAOEventList<ClientAnswer_DAO, IClientAnswer, ClientAnswer> _ClientAnswers;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.ClientAnswers
        /// </summary>
        public IEventList<IClientAnswer> ClientAnswers
        {
            get
            {
                if (null == _ClientAnswers)
                {
                    if (null == _DAO.ClientAnswers)
                        _DAO.ClientAnswers = new List<ClientAnswer_DAO>();
                    _ClientAnswers = new DAOEventList<ClientAnswer_DAO, IClientAnswer, ClientAnswer>(_DAO.ClientAnswers);
                    _ClientAnswers.BeforeAdd += new EventListHandler(OnClientAnswers_BeforeAdd);
                    _ClientAnswers.BeforeRemove += new EventListHandler(OnClientAnswers_BeforeRemove);
                    _ClientAnswers.AfterAdd += new EventListHandler(OnClientAnswers_AfterAdd);
                    _ClientAnswers.AfterRemove += new EventListHandler(OnClientAnswers_AfterRemove);
                }
                return _ClientAnswers;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ClientAnswers = null;
        }
    }
}