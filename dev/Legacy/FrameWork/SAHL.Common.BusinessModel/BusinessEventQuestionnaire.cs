using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO
    /// </summary>
    public partial class BusinessEventQuestionnaire : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO>, IBusinessEventQuestionnaire
    {
        public BusinessEventQuestionnaire(SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO BusinessEventQuestionnaire)
            : base(BusinessEventQuestionnaire)
        {
            this._DAO = BusinessEventQuestionnaire;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO.BusinessEvent
        /// </summary>
        public IBusinessEvent BusinessEvent
        {
            get
            {
                if (null == _DAO.BusinessEvent) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBusinessEvent, BusinessEvent_DAO>(_DAO.BusinessEvent);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BusinessEvent = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BusinessEvent = (BusinessEvent_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BusinessEventQuestionnaire_DAO.Questionnaire
        /// </summary>
        public IQuestionnaire Questionnaire
        {
            get
            {
                if (null == _DAO.Questionnaire) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IQuestionnaire, Questionnaire_DAO>(_DAO.Questionnaire);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Questionnaire = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Questionnaire = (Questionnaire_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}