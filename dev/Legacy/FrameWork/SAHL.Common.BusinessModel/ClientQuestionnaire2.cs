using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO
    /// </summary>
    public partial class ClientQuestionnaire : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO>, IClientQuestionnaire
    {
        public void OnConstruction()
        {
            if (String.IsNullOrEmpty(this._DAO.GUID))
                this._DAO.GUID = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.GUID
        /// </summary>
        public String GUID
        {
            get { return _DAO.GUID; }
            set { _DAO.GUID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.ClientAnswers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnClientAnswers_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.ClientAnswers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnClientAnswers_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.ClientAnswers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnClientAnswers_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientQuestionnaire_DAO.ClientAnswers
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnClientAnswers_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}