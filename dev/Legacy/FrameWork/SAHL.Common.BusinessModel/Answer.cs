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
    /// SAHL.Common.BusinessModel.DAO.Answer_DAO
    /// </summary>
    public partial class Answer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Answer_DAO>, IAnswer
    {
        public Answer(SAHL.Common.BusinessModel.DAO.Answer_DAO Answer)
            : base(Answer)
        {
            this._DAO = Answer;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.AnswerImages
        /// </summary>
        private DAOEventList<AnswerImage_DAO, IAnswerImage, AnswerImage> _AnswerImages;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.AnswerImages
        /// </summary>
        public IEventList<IAnswerImage> AnswerImages
        {
            get
            {
                if (null == _AnswerImages)
                {
                    if (null == _DAO.AnswerImages)
                        _DAO.AnswerImages = new List<AnswerImage_DAO>();
                    _AnswerImages = new DAOEventList<AnswerImage_DAO, IAnswerImage, AnswerImage>(_DAO.AnswerImages);
                    _AnswerImages.BeforeAdd += new EventListHandler(OnAnswerImages_BeforeAdd);
                    _AnswerImages.BeforeRemove += new EventListHandler(OnAnswerImages_BeforeRemove);
                    _AnswerImages.AfterAdd += new EventListHandler(OnAnswerImages_AfterAdd);
                    _AnswerImages.AfterRemove += new EventListHandler(OnAnswerImages_AfterRemove);
                }
                return _AnswerImages;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.QuestionnaireQuestionAnswers
        /// </summary>
        private DAOEventList<QuestionnaireQuestionAnswer_DAO, IQuestionnaireQuestionAnswer, QuestionnaireQuestionAnswer> _QuestionnaireQuestionAnswers;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.QuestionnaireQuestionAnswers
        /// </summary>
        public IEventList<IQuestionnaireQuestionAnswer> QuestionnaireQuestionAnswers
        {
            get
            {
                if (null == _QuestionnaireQuestionAnswers)
                {
                    if (null == _DAO.QuestionnaireQuestionAnswers)
                        _DAO.QuestionnaireQuestionAnswers = new List<QuestionnaireQuestionAnswer_DAO>();
                    _QuestionnaireQuestionAnswers = new DAOEventList<QuestionnaireQuestionAnswer_DAO, IQuestionnaireQuestionAnswer, QuestionnaireQuestionAnswer>(_DAO.QuestionnaireQuestionAnswers);
                    _QuestionnaireQuestionAnswers.BeforeAdd += new EventListHandler(OnQuestionnaireQuestionAnswers_BeforeAdd);
                    _QuestionnaireQuestionAnswers.BeforeRemove += new EventListHandler(OnQuestionnaireQuestionAnswers_BeforeRemove);
                    _QuestionnaireQuestionAnswers.AfterAdd += new EventListHandler(OnQuestionnaireQuestionAnswers_AfterAdd);
                    _QuestionnaireQuestionAnswers.AfterRemove += new EventListHandler(OnQuestionnaireQuestionAnswers_AfterRemove);
                }
                return _QuestionnaireQuestionAnswers;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.GeneralStatus
        /// </summary>
        public IGeneralStatus GeneralStatus
        {
            get
            {
                if (null == _DAO.GeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Answer_DAO.AnswerType
        /// </summary>
        public IAnswerType AnswerType
        {
            get
            {
                if (null == _DAO.AnswerType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAnswerType, AnswerType_DAO>(_DAO.AnswerType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.AnswerType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AnswerType = (AnswerType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _AnswerImages = null;
            _QuestionnaireQuestionAnswers = null;
        }
    }
}