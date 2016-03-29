using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Displays the list of Applicants (no actions permitted).
    /// </summary>
    public class ApplicantsDisplayDebtCounselling : ApplicantsAccountBase
    {
        private IDebtCounselling _dc;
        private IDebtCounsellingRepository _dcRepository;

        protected IDebtCounsellingRepository DCRepo
        {
            get { return _dcRepository; }
        }

        /// <summary>
        /// Consructor. Gets the View and controller pairs.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicantsDisplayDebtCounselling(IApplicants view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _dcRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _dc = DCRepo.GetDebtCounsellingByKey(_node.GenericKey);

            if (_dc == null)
            {
                _view.ShouldRunPage = false;
                return;
            }

            Account = _dc.Account;

            _view.GridHeading = "Account Roles";
            _view.InformationColumnDescription = "Under Debt Counselling";

            // call the base initialise to handle the binding etc
            base.OnViewInitialised(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.ButtonsVisible = false;
        }
    }
}