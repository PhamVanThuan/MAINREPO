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
    /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO
    /// </summary>
    public partial class ApplicationDeclarationAnswer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO>, IApplicationDeclarationAnswer
    {
        public ApplicationDeclarationAnswer(SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO ApplicationDeclarationAnswer)
            : base(ApplicationDeclarationAnswer)
        {
            this._DAO = ApplicationDeclarationAnswer;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.ApplicationDeclarations
        /// </summary>
        private DAOEventList<ApplicationDeclaration_DAO, IApplicationDeclaration, ApplicationDeclaration> _ApplicationDeclarations;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.ApplicationDeclarations
        /// </summary>
        public IEventList<IApplicationDeclaration> ApplicationDeclarations
        {
            get
            {
                if (null == _ApplicationDeclarations)
                {
                    if (null == _DAO.ApplicationDeclarations)
                        _DAO.ApplicationDeclarations = new List<ApplicationDeclaration_DAO>();
                    _ApplicationDeclarations = new DAOEventList<ApplicationDeclaration_DAO, IApplicationDeclaration, ApplicationDeclaration>(_DAO.ApplicationDeclarations);
                    _ApplicationDeclarations.BeforeAdd += new EventListHandler(OnApplicationDeclarations_BeforeAdd);
                    _ApplicationDeclarations.BeforeRemove += new EventListHandler(OnApplicationDeclarations_BeforeRemove);
                    _ApplicationDeclarations.AfterAdd += new EventListHandler(OnApplicationDeclarations_AfterAdd);
                    _ApplicationDeclarations.AfterRemove += new EventListHandler(OnApplicationDeclarations_AfterRemove);
                }
                return _ApplicationDeclarations;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.ApplicationDeclarationQuestionAnswers
        /// </summary>
        private DAOEventList<ApplicationDeclarationQuestionAnswer_DAO, IApplicationDeclarationQuestionAnswer, ApplicationDeclarationQuestionAnswer> _ApplicationDeclarationQuestionAnswers;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.ApplicationDeclarationQuestionAnswers
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

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationDeclarations = null;
            _ApplicationDeclarationQuestionAnswers = null;
        }
    }
}