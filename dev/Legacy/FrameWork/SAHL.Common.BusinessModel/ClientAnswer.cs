using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO
    /// </summary>
    public partial class ClientAnswer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO>, IClientAnswer
    {
        public ClientAnswer(SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO ClientAnswer)
            : base(ClientAnswer)
        {
            this._DAO = ClientAnswer;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.ClientAnswerValue
        /// </summary>
        public IClientAnswerValue ClientAnswerValue
        {
            get
            {
                if (null == _DAO.ClientAnswerValue) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IClientAnswerValue, ClientAnswerValue_DAO>(_DAO.ClientAnswerValue);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ClientAnswerValue = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ClientAnswerValue = (ClientAnswerValue_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.Answer
        /// </summary>
        public IAnswer Answer
        {
            get
            {
                if (null == _DAO.Answer) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAnswer, Answer_DAO>(_DAO.Answer);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Answer = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Answer = (Answer_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.QuestionnaireQuestion
        /// </summary>
        public IQuestionnaireQuestion QuestionnaireQuestion
        {
            get
            {
                if (null == _DAO.QuestionnaireQuestion) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IQuestionnaireQuestion, QuestionnaireQuestion_DAO>(_DAO.QuestionnaireQuestion);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.QuestionnaireQuestion = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.QuestionnaireQuestion = (QuestionnaireQuestion_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientAnswer_DAO.ClientSurvey
        /// </summary>
        public IClientQuestionnaire ClientSurvey
        {
            get
            {
                if (null == _DAO.ClientSurvey) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IClientQuestionnaire, ClientQuestionnaire_DAO>(_DAO.ClientSurvey);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ClientSurvey = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ClientSurvey = (ClientQuestionnaire_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}