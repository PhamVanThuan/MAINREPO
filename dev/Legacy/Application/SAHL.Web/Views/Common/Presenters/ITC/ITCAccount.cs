using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ITC
{
    public class ITCAccount : ITCBase
    {
        private SAHL.Common.BusinessModel.Interfaces.IAccount _account;
        private int _accountKey;

        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ITCAccount(SAHL.Web.Views.Common.Interfaces.IITC view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            //_view.OnDoEnquiryButtonClicked += new EventHandler(_view_OnDoEnquiryButtonClicked);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            _accountKey = base.GenericKey;

            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            // get the account object
            _account = accRepo.GetAccountByKey(_accountKey);
            //_accountKey = 1509295;

            base.AccountSequence = accRepo.GetAccountSequenceByKey(_accountKey);

            // get the legal entity roles off the account
            int[] roles = { (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor };
            IReadOnlyEventList<ILegalEntityNaturalPerson> lst = _account.GetNaturalPersonLegalEntitiesByRoleType(_view.Messages, roles);
            if (lst != null && lst.Count > 0)
                base.ListLE = new List<ILegalEntityNaturalPerson>(lst);

            // call the base to do the rest of the processing
            base.OnViewInitialised(sender, e);
        }
    }
}