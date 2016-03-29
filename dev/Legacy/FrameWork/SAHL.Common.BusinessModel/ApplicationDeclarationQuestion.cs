using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO
    /// </summary>
    public partial class ApplicationDeclarationQuestion : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO>, IApplicationDeclarationQuestion
    {
        public ApplicationDeclarationQuestion(SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO ApplicationDeclarationQuestion)
            : base(ApplicationDeclarationQuestion)
        {
            this._DAO = ApplicationDeclarationQuestion;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.DisplayQuestionDate
        /// </summary>
        public Boolean DisplayQuestionDate
        {
            get { return _DAO.DisplayQuestionDate; }
            set { _DAO.DisplayQuestionDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.DisplaySequence
        /// </summary>
        public Int32 DisplaySequence
        {
            get { return _DAO.DisplaySequence; }
            set { _DAO.DisplaySequence = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.ApplicationDeclarationQuestionAnswers
        /// </summary>
        private DAOEventList<ApplicationDeclarationQuestionAnswer_DAO, IApplicationDeclarationQuestionAnswer, ApplicationDeclarationQuestionAnswer> _ApplicationDeclarationQuestionAnswers;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.ApplicationDeclarationQuestionAnswers
        /// </summary>
        public IEventList<IApplicationDeclarationQuestionAnswer> ApplicationDeclarationQuestionAnswers
        {
            get
            {
                if (null == _ApplicationDeclarationQuestionAnswers)
                {
                    if (null == _DAO.ApplicationDeclarationQuestionAnswers)
                        _DAO.ApplicationDeclarationQuestionAnswers = new List<ApplicationDeclarationQuestionAnswer_DAO>();
                    _ApplicationDeclarationQuestionAnswers = new DAOEventList<ApplicationDeclarationQuestionAnswer_DAO, IApplicationDeclarationQuestionAnswer, ApplicationDeclarationQuestionAnswer>(_DAO.ApplicationDeclarationQuestionAnswers);
                    _ApplicationDeclarationQuestionAnswers.BeforeAdd += new EventListHandler(OnApplicationDeclarationQuestionAnswers_BeforeAdd);
                    _ApplicationDeclarationQuestionAnswers.BeforeRemove += new EventListHandler(OnApplicationDeclarationQuestionAnswers_BeforeRemove);
                    _ApplicationDeclarationQuestionAnswers.AfterAdd += new EventListHandler(OnApplicationDeclarationQuestionAnswers_AfterAdd);
                    _ApplicationDeclarationQuestionAnswers.AfterRemove += new EventListHandler(OnApplicationDeclarationQuestionAnswers_AfterRemove);
                }
                return _ApplicationDeclarationQuestionAnswers;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.ApplicationDeclarationQuestionAnswerConfigurations
        /// </summary>
        private DAOEventList<ApplicationDeclarationQuestionAnswerConfiguration_DAO, IApplicationDeclarationQuestionAnswerConfiguration, ApplicationDeclarationQuestionAnswerConfiguration> _ApplicationDeclarationQuestionAnswerConfigurations;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestion_DAO.ApplicationDeclarationQuestionAnswerConfigurations
        /// </summary>
        public IEventList<IApplicationDeclarationQuestionAnswerConfiguration> ApplicationDeclarationQuestionAnswerConfigurations
        {
            get
            {
                if (null == _ApplicationDeclarationQuestionAnswerConfigurations)
                {
                    if (null == _DAO.ApplicationDeclarationQuestionAnswerConfigurations)
                        _DAO.ApplicationDeclarationQuestionAnswerConfigurations = new List<ApplicationDeclarationQuestionAnswerConfiguration_DAO>();
                    _ApplicationDeclarationQuestionAnswerConfigurations = new DAOEventList<ApplicationDeclarationQuestionAnswerConfiguration_DAO, IApplicationDeclarationQuestionAnswerConfiguration, ApplicationDeclarationQuestionAnswerConfiguration>(_DAO.ApplicationDeclarationQuestionAnswerConfigurations);
                    _ApplicationDeclarationQuestionAnswerConfigurations.BeforeAdd += new EventListHandler(OnApplicationDeclarationQuestionAnswerConfigurations_BeforeAdd);
                    _ApplicationDeclarationQuestionAnswerConfigurations.BeforeRemove += new EventListHandler(OnApplicationDeclarationQuestionAnswerConfigurations_BeforeRemove);
                    _ApplicationDeclarationQuestionAnswerConfigurations.AfterAdd += new EventListHandler(OnApplicationDeclarationQuestionAnswerConfigurations_AfterAdd);
                    _ApplicationDeclarationQuestionAnswerConfigurations.AfterRemove += new EventListHandler(OnApplicationDeclarationQuestionAnswerConfigurations_AfterRemove);
                }
                return _ApplicationDeclarationQuestionAnswerConfigurations;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationDeclarationQuestionAnswers = null;
            _ApplicationDeclarationQuestionAnswerConfigurations = null;
        }
    }
}