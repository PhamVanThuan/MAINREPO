using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class Attorney : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Attorney_DAO>, IAttorney
    {
        /// <summary>
        /// Get list of contacts.
        /// </summary>
        /// <param name="ert"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public IList<ILegalEntity> GetContacts(ExternalRoleTypes ert, GeneralStatuses gs)
        {
            return ExternalRoleHelper.GetExternalRoleList(_DAO.Key, GenericKeyTypes.Attorney, ert, gs);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnOriginationSources_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnOriginationSources_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnOriginationSources_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnOriginationSources_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("AttorneyStatusInactive");
        }
    }
}