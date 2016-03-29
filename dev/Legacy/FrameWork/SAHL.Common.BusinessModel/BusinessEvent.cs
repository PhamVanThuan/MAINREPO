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
    /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO
    /// </summary>
    public partial class BusinessEvent : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO>, IBusinessEvent
    {
        public BusinessEvent(SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO BusinessEvent)
            : base(BusinessEvent)
        {
            this._DAO = BusinessEvent;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.GenericKeyType
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
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.BusinessEventQuestionnaires
        /// </summary>
        private DAOEventList<BusinessEventQuestionnaire_DAO, IBusinessEventQuestionnaire, BusinessEventQuestionnaire> _BusinessEventQuestionnaires;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEvent_DAO.BusinessEventQuestionnaires
        /// </summary>
        public IEventList<IBusinessEventQuestionnaire> BusinessEventQuestionnaires
        {
            get
            {
                if (null == _BusinessEventQuestionnaires)
                {
                    if (null == _DAO.BusinessEventQuestionnaires)
                        _DAO.BusinessEventQuestionnaires = new List<BusinessEventQuestionnaire_DAO>();
                    _BusinessEventQuestionnaires = new DAOEventList<BusinessEventQuestionnaire_DAO, IBusinessEventQuestionnaire, BusinessEventQuestionnaire>(_DAO.BusinessEventQuestionnaires);
                    _BusinessEventQuestionnaires.BeforeAdd += new EventListHandler(OnBusinessEventQuestionnaires_BeforeAdd);
                    _BusinessEventQuestionnaires.BeforeRemove += new EventListHandler(OnBusinessEventQuestionnaires_BeforeRemove);
                    _BusinessEventQuestionnaires.AfterAdd += new EventListHandler(OnBusinessEventQuestionnaires_AfterAdd);
                    _BusinessEventQuestionnaires.AfterRemove += new EventListHandler(OnBusinessEventQuestionnaires_AfterRemove);
                }
                return _BusinessEventQuestionnaires;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _BusinessEventQuestionnaires = null;
        }
    }
}